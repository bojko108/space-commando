using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DroneScript : MonoBehaviour
{
    [Tooltip("Set height for NavMeshAgent.BaseOffset")]
    public int Height = 6;
    [Tooltip("Set scan height for NavMeshAgent.BaseOffset. This values is used when in Scan mode.")]
    public int ScanHeight = 50;
    [Tooltip("Set max distance to the player when in patrol mode")]
    public float MaxDistance = 30f;

    [Tooltip("Set speed when in patrol mode")]
    public float PatrolSpeed = 5f;
    [Tooltip("Set speed when in patrol mode")]
    public float PatrolAngularSpeed = 140f;
    [Tooltip("Set speed when in patrol mode")]
    public float PatrolAcceleration = 10f;

    [Tooltip("Set attack speed, used also when catching up with the player")]
    public float AttackSpeed = 15f;
    [Tooltip("Set attack angular speed, used also when catching up with the player")]
    public float AttackAngularSpeed = 200f;
    [Tooltip("Set attack acceleration, used also when catching up with the player")]
    public float AttackAcceleration = 15f;

    private Animator animator;

    [HideInInspector]
    public GameObject CurrentTarget;
    private List<GameObject> targets;

    private int enemiesLayer;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.targets = new List<GameObject>();

        this.enemiesLayer = LayerMask.NameToLayer(Resources.Layers.Enemies);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SetInPatrolMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SetInScanMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.SetInAttackMode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == this.enemiesLayer)
        {
            if (other.gameObject.CompareTag(Resources.Tags.Worker))
                return;

            // play enemy detected sound

            // add new target to queue
            this.targets.Add(other.gameObject);

            this.SetInAttackMode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == this.enemiesLayer)
        {
            if (other.gameObject.CompareTag(Resources.Tags.Worker))
                return;

            // remove target from queue
            this.targets.Remove(other.gameObject);
        }
    }

    public void SwitchTarget()
    {
        if (this.CurrentTarget != null)
        {
            bool targetDead = this.CurrentTarget.GetComponent<EnemyHealth>().IsDead;

            if (targetDead)
            {
                this.targets.Remove(this.CurrentTarget);
                this.CurrentTarget = null;
            }
            else
            {
                // current target is not dead so don't switch to a new one
                return;
            }
        }

        // switch to a new target

        List<GameObject> availableTargets = this.targets;

        for (int i = 0; i < availableTargets.Count; i++)
        {
            GameObject current = availableTargets[i];

            if (current == null)
            {
                this.targets.RemoveAt(i);
                continue;
            }

            bool targetDead = current.GetComponent<EnemyHealth>().IsDead;

            if (targetDead)
            {
                this.targets.RemoveAt(i);
                continue;
            }

            this.CurrentTarget = current;
        }
    }


    public void RemoveTarget(GameObject deadTarget)
    {
        this.targets.Remove(deadTarget);
    }

    public void SetInWaitMode()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InScan", false);
        this.animator.SetBool("InPatrol", false);

        //TODO: add behaviour...
    }

    public void SetInPatrolMode()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InScan", false);
        this.animator.SetBool("InPatrol", true);
    }

    public void SetInScanMode()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InPatrol", false);
        this.animator.SetBool("InScan", true);

        //TODO: add behaviour...
    }

    public void SetInAttackMode()
    {
        if (this.targets.Count > 0)
        {
            this.SwitchTarget();

            if (this.CurrentTarget == null)
            {
                this.SetInPatrolMode();
            }
            else
            {
                this.animator.SetBool("InAttack", true);
                this.animator.SetBool("InScan", false);
                this.animator.SetBool("InPatrol", false);
            }
        }
        else
        {
            Debug.Log("no targets to attack");
        }
    }
}

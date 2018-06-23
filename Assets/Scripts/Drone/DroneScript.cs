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
    [Tooltip("Max angle at which the drone can shoot at enemies")]
    public float MaxAttackAngle = 90f;

    [Tooltip("Set sound on enemy detected. If more than 5 enemies are detected this will be repeated")]
    public AudioClip AlarmSound;
    [Tooltip("Set drone engine sound")]
    public AudioClip EngineSound;

    private Animator animator;

    [HideInInspector]
    public Transform PlayerTransform;

    [HideInInspector]
    // for calculating path to POIs
    public GameObject CurrentTarget;
    private List<GameObject> targets;

    [HideInInspector]
    public DroneSignalLight SignalLight;

    private AudioSource droneEngineAudioSource;
    private AudioSource droneAlarmAudioSource;

    private void Awake()
    {
        this.targets = new List<GameObject>();

        this.PlayerTransform = GameObject.FindGameObjectWithTag(Resources.Tags.Player).transform;

        this.animator = GetComponent<Animator>();
        this.SignalLight = GetComponentInChildren<DroneSignalLight>();

        // set all audio listeners
        AudioSource[] audioSources = GetComponents<AudioSource>();

        this.droneAlarmAudioSource = audioSources[0];
        this.droneAlarmAudioSource.clip = this.AlarmSound;
        //this.droneAlarmAudioSource.volume = 0.5f;

        this.droneEngineAudioSource = audioSources[1];
        this.droneEngineAudioSource.clip = this.EngineSound;
        this.droneEngineAudioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SetInScanMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SetInAttackMode();
        }

        if (Time.frameCount % 20 == 0)
        {
            if (this.targets.Count > 0)
            {
                this.droneAlarmAudioSource.loop = this.targets.Count >= 5;
            }
            else
            {
                this.droneAlarmAudioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Resources.Tags.BaseCommander)
            || other.gameObject.CompareTag(Resources.Tags.Commander)
            || other.gameObject.CompareTag(Resources.Tags.Soldier))
        {
            this.AddTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Resources.Tags.BaseCommander)
            || other.gameObject.CompareTag(Resources.Tags.Commander)
            || other.gameObject.CompareTag(Resources.Tags.Soldier))
        {
            // remove target from queue
            this.RemoveTarget(other.gameObject);
        }
    }

    public void SetInPatrolMode()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InScan", false);
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
                this.animator.SetBool("InAttack", this.animator.GetBool("InAttack") == false);
                this.animator.SetBool("InScan", false);
            }
        }
        else
        {
            Debug.Log("no targets to attack");
        }
    }

    public void SetInScanMode()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InScan", this.animator.GetBool("InScan") == false);

        //TODO: add behaviour...
    }

    public void SwitchTarget()
    {
        if (this.CurrentTarget != null)
        {
            bool targetDead = this.CurrentTarget.GetComponent<EnemyHealth>().IsDead;

            if (targetDead)
            {
                this.RemoveTarget(this.CurrentTarget);
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

    private void AddTarget(GameObject target)
    {
        if (this.targets.Contains(target) == false)
        {
            bool targetDead = target.GetComponent<EnemyHealth>().IsDead;

            if (targetDead == false)
            {
                // add new target to queue
                this.targets.Add(target);

                if (this.droneAlarmAudioSource.isPlaying == false)
                {
                    this.droneAlarmAudioSource.Play();
                }

                Debug.Log("Enemies: " + this.targets.Count.ToString());
            }
        }
    }

    public void RemoveTarget(GameObject deadTarget)
    {
        if (this.targets.Contains(deadTarget) == false)
        {
            this.targets.Remove(deadTarget);
        }
    }
}

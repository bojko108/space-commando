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
    public float MaxDistance = 20f;
    [Tooltip("Set speed when in patrol mode")]
    public float PatrolSpeed = 5f;
    [Tooltip("Set speed, when catching up")]
    public float MaxSpeed = 15f;

    [Tooltip("Gun damage")]
    public int DamagePerShot = 50;
    [Tooltip("Gun fire rate")]
    public float FireRate = 0.3f;

    private float timer;

    private Animator animator;

    [HideInInspector]
    public GameObject CurrentTarget;
    private List<GameObject> targets;

    private UnityAction onPatrol;
    private UnityAction onScan;
    private UnityAction onAttack;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.targets = new List<GameObject>();
    }

    private void Start()
    {
        this.onPatrol = new UnityAction(this.OnPatrol);
        EventManager.On(Resources.Events.Patrol, this.onPatrol);

        this.onScan = new UnityAction(this.OnScan);
        EventManager.On(Resources.Events.Scan, this.onScan);

        this.onAttack = new UnityAction(this.OnAttack);
        EventManager.On(Resources.Events.Attack, this.onAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.BaseCommander)
            || other.gameObject.tag.Equals(Resources.Tags.Commander)
            || other.gameObject.tag.Equals(Resources.Tags.Soldier))
        {
            // play enemy detected sound

            // add new target to queue
            this.targets.Add(other.gameObject);

            this.OnAttack();

            //// attack target
            //if (this.CurrentTarget == null && this.targets.Count > 0)
            //{
            //    this.CurrentTarget = this.targets[0];
            //}
        }
    }

    private void OnPatrol()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InScan", false);
        this.animator.SetBool("InPatrol", true);
    }

    private void OnScan()
    {
        this.animator.SetBool("InAttack", false);
        this.animator.SetBool("InPatrol", false);
        this.animator.SetBool("InScan", true);
    }

    private void OnAttack()
    {
        if (this.targets.Count > 0)
        {
            if (this.CurrentTarget == null)
            {
                this.CurrentTarget = this.targets[0];
            }

            this.animator.SetBool("InScan", false);
            this.animator.SetBool("InPatrol", false);
            this.animator.SetBool("InAttack", true);
        }
        else
        {
            Debug.Log("no targets to attack");
        }
    }

    public void Shoot(Quaternion direction)
    {
        this.timer += Time.deltaTime;
        if (this.timer >= this.FireRate)
        {
            StartCoroutine(this.FirePlasmaBullet(direction));
        }

        //if (this.timer >= this.FireRate * this.effectsDisplayTime)
        //{
        //    this.DisableEffects();
        //}
    }

    private IEnumerator FirePlasmaBullet(Quaternion direction)
    {
        GameObject bullet = ObjectPooler.Current.GetPooledObject(enumBulletType.Plasma);

        if (bullet != null)
        {
            // move the bullet 10m in front of the gun so it does not collide with the player
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = direction;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 150f;

            yield return new WaitForSeconds(3f);

            bullet.SetActive(false);
        }
    }

    public void HitEnemy(GameObject enemy, Vector3 hitPoint)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            bool attack = enemy.tag.Equals(Resources.Tags.Worker) == false;
            bool runAway = enemy.tag.Equals(Resources.Tags.Worker);

            // harm the enemy
            enemyHealth.TakeDamage(this.DamagePerShot, attack, runAway, hitPoint);
        }
    }
}

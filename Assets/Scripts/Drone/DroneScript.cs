using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DroneScript : MonoBehaviour
{
    #region Properties

    [Tooltip("Select key for activating scan mode")]
    public KeyCode EnterScanModeKey = KeyCode.Q;
    [Tooltip("Select key for activating attack mode")]
    public KeyCode EnterAttackModeKey = KeyCode.E;

    [Tooltip("Set max distance from the player when in patrol mode")]
    public float MaxDistance = 30f;

    [Tooltip("Set drone speed when in patrol mode")]
    public float PatrolSpeed = 5f;
    [Tooltip("Set drone angular speed when in patrol mode")]
    public float PatrolAngularSpeed = 140f;
    [Tooltip("Set drone acceleration when in patrol mode")]
    public float PatrolAcceleration = 10f;

    [Tooltip("Set drone attack speed, used also when catching up with the player")]
    public float AttackSpeed = 15f;
    [Tooltip("Set drone attack angular speed, used also when catching up with the player")]
    public float AttackAngularSpeed = 200f;
    [Tooltip("Set drone attack acceleration, used also when catching up with the player")]
    public float AttackAcceleration = 15f;
    [Tooltip("Set max angle at which the drone can shoot at enemies")]
    public float MaxAttackAngle = 60f;

    [Tooltip("Sound played on enemy detected. If more than 5 enemies are detected this will be repeated")]
    public AudioClip AlarmSound;
    [Tooltip("Set drone engine sound")]
    public AudioClip EngineSound;

    [HideInInspector]
    public Transform PlayerTransform;
    [HideInInspector]
    public GameObject CurrentTarget;    // for calculating path to POIs
    private List<GameObject> targets;

    [Tooltip("Set drone light - signals current drone mode")]
    public DroneSignalLight SignalLight;
    [Tooltip("Set drone scanner position")]
    public Transform ScannerTransform;
    [HideInInspector]
    public ScannerScript ScannerScript;

    [Tooltip("Slider, used to update battery level on screen")]
    public Slider DroneBatterySlider;
    [Tooltip("Set drone battery level when in attack mode")]
    [Range(1f, 500f)]
    public float MaxBatteryLevel = 100f;
    [Tooltip("Set battery recharge factor - use large value for fast recharge")]
    [Range(0f, 2f)]
    public float RechargeFactor = 0.5f;

    // used to stop attack mode and battery draining process
    private bool endAttackMode = false;
    // current battery level
    private float batteryLevel;

    private Animator animator;

    private AudioSource droneEngineAudioSource;
    private AudioSource droneAlarmAudioSource;

    // reference to the trigger collider, responsible for detecting enemies
    private SphereCollider detectionCollider;

    #endregion

    private void Awake()
    {
        this.targets = new List<GameObject>();

        this.PlayerTransform = GameObject.FindGameObjectWithTag(Resources.Tags.Player).transform;

        this.animator = GetComponent<Animator>();

        #region get reference to the trigger collider

        SphereCollider[] colliders = GetComponents<SphereCollider>();
        for(int i = 0;i<colliders.Length; i++)
        {
            if (colliders[i].isTrigger)
            {
                this.detectionCollider = colliders[i];
                break;
            }
        }

        #endregion

        this.SignalLight = GetComponentInChildren<DroneSignalLight>();

        this.ScannerScript = this.ScannerTransform.gameObject.GetComponent<ScannerScript>();

        #region set drone alarm and engine sounds

        this.droneAlarmAudioSource = this.gameObject.AddComponent<AudioSource>();
        this.droneAlarmAudioSource.volume = 0.6f;
        this.droneAlarmAudioSource.loop = false;
        this.droneAlarmAudioSource.playOnAwake = false;
        this.droneAlarmAudioSource.clip = this.AlarmSound;
        this.droneAlarmAudioSource.spatialBlend = 0f;

        this.droneEngineAudioSource = this.gameObject.AddComponent<AudioSource>();
        this.droneEngineAudioSource.spatialBlend = 1f;
        this.droneEngineAudioSource.minDistance = 20f;
        this.droneEngineAudioSource.maxDistance = 100f;
        this.droneEngineAudioSource.loop = true;
        this.droneEngineAudioSource.playOnAwake = false;
        this.droneEngineAudioSource.clip = this.EngineSound;
        this.droneEngineAudioSource.Play();

        #endregion

        this.batteryLevel = this.MaxBatteryLevel;
        this.DroneBatterySlider.minValue = 0;
        this.DroneBatterySlider.maxValue = this.MaxBatteryLevel;
        this.DroneBatterySlider.value = this.MaxBatteryLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(this.EnterScanModeKey))
        {
            this.SetInScanMode();
        }
        if (Input.GetKeyDown(this.EnterAttackModeKey))
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
        if (this.IsEnemy(other.gameObject))
        {
            this.AddTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // remove target from queue
        this.RemoveTarget(other.gameObject);
    }


    // used when loading saved game
    public void SetStatus(Drone savedDrone)
    {
        if (savedDrone.InScan)
        {
            this.SetInScanMode();
        }

        if (savedDrone.InAttack)
        {
            this.SetInAttackMode();
        }
    }
    
    public void SetInPatrolMode()
    {
        if (this.animator.GetBool("InScan"))
        {
            this.ScannerScript.InteruptScan();
        }

        this.animator.SetBool("InScan", false);
        this.animator.SetBool("InAttack", false);
    }

    public void SetInAttackMode()
    {
        if (this.animator.GetBool("InScan"))
        {
            this.ScannerScript.InteruptScan();
        }

        this.targets.Clear();

        this.animator.SetBool("InAttack", this.animator.GetBool("InAttack") == false);
        this.animator.SetBool("InScan", false);

        //if (this.targets.Count > 0)
        //{
        //    this.SwitchTarget();

        //    if (this.CurrentTarget == null)
        //    {
        //        this.SetInPatrolMode();
        //    }
        //    else
        //    {
        //        if (this.animator.GetBool("InScan"))
        //        {
        //            this.ScannerScript.InteruptScan();
        //        }

        //        //this.targets.Clear();

        //        this.animator.SetBool("InAttack", this.animator.GetBool("InAttack") == false);
        //        this.animator.SetBool("InScan", false);
        //    }
        //}
        //else
        //{
        //    Debug.Log("no targets to attack");
        //}
    }

    public void SetInScanMode()
    {
        this.animator.SetBool("InAttack", false);

        if (this.animator.GetBool("InScan"))
        {
            this.ScannerScript.InteruptScan();
            this.animator.SetBool("InScan", false);
        }
        else
        {
            this.animator.SetBool("InScan", true);
        }
    }


    #region Target managers

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
            }
        }
    }

    private void RemoveTarget(GameObject target)
    {
        if (this.targets.Contains(target))
        {
            this.targets.Remove(target);
        }
    }

    private bool IsEnemy(GameObject target)
    {
        return (target.CompareTag(Resources.Tags.BaseCommander)
            || target.CompareTag(Resources.Tags.Commander)
            || target.CompareTag(Resources.Tags.Soldier));
    }

    #endregion


    #region Battery managers

    private IEnumerator batteryDraining;
    private IEnumerator batteryCharging;

    public void StartAttackMode()
    {
        this.endAttackMode = false;

        if (this.detectionCollider != null)
        {
            if (this.colliderExpand != null) StopCoroutine(this.colliderExpand);
            this.colliderExpand = this.ColliderExpandEnumerator();
            StartCoroutine(this.colliderExpand);
        }

        if (this.batteryDraining != null) StopCoroutine(this.batteryDraining);
        this.batteryDraining = this.BatteryDrainingEnumerator();
        StartCoroutine(this.batteryDraining);
    }

    public void EndAttackMode()
    {
        this.endAttackMode = true;

        if (this.detectionCollider != null)
        {
            if (this.colliderShrink != null) StopCoroutine(this.colliderShrink);
            this.colliderShrink = this.ColliderShrinkEnumerator();
            StartCoroutine(this.colliderShrink);
        }

        if (this.batteryCharging != null) StopCoroutine(this.batteryCharging);
        this.batteryCharging = this.BatteryChargingEnumerator();
        StartCoroutine(this.batteryCharging);

        this.SetInPatrolMode();
    }

    private IEnumerator BatteryDrainingEnumerator()
    {
        // stop charging the battery
        if (this.batteryCharging != null) StopCoroutine(this.batteryCharging);

        while (this.endAttackMode == false && this.batteryLevel > 0)
        {
            if (Time.timeScale > 0f)
            {
                this.batteryLevel -= 1f;
                this.DroneBatterySlider.value = this.batteryLevel;
            }

            yield return new WaitForEndOfFrame();
        }

        // the drone has no more battery to continue in attack mode

        this.EndAttackMode();
    }

    private IEnumerator BatteryChargingEnumerator()
    {
        while (this.batteryLevel < this.MaxBatteryLevel)
        {
            if (Time.timeScale > 0f)
            {
                this.batteryLevel += 1f * this.RechargeFactor;
                this.DroneBatterySlider.value = this.batteryLevel;
            }

            yield return new WaitForEndOfFrame();
        }

        // drone's battery is fully charged now
    }

    #endregion


    #region Collider managers

    private IEnumerator colliderExpand;
    private IEnumerator colliderShrink;

    private IEnumerator ColliderExpandEnumerator()
    {
        if (this.colliderShrink != null) StopCoroutine(this.colliderShrink);

        // start expanding the collider
        while (this.detectionCollider.radius < 200f)
        {
            if (Time.timeScale > 0f)
            {
                this.detectionCollider.radius += 10f;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ColliderShrinkEnumerator()
    {
        // start expanding the collider
        while (this.detectionCollider.radius > 1f)
        {
            if (Time.timeScale > 0f)
            {
                this.detectionCollider.radius -= 10f;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}

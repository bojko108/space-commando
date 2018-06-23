using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Tooltip("Minimum distance to detect the player")]
    public float DetectDistance = 50f;
    [Tooltip("Enemy travel distance while using the NavMesh")]
    public float WanderRadius = 100f;
    [Tooltip("Enemy wandering time while using the NavMesh")]
    public float WanderTime = 10f;
    [Tooltip("Is the enemy chasing the player. Can be activated when the player is shooting at the enemy...")]
    public bool IsChasing = false;
    [Tooltip("Is the enemy scared and running away from the player. TRUE for Workers")]
    public bool IsScared = false;
    [Tooltip("Stop moving")]
    public bool Stop = false;
    [Tooltip("Walking speed")]
    public float WalkSpeed = 3;
    [Tooltip("Running speed - when in attack or scared mode")]
    public float RunSpeed = 10;

    //[Tooltip("Save point to which the Workers will go when the player is detected")]
    private Transform savePoint;
    // the enemy should be scared only once
    private bool isAlreadyScared = false;

    // reference to the player
    private GameObject player;
    //public GameObject KillTarget;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    private float timer;

    private void Start()
    {
        this.timer = this.WanderTime;
        this.player = GameObject.FindGameObjectWithTag(Resources.Tags.Player);
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.agent = GetComponent<NavMeshAgent>();
        this.agent.speed = this.WalkSpeed;

        GameObject[] savePoints = GameObject.FindGameObjectsWithTag(Resources.Tags.CommanderSpawnPoint);
        this.savePoint = savePoints[UnityEngine.Random.Range(0, savePoints.Length)].transform;

        StartCoroutine(this.SetDestination());
    }

    private void Update()
    {
        if (this.Stop || this.player == null) return;

        this.timer += Time.deltaTime;

        // execute every 20th frame
        if (Time.frameCount % 20 == 0)
        {
            // detect if the player is near the enemy
            if (this.DistanceToPlayer() < this.DetectDistance)
            {
                // workers will go away from the player
                this.IsScared = this.tag.Equals(Resources.Tags.Worker);

                // commanders and soldiers will attack the player
                this.IsChasing = this.tag.Equals(Resources.Tags.Worker) == false;
            }
        }
    }

    /// <summary>
    /// Sets the playe in attack mode: will go to the player with runnung speed
    /// </summary>
    private void SetToAtackMode()
    {
        this.agent.speed = this.RunSpeed;
        this.agent.stoppingDistance = 1f;
        this.agent.SetDestination(this.player.transform.position);
    }

    /// <summary>
    /// Sets the enemy to scared mode - will not attack but run straight to the save point
    /// </summary>
    private void SetToScaredMode()
    {
        if (this.isAlreadyScared)
            return;

        this.audioSource.Play();
        this.animator.SetBool("IsScared", true);

        // go to the save point
        this.agent.speed = this.RunSpeed;
        this.agent.stoppingDistance = 1f;
        this.agent.SetDestination(this.savePoint.position);

        this.IsScared = true;
        this.isAlreadyScared = true;
    }

    private IEnumerator SetDestination()
    {
        while (this.Stop == false)
        {
            // check if the enemy should attack the player
            if (this.IsChasing)
            {
                this.SetToAtackMode();
            }
            // if the enemy is scared the destination point is set to this.SavePoint
            else if (this.IsScared)
            {
                this.SetToScaredMode();
            }
            // set a new random waypoint
            else
            {
                if (this.timer >= this.WanderTime && this.HasReachedDestination())
                {
                    // set a new random destination point
                    Vector3 destination = this.GetNavMeshTargetPosition(this.transform.position, this.WanderRadius, NavMesh.AllAreas);
                    //this.agent.stoppingDistance = this.tag.Equals(Resources.Tags.Worker) ? 1f : 10f;
                    this.agent.SetDestination(destination);
                    this.timer = 0;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        this.agent.enabled = false;
    }

    private bool HasReachedDestination()
    {
        return this.agent.remainingDistance <= this.agent.stoppingDistance;
    }

    /// <summary>
    /// Calculates a new destination point on the NavMesh
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="maxDistance"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    private Vector3 GetNavMeshTargetPosition(Vector3 origin, float maxDistance, int layerMask)
    {
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * maxDistance;
        randomPosition += origin;

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPosition, out navMeshHit, maxDistance, layerMask))
        {
            return navMeshHit.position;
        }
        else { return Vector3.zero; }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, this.player.transform.position);
    }
}
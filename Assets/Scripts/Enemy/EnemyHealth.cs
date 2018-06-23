using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Enemy health level when the game starts")]
    public int StartingHealth = 100;
    [Tooltip("Current health level")]
    public int CurrentHealth;
    [Tooltip("Sound to play when the enemy is dead")]
    public AudioClip DeathSound;

    [HideInInspector]
    public bool IsDead { get; private set; }

    private AudioSource enemyAudio;
    private Animator animator;
    private ParticleSystem hitParticles;
    // collider for shooting at the enemy
    private CapsuleCollider capsuleCollider;
    
    // movement script
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        this.enemyAudio = GetComponent<AudioSource>();
        this.animator = GetComponent<Animator>();
        this.hitParticles = GetComponentInChildren<ParticleSystem>();
        this.capsuleCollider = GetComponent<CapsuleCollider>();
        this.enemyMovement = GetComponent<EnemyMovement>();
        this.CurrentHealth = this.StartingHealth;
    }

    /// <summary>
    /// Apply damage to the enemy
    /// </summary>
    /// <param name="amount">Amount of damage</param>
    /// <param name="switchToAttackMode">Tell the enemy to swich to attack mode. This is FALSE in case of Worker</param>
    /// <param name="runAway">Tell the enemy to run away. This is TRUE in case of worker</param>
    /// <param name="hitPoint">Bullet hit point</param>
    public void TakeDamage(int amount, bool attack, bool runAway, Vector3 hitPoint)
    {
        StartCoroutine(this.HandleHit());

        this.enemyMovement.IsChasing = attack;
        this.enemyMovement.IsScared = runAway;

        if (this.IsDead)
        {
            return;
        }

        this.CurrentHealth -= amount;

        this.hitParticles.transform.position = hitPoint;
        this.hitParticles.Play();

        if (this.CurrentHealth <= 0)
        {
            this.Dead();
        }
    }

    private IEnumerator HandleHit()
    {
        this.animator.SetBool("IsHit", true);
        yield return new WaitForSeconds(1f);
        this.animator.SetBool("IsHit", false);
    }

    public void SetHealth(int health)
    {
        this.CurrentHealth = health;
    }

    /// <summary>
    /// Kill the enemy
    /// </summary>
    private void Dead()
    {
        this.IsDead = true;

        // stop moving
        this.enemyMovement.Stop = true;

        this.capsuleCollider.enabled = false;

        this.animator.SetTrigger("EnemyDead");

        // play deatch sound
        this.enemyAudio.Stop();
        this.enemyAudio.clip = this.DeathSound;
        this.enemyAudio.playOnAwake = false;
        this.enemyAudio.loop = false;
        this.enemyAudio.Play();

        // destroy minimap icon
        List<GameObject> minimapIcon = this.gameObject.FindChildrenByName(Resources.Various.MinimapIcon);
        if (minimapIcon.Count > 0)
        {
            Destroy(minimapIcon[0]);
        }

        // destroy enemy
        Destroy(this.gameObject, 10f);
    }
}

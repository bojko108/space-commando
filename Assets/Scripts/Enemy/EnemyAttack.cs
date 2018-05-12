using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("Time between attacks")]
    public float TimeBetweenAttacks = 1f;
    [Tooltip("Damage applied to the player")]
    public int AttackDamage = 10;

    private Animator anim;
    private GameObject player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private bool playerInRange;
    private float timer;

    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag(Resources.Tags.Player);
        this.playerHealth = player.GetComponent<PlayerHealth>();
        this.enemyHealth = GetComponent<EnemyHealth>();
        this.anim = GetComponent<Animator>();
    }

    /// <summary>
    /// When the player is in range for attack
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.player)
        {
            this.playerInRange = true;
            this.anim.SetBool("IsAttacking", true);
        }
    }

    /// <summary>
    /// when the player is out of range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            this.playerInRange = false;
            this.anim.SetBool("IsAttacking", false);
        }
    }

    private void Update()
    {
        // add the time since Update was last called to the timer.
        this.timer += Time.deltaTime;

        // if the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (this.timer >= this.TimeBetweenAttacks && this.playerInRange && this.enemyHealth.CurrentHealth > 0)
        {
            this.Attack();
        }

        // if the player is dead - switch to "Idle" animation
        if (this.playerHealth.CurrentHealth <= 0)
        {
            this.anim.SetTrigger("PlayerDead");
        }
    }

    /// <summary>
    /// Attack the player
    /// </summary>
    private void Attack()
    {
        this.timer = 0f;

        if (this.playerHealth.CurrentHealth > 0)
        {
            // apply damage to the player
            this.playerHealth.TakeDamage(this.AttackDamage);
        }
    }
}

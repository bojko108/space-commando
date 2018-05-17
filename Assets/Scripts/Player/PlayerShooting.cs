using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Tooltip("Gun damage")]
    public int DamagePerShot = 20;
    [Tooltip("Gun fire rate")]
    public float FireRate = 0.15f;
    [Tooltip("Gun range")]
    public float Range = 200f;
    [Tooltip("Laser bullet prefab")]
    public GameObject lazerPrefab;

    private Ray shootRay;
    private RaycastHit shootHit;
    private ParticleSystem gunParticles;
    private Light gunLight;
    private AudioSource gunSound;

    private float timer;
    private int shootableMask;
    private float effectsDisplayTime = 0.2f;

    private void Awake()
    {
        // set shootable layers
        this.shootableMask = LayerMask.GetMask(Resources.Layers.Enemies, Resources.Layers.Buildings);

        this.gunParticles = GetComponent<ParticleSystem>();
        this.gunLight = GetComponentInChildren<Light>();
        this.gunSound = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        this.timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && this.timer >= this.FireRate)
        {
            this.Shoot();
        }

        if (this.timer >= this.FireRate * this.effectsDisplayTime)
        {
            this.DisableEffects();
        }
    }

    private void Shoot()
    {
        this.timer = 0f;

        this.gunSound.Play();
        this.gunLight.enabled = true;

        this.gunParticles.Stop();
        this.gunParticles.Play();

        this.shootRay.origin = this.transform.position;
        this.shootRay.direction = this.transform.forward;

        this.FireLaserBullet();

        // fire a ray which will ignore trigger colliders
        if (Physics.Raycast(this.shootRay, out this.shootHit, this.Range, this.shootableMask, QueryTriggerInteraction.Ignore))
        {
            EnemyHealth enemyHealth = this.shootHit.collider.gameObject.GetComponent<EnemyHealth>();
            bool attack = this.shootHit.collider.gameObject.tag.Equals(Resources.Tags.Worker) == false;
            bool runAway = this.shootHit.collider.gameObject.tag.Equals(Resources.Tags.Worker);

            // if the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // harm the enemy
                enemyHealth.TakeDamage(this.DamagePerShot, attack, runAway, this.shootHit.point);
            }
        }
    }

    // does not colllide with any objects, just for displaying bullets
    private void FireLaserBullet()
    {
        // use pooling
        GameObject bullet = Instantiate(this.lazerPrefab, this.transform.position + this.transform.forward * 10, this.transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 150;
        Destroy(bullet, 1f);
    }

    /// <summary>
    /// Disables gun fire effects
    /// </summary>
    private void DisableEffects()
    {
        this.gunLight.enabled = false;
    }
}

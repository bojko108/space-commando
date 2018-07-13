using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [Tooltip("Set bullet velocity")]
    public float Velocity = 150f;
    [Tooltip("Gun damage")]
    public int Damage = 20;
    [Tooltip("Gun fire rate")]
    public float FireRate = 0.15f;
    [Tooltip("Gun fire sound")]
    public AudioClip GunSound;
    [Tooltip("Choose bullets type")]
    public enumBulletType BulletType = enumBulletType.Laser;

    [HideInInspector]
    public float Timer;
    [HideInInspector]
    public float EffectsDisplayTime = 0.2f;

    private ParticleSystem gunParticles;
    private Light gunLight;
    private AudioSource gunSoundSource;

    private Vector3 bulletOrigin;
    private Quaternion bulletRotation;

    private void Awake()
    {
        this.gunParticles = GetComponent<ParticleSystem>();
        this.gunLight = GetComponentInChildren<Light>();
        this.gunSoundSource = this.gameObject.GetComponent<AudioSource>();
        this.gunSoundSource.clip = this.GunSound;
    }

    public virtual void Shoot(Vector3 origin, Quaternion rotation)
    {
        this.bulletOrigin = origin;
        this.bulletRotation = rotation;

        this.Timer = 0f;

        this.gunSoundSource.Play();
        this.gunLight.enabled = true;

        this.gunParticles.Stop();
        this.gunParticles.Play();
    }

    public IEnumerator FireBullet()
    {
        GameObject bullet = this.GetBullet(this.BulletType);

        if (bullet != null)
        {
            yield return new WaitForSeconds(3f);

            bullet.SetActive(false);
        }
    }

    public GameObject GetBullet(enumBulletType type)
    {
        GameObject bullet = ObjectPooler.Current.GetPooledObject(type);

        if (bullet != null)
        {
            bullet.transform.position = this.bulletOrigin;
            bullet.transform.rotation = this.bulletRotation;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * this.Velocity;
        }

        return bullet;
    }

    public void HitEnemy(GameObject enemy, Vector3 hitPoint)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            bool attack = enemy.tag.Equals(Resources.Tags.Worker) == false;
            bool runAway = enemy.tag.Equals(Resources.Tags.Worker);

            // harm the enemy
            enemyHealth.TakeDamage(this.Damage, attack, runAway, hitPoint);
        }
    }

    public void DisableEffects()
    {
        this.gunLight.enabled = false;
    }
}

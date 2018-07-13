using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts;

public class PlayerShooting : ShootingScript
{
    private void Update()
    {
        this.Timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && this.Timer >= this.FireRate)
        {
            // move the bullet 10m in front of the gun so it does not collide with the player
            Vector3 origin = this.transform.position + this.transform.forward * 10f;
            Quaternion rotation = this.transform.rotation;

            this.Shoot(origin, rotation);
        }

        if (this.Timer >= this.FireRate * this.EffectsDisplayTime)
        {
            this.DisableEffects();
        }
    }

    public override void Shoot(Vector3 origin, Quaternion rotation)
    {
        base.Shoot(origin, rotation);

        StartCoroutine(this.FireBullet());
    }
}

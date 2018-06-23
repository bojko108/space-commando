using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : BulletScript
{
    private void Awake()
    {
        base.OnAwake();

        this.ShootingScript = GameObject.FindGameObjectWithTag(Resources.Tags.Player).GetComponentInChildren<PlayerShooting>();
    }
}

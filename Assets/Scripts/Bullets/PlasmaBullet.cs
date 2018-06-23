using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBullet : BulletScript {
    
    private void Awake()
    {
        base.OnAwake();

        this.ShootingScript = GameObject.FindGameObjectWithTag(Resources.Tags.Drone).GetComponentInChildren<DroneShooting>();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private int enemiesLayer;
    private PlayerShooting playerShooting;
    private Rigidbody bulletBody;

    private void Start()
    {
        this.bulletBody = this.GetComponent<Rigidbody>();
        this.enemiesLayer = LayerMask.NameToLayer(Resources.Layers.Enemies);
        this.playerShooting = GameObject.FindGameObjectWithTag(Resources.Tags.Player).GetComponentInChildren<PlayerShooting>();
    }

    private void OnDisable()
    {
        // after a collision, velocity parameters get changed...
        this.bulletBody.velocity = Vector3.zero;
        this.bulletBody.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);

        GameObject hit = collision.gameObject;

        if (hit.layer == this.enemiesLayer)
        {
            this.playerShooting.HitEnemy(hit, collision.contacts[0].point);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector]
    public ShootingScript ShootingScript;
    private int enemiesLayer;    
    private Rigidbody bulletBody;

    public void OnAwake()
    {
        this.bulletBody = this.GetComponent<Rigidbody>();
        this.enemiesLayer = LayerMask.NameToLayer(Resources.Layers.Enemies);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;

        if (hit != null && hit.layer == this.enemiesLayer)
        {
            this.ShootingScript.HitEnemy(hit, collision.contacts[0].point);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // after a collision, velocity parameters get changed...
        this.bulletBody.velocity = Vector3.zero;
        this.bulletBody.angularVelocity = Vector3.zero;
    }
}
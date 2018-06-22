using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaScript : MonoBehaviour {

    private int enemiesLayer;
    private DroneScript droneLogic;

    private void Awake()
    {
        this.enemiesLayer = LayerMask.NameToLayer(Resources.Layers.Enemies);
        this.droneLogic = GameObject.FindGameObjectWithTag(Resources.Tags.Drone).GetComponentInChildren<DroneScript>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;

        if (hit != null && hit.layer == this.enemiesLayer)
        {
            this.droneLogic.HitEnemy(hit, collision.contacts[0].point);
        }

        this.gameObject.SetActive(false);
    }
}

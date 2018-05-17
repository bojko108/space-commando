using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngineScript : MonoBehaviour
{
    public AudioClip BrokenEngineSound;
    public AudioClip RepairedEngineSound;

    private AudioSource source;
    private SphereCollider sphereCollider;
    private float initialRadius;
    private float repairingRadius;
    private GameObject engine;

    private void Awake()
    {
        this.source = this.GetComponent<AudioSource>();

        this.sphereCollider = this.GetComponent<SphereCollider>();
        this.initialRadius = this.sphereCollider.radius;
        this.repairingRadius = this.initialRadius * 2;

        this.engine = this.gameObject.FindChildrenByName(Resources.Various.Engine)[0];

        this.NotRepaired();
    }

    public void NotRepaired()
    {
        this.sphereCollider.radius = this.initialRadius;

        this.source.Stop();
        this.source.clip = this.BrokenEngineSound;
        this.source.loop = true;
        this.source.Play();

        InvokeRepeating("AnimateShipEngine", 0f, 0.4f);
    }

    public void Repaired()
    {
        this.sphereCollider.radius = this.repairingRadius;

        this.engine.SetActive(false);
        CancelInvoke("AnimateShipEngine");

        this.source.Stop();
        this.source.clip = this.RepairedEngineSound;
        this.source.loop = false;
        this.source.Play();
    }

    private void AnimateShipEngine()
    {
        // animate ship's engine: hide/show :)
        this.engine.SetActive(!this.engine.activeSelf);
    }
}

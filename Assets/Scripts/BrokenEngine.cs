using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEngine : MonoBehaviour
{
    public AudioClip BrokenEngineSound;
    public AudioClip RepairedEngineSound;

    private AudioSource source;

    private GameObject engine;

    private void Start()
    {
        this.source = this.GetComponent<AudioSource>();
        this.source.clip = this.BrokenEngineSound;
        this.source.loop = true;
        this.source.Play();

        this.engine = this.gameObject.FindChildrenByName(Resources.Various.Engine)[0];
        InvokeRepeating("AnimateShipEngine", 0f, 0.4f);
    }

    public void Repaired()
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedpackScript : MonoBehaviour
{
    [Tooltip("Rotation speed used to animate the medpack")]
    public float RotationSpeed = 30f;

    private AudioSource healSound;
    
    // to freeze minimap icon rotation later - minimap icon will not be rotated
    private Quaternion initRotation;
    private Transform minimapIcon;

    private Transform thisTransform;

    private void Start()
    {
        this.thisTransform = this.transform;

        this.minimapIcon = this.gameObject.FindChildrenByTag(Resources.Tags.MinimapIcon)[0].transform;
        this.initRotation = this.minimapIcon.rotation;

        this.healSound = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // rotate the medpack object
        this.thisTransform.Rotate(0, this.RotationSpeed * Time.deltaTime, 0);
    }

    private void LateUpdate()
    {
        // freeze minimap icon rotation
        this.minimapIcon.rotation = this.initRotation;
    }

    private void OnBecameVisible()
    {
        this.enabled = true;
    }

    private void OnBecameInvisible()
    {
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.Player))
        {
            this.healSound.Play();

            other.gameObject.GetComponent<PlayerHealth>().HealPlayer();
        }
    }
}

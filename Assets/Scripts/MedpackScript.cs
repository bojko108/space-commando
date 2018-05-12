using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedpackScript : MonoBehaviour
{
    [Tooltip("Rotation speed used to animate the medpack")]
    public float RotationSpeed = 30f;

    // to freeze minimap icon rotation later - minimap icon will not be rotated
    private Quaternion initRotation;
    private Transform minimapIcon;

    private void Start()
    {
        this.minimapIcon = this.gameObject.FindChildrenByTag(Resources.Tags.MinimapIcon)[0].transform;
        this.initRotation = this.minimapIcon.rotation;
    }

    private void Update()
    {
        // rotate the medpack object
        this.transform.Rotate(0, this.RotationSpeed * Time.deltaTime, 0);
    }

    private void LateUpdate()
    {
        // freeze minimap icon rotation
        this.minimapIcon.rotation = this.initRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.Player))
        {
            other.gameObject.GetComponent<PlayerHealth>().HealPlayer();
        }
    }
}

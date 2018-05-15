using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInScript : MonoBehaviour
{
    [Tooltip("the value of Camera's 'field of view' parameter when zoomed")]
    public float ZoomedFieldOfView = 30;

    private Camera targetCamera;
    private float cameraInitialFOV;

    private void Start()
    {
        this.targetCamera = Camera.main;
        this.cameraInitialFOV = this.targetCamera.fieldOfView;
    }

    private void Update()
    {
        // if right mouse click
        this.targetCamera.fieldOfView = Input.GetMouseButton(1) ? this.ZoomedFieldOfView : this.cameraInitialFOV;
    }
}

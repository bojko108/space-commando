using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumDronMode
{
    Patrol,
    Scan,
    Attack
}

public class DroneSignalLight : MonoBehaviour
{
    [Tooltip("Set signal light blink time in seconds")]
    [Range(0.1f, 3f)]
    public float BlinkTime = 1f;
    [Tooltip("Set signal light blink intensity")]
    [Range(0.01f, 1f)]
    public float BlinkIntensity = 0.2f;
    [Tooltip("Set drone signal light color when in partol mode")]
    public Color PatrolColor;
    [Tooltip("Set drone signal light color when in scan mode")]
    public Color ScanColor;
    [Tooltip("Set drone signal light color when in attack mode")]
    public Color AttackColor;

    [HideInInspector]
    public enumDronMode DronMode;

    private Light lightSource;
    private float initialIntensity;

    private void Start()
    {
        this.lightSource = GetComponent<Light>();
        this.initialIntensity = this.lightSource.intensity;

        this.StartCoroutine(this.AnimateLight());
    }

    private IEnumerator AnimateLight()
    {
        while (true)
        {
            switch (this.DronMode)
            {
                case enumDronMode.Scan:
                    this.lightSource.color = this.ScanColor;
                    break;
                case enumDronMode.Attack:
                    this.lightSource.color = this.AttackColor;
                    break;
                default:
                    this.lightSource.color = this.PatrolColor;
                    break;
            }

            yield return new WaitForSeconds(this.BlinkTime);
            this.lightSource.intensity = this.BlinkIntensity;

            yield return new WaitForSeconds(this.BlinkTime);
            this.lightSource.intensity = this.initialIntensity;
        }
    }
}

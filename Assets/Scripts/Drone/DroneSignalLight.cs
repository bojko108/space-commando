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
    public float BlinkTime = 1f;
    public float BlinkIntensity = 0.2f;
    public Color PatrolColor;
    public Color ScanColor;
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

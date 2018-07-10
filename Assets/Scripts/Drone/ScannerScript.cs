using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
public class ScannerTarget
{
    [Tooltip("Target position")]
    public Transform Target;
    [Tooltip("Icon prebab used to display directions")]
    public GameObject IconPrefab;
    [Tooltip("This is the icon offset from the player's position")]
    public Vector3 IconOffset;
}

public class ScannerScript : MonoBehaviour
{
    [Tooltip("Sound played when scan is initiated")]
    public AudioClip ScannerSound;
    [Tooltip("Material used for drawing direction lines")]
    public Material DirectionsMaterial;
    [Tooltip("Scanner increase size for every frame")]
    public Vector3 IncreaseSize;
    [Tooltip("Max scanner size")]
    public Vector3 TargetSize;

    // make this private!
    public ScannerTarget[] Targets;

    [HideInInspector]
    public bool ScanFinished = false;

    private Transform playerTransform;

    private AudioSource scannerAudioSource;

    private Transform scannerTransform;
    private Vector3 initialScale;

    private bool interruptScan = false;

    private List<GameObject> directions;

    private void Awake()
    {
        this.scannerAudioSource = this.GetComponent<AudioSource>();
        this.scannerAudioSource.clip = this.ScannerSound;

        this.scannerTransform = this.transform;
        this.initialScale = this.scannerTransform.localScale;

        this.playerTransform = GameObject.FindGameObjectWithTag(Resources.Tags.Player).transform;
    }

    public void InitiateScan()
    {
        this.directions = new List<GameObject>();

        this.scannerAudioSource.Play();

        this.interruptScan = false;

        this.StartCoroutine(this.Scan());
    }

    public void InteruptScan()
    {
        this.scannerAudioSource.Stop();

        this.scannerTransform.localScale = this.initialScale;

        this.ScanFinished = false;
        this.interruptScan = true;
    }

    private IEnumerator Scan()
    {
        while (this.interruptScan == false)
        {
            this.scannerTransform.localScale += this.IncreaseSize;

            if (this.scannerTransform.localScale.x >= this.TargetSize.x)
            {
                this.interruptScan = true;
                this.ScanFinished = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void AddDirections(ScannerTarget target, Vector3[] corners)
    {
        #region create target icon

        // calculate rotation from player's position to first corner
        Quaternion rotationToTarget = Quaternion.LookRotation(corners[1] - this.playerTransform.position);

        // calculate icon's position = player.position + icon offset
        Vector3 iconPosition = this.playerTransform.position + rotationToTarget * target.IconOffset;

        // create icon game object at calculated location
        GameObject iconGameObject = GameObject.Instantiate(target.IconPrefab);
        iconGameObject.transform.position = iconPosition;
        // rotate towards player
        iconGameObject.transform.LookAt(this.playerTransform.position);

        this.directions.Add(iconGameObject);

        #endregion

        #region create direction line

        GameObject lineGameObject = new GameObject(target.Target.name + " - directions");

        LineRenderer line = lineGameObject.AddComponent<LineRenderer>();
        line.positionCount = corners.Length;
        line.startColor = Color.blue;
        line.endColor = Color.blue;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.material = this.DirectionsMaterial;
        
        //TODO: increase line height above ground
        line.SetPositions(corners);

        this.directions.Add(lineGameObject);

        #endregion
    }

    public void RemoveDirections()
    {
        for (int i = 0; i < this.directions.Count; i++)
        {
            Destroy(this.directions[i]);
        }
    }
}

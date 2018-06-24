using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScannerScript : MonoBehaviour
{
    public AudioClip ScannerSound;
    public Vector3 IncreaseSize;
    public Vector3 TargetSize;

    // make this private!
    public Transform[] Targets;

    [HideInInspector]
    public bool ScanFinished = false;

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

    public void AddDirections(Transform target, Vector3[] corners)
    {
        GameObject lineGameObject = new GameObject(target.name + " - directions");
        LineRenderer line = lineGameObject.AddComponent<LineRenderer>();
        line.positionCount = corners.Length;
        line.startColor = Color.blue;
        line.endColor = Color.blue;
        line.startWidth = 1f;
        line.endWidth = 1f;

        line.SetPositions(corners);

        this.directions.Add(lineGameObject);
    }

    public void RemoveDirections()
    {
        for(int i = 0; i < this.directions.Count; i++)
        {
            Destroy(this.directions[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersSafePointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.Worker))
        {
            if (other.gameObject.GetComponent<EnemyMovement>().IsScared)
                // the worker is in a safe place now :D
                Destroy(other.gameObject);
        }
    }
}

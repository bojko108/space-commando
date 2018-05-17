using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Current;
    [Tooltip("Object prefab to add to the pool")]
    public GameObject PooledPrefab;
    [Tooltip("Number of objects to add to the pool when the game starts")]
    public int PooledAmount = 10;
    [Tooltip("Will the pool grow automatically when there are no objects left in it")]
    public bool WillGrow = true;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < this.PooledAmount; i++)
        {
            this.pooledObjects.Add(this.CreateNewPooledObject());
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < this.pooledObjects.Count; i++)
        {
            if (this.pooledObjects[i].activeInHierarchy == false)
            {
                return this.pooledObjects[i];
            }
        }

        if (this.WillGrow)
        {
            GameObject obj = this.CreateNewPooledObject();
            this.pooledObjects.Add(obj);

            return obj;
        }

        return null;
    }

    private GameObject CreateNewPooledObject()
    {
        GameObject obj = Instantiate(this.PooledPrefab, this.transform) as GameObject;
        obj.SetActive(false);
        return obj;
    }
}

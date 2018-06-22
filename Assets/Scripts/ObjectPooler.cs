using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public enum enumBulletType
{
    Laser,
    Plasma
}

public class ObjectPooler : MonoBehaviour
{
    [HideInInspector]
    public static ObjectPooler Current;
    [Tooltip("Laser prefab to add to the pool")]
    public GameObject LaserPrefab;
    [Tooltip("Plasma prefab to add to the pool")]
    public GameObject PlasmaPrefab;
    [Tooltip("Number of objects to add to the pool when the game starts")]
    public int PooledAmount = 10;
    [Tooltip("Will the pool grow automatically when there are no objects left in it")]
    public bool WillGrow = true;
    
    private List<GameObject> laserObjects;
    private List<GameObject> plasmaObjects;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        laserObjects = new List<GameObject>();
        for (int i = 0; i < this.PooledAmount; i++)
        {
            this.laserObjects.Add(this.CreateNewPooledObject(enumBulletType.Laser));
        }

        plasmaObjects = new List<GameObject>();
        for (int i = 0; i < this.PooledAmount; i++)
        {
            this.plasmaObjects.Add(this.CreateNewPooledObject(enumBulletType.Plasma));
        }
    }

    public GameObject GetPooledObject(enumBulletType type)
    {
        switch (type)
        {
            case enumBulletType.Laser:
                for (int i = 0; i < this.laserObjects.Count; i++)
                {
                    if (this.laserObjects[i].activeInHierarchy == false)
                    {
                        return this.laserObjects[i];
                    }
                }
                break;
            case enumBulletType.Plasma:
                for (int i = 0; i < this.plasmaObjects.Count; i++)
                {
                    if (this.plasmaObjects[i].activeInHierarchy == false)
                    {
                        return this.plasmaObjects[i];
                    }
                }
                break;
        }

        if (this.WillGrow)
        {
            GameObject obj = this.CreateNewPooledObject(type);

            switch (type)
            {
                case enumBulletType.Laser:
                    this.laserObjects.Add(obj);
                    return obj;
                case enumBulletType.Plasma:
                    this.plasmaObjects.Add(obj);
                    return obj;
            }
        }

        return null;
    }

    private GameObject CreateNewPooledObject(enumBulletType type)
    {
        switch (type)
        {
            case enumBulletType.Laser:
                GameObject laser = Instantiate(this.LaserPrefab, this.transform) as GameObject;
                laser.SetActive(false);
                return laser;
            case enumBulletType.Plasma:
                GameObject plasma = Instantiate(this.PlasmaPrefab, this.transform) as GameObject;
                plasma.SetActive(false);
                return plasma;
        }

        return null;
    }
}

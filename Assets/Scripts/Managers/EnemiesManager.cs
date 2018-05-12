using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SaveLoad;

[Serializable]
public class EnemiesManager
{
    [SerializeField]
    public int NumberOfCommanders;
    [SerializeField]
    public int NumberOfSoldiers;
    [SerializeField]
    public int NumberOfWorkers;

    [SerializeField]
    public GameObject CommanderPrefab;
    [SerializeField]
    public GameObject SoldierPrefab;
    [SerializeField]
    public GameObject WorkerPrefab;

    private List<Transform> commanderSpawnPoints;
    private List<Transform> soldiersSpawnPoints;
    private List<Transform> workersSpawnPoints;

    /// <summary>
    /// sets all spawn points available for spawning enemies
    /// </summary>
    /// <param name="commandersSpawnPoints"></param>
    /// <param name="soldiersSpawnPoints"></param>
    /// <param name="workersSpawnPoints"></param>
    public void SetSpawnPoints(Transform[] commandersSpawnPoints, Transform[] soldiersSpawnPoints, Transform[] workersSpawnPoints)
    {
        this.commanderSpawnPoints = new List<Transform>();
        this.commanderSpawnPoints.AddRange(commandersSpawnPoints);
        this.soldiersSpawnPoints = new List<Transform>();
        this.soldiersSpawnPoints.AddRange(soldiersSpawnPoints);
        this.workersSpawnPoints = new List<Transform>();
        this.workersSpawnPoints.AddRange(workersSpawnPoints);
    }

    /// <summary>
    /// spawn all commanders
    /// </summary>
    public void SpawnCommanders()
    {
        for (int i = 0; i < this.NumberOfCommanders; i++)
        {
            this.SpawnCommander();
        }
    }

    /// <summary>
    /// spawn all soldiers
    /// </summary>
    public void SpawnSoldiers()
    {
        for (int i = 0; i < this.NumberOfSoldiers; i++)
        {
            this.SpawnSoldier();
        }
    }

    /// <summary>
    /// spawn all workers
    /// </summary>
    public void SpawnWorkers()
    {
        for (int i = 0; i < this.NumberOfWorkers; i++)
        {
            this.SpawnWorker();
        }
    }

    /// <summary>
    /// spawn a commanders
    /// </summary>
    public void SpawnCommander()
    {
        int index = UnityEngine.Random.Range(0, this.commanderSpawnPoints.Count);
        this.CreateEnemy(this.CommanderPrefab, this.commanderSpawnPoints[index]);
    }

    /// <summary>
    /// spawn a soldier
    /// </summary>
    public void SpawnSoldier()
    {
        int index = UnityEngine.Random.Range(0, this.soldiersSpawnPoints.Count);
        this.CreateEnemy(this.SoldierPrefab, this.soldiersSpawnPoints[index]);
    }

    /// <summary>
    /// spawn a worker
    /// </summary>
    public void SpawnWorker()
    {
        int index = UnityEngine.Random.Range(0, this.workersSpawnPoints.Count);
        this.CreateEnemy(this.WorkerPrefab, this.workersSpawnPoints[index]);
    }

    /// <summary>
    /// creates an enemy
    /// </summary>
    /// <param name="prefab">type of enemy to create</param>
    /// <param name="spawnPoint">parent transform</param>
    /// <returns></returns>
    public GameObject CreateEnemy(GameObject prefab, Transform spawnPoint = null)
    {
        return GameObject.Instantiate(prefab, spawnPoint);
    }

    /// <summary>
    /// restores an enemy after a save
    /// </summary>
    /// <param name="savedEnemy">enemy saved parameters</param>
    /// <returns></returns>
    public GameObject RestoreEnemy(Enemy savedEnemy)
    {
        Transform transform = new GameObject().transform;
        transform.position = savedEnemy.Position.ToVector3();
        transform.rotation = Quaternion.Euler(savedEnemy.Rotation.ToVector3());

        switch (savedEnemy.Tag)
        {
            case Resources.Tags.Commander: return this.CreateEnemy(this.CommanderPrefab, transform);
            case Resources.Tags.Soldier: return this.CreateEnemy(this.SoldierPrefab, transform);
            case Resources.Tags.Worker: return this.CreateEnemy(this.WorkerPrefab, transform);
            default: return null;
        }
    }

    //public GameObject CreateEnemy(string tag, string spawnPointName)
    //{
    //    Transform spawnPoint = this.FindSpawnPoint(tag, spawnPointName);

    //    switch (tag)
    //    {
    //        case Resources.Tags.Commander: return this.CreateEnemy(this.CommanderPrefab, spawnPoint);
    //        case Resources.Tags.Soldier: return this.CreateEnemy(this.SoldierPrefab, spawnPoint);
    //        case Resources.Tags.Worker: return this.CreateEnemy(this.WorkerPrefab, spawnPoint);
    //        default: return null;
    //    }
    //}

    //private Transform FindSpawnPoint(string type, string spawnPointName)
    //{
    //    switch (type)
    //    {
    //        case Resources.Tags.Commander: return this.commanderSpawnPoints.Find(tr => tr.name.Equals(spawnPointName));
    //        case Resources.Tags.Soldier: return this.soldiersSpawnPoints.Find(tr => tr.name.Equals(spawnPointName));
    //        case Resources.Tags.Worker: return this.workersSpawnPoints.Find(tr => tr.name.Equals(spawnPointName));
    //        default: return new GameObject().transform;
    //    }
    //}
}
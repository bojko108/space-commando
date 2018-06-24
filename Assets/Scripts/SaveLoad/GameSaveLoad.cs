using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

namespace Assets.Scripts.SaveLoad
{
    public static class GameSaveLoad
    {
        /// <summary>
        /// set this to TRUE if save must be loaded
        /// </summary>
        public static bool LoadSavedGame = false;
        /// <summary>
        /// set this to TRUE when the player dies
        /// </summary>
        public static bool IsPlayerDead = false;
        
        private static string fileName = String.Format("{0}/{1}", Application.persistentDataPath, Resources.Various.SaveFileName);

        /// <summary>
        /// loads saved game progress
        /// </summary>
        /// <returns></returns>
        public static GameProgress Load()
        {
            GameProgress gameProgress = null;

            if (HasSave())
            {
                // read saved file
                using (FileStream fs = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    gameProgress = bf.Deserialize(fs) as GameProgress;
                }
            }
            else
            {
                // all parameters are set to initial values
                gameProgress = new GameProgress();
            }

            return gameProgress;
        }

        /// <summary>
        /// saves current game progress
        /// </summary>
        /// <param name="gameProgress"></param>
        public static void Save(GameProgress gameProgress)
        {
            Player player = SavePlayer();

            Drone drone = SaveDrone();

            List<Character> enemies = SaveEnemies();
            
            gameProgress.Player = player;

            gameProgress.Drone = drone;

            gameProgress.Enemies = new List<Character>();
            gameProgress.Enemies.AddRange(enemies);

            BinaryFormatter bf = new BinaryFormatter();

            // write save file
            using (FileStream fs = File.Open(fileName, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, gameProgress);
            }
        }

        /// <summary>
        /// deletes the saved file
        /// </summary>
        public static void DeleteSave()
        {
            if (HasSave())
            {
                // ask player???

                File.Delete(fileName);
            }
        }

        /// <summary>
        /// checks if a save file exists
        /// </summary>
        /// <returns></returns>
        public static bool HasSave()
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// saves player data
        /// </summary>
        /// <returns></returns>
        private static Player SavePlayer()
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag(Resources.Tags.Player);

            Player player = new Player();
            player.Tag = playerGO.tag;
            player.HaveOxygen = playerGO.GetComponent<PlayerHealth>().HaveOxygen;
            player.HealthLevel = playerGO.GetComponent<PlayerHealth>().CurrentHealth;
            player.OxygenLevel = playerGO.GetComponent<PlayerHealth>().CurrentOxygen;

            Vector position, rotation;

            ExtractPosition(playerGO.transform, out position, out rotation);

            player.Position = position;
            player.Rotation = rotation;

            return player;
        }

        private static Drone SaveDrone()
        {
            GameObject droneGO = GameObject.FindGameObjectWithTag(Resources.Tags.Drone);

            Drone drone = new Drone();
            drone.Tag = droneGO.tag;
            drone.InAttack = droneGO.GetComponent<Animator>().GetBool("InAttack");
            drone.InScan = droneGO.GetComponent<Animator>().GetBool("InScan");

            Vector position, rotation;

            ExtractPosition(droneGO.transform, out position, out rotation);

            drone.Position = position;
            drone.Rotation = rotation;

            return drone;
        }

        /// <summary>
        /// saves all alive enemies
        /// </summary>
        /// <returns></returns>
        private static List<Character> SaveEnemies()
        {
            List<Character> characters = new List<Character>();

            foreach (GameObject enemyGO in GetEnemies())
            {
                if (enemyGO.GetComponent<EnemyHealth>().IsDead == false)
                {
                    Enemy enemy = new Enemy();
                    enemy.Tag = enemyGO.tag;
                    enemy.HealthLevel = enemyGO.GetComponent<EnemyHealth>().CurrentHealth;
                    enemy.IsChasing = enemyGO.GetComponent<EnemyMovement>().IsChasing;
                    enemy.IsScared = enemyGO.GetComponent<EnemyMovement>().IsScared;
                    enemy.Stop = enemyGO.GetComponent<EnemyMovement>().Stop;
                    
                    Vector position, rotation;

                    ExtractPosition(enemyGO.transform, out position, out rotation);

                    //enemy.SpawnPointName = enemyGO.transform.parent.name;
                    enemy.Position = position;
                    enemy.Rotation = rotation;

                    characters.Add(enemy);
                }
            }

            return characters;
        }

        /// <summary>
        /// gets all enemies by their tags
        /// </summary>
        /// <returns></returns>
        private static List<GameObject> GetEnemies()
        {
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(GameObject.FindGameObjectsWithTag(Resources.Tags.BaseCommander));
            enemies.AddRange(GameObject.FindGameObjectsWithTag(Resources.Tags.Commander));
            enemies.AddRange(GameObject.FindGameObjectsWithTag(Resources.Tags.Soldier));
            enemies.AddRange(GameObject.FindGameObjectsWithTag(Resources.Tags.Worker));
            return enemies;
        }

        /// <summary>
        /// extracts position and rotation from a transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        private static void ExtractPosition(Transform transform, out Vector position, out Vector rotation)
        {
            position = new Vector();
            position.x = transform.position.x;
            position.y = transform.position.y;
            position.z = transform.position.z;
            //position.x = transform.localPosition.x;
            //position.y = transform.localPosition.y;
            //position.z = transform.localPosition.z;

            rotation = new Vector();
            rotation.x = transform.rotation.eulerAngles.x;
            rotation.y = transform.rotation.eulerAngles.y;
            rotation.z = transform.rotation.eulerAngles.z;
            //rotation.x = transform.localRotation.eulerAngles.x;
            //rotation.y = transform.localRotation.eulerAngles.y;
            //rotation.z = transform.localRotation.eulerAngles.z;
        }
    }
}
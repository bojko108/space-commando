using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    /// <summary>
    /// character class - used for saving GameObject's data
    /// </summary>
    [Serializable]
    public class Character
    {
        /// <summary>
        /// GameObject Tag
        /// </summary>
        public string Tag;
        /// <summary>
        /// [DEPRECATED] spawn point name
        /// </summary>
        public string SpawnPointName;
        /// <summary>
        /// GameObject's world Position
        /// </summary>
        public Vector Position;
        /// <summary>
        /// GameObject's world Rotation
        /// </summary>
        public Vector Rotation;
    }

    /// <summary>
    /// Player data
    /// </summary>
    [Serializable]
    public class Player : Character
    {
        /// <summary>
        /// Player health level
        /// </summary>
        public int HealthLevel;
        /// <summary>
        /// Player oxygen level
        /// </summary>
        public int OxygenLevel;
        /// <summary>
        /// Does the player have oxygen
        /// </summary>
        public bool HaveOxygen;
    }

    [Serializable]
    public class Drone : Character
    {
        public bool InScan;
        public bool InAttack;
    }

    /// <summary>
    /// Enemy data
    /// </summary>
    [Serializable]
    public class Enemy : Character
    {
        /// <summary>
        /// Enemy health level
        /// </summary>
        public int HealthLevel;
        /// <summary>
        /// Is the enemy chasing the player
        /// </summary>
        public bool IsChasing;
        /// <summary>
        /// Is the enemy scared
        /// </summary>
        public bool IsScared;
        /// <summary>
        /// Is the enemy moving at all
        /// </summary>
        public bool Stop;
    }

    /// <summary>
    /// Class for storing Vector3 x, y and z components. 
    /// Can be used for both position and rotation
    /// </summary>
    [Serializable]
    public class Vector
    {
        /// <summary>
        /// x component of the Vector3
        /// </summary>
        public float x;
        /// <summary>
        /// y component of the Vector3
        /// </summary>
        public float y;
        /// <summary>
        /// z component of the Vector3
        /// </summary>
        public float z;

        /// <summary>
        /// Convert to Vector3
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
    }
}

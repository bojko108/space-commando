using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    [Serializable]
    public class GameProgress
    {
        private bool playerDead;
        public bool IsPlayerDead
        {
            get { return this.playerDead; }
            set { this.playerDead = value; GameSaveLoad.IsPlayerDead = this.playerDead; }
        }

        public bool IsMainControlRoomFound = false;
        public bool IsPasswordFound = false;
        public bool IsStorageRoomFound = false;
        public bool IsDarkMatterModuleFound = false;
        public bool IsSpaceshipRepaired = false;
        /// <summary>
        /// player saved data
        /// </summary>
        public Character Player;
        /// <summary>
        /// enemies saved data
        /// </summary>
        public List<Character> Enemies;

        public Drone Drone;
    }
}
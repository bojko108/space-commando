using UnityEngine;

public static class Resources
{
    public static class Various
    {
        //public const string SaveFileName = "Saves/saved_game.wtf";
        public const string SaveFileName = "saved_game.wtf";
        public const string PC = "PC";
        public const string DesktopScreen = "DesktopScreen";
        public const string Engine = "Engine";
        public const string MinimapCamera = "Minimap Camera";
        public const string MinimapIcon = "Minimap Icon";
        public static string PlayerDeadText = "You died!";
        public static string FinalText = "You win!";
    }

    public static class Scenes
    {
        public const string GameMenu = "Menu";
        public const string Intro = "Intro";
        public const string Level1 = "Level1";
        public const string Level2 = "Level2";
        public const string Level3 = "Level3";
        public const string Outro = "Outro";
    }

    public static class Messages
    {
        public const string EnterPassword = "Enter password";
        public const string GetItem = "Press F to get Dark Matter Module";
        public const string RepairShip = "Press F to repair Spaceship";
        public const string FindDarkMatterModule = "Find Dark Matter Module to repair Spaceship!";
        public const string RepairingShip = "Repairing ship. Stay close to avoid process interruption!";
        public const string BoardShip = "Press F to get on board";
        public const string KillBaseCommanders = "Base Commanders are blocking the runway!";
    }

    public static class Events
    {
        public const string PlayerEnteredImportantArea = "PlayerEnteredImportantArea";
        public const string PlayerExitedImportantArea = "PlayerExitedImportantArea";
        public const string MainComputerFound = "MainComputerFound";
        public const string StorageRoomFound = "StorageRoomFound";
        public const string DarkMatterModuleFound = "DarkMatterModuleFound";
        public const string SpaceshipFound = "SpaceshipFound";
        public const string PlayerDead = "PlayerDead";
        public const string GameFinish = "GameFinish";

        public const string ResumeGame = "ResumeGame";
        public const string PauseGame = "PauseGame";
        public const string SaveGame = "SaveGame";
        public const string GoToMenu = "GoToMenu";
        public const string QuitGame = "QuitGame";

        public const string Patrol = "Patrol";
        public const string Scan = "Scan";
        public const string Attack = "Attack";
    }

    public static class Tasks
    {
        public const string FindMainControlRoom = "FindMainControlRoom";
        public const string FindPassword = "FindPassword";
        public const string FindStorageRoom = "FindStorageRoom";
        public const string FindDarkMatterModule = "FindDarkMatterModule";
        public const string FindSpaceship = "FindSpaceship";
    }

    public static class Layers
    {
        public const string Buildings = "Buildings";
        public const string Enemies = "Enemies";
        public const string Minimap = "Minimap";
    }

    public static class Tags
    {
        public const string MapProperties = "MapProperties";
        public const string Player = "Player";
        public const string Drone = "Drone";
        public const string MainCamera = "MainCamera";
        public const string MainComputer = "MainComputer";
        public const string StorageRoom = "StorageRoom";
        public const string Building = "Building";
        public const string MinimapIcon = "MinimapIcon";
        public const string Ship = "Ship";
        //public const string BarrelEnd = "BarrelEnd";
        public const string BaseCommander = "BaseCommander";
        public const string Commander = "Commander";
        public const string Soldier = "Soldier";
        public const string Worker = "Worker";
        public const string Tasks = "Tasks";
        public const string Commands = "Commands";
        public const string CommandScan = "CommandScan";
        public const string CommandAttack = "CommandAttack";
        public const string BaseCommanderSpawnPoint = "BaseCommanderSpawnPoint";
        public const string CommanderSpawnPoint = "CommanderSpawnPoint";
        public const string SoldierSpawnPoint = "SoldierSpawnPoint";
        public const string WorkerSpawnPoint = "WorkerSpawnPoint";
        public const string Medpack = "Medpack";
        public const string DarkMatterModule = "DarkMatterModule";
        public const string InfoText = "InfoText";
        public const string PauseMenu = "PauseMenu";
        public const string RepairSlider = "RepairSlider";
    }
}

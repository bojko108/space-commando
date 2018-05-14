using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.SaveLoad;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector]
    public GameProgress ProgressInGame;
    [Tooltip("Login accepted desktop screen")]
    public Sprite LoginAccepted;
    [Tooltip("Set number of enemies")]
    public EnemiesManager ManageEnemies;

    private GameObject pauseMenu;
    private GameObject tasks;
    private GameObject player;
    private BrokenEngine shipEngine;

    // displays important messages to the player
    private GameObject infoText;
    // main ocmputer password
    private KeyCode[] computerPassword = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha0, KeyCode.Alpha8 };
    private int passwordIndex = 0;
    // is thep layer near and important area - pausing the game will not be possible
    private bool playerIsNear = false;
    private bool gameIsPaused = false;
    // tasks will be displayed while this is set to true
    private bool displayTasks = false;
    private Slider repairSlider;

    // game event listeners
    private UnityAction onPlayerDead;
    private UnityAction onGameFinish;
    private UnityAction onPauseGame;
    private UnityAction onResumeGame;
    private UnityAction onGoToMenu;
    private UnityAction onSaveGame;
    private UnityAction onQuitGame;
    private UnityAction onPlayerEnteredImportantArea;
    private UnityAction onPlayerExitedImportantArea;
    private UnityAction onMainComputerFound;
    private UnityAction onStorageRoomFound;
    private UnityAction onSpaceshipFound;
    private UnityAction onDarkMatterModuleFound;

    private void Start()
    {
        this.ProgressInGame.IsPlayerDead = false;

        this.RegisterEvents();

        this.player = GameObject.FindGameObjectWithTag(Resources.Tags.Player);
        this.shipEngine = GameObject.FindGameObjectWithTag(Resources.Tags.Ship).GetComponent<BrokenEngine>();
        this.tasks = GameObject.FindGameObjectWithTag(Resources.Tags.Tasks);
        this.infoText = GameObject.FindGameObjectWithTag(Resources.Tags.InfoText);
        this.pauseMenu = GameObject.FindGameObjectWithTag(Resources.Tags.PauseMenu);
        this.pauseMenu.SetActive(false);
        this.repairSlider = GameObject.FindGameObjectWithTag(Resources.Tags.RepairSlider).GetComponent<Slider>();
        this.repairSlider.gameObject.SetActive(false);

        Transform[] commandersSpawnPoints = Array.ConvertAll(GameObject.FindGameObjectsWithTag(Resources.Tags.CommanderSpawnPoint), item => item.transform);
        Transform[] soldiersSpawnPoints = Array.ConvertAll(GameObject.FindGameObjectsWithTag(Resources.Tags.SoldierSpawnPoint), item => item.transform);
        Transform[] workersSpawnPoints = Array.ConvertAll(GameObject.FindGameObjectsWithTag(Resources.Tags.WorkerSpawnPoint), item => item.transform);
        this.ManageEnemies.SetSpawnPoints(commandersSpawnPoints, soldiersSpawnPoints, workersSpawnPoints);

        // load saved game or start new...
        this.ProgressInGame = (GameSaveLoad.LoadSavedGame ? GameSaveLoad.Load() : new GameProgress());

        this.ReadGameProgress(GameSaveLoad.LoadSavedGame);
    }

    private void Update()
    {
        if (this.displayTasks == false)
        {
            // display tasks while TAB key is pressed
            this.tasks.transform.localScale = Input.GetKey(KeyCode.Tab) ? Vector3.one : Vector3.zero;
        }

        // disable game pause if the player is in important area
        if (this.playerIsNear == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (this.gameIsPaused == true)
                {
                    EventManager.Emit(Resources.Events.ResumeGame);
                }
                else
                {
                    EventManager.Emit(Resources.Events.PauseGame);
                }
            }
        }
    }


    private void RegisterEvents()
    {
        this.onPauseGame = new UnityAction(this.OnPauseGame);
        EventManager.On(Resources.Events.PauseGame, this.onPauseGame);
        this.onResumeGame = new UnityAction(this.OnResumeGame);
        EventManager.On(Resources.Events.ResumeGame, this.onResumeGame);
        this.onGoToMenu = new UnityAction(this.OnGoToMenu);
        EventManager.On(Resources.Events.GoToMenu, this.onGoToMenu);
        this.onSaveGame = new UnityAction(this.OnSaveGame);
        EventManager.On(Resources.Events.SaveGame, this.onSaveGame);
        this.onQuitGame = new UnityAction(this.OnQuitGame);
        EventManager.On(Resources.Events.QuitGame, this.onQuitGame);

        // to set this.playerInNear to TRUE
        this.onPlayerEnteredImportantArea = new UnityAction(this.OnPlayerEnteredImportantArea);
        EventManager.On(Resources.Events.PlayerEnteredImportantArea, this.onPlayerEnteredImportantArea);
        // to set this.playerInNear to FALSE
        // and to hide info text
        this.onPlayerExitedImportantArea = new UnityAction(this.OnPlayerExitedImportantArea);
        EventManager.On(Resources.Events.PlayerExitedImportantArea, this.onPlayerExitedImportantArea);

        this.onMainComputerFound = new UnityAction(this.OnMainComputerFound);
        EventManager.On(Resources.Events.MainComputerFound, this.onMainComputerFound);

        this.onStorageRoomFound = new UnityAction(this.OnStorageRoomFound);
        EventManager.On(Resources.Events.StorageRoomFound, this.onStorageRoomFound);

        this.onDarkMatterModuleFound = new UnityAction(this.OnDarkMatterModuleFound);
        EventManager.On(Resources.Events.DarkMatterModuleFound, this.onDarkMatterModuleFound);

        this.onSpaceshipFound = new UnityAction(this.OnSpaceshipFound);
        EventManager.On(Resources.Events.SpaceshipFound, this.onSpaceshipFound);

        this.onPlayerDead = new UnityAction(this.OnPlayerDead);
        EventManager.On(Resources.Events.PlayerDead, this.onPlayerDead);

        this.onGameFinish = new UnityAction(this.OnGameFinish);
        EventManager.On(Resources.Events.GameFinish, this.onGameFinish);
    }


    /// <summary>
    /// executed if the player dies
    /// </summary>
    private void OnPlayerDead()
    {
        this.ProgressInGame.IsPlayerDead = true;
        //Debug.Log("GAME OVER");
        EventManager.Emit(Resources.Events.GameFinish);
    }

    /// <summary>
    /// executed if the player finishes the game or dies
    /// </summary>
    private void OnGameFinish()
    {
        //Debug.Log("END GAME");

        Time.timeScale = 1f;
        SceneManager.LoadScene(Resources.Scenes.Outro);
    }

    /// <summary>
    /// executed when the game is resumed
    /// </summary>
    private void OnResumeGame()
    {
        this.gameIsPaused = false;
        this.pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // enable player movement and shooting logic
        this.player.GetComponent<FirstPersonController>().enabled = true;
        this.player.GetComponentInChildren<PlayerShooting>().enabled = true;
    }

    /// <summary>
    /// executed when the game is paused
    /// </summary>
    private void OnPauseGame()
    {
        this.gameIsPaused = true;
        this.pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // disable player movement and shooting logic
        this.player.GetComponent<FirstPersonController>().enabled = false;
        this.player.GetComponentInChildren<PlayerShooting>().enabled = false;
    }

    private void OnGoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Resources.Scenes.GameMenu);
    }

    /// <summary>
    /// executed when the game is saved
    /// </summary>
    private void OnSaveGame()
    {
        // save current game progress
        GameSaveLoad.Save(this.ProgressInGame);
        // go to main menu
        EventManager.Emit(Resources.Events.GoToMenu);
    }

    private void OnQuitGame()
    {
        // ask player???

        Application.Quit();
    }

    /// <summary>
    /// used to disable pause menu when the player is in some important areas
    /// </summary>
    private void OnPlayerEnteredImportantArea()
    {
        this.playerIsNear = true;
    }

    /// <summary>
    /// used to enable pause menu
    /// </summary>
    private void OnPlayerExitedImportantArea()
    {
        this.playerIsNear = false;
        this.HideInfoText();
    }

    /// <summary>
    /// executed when the player is near the main computer
    /// </summary>
    private void OnMainComputerFound()
    {
        if (this.ProgressInGame.IsMainControlRoomFound == false)
        {
            this.ProgressInGame.IsMainControlRoomFound = true;
            this.FinishTask(Resources.Tasks.FindMainControlRoom);
        }

        if (this.ProgressInGame.IsPasswordFound == false)
        {
            this.DisplayInfoText(Resources.Messages.EnterPassword);
            // wait for user to enter the password
            StartCoroutine(this.WaitForPasswordInput());
        }
        else if (this.ProgressInGame.IsPasswordFound == true)
        {
            EventManager.Off(Resources.Events.MainComputerFound, this.onMainComputerFound);
        }
    }

    /// <summary>
    /// executed when the player is near the storage room
    /// </summary>
    private void OnStorageRoomFound()
    {
        this.ProgressInGame.IsStorageRoomFound = true;

        this.FinishTask(Resources.Tasks.FindStorageRoom);

        EventManager.Off(Resources.Events.StorageRoomFound, this.onStorageRoomFound);
    }

    /// <summary>
    /// executed when the player is near the dark matter module
    /// </summary>
    private void OnDarkMatterModuleFound()
    {
        if (this.ProgressInGame.IsDarkMatterModuleFound == false)
        {
            this.DisplayInfoText(Resources.Messages.GetItem);

            // wait for user to get the dark matter module
            StartCoroutine(this.WaitForUserToGetTheItem());
        }
    }

    /// <summary>
    /// executed when the player is near the spaceship
    /// </summary>
    private void OnSpaceshipFound()
    {
        if (this.ProgressInGame.IsSpaceshipRepaired == false)
        {
            if (this.ProgressInGame.IsDarkMatterModuleFound)
            {
                this.DisplayInfoText(Resources.Messages.RepairShip);

                // wait for player to repair the ship
                StartCoroutine(this.WaitForUserToRepairTheShip());
            }
            else
            {
                this.DisplayInfoText(Resources.Messages.FindDarkMatterModule);
            }
        }
    }

    /// <summary>
    /// executed when the player has the main computer password
    /// </summary>
    private void PlayerHavePassword()
    {
        this.ProgressInGame.IsPasswordFound = true;
        this.FinishTask(Resources.Tasks.FindPassword);
        this.player.GetComponent<PlayerHealth>().PlayerHaveOxygen();

        // play login sound
        GameObject.FindGameObjectWithTag(Resources.Tags.MainComputer).FindChildrenByName(Resources.Various.PC)[0].GetComponent<AudioSource>().Play();
        // change desktop screen
        GameObject.FindGameObjectWithTag(Resources.Tags.MainComputer).FindChildrenByName(Resources.Various.DesktopScreen)[0].GetComponent<SpriteRenderer>().sprite = this.LoginAccepted;

        // add storage room and buildings layer to the radar
        this.player.GetComponent<RadarScript>().AddTarget(GameObject.FindGameObjectWithTag(Resources.Tags.StorageRoom));
        this.player.GetComponent<RadarScript>().AddLayer(Resources.Layers.Buildings);

        // set all soldiers in chasing mode after 20 seconds
        StartCoroutine(this.SetEnemiesInAttackMode(Resources.Tags.Soldier, 20f));
    }

    /// <summary>
    /// executed when the player has the dark matter module
    /// </summary>
    private void PlayerHaveDarkMatterModule()
    {
        GameObject.FindGameObjectWithTag(Resources.Tags.DarkMatterModule).SetActive(false);
        this.ProgressInGame.IsDarkMatterModuleFound = true;
        this.FinishTask(Resources.Tasks.FindDarkMatterModule);

        // set all commanders in chasing mode
        StartCoroutine(this.SetEnemiesInAttackMode(Resources.Tags.Commander, 0f));
    }

    /// <summary>
    /// reads game progress
    /// </summary>
    /// <param name="savedGame">if null the game starts from the beginning</param>
    private void ReadGameProgress(bool savedGame)
    {
        if (this.ProgressInGame.IsPlayerDead)
        {
            EventManager.Emit(Resources.Events.PlayerDead);
        }
        else
        {
            // add main control room and spaceship to the radar
            this.player.GetComponent<RadarScript>().AddTarget(GameObject.FindGameObjectWithTag(Resources.Tags.MainComputer));
            this.player.GetComponent<RadarScript>().AddTarget(GameObject.FindGameObjectWithTag(Resources.Tags.Ship));
            this.player.GetComponent<RadarScript>().AddLayer(Resources.Layers.Minimap);

            #region SET PLAYER PROPERTIES

            if (savedGame)
            {
                Player savedPlayer = this.ProgressInGame.Player as Player;
                this.player.transform.position = savedPlayer.Position.ToVector3();
                this.player.transform.rotation = Quaternion.Euler(savedPlayer.Rotation.ToVector3());

                // update initial mouse look parameters with the saved ones
                this.player.GetComponent<FirstPersonController>().InitMouseLook(this.player.transform);

                this.player.GetComponent<PlayerHealth>().SetLevels(savedPlayer.HealthLevel, savedPlayer.OxygenLevel);

                // if the player has found hte main computer password
                this.player.GetComponent<PlayerHealth>().HaveOxygen = savedPlayer.HaveOxygen;
            }
            // else use starting properties

            #endregion

            #region CHECK COMLETED TASKS

            if (this.ProgressInGame.IsMainControlRoomFound)
            {
                this.FinishTask(Resources.Tasks.FindMainControlRoom);

                if (this.ProgressInGame.IsPasswordFound)
                {
                    EventManager.Emit(Resources.Events.MainComputerFound);
                    this.FinishTask(Resources.Tasks.FindPassword);
                    this.PlayerHavePassword();
                }
            }

            if (this.ProgressInGame.IsStorageRoomFound)
            {
                EventManager.Emit(Resources.Events.StorageRoomFound);
            }

            if (this.ProgressInGame.IsDarkMatterModuleFound)
            {
                EventManager.Emit(Resources.Events.DarkMatterModuleFound);
                this.PlayerHaveDarkMatterModule();
            }

            if (this.ProgressInGame.IsSpaceshipRepaired)
            {
                this.shipEngine.Repaired();
                EventManager.Emit(Resources.Events.GameFinish);
            }

            #endregion

            #region SPAWN ENEMIES

            if (savedGame)
            {
                foreach (Enemy enemy in this.ProgressInGame.Enemies)
                {
                    //GameObject enemyGO = this.ManageEnemies.CreateEnemy(enemy.Tag, enemy.SpawnPointName);
                    //enemyGO.transform.position = enemy.Position.ToVector3();
                    //enemyGO.transform.rotation = Quaternion.Euler(enemy.Rotation.ToVector3());
                    //enemyGO.transform.localPosition = enemy.Position.ToVector3();
                    //enemyGO.transform.localRotation = Quaternion.Euler(enemy.Rotation.ToVector3());

                    GameObject enemyGO = this.ManageEnemies.RestoreEnemy(enemy);

                    enemyGO.GetComponent<EnemyHealth>().SetHealth(enemy.HealthLevel);
                    enemyGO.GetComponent<EnemyMovement>().IsChasing = enemy.IsChasing;
                    enemyGO.GetComponent<EnemyMovement>().IsScared = enemy.IsScared;
                    enemyGO.GetComponent<EnemyMovement>().Stop = enemy.Stop;
                }
            }
            else
            {
                this.ManageEnemies.SpawnWorkers();
                this.ManageEnemies.SpawnSoldiers();
                this.ManageEnemies.SpawnCommanders();
            }

            #endregion
        }
    }


    /// <summary>
    /// shows info text: when the player is in important area
    /// </summary>
    /// <param name="text">text to display</param>
    private void DisplayInfoText(string text)
    {
        this.infoText.transform.localScale = Vector3.one;
        this.infoText.GetComponent<Text>().text = text;
    }

    /// <summary>
    /// hides info text: when the player leaves an important area
    /// </summary>
    private void HideInfoText()
    {
        this.infoText.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// marks a task as finished
    /// </summary>
    /// <param name="taskName">task to mark as finished</param>
    private void FinishTask(string taskName)
    {
        GameObject toggle = this.tasks.FindChildrenByName(taskName)[0];
        toggle.GetComponent<UnityEngine.UI.Toggle>().isOn = true;

        // display tasks for 3 seconds
        StartCoroutine(this.DisplayTasksFor(3f));
    }

    /// <summary>
    /// sets enemies in chasing mode
    /// </summary>
    /// <param name="tag">type of enemies to update</param>
    /// <param name="waitFor">time to wait before updat in seconds</param>
    /// <returns></returns>
    private IEnumerator SetEnemiesInAttackMode(string tag, float waitFor)
    {
        yield return new WaitForSeconds(waitFor);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(tag))
        {
            enemy.GetComponent<EnemyMovement>().IsChasing = true;
        }
    }

    /// <summary>
    /// shows tasks 
    /// </summary>
    /// <param name="seconds">time to show the tasks in seconds</param>
    /// <returns></returns>
    private IEnumerator DisplayTasksFor(float seconds)
    {
        this.displayTasks = true;
        this.tasks.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(seconds);

        this.displayTasks = false;
        this.tasks.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// waits for the player to enter the password
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForPasswordInput()
    {
        // while the player is near the computer
        while (this.playerIsNear)
        {
            if (Input.GetKeyDown(this.computerPassword[this.passwordIndex]))
            {
                this.passwordIndex++;

                if (this.passwordIndex == this.computerPassword.Length)
                {
                    this.PlayerHavePassword();

                    break;
                }
            }
            // reset the password index
            else if (Input.anyKeyDown)
            {
                this.passwordIndex = 0;

                this.DisplayInfoText(Resources.Messages.EnterPassword);
            }

            yield return null;
        }

        this.HideInfoText();
    }

    /// <summary>
    /// waits for the player to get the part
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForUserToGetTheItem()
    {
        // while the player is near the dark matter module
        while (this.playerIsNear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.PlayerHaveDarkMatterModule();

                break;
            }

            yield return null;
        }

        this.HideInfoText();
    }

    /// <summary>
    /// waits for the player to start repairing the spaceship
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForUserToRepairTheShip()
    {
        // while the player is near the spaceship
        while (this.playerIsNear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.DisplayInfoText(Resources.Messages.RepairingShip);

                yield return StartCoroutine(this.RepairingShip());

                break;
            }

            yield return null;
        }

        this.HideInfoText();
    }

    /// <summary>
    /// wait to repair spaceship
    /// </summary>
    private IEnumerator RepairingShip()
    {
        // set to repaired: play engine sound....
        this.shipEngine.Repaired();
        
        this.repairSlider.gameObject.SetActive(true);
        
        // while the player is near the spaceship
        while (this.playerIsNear)
        {
            if (this.repairSlider.value < this.repairSlider.maxValue)
            {
                this.repairSlider.value += 1;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                this.repairSlider.gameObject.SetActive(false);

                EventManager.Emit(Resources.Events.GameFinish);

                break;
            }
        }

        this.shipEngine.NotRepaired();
        
        this.repairSlider.value = 0;
        this.repairSlider.gameObject.SetActive(false);
    }
}

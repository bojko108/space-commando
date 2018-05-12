using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.SaveLoad;

public class MainMenu : MonoBehaviour
{
    public Button LoadSaveButton;
    public Button DeleteSaveButton;

    private void Start()
    {
        this.SetupButtonsForLoadingSaves();
    }

    public void GoToMenu()
    {
        GameSaveLoad.LoadSavedGame = false;
        this.LoadLevel(Resources.Scenes.GameMenu);
    }

    public void StartNewGame()
    {
        this.LoadLevel(Resources.Scenes.Intro);
    }

    public void StartGame()
    {
        GameSaveLoad.LoadSavedGame = false;
        this.LoadLevel(Resources.Scenes.Level3);
    }

    public void LoadSavedGame()
    {
        GameSaveLoad.LoadSavedGame = true;
        this.LoadLevel(Resources.Scenes.Level3);
    }

    public void DeleteSavedGame()
    {
        GameSaveLoad.DeleteSave();
        this.SetupButtonsForLoadingSaves();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    private void SetupButtonsForLoadingSaves()
    {
        // enable buttons if there is a save
        if (this.LoadSaveButton != null)
        {
            this.LoadSaveButton.GetComponent<Button>().interactable = GameSaveLoad.HasSave();
        }

        if (this.DeleteSaveButton)
        {
            this.DeleteSaveButton.GetComponent<Button>().interactable = GameSaveLoad.HasSave();
        }
    }
}

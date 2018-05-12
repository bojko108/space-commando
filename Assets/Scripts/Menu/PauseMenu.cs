using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public void Resume()
    {
        EventManager.Emit(Resources.Events.ResumeGame);
    }

    public void GoToMenu()
    {
        EventManager.Emit(Resources.Events.GoToMenu);
    }

    public void SaveGame()
    {
        EventManager.Emit(Resources.Events.SaveGame);
    }

    public void QuitGame()
    {
        EventManager.Emit(Resources.Events.QuitGame);
    }
}

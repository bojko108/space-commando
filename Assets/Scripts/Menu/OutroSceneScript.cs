using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SaveLoad;

public class OutroSceneScript : MonoBehaviour
{
    public RawImage Image;
    public Text FinalText;
    public Button LoadSaveButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameSaveLoad.IsPlayerDead)
        {
            this.Image.color = Color.black;
            this.FinalText.color = Color.white;
            this.FinalText.text = Resources.Various.PlayerDeadText;
        }
        else
        {
            this.Image.color = Color.white;
            this.FinalText.color = Color.black;
            this.FinalText.text = Resources.Various.FinalText;
        }

        this.gameObject.FindChildrenByName("EndGameMenu")[0].SetActive(GameSaveLoad.IsPlayerDead == false);
        this.gameObject.FindChildrenByName("PlayerDeadMenu")[0].SetActive(GameSaveLoad.IsPlayerDead);

        if (this.LoadSaveButton)
        {
            this.LoadSaveButton.gameObject.SetActive(GameSaveLoad.IsPlayerDead == true);
            this.LoadSaveButton.GetComponent<Button>().interactable = GameSaveLoad.HasSave();
        }
    }
}


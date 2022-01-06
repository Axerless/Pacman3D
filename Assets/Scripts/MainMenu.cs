using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public AudioMixer enemiesAudioMixer;
    public Texture2D cursorTexture;
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;
    public GameObject levelSelectButton;

    void Start()
    {
       Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SelectLevelButton()
    {
        mainMenuPanel.SetActive(false);
        levelSelectButton.SetActive(true);
    }
    public void OptionsButton()
    {
        if(!optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetEnemiesVolume(float volume)
    {
        enemiesAudioMixer.SetFloat("enemyVolume", volume);
    }

    public void BackButton()
    {
        levelSelectButton.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Animator cameraAnimator;
    public Text levelEndText;
    public GameObject optionsMenuUI;
    public GameObject pauseMenuUI;
    public GameObject levelEndUI;
    private PlayerMovement playerMovement;
    private EnemySpawner enemySpawner;
    private Volume volume;
    private bool gameIsPaused = false;

    public void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        volume = FindObjectOfType<Volume>();
        Resume();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           if(!pauseMenuUI.activeInHierarchy)
           {
               Pause();
           }
           else
           {
               Resume();
           }
        }
    }
    public void ReturnToMenuButton()
    {
        AudioManager.Instance.StopPlaying("GhostEnemy");
        SceneManager.LoadScene(0);
    }

    public void LevelCompletePanel(bool hasPlayerWon)
    {
        if(hasPlayerWon)
        {
            levelEndText.text = "You Won";
            levelEndText.color = new Color32(145, 255, 35, 255);
        }
        else
        {
            levelEndText.text = "Game Over";
            levelEndText.color = new Color32(255, 35, 50, 255);
        }

        cameraAnimator.Play("DeathCamera");
        enemySpawner.StopEnemies();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = true;
        }
        levelEndUI.SetActive(true);
    }
    public void Pause()
    {
        if(!playerMovement.isDead)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            if(volume.profile.TryGet(out DepthOfField depthOfField))
            {
                depthOfField.active = true;
            }
            gameIsPaused = true;
        }
    }
    public void Resume()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        if(volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = false;
        }
        gameIsPaused = false;
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{   
    public Animator animator;
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public Text levelDescriptionText;
    [TextArea(1, 4)]public string[] levelDescription;
    [TextArea(1, 4)]public string[] ghostDescription;


    void Start()
    {
        animator.SetTrigger("Play");
    }
    public void SetGhostDescription(int ghostType)
    {
        levelDescriptionText.text = ghostDescription[ghostType];
        levelDescriptionText.gameObject.SetActive(true);
    }
    public void SetDescription(int levelDifficulty)
    {
        levelDescriptionText.text = levelDescription[levelDifficulty];
        levelDescriptionText.gameObject.SetActive(true);
    }
    public void SetOffDescription()
    {
        levelDescriptionText.gameObject.SetActive(false);
    }
    public void EasyButton()
    {
        StartCoroutine(LoadAsynchronusly(1));
    }
    public void MediumButton()
    {
        StartCoroutine(LoadAsynchronusly(2));
    }
    public void HardButton()
    {
        StartCoroutine(LoadAsynchronusly(3));
    }
    public void MapCreatorButton()
    {
        StartCoroutine(LoadAsynchronusly(4));
    }

    IEnumerator LoadAsynchronusly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            float roundedProgress = Mathf.Round(progress);
            slider.value = roundedProgress*100;
            progressText.text = (roundedProgress * 100).ToString() + "%";
            yield return null;
        }
    }
    
}

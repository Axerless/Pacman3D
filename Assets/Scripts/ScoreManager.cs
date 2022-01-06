using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject("ScoreManager");
                gameObject.AddComponent<ScoreManager>();
            }
            return instance;
        }
    }

    public PauseMenu pauseMenu;
    public Text currentDotAmountText;
    public Text maxDotAmountText;
    private List<GameObject> dotList = new List<GameObject>();
    private int dotsCollected;

    void Awake()
    {
        instance = this;
    }

    //Adding every dot on map to list.
    public void SetDotList(GameObject dot)
    {
        dotList.Add(dot);
        maxDotAmountText.text = dotList.Count.ToString();
    }

    //Checking if all dots are collected if so the player wins.
    public void SetScore()
    {
        dotsCollected++;
        currentDotAmountText.text = dotsCollected.ToString()+" /";

        if(dotsCollected >= dotList.Count)
        {
           pauseMenu.LevelCompletePanel(true);
        }
    }
}

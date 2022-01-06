using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public bool ifGhostTypeIcon;
    public Sprite ghostIconSprite;
    [SerializeField] private int levelDifficulty;
    private LevelSelection levelSelection;
    private Image ghostImage;
    private Sprite spriteTemp;


    public void Awake()
    {
        levelSelection = FindObjectOfType<LevelSelection>();
        if(ifGhostTypeIcon)
        {
            ghostImage = GetComponent<Image>();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ifGhostTypeIcon)
        {
            spriteTemp = ghostImage.sprite;
            ghostImage.sprite = ghostIconSprite;
            levelSelection.SetGhostDescription(levelDifficulty);
        }
        else
        {
            levelSelection.SetDescription(levelDifficulty);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(ifGhostTypeIcon)
        {
           ghostImage.sprite = spriteTemp;
        }
        levelSelection.SetOffDescription();
    }
}

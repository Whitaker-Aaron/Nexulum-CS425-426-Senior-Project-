using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial", fileName = "newTutorialObject")]
public class TutorialObject : ScriptableObject
{
    [SerializeField] public List<string> tutorialDialogueList = new List<string>();
    [SerializeField] public List<Texture2D> tutorialDialogueImages = new List<Texture2D>();
    [SerializeField] public List<pageType> tutorialPageTypes = new List<pageType>();
    [SerializeField] public List<string> tutorialAbilityName = new List<string>();
    [SerializeField] public List<Sprite> tutorialAbilitySpriteKeyboard = new List<Sprite>();
    [SerializeField] public List<Sprite> tutorialAbilitySpriteGamepad = new List<Sprite>();
    [SerializeField] public string tutorialName = "";

    public enum pageType
    {
        first,
        middle,
        last,
        only
    }
}

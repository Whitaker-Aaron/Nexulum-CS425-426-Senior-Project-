using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KnightSkillMenu : MonoBehaviour
{
    SkillTreeManager skillTreeManager;
    classAbilties abilities;
    [SerializeField] GameObject backButton;

    // Start is called before the first frame update
    private void Awake()
    {
        abilities = GameObject.Find("InputandAnimationManager").GetComponent<classAbilties>();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void bubbleRad1()
    {
        abilities.modifyBubbleRad(5f);
    }
}

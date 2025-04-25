using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KnightSkillMenu : MonoBehaviour
{
    SkillTreeManager skillTreeManager;
    classAbilties abilities;
    [SerializeField] GameObject backButton;
    [SerializeField] TMP_Text classLvl;
    [SerializeField] TMP_Text classSp;
    [SerializeField] List<skillTreePanel> panels;


    // Start is called before the first frame update
    private void Awake()
    {
        abilities = GameObject.Find("InputandAnimationManager").GetComponent<classAbilties>();
        setLvlSp();
    }

    public void setLvlSp()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        var curSp = character.knightObject.numSkillPoints;
        var curLvl = character.knightObject.currentLvl;
        classLvl.text = curLvl.ToString();
        classSp.text = curSp.ToString();
    }

    public void updatePanels()
    {
        foreach (var panel in panels)
        {
            panel.setColors();
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void bubbleRad1()
    {
        abilities.modifyBubbleRad(0.5f);
    }

    public void increaseSwordShotSpeed1()
    {
        abilities.increaseSwordShotSpeed(1f);
    }

    public void increaseSwordShotDamage1()
    {
        abilities.increaseSwordShotDamage(5);
    }

    public void combatRad1()
    {
        abilities.modifyCombatAuraRad(1f);
    }

    public void bubbleTimeIncrease1()
    {
        abilities.modifyBubbleDuration(0.5f);
    }



    public void resetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        updatePanels();
        setLvlSp();
    }
}

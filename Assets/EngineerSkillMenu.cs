using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EngineerSkillMenu : MonoBehaviour
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

    public void updatePanels()
    {
        foreach (var panel in panels)
        {
            panel.setColors();
        }
    }

    public void setLvlSp()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        var curSp = character.engineerObject.numSkillPoints;
        var curLvl = character.engineerObject.currentLvl;
        classLvl.text = curLvl.ToString();
        classSp.text = curSp.ToString();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void increaseTurrDamage1()
    {
        abilities.increaseTurretDamage(5);
    }

    public void increaseTurretRange1()
    {
        abilities.increaseTurretRange(5);
    }

    public void increaseTurretFireRate1()
    {
        abilities.increaseTurretFireRate(1);
    }

    public void increaseTeslaDamage1()
    {
        abilities.increaseTeslaDamage(5);
    }

    public void increaseCloneDuration1()
    {
        abilities.increaseCloneDuration(1f);
    }

    public void resetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        updatePanels();
        setLvlSp();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GunnerSkillsMenu : MonoBehaviour
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
        var curSp = character.gunnerObject.numSkillPoints;
        var curLvl = character.gunnerObject.currentLvl;
        classLvl.text = curLvl.ToString();
        classSp.text = curSp.ToString();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void rocketRad1()
    {
        abilities.modifyRocketRad(0.5f);
    }

    public void grenadeDmg1()
    {
        abilities.increaseGrenadeDamage(5);
    }

    public void laserDamage1()
    {
        abilities.increaseLaserDamage(5);
    }

    public void resetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        updatePanels();
        setLvlSp();
    }
}

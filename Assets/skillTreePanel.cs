using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class skillTreePanel : MonoBehaviour
{
    [SerializeField] TMP_Text classLvlText;
    [SerializeField] TMP_Text spAmount;
    [SerializeField] GameObject disabledPanel;
    [SerializeField] GameObject unlockButton;
    [SerializeField] Text unlockText;

    public WeaponBase.weaponClassTypes classType;
    public int reqSp = 0;
    public int reqLvl = 0;
    public bool unlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        setColors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setColors()
    {
        if (unlocked) return;
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        if (classType == null) return;
        float curSp = 0;
        int curLvl = 0;
        Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
        Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        

        switch (classType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                curSp = character.knightObject.numSkillPoints;
                curLvl = character.knightObject.currentLvl;
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                curSp = character.gunnerObject.numSkillPoints;
                curLvl = character.gunnerObject.currentLvl;
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                curSp = character.engineerObject.numSkillPoints;
                curLvl = character.engineerObject.currentLvl;
                break;
        }
        if(curSp < reqSp || curLvl < reqLvl)
        {
            if (!unlocked)
            {
                if (curSp < reqSp) spAmount.color = red;
                else spAmount.color = white;

                if (curLvl < reqLvl) classLvlText.color = red;
                else classLvlText.color = white;
            }
            

            disabledPanel.SetActive(true);
            unlockButton.GetComponent<Button>().interactable = false;

        }
        else
        {
            disabledPanel.SetActive(false);
            unlockButton.GetComponent<Button>().interactable = true;
            spAmount.color = white;
            classLvlText.color = white;
        }

    }

    public void onAbilityUnlock()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        disabledPanel.SetActive(true);
        unlockButton.GetComponent<Button>().interactable = false;
        unlockText.text = "Unlocked";
        unlocked = true;
        switch (classType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                character.knightObject.numSkillPoints -= reqSp;
                GameObject.Find("KnightSkillsMenu").GetComponent<KnightSkillMenu>().resetSelection();
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                character.gunnerObject.numSkillPoints -= reqSp;
                GameObject.Find("GunnerSkillsMenu").GetComponent<GunnerSkillsMenu>().resetSelection();
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                character.engineerObject.numSkillPoints -= reqSp;
                GameObject.Find("EngineerSkillsMenu").GetComponent<EngineerSkillMenu>().resetSelection();
                break;
        }
        


    }
}

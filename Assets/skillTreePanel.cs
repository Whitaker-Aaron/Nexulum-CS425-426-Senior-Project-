using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEditor.U2D.Sprites;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class skillTreePanel : MonoBehaviour
{
    [SerializeField] TMP_Text classLvlText;
    [SerializeField] TMP_Text spAmount;
    [SerializeField] GameObject disabledPanel;
    [SerializeField] GameObject unlockButton;

    public WeaponBase.weaponClassTypes classType;
    public int reqSp = 0;
    public int reqLvl = 0;
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
            if (curSp < reqSp) spAmount.color = red;
            else spAmount.color = white;

            if (curLvl < reqLvl) classLvlText.color = red;
            else classLvlText.color = white;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Data;
//using static UnityEditor.Progress;

public class EquipMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject equipOptionPrefab;
    [SerializeField] GameObject equippedRunePrefab;
    [SerializeField] GameObject knightEquipButton;

    [SerializeField] GameObject weaponsScrollBar;
    [SerializeField] GameObject runesScrollBar;
    [SerializeField] GameObject itemsScrollBar;

    [SerializeField] GameObject backButton;
    [SerializeField] GameObject backButton2;
    [SerializeField] GameObject backButton3;
    [SerializeField] GameObject backButton4;

    [SerializeField] GameObject disabledPanel;
    


    List<GameObject> currentEquipmentObjects = new List<GameObject>();
    List<GameObject> currentScrollObjects = new List<GameObject>();

    GameObject mainButtons;
    GameObject mainSelection;
    GameObject weaponsScroll;
    GameObject runesScroll;
    GameObject runeSwapScroll;
    GameObject classScroll;
    GameObject weaponsScrollContent;
    GameObject classScrollContent;
    GameObject runesScrollContent;
    GameObject[] swapOptions = new GameObject[3];
    GameObject runeToSwapIn;

    GameObject equippedContainer;
    GameObject equippedPanel;

    GameObject currentWeaponContent;
    GameObject currentClassContent;
    GameObject currentRuneContainer;

    GameObject classChangeContainer;

    GameObject classChangeButton;

    GameObject equippedBackdrop;

    // Start is called before the first frame update
    void Start()
    {
        mainButtons = GameObject.Find("MainButtons");
        mainSelection = GameObject.Find("MainSelection");
        weaponsScroll = GameObject.Find("WeaponsScroll");
        runesScroll = GameObject.Find("RunesScroll");
        runeSwapScroll = GameObject.Find("RuneSwapScroll");
        swapOptions[0] = GameObject.Find("SwapOption1");
        swapOptions[1] = GameObject.Find("SwapOption2");
        swapOptions[2] = GameObject.Find("SwapOption3");
        runeToSwapIn = GameObject.Find("RuneToSwapIn");
        runeSwapScroll.SetActive(false);
        classScroll = GameObject.Find("ClassScroll");

        



        weaponsScrollContent = GameObject.Find("WeaponsScrollContent");
        classScrollContent = GameObject.Find("ClassScrollContent");
        runesScrollContent = GameObject.Find("RunesScrollContent");

        equippedContainer = GameObject.Find("EquippedScroll");
        equippedPanel = GameObject.Find("EquippedPanel");

        classChangeContainer = GameObject.Find("ClassScrollContent");
        classChangeContainer.SetActive(false);

        currentWeaponContent = GameObject.Find("CurrentWeaponPlaceholder");
        currentClassContent = GameObject.Find("CurrentClassPlaceholder");
        currentRuneContainer = GameObject.Find("CurrentRunesList");

        equippedBackdrop = GameObject.Find("EquippedBackdrop");

        
        FillEquipment();
        
        

        weaponsScroll.SetActive(false);
        runesScroll.SetActive(false);
        classScroll.SetActive(false);


        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);


    }

    private void Awake()
    {
        classChangeButton = GameObject.Find("ClassButton");
        if (GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().currentScene != "BaseCamp")
        {
            //classChangeButton.GetComponent<Button>().interactable = false;
            classChangeButton.SetActive(false);
            disabledPanel.SetActive(true);
        }
        else
        {
            classChangeButton.SetActive(true);
            disabledPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetWeaponCraftSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(weaponsScrollBar);
        //EventSystem.current.SetSelectedGameObject(currentWeaponScrollObjects[currentWeaponScrollObjects.Count - 1].GetComponent<CraftRecipePrefab>().craftButton);
    }

    public void ResetRuneCraftSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(runesScrollBar);
        //EventSystem.current.SetSelectedGameObject(currentRuneScrollObjects[currentRuneScrollObjects.Count - 1].GetComponent<CraftRecipePrefab>().craftButton);
    }

    public void NavigateToMaterialMenu()
    {
        Debug.Log("Back Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToMaterialMenu();
    }

    public void NavigateToWeaponEquipMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        weaponsScroll.SetActive(true);

        equippedPanel.SetActive(false);
        equippedContainer.SetActive(false);
        equippedBackdrop.SetActive(false);

        backButton.SetActive(false);
        backButton3.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton3);


        populateWeaponsScroll();
        
    }

    public void NavigateToClassEquipMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        classScroll.SetActive(true);

        equippedPanel.SetActive(false);
        equippedContainer.SetActive(false);
        equippedBackdrop.SetActive(false);

        backButton.SetActive(false);
        backButton4.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton4);

        populateClassScroll();
    }

    public void NavigateToRuneEquipMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        runesScroll.SetActive(true);

        equippedPanel.SetActive(false);
        equippedContainer.SetActive(false);
        equippedBackdrop.SetActive(false);

        backButton.SetActive(false);
        backButton4.SetActive(false);
        backButton3.SetActive(false);
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);

        populateRunesScroll();
    }

    public void navigateToRuneSwapMenu(Rune runeToAdd)
    {
        populateRuneSwap(runeToAdd);
        runesScroll.SetActive(false);
        runeSwapScroll.SetActive(true);

        backButton.SetActive(false);
        backButton4.SetActive(true);
        backButton3.SetActive(false);
        backButton2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton4);

    }

    public void ResetMenu()
    {
        
        CleanEquipment();
        FillEquipment();

        mainButtons.SetActive(true);
        mainSelection.SetActive(true);

        equippedPanel.SetActive(true);
        equippedContainer.SetActive(true);
        equippedBackdrop.SetActive(true);

        classScroll.SetActive(false);
        runesScroll.SetActive(false);
        runeSwapScroll.SetActive(false);
        weaponsScroll.SetActive(false);

        backButton2.SetActive(false);
        backButton3.SetActive(false);
        backButton4.SetActive(false);
        backButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);

        

        CleanScroll();

        
    }

    public void CleanEquipment()
    {
        for(int i = 0; i < currentEquipmentObjects.Count; i++)
        {
            if (currentEquipmentObjects[i] != null)
            {
                Destroy(currentEquipmentObjects[i]);
                currentEquipmentObjects.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }

        }
    }

    public void CleanScroll()
    {
        for (int i = 0; i < currentScrollObjects.Count; i++)
        {
            if (currentScrollObjects[i] != null)
            {
                Destroy(currentScrollObjects[i]);
                currentScrollObjects.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }

        }

    }

    public void FillEquipment()
    {
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        currentWeaponContent.GetComponent<TMP_Text>().text = characterRef.equippedWeapon.weaponName;
        switch (characterRef.equippedWeapon.weaponClassType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                currentClassContent.GetComponent<TMP_Text>().text = "Knight";
                break;

            case WeaponBase.weaponClassTypes.Engineer:
                currentClassContent.GetComponent<TMP_Text>().text = "Engineer";
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                currentClassContent.GetComponent<TMP_Text>().text = "Gunner";
                break;

        }

        for (int i = 0; i < characterRef.equippedRunes.Length; i++)
        {
            if (characterRef.equippedRunes[i] != null)
            {

                equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeName.GetComponent<TMP_Text>().text = characterRef.equippedRunes[i].runeName;
                switch (characterRef.equippedRunes[i].runeType)
                {
                    case Rune.RuneType.Buff:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().buffRuneType.SetActive(true);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().classRuneType.SetActive(false);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().spellRuneType.SetActive(false);
                        break;
                    case Rune.RuneType.Class:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().classRuneType.SetActive(true);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().spellRuneType.SetActive(false);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().buffRuneType.SetActive(false);
                        break;
                    case Rune.RuneType.Spell:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().spellRuneType.SetActive(true);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().buffRuneType.SetActive(false);
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().classRuneType.SetActive(false);
                        break;
                }

                var equipRuneRef = Instantiate(equippedRunePrefab);
                equipRuneRef.transform.SetParent(currentRuneContainer.transform, false);
                equipRuneRef.transform.localScale = Vector3.one;

                currentEquipmentObjects.Add(equipRuneRef);
            }
        }
    }

    public void populateWeaponsScroll()
    {
        var weaponsInventory = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().GetWeaponsInventory();
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        bool eventSystemSelected = false;

        for (int i = 0; i < weaponsInventory.Length; i++)
        {
            

            if (weaponsInventory[i] != null && characterRef.weaponClass.classType == weaponsInventory[i].weaponClassType ) {
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().weapon = weaponsInventory[i];
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().type = EquipOptionPrefab.EquipTypes.Weapon;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<TMP_Text>().text = weaponsInventory[i].weaponName;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDescription.GetComponent<TMP_Text>().text = weaponsInventory[i].weaponDescription;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().unequipOptionButton.SetActive(false);

                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.SetActive(false);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDamage.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDamage.transform.Find("DamageDescription").gameObject.GetComponent<TMP_Text>().text = "+ " + weaponsInventory[i].weaponAttack + " ATK";

                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.SetActive(false);


                switch (equipOptionPrefab.GetComponent<EquipOptionPrefab>().weapon.weaponClassType)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("KnightClass").gameObject.SetActive(true);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("GunnerClass").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("EngineerClass").gameObject.SetActive(false);
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("KnightClass").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("GunnerClass").gameObject.SetActive(true);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("EngineerClass").gameObject.SetActive(false);
                        break;
                    case WeaponBase.weaponClassTypes.Engineer:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("KnightClass").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("GunnerClass").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.transform.Find("EngineerClass").gameObject.SetActive(true);
                        break;
                }


                bool equipped = false;
                if(characterRef.equippedWeapon.weaponName == weaponsInventory[i].weaponName)
                {
                    equipped = true;
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equipped";
                    
                    
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equip";
                }

                var equipRec = Instantiate(equipOptionPrefab);
                equipRec.transform.SetParent(weaponsScrollContent.transform, false);
                if (equipped)
                {
                    equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(false);
                    equipRec.GetComponent<EquipOptionPrefab>().disabledPanel.SetActive(true);
                }
                else
                {
                    equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(true);
                    equipRec.GetComponent<EquipOptionPrefab>().disabledPanel.SetActive(false);
                }
                if (!eventSystemSelected)
                {
                    eventSystemSelected = true;
                    //EventSystem.current.SetSelectedGameObject(null);
                    //EventSystem.current.SetSelectedGameObject(equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton);
                }
                currentScrollObjects.Add(equipRec);
            }
            else if(weaponsInventory[i] == null)
            {
                break;
            }
        }
    }

    public void populateRuneSwap(Rune runeToAdd)
    {
        Rune[] charRunes = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().equippedRunes;

        for(int i =0; i < charRunes.Length; i++)
        {
            if (swapOptions[i] == null) break;
            var prefab = swapOptions[i].GetComponent<RuneSwapPrefab>();
            prefab.rune = charRunes[i];
            prefab.swapName.GetComponent<TMP_Text>().text = prefab.rune.runeName;
            prefab.swapEffect.GetComponent<TMP_Text>().text = prefab.rune.runeEffect;
            prefab.runeToEquip = runeToAdd;
            prefab.index = i;

        }


        runeToSwapIn.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<TMP_Text>().text = runeToAdd.runeName;
        runeToSwapIn.GetComponent<EquipOptionPrefab>().equipOptionDescription.GetComponent<TMP_Text>().text = runeToAdd.runeDescription;
        runeToSwapIn.GetComponent<EquipOptionPrefab>().equipOptionEffect.GetComponent<TMP_Text>().text = "Effect +" + runeToAdd.runeEffect;
    }


    public void populateRunesScroll()
    {
        var runeInventory = GameObject.Find("RuneManager").GetComponent<RuneManager>().GetRuneInventory();
        bool eventSystemSelected = false;
        for (int i = 0; i < runeInventory.Length; i++)
        {
            if (runeInventory[i] != null)
            {
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().rune = runeInventory[i];
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().type = EquipOptionPrefab.EquipTypes.Rune;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<TMP_Text>().text = runeInventory[i].runeName;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDescription.GetComponent<TMP_Text>().text = runeInventory[i].runeDescription;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().unequipOptionButton.SetActive(false);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.SetActive(true);

                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.SetActive(true);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDamage.SetActive(false);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.transform.Find("EffectDescription").gameObject.GetComponent<TMP_Text>().text = "+ " + runeInventory[i].runeEffect;

                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionClassUI.SetActive(false);
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.SetActive(true);

                switch (equipOptionPrefab.GetComponent<EquipOptionPrefab>().rune.runeType)
                {
                    case Rune.RuneType.Class:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("ClassUI").gameObject.SetActive(true);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("BuffUI").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("SpellUI").gameObject.SetActive(false);
                        break;
                    case Rune.RuneType.Spell:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("ClassUI").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("BuffUI").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("SpellUI").gameObject.SetActive(true);
                        break;
                    case Rune.RuneType.Buff:
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("ClassUI").gameObject.SetActive(false);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("BuffUI").gameObject.SetActive(true);
                        equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionRuneUI.transform.Find("SpellUI").gameObject.SetActive(false);
                        break;
                }

                var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
                bool runeEquipped = false;
                for(int j = 0; j < characterRef.equippedRunes.Length; j++)
                {
                    if (characterRef.equippedRunes[j] != null)
                    {
                        if (runeInventory[i].runeName == characterRef.equippedRunes[j].runeName)
                        {
                            runeEquipped = true;
                            if (equipOptionPrefab.GetComponent<EquipOptionPrefab>().type == EquipOptionPrefab.EquipTypes.Rune)
                            {
                                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(false);
                                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.SetActive(false);
                                equipOptionPrefab.GetComponent<EquipOptionPrefab>().unequipOptionButton.SetActive(true);
                            }
                            else
                            {
                                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(true);
                                equipOptionPrefab.GetComponent<EquipOptionPrefab>().unequipOptionButton.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                }
                bool equipped = false;
                if (runeEquipped){
                    equipped = true;
                    //equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equipped";
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equip";

                }
                
               

                var equipRec = Instantiate(equipOptionPrefab); 
                equipRec.transform.SetParent(runesScrollContent.transform, false);
                /*if (equipped)
                {
                    equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(false);
                    equipRec.GetComponent<EquipOptionPrefab>().disabledPanel.SetActive(true);
                }
                else
                {
                    equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton.SetActive(true);
                    equipRec.GetComponent<EquipOptionPrefab>().disabledPanel.SetActive(false);
                }*/
                Debug.Log(equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton.activeSelf);
                if (!eventSystemSelected)
                {
                    eventSystemSelected = true;
                    //EventSystem.current.SetSelectedGameObject(null);
                    //EventSystem.current.SetSelectedGameObject(equipRec.GetComponent<EquipOptionPrefab>().equipOptionButton);
                }
                currentScrollObjects.Add(equipRec);
            }
            else
            {
                break;
            }
        }
        if (!eventSystemSelected)
        {

        }
    }

    public void populateClassScroll()
    {
        classChangeContainer.SetActive(true);
    }

    public void changeClassKnight()
    {
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Knight);
        ResetMenu();

    }

    public void changeClassGunner()
    {
        Debug.Log("Changing class to Gunner");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Gunner);
        ResetMenu();
    }

    public void changeClassEngineer()
    {
        Debug.Log("Changing class to Engineer");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Engineer);
        ResetMenu();
    }
}

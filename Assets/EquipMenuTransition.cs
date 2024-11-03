using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
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


    List<GameObject> currentEquipmentObjects = new List<GameObject>();
    List<GameObject> currentScrollObjects = new List<GameObject>();

    GameObject mainButtons;
    GameObject mainSelection;
    GameObject weaponsScroll;
    GameObject runesScroll;
    GameObject classScroll;
    GameObject weaponsScrollContent;
    GameObject classScrollContent;
    GameObject runesScrollContent;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(classChangeButton.GetComponent<Button>().interactable);
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
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);


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
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);

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
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);

        populateRunesScroll();
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
        weaponsScroll.SetActive(false);

        backButton2.SetActive(false);
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
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<TMP_Text>().text = "[Buff]";
                        break;
                    case Rune.RuneType.Defense:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<TMP_Text>().text = "[Defense]";
                        break;
                    case Rune.RuneType.Health:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<TMP_Text>().text = "[Health]";
                        break;
                    case Rune.RuneType.Projectile:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<TMP_Text>().text = "[Weapon]";
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
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.GetComponent<TMP_Text>().text = "Attack +" + weaponsInventory[i].weaponAttack;

                
                
                if(characterRef.equippedWeapon.weaponName == weaponsInventory[i].weaponName)
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equipped";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equip";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = true;
                }

                var equipRec = Instantiate(equipOptionPrefab);
                equipRec.transform.SetParent(weaponsScrollContent.transform, false);
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
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.GetComponent<TMP_Text>().text = "Effect +" + runeInventory[i].runeEffect;

                var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
                bool runeEquipped = false;
                for(int j = 0; j < characterRef.equippedRunes.Length; j++)
                {
                    if (characterRef.equippedRunes[j] != null)
                    {
                        if (runeInventory[i].runeName == characterRef.equippedRunes[j].runeName)
                        {
                            runeEquipped = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                }
                if (runeEquipped){
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equipped";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<TMP_Text>().text = "Equip";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = true;
                    
                }
                
               

                var equipRec = Instantiate(equipOptionPrefab); 
                equipRec.transform.SetParent(runesScrollContent.transform, false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject equipOptionPrefab;
    [SerializeField] GameObject equippedRunePrefab;

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


        FillEquipment();
        
        

        weaponsScroll.SetActive(false);
        runesScroll.SetActive(false);
        classScroll.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
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

        classScroll.SetActive(false);
        runesScroll.SetActive(false);
        weaponsScroll.SetActive(false);

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
        currentWeaponContent.GetComponent<Text>().text = characterRef.equippedWeapon.weaponName;
        switch (characterRef.equippedWeapon.weaponClassType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                currentClassContent.GetComponent<Text>().text = "Knight";
                break;

            case WeaponBase.weaponClassTypes.Engineer:
                currentClassContent.GetComponent<Text>().text = "Engineer";
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                currentClassContent.GetComponent<Text>().text = "Gunner";
                break;

        }

        for (int i = 0; i < characterRef.equippedRunes.Length; i++)
        {
            if (characterRef.equippedRunes[i] != null)
            {

                equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeName.GetComponent<Text>().text = characterRef.equippedRunes[i].runeName;
                switch (characterRef.equippedRunes[i].runeType)
                {
                    case Rune.RuneType.Buff:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<Text>().text = "[Buff]";
                        break;
                    case Rune.RuneType.Defense:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<Text>().text = "[Defense]";
                        break;
                    case Rune.RuneType.Health:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<Text>().text = "[Health]";
                        break;
                    case Rune.RuneType.Projectile:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<Text>().text = "[Projectile]";
                        break;
                    case Rune.RuneType.Weapon:
                        equippedRunePrefab.GetComponent<EquippedRunePrefab>().runeType.GetComponent<Text>().text = "[Weapon]";
                        break;
                }

                var equipRuneRef = Instantiate(equippedRunePrefab);
                equipRuneRef.transform.SetParent(currentRuneContainer.transform);
                equipRuneRef.transform.localScale = Vector3.one;

                currentEquipmentObjects.Add(equipRuneRef);
            }
        }
    }

    public void populateWeaponsScroll()
    {
        var weaponsInventory = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().GetWeaponsInventory();
        for(int i = 0; i < weaponsInventory.Length; i++)
        {
            if(weaponsInventory[i] != null ) {
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().weapon = weaponsInventory[i];
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().type = EquipOptionPrefab.EquipTypes.Weapon;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<Text>().text = weaponsInventory[i].weaponName;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDescription.GetComponent<Text>().text = weaponsInventory[i].weaponDescription;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.GetComponent<Text>().text = "Attack +" + weaponsInventory[i].weaponAttack;

                var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

                if(characterRef.equippedWeapon.weaponName == weaponsInventory[i].weaponName)
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<Text>().text = "Equipped";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<Text>().text = "Equip";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = true;
                }

                var equipRec = Instantiate(equipOptionPrefab);
                equipRec.transform.SetParent(weaponsScrollContent.transform);
                currentScrollObjects.Add(equipRec);
            }
            else
            {
                break;
            }
        }
    }

    public void populateRunesScroll()
    {
        var runeInventory = GameObject.Find("RuneManager").GetComponent<RuneManager>().GetRuneInventory();
        for (int i = 0; i < runeInventory.Length; i++)
        {
            if (runeInventory[i] != null)
            {
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().rune = runeInventory[i];
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().type = EquipOptionPrefab.EquipTypes.Rune;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<Text>().text = runeInventory[i].runeName;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionDescription.GetComponent<Text>().text = runeInventory[i].runeDescription;
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEffect.GetComponent<Text>().text = "Effect +" + runeInventory[i].runeEffect;

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
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<Text>().text = "Equipped";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionEquipText.GetComponent<Text>().text = "Equip";
                    equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionButton.GetComponent<Button>().interactable = true;
                }

                var equipRec = Instantiate(equipOptionPrefab);
                equipRec.transform.SetParent(runesScrollContent.transform);
                currentScrollObjects.Add(equipRec);
            }
            else
            {
                break;
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject equipOptionPrefab;
    GameObject mainButtons;
    GameObject mainSelection;
    GameObject weaponsScroll;
    GameObject runesScroll;
    GameObject classScroll;
    GameObject weaponsScrollContent;
    GameObject classScrollContent;
    GameObject runesScrollContent;
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
        populateWeaponsScroll();
        
    }

    public void NavigateToClassEquipMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        classScroll.SetActive(true);
    }

    public void NavigateToRuneEquipMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        runesScroll.SetActive(true);
    }

    public void populateWeaponsScroll()
    {
        var weaponsInventory = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().GetWeaponsInventory();
        for(int i = 0; i < weaponsInventory.Length; i++)
        {
            if(weaponsInventory[i] != null ) {
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().weapon = weaponsInventory[i];
                equipOptionPrefab.GetComponent<EquipOptionPrefab>().type = EquipOptionPrefab.EquipTypes.Weapon;
                var name = equipOptionPrefab.GetComponent<EquipOptionPrefab>().equipOptionName.GetComponent<Text>().text = weaponsInventory[i].weaponName;
                var equipRec = Instantiate(equipOptionPrefab);
                equipRec.transform.SetParent(weaponsScrollContent.transform);
            }
            else
            {
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject craftRecipePrefab;
    GameObject mainButtons;
    GameObject mainSelection;
    GameObject weaponsScroll;
    GameObject runesScroll;
    GameObject itemsScroll;
    GameObject weaponsScrollContent;
    GameObject itemsScrollContent;
    GameObject runesScrollContent;
    
    void Start()
    {
        mainButtons = GameObject.Find("MainButtons");
        mainSelection = GameObject.Find("MainSelection");
        weaponsScroll = GameObject.Find("WeaponsScroll");
        runesScroll = GameObject.Find("RunesScroll");
        itemsScroll = GameObject.Find("ItemsScroll");

        weaponsScrollContent = GameObject.Find("WeaponsScrollContent");
        itemsScrollContent = GameObject.Find("ItemsScrollContent");
        runesScrollContent = GameObject.Find("RunesScrollContent");

        weaponsScroll.SetActive(false);
        runesScroll.SetActive(false);
        itemsScroll.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void populateWeaponsScroll()
    {
        var weaponCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnWeaponsCraftList();
        foreach (var item in weaponCraftRecipes)
        {
            craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
            var craftRec = Instantiate(craftRecipePrefab);
            craftRec.transform.SetParent(weaponsScrollContent.transform);
        }
    }

    public void populateRunesScroll()
    {
        var runesCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnRunesCraftList();
        foreach(var item in runesCraftRecipes)
        {
            craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
            var craftRec = Instantiate(craftRecipePrefab);
            craftRec.transform.SetParent(runesScrollContent.transform);
        }
    }

    public void populateItemsScroll()
    {
        var itemsCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnItemsCraftList();
        foreach (var item in itemsCraftRecipes)
        {
            craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
            var craftRec = Instantiate(craftRecipePrefab);
            craftRec.transform.SetParent(itemsScrollContent.transform);
        }
    }



    public void NavigateToMaterialMenu()
    {
        Debug.Log("Back Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToMaterialMenu();
    }

    public void NavigateToWeaponCraftMenu()
    {
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        weaponsScroll.SetActive(true);
        populateWeaponsScroll();
    }

    public void NavigateToRunesCraftMenu()
    {
        Debug.Log("Rune Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        runesScroll.SetActive(true);
        populateRunesScroll();
    }

    public void NavigateToItemsCraftMenu()
    {
        Debug.Log("Items Button pressed");
        mainButtons.SetActive(false);
        mainSelection.SetActive(false);
        itemsScroll.SetActive(true);
        populateItemsScroll();
    }




}
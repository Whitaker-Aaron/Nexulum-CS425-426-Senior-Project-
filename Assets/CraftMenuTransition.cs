using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject craftRecipePrefab;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject weaponsScrollBar;
    [SerializeField] GameObject runesScrollBar;
    [SerializeField] GameObject itemsScrollBar;


    List<GameObject> currentWeaponScrollObjects = new List<GameObject>();
    List<GameObject> currentRuneScrollObjects = new List<GameObject>();
    List<GameObject> currentItemScrollObjects = new List<GameObject>();

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
        
    }

    private void Awake()
    {
        mainButtons = GameObject.Find("MainButtons");
        mainSelection = GameObject.Find("MainSelection");
        weaponsScroll = GameObject.Find("WeaponsScroll");
        runesScroll = GameObject.Find("RunesScroll");
        itemsScroll = GameObject.Find("ItemsScroll");

        weaponsScrollContent = GameObject.Find("WeaponsScrollContent");
        itemsScrollContent = GameObject.Find("ItemsScrollContent");
        runesScrollContent = GameObject.Find("RunesScrollContent");

        
        //populateItemsScroll();
        //populateRunesScroll();

        //weaponsScroll.SetActive(true);
        //runesScroll.SetActive(true);
        //itemsScroll.SetActive(true);

        weaponsScroll.SetActive(false);
        runesScroll.SetActive(false);
        itemsScroll.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(mainButtons.transform.GetChild(0).gameObject);
        EventSystem.current.SetSelectedGameObject(backButton);
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

    public void ResetItemsCraftSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentItemScrollObjects[currentItemScrollObjects.Count - 1].GetComponent<CraftRecipePrefab>().craftButton);
    }

    public void populateWeaponsScroll()
    {
        var weaponCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnWeaponsCraftList();
        bool eventSystemSelected = false;
        //EventSystem.current.enabled = true;
        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(weaponsScrollBar);
        

        foreach (var item in weaponCraftRecipes)
        {
            if(item.hasCrafted != true)
            {
                craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
                var craftRec = Instantiate(craftRecipePrefab);
                craftRec.transform.SetParent(weaponsScrollContent.transform, false);
                if (!eventSystemSelected)
                {
                    eventSystemSelected = true;
                    //EventSystem.current.SetSelectedGameObject(null);
                    //EventSystem.current.SetSelectedGameObject(craftRec.GetComponent<CraftRecipePrefab>().craftButton);
                    //craftRec.GetComponent<CraftRecipePrefab>().craftButton.GetComponent<Button>().Select();
                    //craftRec.GetComponent<CraftRecipePrefab>().craftButton.GetComponent<Button>().OnSelect(null);
                }
                currentWeaponScrollObjects.Add(craftRec);
            }
        }
    }

    public void populateRunesScroll()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(weaponsScrollBar);
        var runesCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnRunesCraftList();
        bool eventSystemSelected = false;
        foreach(var item in runesCraftRecipes)
        {
            if(item.hasCrafted != true)
            {
                craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
                var craftRec = Instantiate(craftRecipePrefab);
                craftRec.transform.SetParent(runesScrollContent.transform, false);
                if (!eventSystemSelected)
                {
                    eventSystemSelected = true;
                    //EventSystem.current.SetSelectedGameObject(null);
                    //EventSystem.current.SetSelectedGameObject(craftRec.GetComponent<CraftRecipePrefab>().craftButton);
                }
                currentRuneScrollObjects.Add(craftRec);
            }
            
        }
    }

    public void populateItemsScroll()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemsScrollBar);
        var itemsCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnItemsCraftList();
        var eventSystemSelected = false;
        foreach (var item in itemsCraftRecipes)
        {
            craftRecipePrefab.GetComponent<CraftRecipePrefab>().craftRecipe = item;
            var craftRec = Instantiate(craftRecipePrefab);
            craftRec.transform.SetParent(itemsScrollContent.transform, false);
            if (!eventSystemSelected)
            {
                eventSystemSelected = true;
                //EventSystem.current.SetSelectedGameObject(null);
                //EventSystem.current.SetSelectedGameObject(craftRec.GetComponent<CraftRecipePrefab>().craftButton);
            }
            currentItemScrollObjects.Add(craftRec);
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

    public void DestroyRecipe(CraftRecipe recipe, CraftRecipe.CraftTypes type)
    {
        if(type == CraftRecipe.CraftTypes.Weapon)
        {
            for(int i =0; i < currentWeaponScrollObjects.Count; i++)
            {
                if(recipe.recipeName == currentWeaponScrollObjects[i].GetComponent<CraftRecipePrefab>().craftRecipe.recipeName)
                {
                    Destroy(currentWeaponScrollObjects[i]);
                    currentWeaponScrollObjects.RemoveAt(i);
                    break;
                }
            }
        }
        else if(type == CraftRecipe.CraftTypes.Rune)
        {
            for (int i = 0; i < currentRuneScrollObjects.Count; i++)
            {
                if (recipe.recipeName == currentRuneScrollObjects[i].GetComponent<CraftRecipePrefab>().craftRecipe.recipeName)
                {
                    Destroy(currentRuneScrollObjects[i]);
                    currentRuneScrollObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }





}

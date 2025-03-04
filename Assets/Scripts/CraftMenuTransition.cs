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
    [SerializeField] GameObject backButton2;
    [SerializeField] GameObject weaponsScrollBar;
    [SerializeField] GameObject runesScrollBar;
    [SerializeField] GameObject itemsScrollBar;

    GameObject previousSelectedObjected;


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

    ScrollRect weaponsScrollRect;
    ScrollRect runesScrollRect;
    ScrollRect itemsScrollRect;

    ScrollSelection curScrollSelection = ScrollSelection.none;
    public bool isTerminal = false;
    
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

        weaponsScrollRect = weaponsScroll.GetComponent<ScrollRect>();
        runesScrollRect = runesScroll.GetComponent<ScrollRect>();
        itemsScrollRect = itemsScroll.GetComponent<ScrollRect>();


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
        if(curScrollSelection != ScrollSelection.none && EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name == "CraftRecipe(Clone)")
        {
            var selectedItem = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent;
            RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();
            switch (curScrollSelection)
            {
                case ScrollSelection.items:
                    var itemContentPanel = itemsScrollContent.GetComponent<RectTransform>();
                    Vector2 newItemPos = (Vector2)itemsScrollRect.transform.InverseTransformPoint(itemContentPanel.position)
                    - (Vector2)itemsScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
                    float newItemPosY = (float)newItemPos.y;
                    itemContentPanel.anchoredPosition = new Vector2(itemContentPanel.anchoredPosition.x, newItemPosY-150f);

                    break;
                case ScrollSelection.weapons:
                    var weaponContentPanel = weaponsScrollContent.GetComponent<RectTransform>();
                    Vector2 newWeaponPos = (Vector2)weaponsScrollRect.transform.InverseTransformPoint(weaponContentPanel.position)
                    - (Vector2)weaponsScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
                    float newWeaponPosY = (float)newWeaponPos.y;
                    weaponContentPanel.anchoredPosition = new Vector2(weaponContentPanel.anchoredPosition.x, newWeaponPosY - 150f);
                    break;
                case ScrollSelection.runes:
                    var runesContentPanel = runesScrollContent.GetComponent<RectTransform>();
                    Vector2 newRunePos = (Vector2)runesScrollRect.transform.InverseTransformPoint(runesContentPanel.position)
                    - (Vector2)runesScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
                    float newRunePosY = (float)newRunePos.y;
                    runesContentPanel.anchoredPosition = new Vector2(runesContentPanel.anchoredPosition.x, newRunePosY - 150f);
                    break;
            }
        }
        
        
    }


    public void ResetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton2);
    }

    public void populateWeaponsScroll()
    {
        var weaponCraftRecipes = GameObject.Find("MenuManager").GetComponent<MenuManager>().returnWeaponsCraftList();
        bool eventSystemSelected = false;
        //EventSystem.current.enabled = true;
        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        
        

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
                }
                currentWeaponScrollObjects.Add(craftRec);
            }
        }
    }

    public void populateRunesScroll()
    {
        
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

    public void NavigateBackToMainSelection()
    {
        ResetScrolls();
        curScrollSelection = ScrollSelection.none;
        mainButtons.SetActive(true);
        mainSelection.SetActive(true);
        weaponsScroll.SetActive(false);
        runesScroll.SetActive(false);
        itemsScroll.SetActive(false);
        backButton.SetActive(true);
        backButton2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Automatic;
        backButton.GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("WeaponsPanel").Find("WeaponsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("RunesPanel").Find("RunesButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("ItemsPanel").Find("ItemsButton").GetComponent<Button>().navigation = navigation;

    }

    public void NavigateToMaterialMenu()
    {
        Debug.Log("Back Button pressed");
        if (isTerminal) GameObject.Find("MenuManager").GetComponent<MenuManager>().openTerminalMenu();
        else GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToMaterialMenu();
    }

    public void NavigateToWeaponCraftMenu()
    {
        curScrollSelection = ScrollSelection.weapons;
        EventSystem.current.SetSelectedGameObject(null);
        backButton.SetActive(false);
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton2);
        Debug.Log("Weapon Button pressed");
        mainButtons.SetActive(false);
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        backButton.GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("WeaponsPanel").Find("WeaponsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("RunesPanel").Find("RunesButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("ItemsPanel").Find("ItemsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.SetActive(false);
        //backButtonRunes.GetComponent<Button>().navigation = navigation;
        //backButtonItems.GetComponent<Button>().navigation = navigation;
        weaponsScroll.SetActive(true);
        populateWeaponsScroll();
    }

    public void NavigateToRunesCraftMenu()
    {
        curScrollSelection = ScrollSelection.runes;
        Debug.Log("Rune Button pressed");
        EventSystem.current.SetSelectedGameObject(null);
        backButton.SetActive(false);
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton2);
        mainButtons.SetActive(false);
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        mainSelection.transform.Find("WeaponsPanel").Find("WeaponsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("RunesPanel").Find("RunesButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("ItemsPanel").Find("ItemsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.SetActive(false);
        runesScroll.SetActive(true);

        backButton.SetActive(false);
        populateRunesScroll();
    }

    public void NavigateToItemsCraftMenu()
    {
        curScrollSelection = ScrollSelection.items;
        Debug.Log("Items Button pressed");
        EventSystem.current.SetSelectedGameObject(null);
        backButton.SetActive(false);
        backButton2.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton2);
        mainButtons.SetActive(false);
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        mainSelection.transform.Find("WeaponsPanel").Find("WeaponsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("RunesPanel").Find("RunesButton").GetComponent<Button>().navigation = navigation;
        mainSelection.transform.Find("ItemsPanel").Find("ItemsButton").GetComponent<Button>().navigation = navigation;
        mainSelection.SetActive(false);
        itemsScroll.SetActive(true);
        backButton.SetActive(false);
        populateItemsScroll();
    }

    public void ResetScrolls()
    {
        var items = GameObject.Find("ItemsScrollContent");
        var runes = GameObject.Find("RunesScrollContent");
        var weapons = GameObject.Find("WeaponsScrollContent");

        if(items != null)
        {
            for (int i = 0; i < items.transform.childCount; i++)
            {
                Destroy(items.transform.GetChild(i).gameObject);
            }
        }
        if(weapons != null)
        {
            for (int i = 0; i < weapons.transform.childCount; i++)
            {
                Destroy(weapons.transform.GetChild(i).gameObject);
            }
        }
        if(runes != null)
        {
            for (int i = 0; i < runes.transform.childCount; i++)
            {
                Destroy(runes.transform.GetChild(i).gameObject);
            }
        }
        
        
        
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

    enum ScrollSelection
    {
        none,
        weapons,
        items,
        runes
    }





}

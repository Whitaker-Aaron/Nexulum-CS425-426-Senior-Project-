using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BaseShopMenu : MonoBehaviour
{
    MenuManager menuManager;
    [SerializeField] GameObject availableRecipes;
    [SerializeField] GameObject availableItems;
    [SerializeField] GameObject availableWeapons;
    [SerializeField] GameObject shopTitle;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject backButton2;
    [SerializeField] GameObject selection;
    [SerializeField] GameObject scroll;
    [SerializeField] GameObject florentineCount;
    ScrollRect storeScrollRect;
    public GameObject storeScrollContent;

    AudioManager audioManager;
    GameObject curEventSystem;

    List<GameObject> curShopListings = new List<GameObject>();
    public void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        availableRecipes.SetActive(false);
        availableItems.SetActive(false);
        availableWeapons.SetActive(false);
        shopTitle.SetActive(true);
        storeScrollContent = GameObject.Find("StoreScrollContent");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        storeScrollRect = scroll.GetComponent<ScrollRect>();

        backButton.SetActive(true);
        backButton2.SetActive(false);

        florentineCount.GetComponent<TMP_Text>().text = 
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().GetFlorentine().ToString();

        resetSelection();
    }
    // Start is called before the first frame update

    public void Update()
    {
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject;
        else if (EventSystem.current.currentSelectedGameObject != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject;
            audioManager.PlaySFX("UIChange");
        }

        if (EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name == "StoreItem")
        {
            var selectedItem = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent;
            RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();


            var itemContentPanel = storeScrollContent.GetComponent<RectTransform>();
            Vector2 newItemPos = (Vector2)storeScrollRect.transform.InverseTransformPoint(itemContentPanel.position)
            - (Vector2)storeScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
            float newItemPosY = (float)newItemPos.y;
            itemContentPanel.anchoredPosition = new Vector2(itemContentPanel.anchoredPosition.x, newItemPosY - 150f);

        }
    }
    public void onRecipeButton()
    {
        scroll.SetActive(true);
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Recipe);
        availableItems.SetActive(false);
        availableWeapons.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        backButton.SetActive(false);
        backButton2.SetActive(true);
        resetSelection();

        availableRecipes.SetActive(true);

        
    }

    public void updateShopListings()
    {
        float curFlorentine = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().GetFlorentine();
        var list = GameObject.Find("ItemsList").GetComponent<ItemsList>();
        for (int i =0; i < curShopListings.Count; i++)
        {
            if (curShopListings[i] == null) continue;
            var shopItem = curShopListings[i].GetComponent<StoreItem>();
            var itemCost = int.Parse((shopItem.florentineRequiredAmount.GetComponent<Text>().text).Substring(1));

            Color32 red = new Color32(0xE4, 0x3c, 0x54, 0xFF);
            Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            if (curFlorentine >= itemCost)
            {
                shopItem.purchaseButton.GetComponent<Button>().interactable = true;
                shopItem.disabledPanel.SetActive(false);
                shopItem.florentineRequiredAmount.GetComponent<Text>().color = white;
            }
            else
            {
                shopItem.purchaseButton.GetComponent<Button>().interactable = false;
                shopItem.disabledPanel.SetActive(true);
                shopItem.florentineRequiredAmount.GetComponent<Text>().color = red;
            }
            if(shopItem.storeType == StoreItem.StoreItemType.Item)
            {
                var item = list.ReturnItem(shopItem.playerItem.itemName);
                shopItem.itemQuantity.GetComponent<TMP_Text>().text = "Have: " + item.itemAmount.ToString() + "/" + item.maxItemAmount.ToString();
            }
        }
    }

    public void addToCurShopListings(GameObject listingToAdd)
    {
        curShopListings.Add(listingToAdd);
    }

    public void clearCurShopListings()
    {
        for (int i = 0; i < curShopListings.Count;  i++)
        {
            Destroy(curShopListings[i]);
        }
        curShopListings.Clear();
    }

    public void onItemsButton()
    {
        audioManager.PlaySFX("UIConfirm");
        scroll.SetActive(true);
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Item);
        availableWeapons.SetActive(false);
        availableRecipes.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        availableItems.SetActive(true);

        backButton.SetActive(false);
        backButton2.SetActive(true);
        resetSelection();
    }

    public void onWeaponsButton()
    {
        audioManager.PlaySFX("UIConfirm");
        scroll.SetActive(true);
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Weapon);
        availableItems.SetActive(false);
        availableRecipes.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        availableWeapons.SetActive(true);

        backButton.SetActive(false);
        backButton2.SetActive(true);
        resetSelection();
    }

    public void updateFlorentineCount(float newCount)
    {
        int curAmount = int.Parse(florentineCount.GetComponent<TMP_Text>().text);
        int newAmount = curAmount - (int)newCount;
        florentineCount.GetComponent<TMP_Text>().text = newAmount.ToString();
    }

    public void resetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(backButton.activeSelf) EventSystem.current.SetSelectedGameObject(backButton);
        else if(backButton2.activeSelf) EventSystem.current.SetSelectedGameObject(backButton2);

    }

    public void onBackButton()
    {
        menuManager.openTerminalMenu(true);
    }

    public void onBackButton2()
    {
        audioManager.PlaySFX("UIBack");
        returnToMainSelection();
        resetSelection();
    }

    public void returnToMainSelection()
    {
        availableItems.SetActive(false);
        availableRecipes.SetActive(false);
        availableWeapons.SetActive(false);
        backButton.SetActive(true);
        backButton2.SetActive(false);

        shopTitle.SetActive(true);
        selection.SetActive(true);
        scroll.SetActive(false);

        clearCurShopListings();
    }

    enum ScrollSelection
    {
        none,
        weapons,
        recipe,
        item
    }
}

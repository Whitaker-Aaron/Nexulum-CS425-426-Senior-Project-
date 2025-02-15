using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BaseShopMenu : MonoBehaviour
{
    MenuManager menuManager;
    [SerializeField] GameObject availableRecipes;
    [SerializeField] GameObject availableItems;
    [SerializeField] GameObject availableWeapons;
    [SerializeField] GameObject shopTitle;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject selection;
    [SerializeField] GameObject florentineCount;
    public void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        availableRecipes.SetActive(false);
        availableItems.SetActive(false);
        availableWeapons.SetActive(false);
        shopTitle.SetActive(true);

        florentineCount.GetComponent<TMP_Text>().text = 
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().GetFlorentine().ToString();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
    // Start is called before the first frame update
    public void onRecipeButton()
    {
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Recipe);
        availableItems.SetActive(false);
        availableWeapons.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        availableRecipes.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void onItemsButton()
    {
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Item);
        availableWeapons.SetActive(false);
        availableRecipes.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        availableItems.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void onWeaponsButton()
    {
        menuManager.populateBaseShopOptions(StoreItem.StoreItemType.Weapon);
        availableItems.SetActive(false);
        availableRecipes.SetActive(false);
        shopTitle.SetActive(false);
        selection.SetActive(false);

        availableWeapons.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
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
        EventSystem.current.SetSelectedGameObject(backButton);
    }
}

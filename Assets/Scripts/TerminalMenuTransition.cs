using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    MenuManager menuManager;
    [SerializeField] GameObject ShopButton;
    [SerializeField] GameObject backButton;
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void TransitionToMaterials()
    {
        menuManager.navigateToBaseMaterialsMenu();
    }

    public void TransitionToShop()
    {
        menuManager.navigateToBaseShopMenu();
    }

    public void TransitionToEquip()
    {
        menuManager.navigateToBaseEquipMenu();
    }

    public void TransitionToCraft()
    {
        menuManager.navigateToBaseCraftMenu();
    }


    public void OnBackButton()
    {
        menuManager.CloseMenu();
    }
}

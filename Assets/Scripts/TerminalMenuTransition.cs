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
    string curEventSystem;
    AudioManager audioManager;

    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }
    }

    public void TransitionToMaterials()
    {
        menuManager.navigateToBaseMaterialsMenu();
        audioManager.PlaySFX("UIConfirm");
    }

    public void TransitionToShop()
    {
        audioManager.PlaySFX("UIConfirm");
        menuManager.navigateToBaseShopMenu();
    }

    public void TransitionToEquip()
    {
        audioManager.PlaySFX("UIConfirm");
        menuManager.navigateToBaseEquipMenu();
    }

    public void TransitionToCraft()
    {
        audioManager.PlaySFX("UIConfirm");
        menuManager.navigateToBaseCraftMenu();
    }


    public void OnBackButton()
    {
        audioManager.PlaySFX("UIBack");
        menuManager.CloseMenu();
    }
}

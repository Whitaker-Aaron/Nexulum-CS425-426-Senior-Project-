using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    MenuManager menuManager;
    [SerializeField] GameObject ShopButton;
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ShopButton);
    }

    public void TransitionToMaterials()
    {
        menuManager.navigateToBaseMaterialsMenu();
    }
}

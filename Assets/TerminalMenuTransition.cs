using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    MenuManager menuManager;
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    public void TransitionToMaterials()
    {
        menuManager.navigateToBaseMaterialsMenu();
    }
}

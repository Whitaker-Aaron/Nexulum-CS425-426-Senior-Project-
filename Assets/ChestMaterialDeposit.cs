using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestMaterialDeposit : MonoBehaviour
{
    MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButton()
    {
        menuManager.closeChestMenu();
    }
}

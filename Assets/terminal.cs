using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terminal : MonoBehaviour, i_Interactable
{
    [SerializeField] GameObject terminalUI;
    MenuManager menuManager;
    CharacterBase character;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(Interactor interactor)
    {
        if(menuManager.menuActive) return false;
        menuManager.openTerminalMenu();
        return true;

    }

    public void ShowUI()
    {
        if (terminalUI != null)
        {
            terminalUI.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (terminalUI != null)
        {
            terminalUI.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MaterialMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject craftButton;
    AudioManager audioManager;
    string curEventSystem;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(craftButton);
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }
    }

    public void CloseMaterialsMenu()
    {
        audioManager.PlaySFX("Pause");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().CloseMenu();
    }

    public void NavigateToCraftMenu()
    {
        Debug.Log("Button pressed");
        audioManager.PlaySFX("UIConfirm");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToCraftMenu();
    }

    public void NavigateToEquipMenu()
    {
        Debug.Log("Button pressed");
        audioManager.PlaySFX("UIConfirm");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToEquipMenu();
    }

    public void NavigateToItemsMenu()
    {
        Debug.Log("Button pressed");
        audioManager.PlaySFX("UIConfirm");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToItemsMenu();
    }
}

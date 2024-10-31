using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MaterialMenuTransition : MonoBehaviour
{
    [SerializeField] GameObject craftButton;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(craftButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateToCraftMenu()
    {
        Debug.Log("Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToCraftMenu();
    }

    public void NavigateToEquipMenu()
    {
        Debug.Log("Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToEquipMenu();
    }
}

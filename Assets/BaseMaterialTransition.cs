using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BaseMaterialTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject backButton;
    [SerializeField] TMP_Text materialInventoryAmount;
    [SerializeField] TMP_Text baseMaterialInventoryAmount;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ReturnToTerminal()
    {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().openTerminalMenu();
    }

    public void ResetSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, i_Interactable
{
    [SerializeField] private string prompt;
    public GameObject doorUI;

    public string interactionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Opening the Door");
        return true;
    }
    public void ShowUI()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(true);
        }
    }
    public void HideUi()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(false);
        }
    }
}

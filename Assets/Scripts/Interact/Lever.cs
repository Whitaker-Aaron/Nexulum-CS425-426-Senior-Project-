using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour , i_Interactable
{
    [SerializeField] private string prompt;
    public GameObject leverUI;
    public GameObject controlledDoor;

    public string interactionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Activating Lever");
        Door door = controlledDoor.GetComponent<Door>();
        return true;
    }
    public void ShowUI()
    {
        if (leverUI != null)
        {
            leverUI.SetActive(true);
        }
    }
    public void HideUI()
    {
        if (leverUI != null)
        {
            leverUI.SetActive(false);
        }
    }
}

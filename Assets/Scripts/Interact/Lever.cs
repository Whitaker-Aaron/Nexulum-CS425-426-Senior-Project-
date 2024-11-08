using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Lever : MonoBehaviour, i_Interactable
{
    [SerializeField] private string prompt;
    public GameObject leverUI;
    public GameObject controlledDoor;
    private Animator animator;
    private bool leverToggled;

    public string interactionPrompt => prompt;

    public void Start()
    {
        leverToggled = false;

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("No animator component found");
        }
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Activating Lever");

        if (controlledDoor != null)
        {
            Door door = controlledDoor.GetComponent<Door>();
            if (door != null)
            {
                if (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood)
                {
                    leverToggled = !leverToggled;
                    animator.SetBool("isToggled", leverToggled);
                    door.isLocked = false;
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("This lever cannot control the selected door type.");
                }
            }
            else
            {
                Debug.LogError("No Door component found on the controlled door object.");
            }
        }
        else
        {
            Debug.LogError("No controlled door assigned to this lever.");
        }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { Gate, Wood, Breakable }

public class Door : MonoBehaviour, i_Interactable
{
    [SerializeField] private string prompt;
    [SerializeField] public DoorType doorType;
    public GameObject doorUI;
    private Animator animator;
    public string interactionPrompt => prompt;
    public bool isLocked;
    private bool isOpen;

    public void Start()
    {
        isOpen = false;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("No animator component found");
        }
    }

    // Only allow interaction if the door is not a Gate type
    public bool Interact(Interactor interactor)
    {
        if (doorType == DoorType.Gate)
        {
            Debug.Log("This door can only be controlled by a lever.");
            return false; // Block interaction for Gate doors
        }

        if (!isLocked)
        {
            ToggleDoor();
        }
        else
        {
            Debug.Log("Door is locked");
        }

        return true;
    }

    // Method to toggle door open/close, accessible by lever
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (doorType == DoorType.Gate || doorType == DoorType.Wood)
        {
            animator.SetBool("isOpen", true);
            Debug.Log("Opening the Door");
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (doorType == DoorType.Gate || doorType == DoorType.Wood)
        {
            animator.SetBool("isOpen", false);
            Debug.Log("Closing Door");
            isOpen = false;
        }
    }

    public void ShowUI()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(false);
        }
    }
}

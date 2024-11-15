using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { Gate, Wood, Breakable, IronGate }

public class Door : MonoBehaviour, i_Interactable
{
    [SerializeField] public DoorType doorType;
    public GameObject doorUI;
    private Animator animator;
    public bool isLocked;
    private bool isOpen;
    public bool forceOpen;

    public void Start()
    {
        isOpen = false;
        forceOpen = false;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("No animator component found");
        }
    }

    public bool Interact(Interactor interactor)
    {
        if (doorType == DoorType.Gate)
        {
            Debug.Log("This door can only open when activated.");
            return false;
        }

        if (isLocked == false)
        {
            ToggleDoor();
        }
        else
        {
            Debug.Log("Door is locked");
        }

        return true;
    }

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
        if (forceOpen && doorType == DoorType.IronGate)
        {
            animator.SetBool("isOpen", true);
            Debug.Log("Forcing Iron Gate to Open");
            isOpen = true;
            return;
        }

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

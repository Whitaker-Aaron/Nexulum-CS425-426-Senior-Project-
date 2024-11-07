using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { Gate, Wood, Breakable } //breakable will be for future bomb related schenanigans

public class Door : MonoBehaviour, i_Interactable
{
    [SerializeField] private string prompt;
    [SerializeField] private DoorType doorType;
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
    public bool Interact(Interactor interactor)
    {
        if (!isLocked)
        {
            if (isOpen == false)
            {
                ShowUI();
                OpenDoor(this);
                Debug.Log("Opening the Door");
            }
            else
            {
                ShowUI();
                CloseDoor(this);
                Debug.Log("Closing Door");
            }
        }
        else
        {
            Debug.Log("Door is locked");
        }

        return true;
    }

    public void OpenDoor(Door door)
    {
        if (doorType == DoorType.Wood)
        {
            animator.SetBool("isOpen", true);
        }

        isOpen = true;
    }
    public void CloseDoor(Door door)
    {
        if (doorType == DoorType.Wood)
        {
            animator.SetBool("isOpen", false);
        }

        isOpen = false;

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

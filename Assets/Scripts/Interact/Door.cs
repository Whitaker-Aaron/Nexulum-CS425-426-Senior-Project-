
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { Gate, Wood, Breakable, IronGate }

public class Door : MonoBehaviour, i_Interactable
{
    [SerializeField] public DoorType doorType;
    public ItemManager itemManager;
    public GameObject doorUI;
    public GameObject lockedUI;
    public LockedDoorUI lockedDoorUI;
    private Animator animator;
    public bool isLocked;
    public bool isOpen;
    public bool forceOpen;

    public void Start()
    {
        isOpen = false;
        forceOpen = false;
        animator = GetComponent<Animator>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        if (animator == null)
        {
            Debug.Log("No animator component found");
        }
    }

    public void Update()
    {
        if(isLocked && doorType == DoorType.Wood) lockedDoorUI.UpdateKeyAmount(GetKeyAmountFromInventory());

    }

    public bool Interact(Interactor interactor)
    {
        if (doorType == DoorType.Gate)
        {
            Debug.Log("This door can only open when activated.");
            return false;
        }

        if (!isLocked && !isOpen)
        {
            ToggleDoor();
        }
        else if(isLocked && doorType == DoorType.Wood)
        {
            UseKeyFromInventory();
        }
        else
        {
            Debug.Log("Door is locked");
        }

        return true;
    }

    public int GetKeyAmountFromInventory()
    {
        var inventory = itemManager.GetInventory();
        foreach (var item in inventory)
        {
            if (item == null) continue;
            if (item.itemName == "Small Key")
            {
                return item.itemAmount;
            }
        }
        return 0;

    }

    public void UseKeyFromInventory()
    {
        var inventory = itemManager.GetInventory();
        foreach (var item in inventory)
        {
            if (item == null) continue;
            if (item.itemName == "Small Key")
            {
                isLocked = false;
                ToggleDoor();
                itemManager.RemoveFromInventory(item, 1);
                if (lockedUI != null) lockedUI.SetActive(false);
                if(doorUI != null) doorUI.SetActive(false);
                return;
            }
        }
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
            if(isLocked) doorUI.SetActive(true);
            if (lockedUI != null && isLocked)
            {
                
                lockedUI.SetActive(true);
            }
        }
    }

    public void HideUI()
    {
        if (doorUI != null)
        {
            if(isLocked) doorUI.SetActive(false);
            if (lockedUI != null && isLocked) lockedUI.SetActive(false);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { Gate, Wood, Breakable, IronGate }

public class Door : MonoBehaviourID, i_Interactable
{
    [SerializeField] public DoorType doorType;
    public ItemManager itemManager;
    [SerializeField] public string doorGuid;
    public GameObject doorUI;
    public GameObject lockedUI;
    public LockedDoorUI lockedDoorUI;
    RoomInformation roomInfo;
    private Animator animator;
    public bool isLocked;
    public bool isOpen;
    public bool forceOpen;
    [SerializeField] bool hasTrigger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (hasTrigger) animator.keepAnimatorStateOnDisable = true;
        //if (isOpen) animator.SetBool("isOpen", true);
        //else animator.SetBool("isOpen", false);
    }
    public void Start()
    {
        Debug.Log("INSIDE DOOR START FUNCTION");
        isOpen = false;
        forceOpen = false;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        if (animator == null)
        {
            Debug.Log("No animator component found");
        }
    }

    public void OnEnable()
    {
        //animator = GetComponent<Animator>();
        //if (isOpen) animator.SetBool("isOpen", true);
        //else animator.SetBool("isOpen", false);
    }

    public void UnlockDoor()
    {
        ToggleDoor();
        isLocked = false;
    }
    public void Update()
    {
        if(isLocked && doorType == DoorType.Wood) lockedDoorUI.UpdateKeyAmount(GetKeyAmountFromInventory());
        

    }

    public void SetRoomInfo(RoomInformation roomInfo_)
    {
        roomInfo = roomInfo_;
    }

    public void UpdateDoorState()
    {
        if(doorGuid != null && roomInfo != null) roomInfo.UpdateDoorState(doorGuid, isLocked);
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
        UpdateDoorState();
    }

    public void OpenDoor()
    {
        if (forceOpen && doorType == DoorType.IronGate)
        {
            animator.SetBool("isOpen", true);
            Debug.Log("Forcing Iron Gate to Open");
            isOpen = true;
            isLocked = false;
            return;
        }

        if (doorType == DoorType.Gate || doorType == DoorType.Wood)
        {
            animator.SetBool("isOpen", true);
            Debug.Log("Opening the Door");
            isOpen = true;
            isLocked = false;
        }
    }

    public void CloseDoor()
    {
        if (doorType == DoorType.Gate || doorType == DoorType.Wood)
        {
            Debug.Log("CLOSING DOOR BOOL: " + animator.GetBool("isOpen"));
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

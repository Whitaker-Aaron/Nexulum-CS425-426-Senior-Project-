
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Barrier : MonoBehaviourID, i_Interactable
{
    public ItemManager itemManager;
    private CameraFollow camera;

    [SerializeField] public string barrierGuid;
    [SerializeField] Door doorToUnlock;
    public GameObject barrierUI;
    public GameObject lockedUI;
    public GameObject craftGlobeUI;
    public GameObject particleSystem;
    public BarrierRequirementUI requirementUI;
    public bool hasTriggered = false;
    public bool canTrigger;
    RoomInformation roomInfo;
    [SerializeField] bool hasTrigger;
    [SerializeField] GameObject objectToBuild;

    private void Awake()
    {

    }
    public void Start()
    {
        Debug.Log("INSIDE DOOR START FUNCTION");

        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        if (!doorToUnlock.isLocked)
        {
            StartCoroutine(unlockBarrierDelay());
        }
    }

    public IEnumerator unlockBarrierDelay()
    {
        yield return new WaitForSeconds(0.2f);
        UnlockBarrier();
        yield return new WaitForSeconds(0.1f);
        objectToBuild.GetComponent<BarrierBuildObjct>().placeBuildObjectInFinishedState();

    }

    public void OnEnable()
    {
        //animator = GetComponent<Animator>();
        //if (isOpen) animator.SetBool("isOpen", true);
        //else animator.SetBool("isOpen", false);
    }

    public void SetCanTrigger(bool value)
    {
        canTrigger = value;
    }

    public void UnlockDoor()
    {
        ToggleDoor();
    }
    public void Update()
    {
        

    }

    public void SetRoomInfo(RoomInformation roomInfo_)
    {
        roomInfo = roomInfo_;
    }

    public void UpdateBarrierState()
    {
        //if(barrierGuid != null && roomInfo != null) roomInfo.UpdateBarrierState(barrierGuid, hasTriggered);
    }

    public bool Interact(Interactor interactor)
    {

        if (!hasTriggered && canTrigger && doorToUnlock.isLocked)
        {
            UnlockBarrier();
            requirementUI.removeMatFromInventory();
            StartCoroutine(animateCraft());
        }

        return true;
    }

    public void UnlockBarrier()
    {
        hasTriggered = true;
        if (barrierUI != null) barrierUI.SetActive(false);
        if (lockedUI != null) lockedUI.SetActive(false);
        if (craftGlobeUI != null) craftGlobeUI.SetActive(false);
        if (particleSystem != null) particleSystem.SetActive(false);
        if (objectToBuild != null) objectToBuild.SetActive(true);
    }

    public IEnumerator animateCraft()
    {
        BarrierBuildObjct buildObject = objectToBuild.GetComponent<BarrierBuildObjct>();
        camera.StartPan(buildObject.transform.position, true, true, 0.05f);
        yield return StartCoroutine(buildObject.animateBuildObject());
        ToggleDoor();
        camera.StartPan(doorToUnlock.transform.position, true, true, 0.05f);
        yield break;
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
        /*foreach (var item in inventory)
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
        }*/
    }

    public void ToggleDoor()
    {
        doorToUnlock.ToggleDoor(true);
        /*if (doorToUnlock.isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
            
        }*/
        //pdateDoorState();
    }

    public void OpenDoor()
    {
        doorToUnlock.OpenDoor();
    }

    public void CloseDoor()
    {
        doorToUnlock.CloseDoor();
    }

    public void ShowUI()
    {
        if (barrierUI != null)
        {
            if(doorToUnlock.isLocked) barrierUI.SetActive(true);
            if (lockedUI != null && doorToUnlock.isLocked)
            {
                
                lockedUI.SetActive(true);
            }
            if (craftGlobeUI != null && doorToUnlock.isLocked)
            {

                craftGlobeUI.SetActive(true);
            }
        }
    }

    public void HideUI()
    {
        if (barrierUI != null)
        {
            if(doorToUnlock.isLocked) barrierUI.SetActive(false);
            if (lockedUI != null && doorToUnlock.isLocked) lockedUI.SetActive(false);
            if (craftGlobeUI != null && doorToUnlock.isLocked) craftGlobeUI.SetActive(false);
        }
    }
}

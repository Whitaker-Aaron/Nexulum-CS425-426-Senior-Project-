using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class Chest : MonoBehaviour, i_Interactable
{
    private Animator animator;
    private MaterialScrollManager materialManager;
    private MenuManager menuManager;
    private Key key;
    public enum chestType { material, weapon, item }

    [Header("Locked Chest Options")]
    public bool isLocked;
    public string chestID;
    public bool isOpen;
    public GameObject chestUI;
    public GameObject depositUI;

    [Header("Items to Add")]
    public List<ItemList> itemsToAdd;

    public void Awake()
    {
        materialManager = FindObjectOfType<MaterialScrollManager>();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        animator = GetComponent<Animator>();
        if (materialManager == null)
        {
            Debug.LogError("MaterialScrollManager not found in the scene.");
        }
    }

    public void Start()
    {
        isOpen = false;

        if (itemsToAdd == null)
        {
            itemsToAdd = new List<ItemList>();
        }
    }

    public void Update()
    {
        if (itemsToAdd.Count <= 0)
        {
            if (isOpen && menuManager.chestMenuActive)
            {
                menuManager.closeChestMenu();
                HideUI();
            }
            isLocked = true;
            isOpen = false;
        }
    }

    public void UpdateChestItem(int index, int amountRemoved)
    {
        Debug.Log("Index: " + index + "Amount removed: " + amountRemoved);
        if(itemsToAdd[index] != null && amountRemoved < itemsToAdd[index].amount) itemsToAdd[index].amount -= amountRemoved;
        else itemsToAdd.RemoveAt(index);

    }

    public bool Interact(Interactor interactor)
    {
        if (materialManager == null)
        {
            Debug.LogError("Material Manager is not assigned.");
            return false;
        }

        if (!isLocked)
        {
            if (!isOpen)
            {
                isOpen = true;
                //ShowUI();
                Debug.Log("Opening the Chest");
                //animator.SetBool("isOpen", true);
                //if (itemsToAdd.Count > 0) AddItemToInventory(itemsToAdd[0]);
                menuManager.openChestMenu(this);

            }
            else
            {
                Debug.Log("Chest is already open");
            }
        }
        else
        {
            Debug.Log("Chest is locked");
        }

        return true;
    }

    public void OnMenuClosed()
    {
        if(itemsToAdd.Count > 0) isOpen = false;
    }

    public void ShowUI()
    {
        if (chestUI != null && !isOpen && itemsToAdd.Count > 0)
        {
            chestUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Chest UI is not assigned.");
        }
    }

    public void HideUI()
    {
        if (chestUI != null)
        {
            chestUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Chest UI is not assigned.");
        }
    }
    private void AddItemToInventory(ItemList item)
    {
        if (materialManager != null)
        {
            materialManager.AddToMaterialsInventory(item.material, item.amount);
            materialManager.UpdateScroll(item.material.materialTexture, item.material.materialName, item.amount);
            Debug.Log($"Added {item.material.name} (x{item.amount}) to inventory");
        }
    }

    private void AddAllItemsToInventory()
    {
        foreach (var item in itemsToAdd)
        {
            if (item != null)
            {
                AddItemToInventory(item);
            }
        }
    }
}

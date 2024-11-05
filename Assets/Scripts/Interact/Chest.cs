using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class Chest : MonoBehaviour, i_Interactable
{
    [SerializeField] private string prompt;

    private Animator animator;
    private MaterialScrollManager materialManager;
    private Key key;
    public enum chestType { material, weapon, item }

    [Header("Locked Chest Options")]
    public bool isLocked;
    public string chestID;
    public bool isOpen;
    public GameObject chestUI;

    [Header("Items to Add")]
    public List<ItemList> itemsToAdd;
    public string interactionPrompt => prompt;

    public void Awake()
    {
        materialManager = FindObjectOfType<MaterialScrollManager>();

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
                ShowUI();
                Debug.Log("Opening the Chest");
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

    public void ShowUI()
    {
        if (chestUI != null)
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

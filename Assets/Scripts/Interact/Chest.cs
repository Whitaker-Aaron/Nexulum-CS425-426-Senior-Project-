using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Ensure the materialManager is assigned to avoid null reference
        materialManager = FindObjectOfType<MaterialScrollManager>();

        if (materialManager == null)
        {
            Debug.LogError("MaterialScrollManager not found in the scene.");
        }
    }

    public void Start()
    {
        isOpen = false;

        // Check if itemsToAdd is initialized
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
                Debug.Log("Opening the Chest");

                foreach (var itemPair in itemsToAdd)
                {
                    if (itemPair != null)
                    {
                        materialManager.AddToMaterialsInventory(itemPair.material, itemPair.amount);
                    }
                    else
                    {
                        Debug.LogWarning("ItemPair in itemsToAdd is null.");
                    }
                }

                isOpen = true;
            }
            else
            {
                Debug.Log("Chest is already open");
            }
        }
        else
        {
            Debug.Log("Chest is locked");
            // Further logic for locked chest can be added here
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

    public void HideUi()
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
}

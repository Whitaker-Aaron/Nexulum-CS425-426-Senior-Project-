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

    public void Start()
    {
        isOpen = false;
    }

    public bool Interact(Interactor interactor)
    {
        if (!isLocked)
        {
            if (isOpen == false)
            {
                Debug.Log("Opening the Chest");

                foreach (var itemPair in itemsToAdd)
                {
                    materialManager.AddToMaterialsInventory(itemPair.material, itemPair.amount);
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
            

            //if (chestID == keyID)
            //{

            //}
        }

        return true;
    }

    public void ShowUI()
    {
        if (chestUI != null)
        {
            chestUI.SetActive(true);
        }
    }
    public void HideUi()
    {
        if (chestUI != null)
        {
            chestUI.SetActive(false);
        }
    }
}

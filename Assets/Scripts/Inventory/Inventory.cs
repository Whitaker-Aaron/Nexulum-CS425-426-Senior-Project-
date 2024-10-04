using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
//using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    //Makes the class STATIC
    //This class will need to interact with almost every aspect of the game,
    //so it needs to be static
    public static Inventory instance { get; set; }
    public GameObject[] inventoryItems;
    public InventoryManager inventoryMan;
    private void Awake()
    {
        instance = this;
    }

    //This function recieves the number of a certain item in the players inventory
    public int GetItemCount(Item item)
    {
        int total = 0;
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] != null)
            {
                if (inventoryItems[i].GetComponent<Item_Script>().heldProperties.itemName == item.itemName)
                {
                    total += inventoryItems[i].GetComponent<Item_Script>().heldProperties.currentAmount;
                }
            }
        }
        return total;
    }

    public void AddItem(GameObject item, int amount)
    {
        if(GetItemCount(item.GetComponent<Item_Script>().itemObject) > 0)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] != null)
                {
                    Item inventoryItem = inventoryItems[i].GetComponent<Item_Script>().heldProperties;
                    Item inventoryItemObject = item.GetComponent<Item_Script>().itemObject;

                    if (inventoryItem.itemName == inventoryItemObject.itemName)
                    {
                        if (inventoryItem.currentAmount < inventoryItem.maxStackAmount)
                        {
                            int remainingStack = inventoryItem.maxStackAmount - inventoryItem.currentAmount;

                            if (remainingStack >= amount)
                            {
                                inventoryItem.currentAmount += amount;
                                inventoryMan.SetList();
                                return;
                            }
                            else
                            {
                                inventoryItem.currentAmount = inventoryItem.maxStackAmount;
                                amount -= remainingStack;
                            }
                        }
                    }
                }
            }
        }
        if (amount > 0)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == null)
                {
                    GameObject itemObjects = item;
                    itemObjects.GetComponent<Item_Script>().heldProperties.currentAmount = amount;
                    itemObjects.GetComponent<Item_Script>().SetHeldProperties(item.GetComponent<Item_Script>().itemObject);
                    inventoryItems[i] = itemObjects;
                    inventoryMan.SetList();
                    return;
                }
            }
        }

    }

    public void RemoveItem(Item item, int amount)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] != null)
            {
                Item_Script current = inventoryItems[i].GetComponent<Item_Script>();
                
                if (current.heldProperties.itemName == item.itemName)
                {
                    if (amount <= current.heldProperties.currentAmount)
                    {
                        current.heldProperties.currentAmount -= amount;

                        //long list of loops to check for the same item then delete it
                        if (current.heldProperties.currentAmount == 0)
                        {
                            inventoryItems[i] = null;
                        }
                        inventoryMan.SetList();
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < inventoryItems.Length; j++)
                        {
                            if (inventoryItems[j] != null)
                            {
                                if (inventoryItems[j].GetComponent<Item_Script>().heldProperties.itemName == item.itemName)
                                {
                                    int currentItemCount = inventoryItems[j].GetComponent<Item_Script>().heldProperties.currentAmount;

                                    if (amount <= currentItemCount)
                                    {
                                        inventoryItems[j].GetComponent<Item_Script>().heldProperties.currentAmount -= amount;
                                        
                                        if (inventoryItems[j].GetComponent<Item_Script>().heldProperties.currentAmount == 0)
                                        {
                                            inventoryItems[j] = null;
                                        }
                                        inventoryMan.SetList();
                                        return;
                                    }
                                    else if (amount > currentItemCount)
                                    {
                                        inventoryItems[j] = null;
                                        amount -= currentItemCount;
                                        if (amount <= 0)
                                        {
                                            return;
                                        }
                                    }
                                    inventoryMan.SetList();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

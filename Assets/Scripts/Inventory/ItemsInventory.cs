using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInventory : MonoBehaviour, SaveSystemInterface
{
    // Start is called before the first frame update
    PlayerItem[] inventory = new PlayerItem[100];
    int nextFreeIndex = 0;
    ItemsList itemsList;
    void Start()
    {
        itemsList = GameObject.Find("ItemsList").GetComponent<ItemsList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData(ref SaveData data)
    {
        for (int index = 0; index < nextFreeIndex; index++)
        {
            data.itemInventory[index].itemName = inventory[index].itemName;
            data.itemInventory[index].itemAmount = inventory[index].itemAmount;
        }
    }

    public void LoadData(SaveData data)
    {

        for (int index = 0; index < inventory.Length; index++)
        {
            if (data.itemInventory[index].itemName != "" && data.itemInventory[index] != null && !data.isNewFile)
            {
                AddToInventory(itemsList.ReturnItem(data.itemInventory[index].itemName), data.itemInventory[index].itemAmount);
            }

        }
    }


    public void AddToInventory(PlayerItem itemToAdd, int amount)
    {
        bool itemFound = false;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].itemName == itemToAdd.itemName)
                {
                    Debug.Log(itemToAdd.itemName + " is already in inventory!");
                    if (!(inventory[i].itemAmount + amount > inventory[i].maxItemAmount))
                    {
                        inventory[i].itemAmount += amount;
                        Debug.Log("Amount of " + itemToAdd.itemName + " inside inventory: " + inventory[i].itemAmount);
                    }
                    else
                    {
                        inventory[i].itemAmount = inventory[i].maxItemAmount;
                    }

                    itemFound = true;
                }

            }

        }
        if (!itemFound)
        {
            inventory[nextFreeIndex] = itemToAdd;
            if (amount >= inventory[nextFreeIndex].maxItemAmount)
            {
                inventory[nextFreeIndex].itemAmount = inventory[nextFreeIndex].maxItemAmount;

            }
            else
            {
                inventory[nextFreeIndex].itemAmount = amount;
            }
            Debug.Log(inventory[nextFreeIndex].itemName + " has been added to inventory!");
            nextFreeIndex++;

        }
    }

    public void RemoveFromInventory(PlayerItem itemToRemove, int amountToRemove)
    {

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].itemName == itemToRemove.itemName)
                {
                    Debug.Log(itemToRemove.itemName + " is already in inventory!");
                    if (inventory[i].itemAmount - amountToRemove > 0)
                    {
                        inventory[i].itemAmount -= amountToRemove;
                        return;
                    }
                    else
                    {
                        for (int j = i; j < inventory.Length; j++)
                        {
                            if (inventory[j + 1] != null)
                            {
                                inventory[j] = inventory[j + 1];
                            }
                            else
                            {
                                inventory[j] = null;
                                nextFreeIndex--;
                                return;
                            }

                        }
                        return;
                    }
                }

            }

        }

    }

    public PlayerItem[] GetInventory()
    {
        return inventory;
    }


}

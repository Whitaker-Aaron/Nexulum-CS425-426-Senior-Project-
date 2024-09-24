using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInventory : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerItem[] inventory = new PlayerItem[100];
    int nextFreeIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(PlayerItem itemToAdd)
    {
        if (nextFreeIndex != inventory.Length)
        {
            Debug.Log(itemToAdd.itemName + " added to inventory!");
            inventory[nextFreeIndex] = itemToAdd;
            nextFreeIndex++;
        }


        for (int i = 0; i < nextFreeIndex; i++)
        {
            Debug.Log("Inventory slot " + i + ": " + inventory[i].itemName);
        }


    }


}

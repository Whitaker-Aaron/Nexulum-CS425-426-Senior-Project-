using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    ItemsInventory itemsInventory;
    CharacterBase characterReference;
    // Start is called before the first frame update
    void Start()
    {
       
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        itemsInventory = GameObject.Find("ItemsInventory").GetComponent<ItemsInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(PlayerItem itemToAdd, int amount)
    {
        itemsInventory.GetComponent<ItemsInventory>().AddToInventory(itemToAdd, amount);
    }

    public void RemoveFromInventory(PlayerItem itemToRemove, int amount)
    {
        itemsInventory.GetComponent<ItemsInventory>().RemoveFromInventory(itemToRemove, amount);
    }

    public PlayerItem[] GetInventory()
    {
        return itemsInventory.GetInventory();
    }

    public bool FindItemAndAdd(string itemName)
    {
        var items = GameObject.Find("itemsEquipList").GetComponent<ItemsEquipList>().allItems;
        if (items != null && items.Count > 0)
        {
            Debug.Log("Inside Weapons Manager if statement");
            foreach (var item in items)
            {
                if (itemName == item.name)
                {
                    AddToInventory(item, 1);
                    return true;
                }
            }
        }
        return false;

    }

    public void ExecuteItemLogic(PlayerItem item)
    {
        switch (item.itemType)
        {
            case PlayerItem.ItemType.Health:
                characterReference.restoreHealth((int)item.statAmount);
                RemoveFromInventory(item, 1);
                break;
            case PlayerItem.ItemType.Projectile:
                break;
            case PlayerItem.ItemType.Stat:
                break;
        }
        GameObject.Find("MenuManager").GetComponent<MenuManager>().CloseMenu();
    }
}

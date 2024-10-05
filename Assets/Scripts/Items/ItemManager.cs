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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(PlayerItem itemToAdd)
    {
        itemsInventory.GetComponent<ItemsInventory>().AddToInventory(itemToAdd);
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
                    AddToInventory(item);
                    return true;
                }
            }
        }
        return false;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInventory : MonoBehaviour, SaveSystemInterface
{
    // Start is called before the first frame update
    Rune[] inventory = new Rune[100];
    RuneList runeList;
    int nextFreeIndex = 0;
    //public Rune equippedRune;
    void Start()
    {
        runeList = GameObject.Find("RunesList").GetComponent<RuneList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(Rune runeToAdd)
    {
        if (nextFreeIndex != inventory.Length)
        {
            Debug.Log(runeToAdd.runeName + " added to inventory!");
            inventory[nextFreeIndex] = runeToAdd;
            nextFreeIndex++;
        }


        for (int i = 0; i < nextFreeIndex; i++)
        {
            Debug.Log("Inventory slot " + i + ": " + inventory[i].runeName);
        }


    }

    public Rune[] GetInventory()
    {
        return inventory;
    }

    public void SaveData(ref SaveData data)
    {
        for (int index = 0; index < nextFreeIndex; index++)
        {
            data.runeInventory[index] = inventory[index].runeName;
        }
    }
    public void LoadData(SaveData data)
    {
        
        for (int index = 0; index < inventory.Length; index++)
        {
            if (data.runeInventory[index] != "" && data.runeInventory[index] != null)
            {
                AddToInventory(runeList.ReturnRune(data.runeInventory[index]));
            }

        }
    }

}

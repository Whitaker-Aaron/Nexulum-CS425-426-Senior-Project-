using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialsInventory : MonoBehaviour
{
    // Start is called before the first frame update
    CraftMaterial[] inventory = new CraftMaterial[50];
    int nextFreeIndex = 0;
    void Start()
    {
        //Will later initialize this variable with save data.
        nextFreeIndex = 0;
        Debug.Log("Inventory size: " + inventory.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(CraftMaterial materialToAdd)
    {
        bool itemFound = false;
        for(int i =0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].materialName == materialToAdd.materialName)
                {
                    Debug.Log(materialToAdd.materialName + " is already in inventory!");
                    itemFound = true;
                }

            }
            
        }
        if (!itemFound)
        {
            inventory[nextFreeIndex] = materialToAdd;
            Debug.Log(inventory[nextFreeIndex].materialName + " has been added to inventory!");
            nextFreeIndex++;

        }
    }
}

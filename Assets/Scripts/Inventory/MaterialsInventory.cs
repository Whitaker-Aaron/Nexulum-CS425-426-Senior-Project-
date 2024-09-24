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
                    if (inventory[i].currentAmount != inventory[i].maxMaterialAmount)
                    {
                        inventory[i].currentAmount++;
                        Debug.Log("Amount of " + materialToAdd.materialName + " inside inventory: " + inventory[i].currentAmount);
                    }
                    
                    itemFound = true;
                }

            }
            
        }
        if (!itemFound)
        {
            inventory[nextFreeIndex] = materialToAdd;
            inventory[nextFreeIndex].currentAmount = 1;
            Debug.Log(inventory[nextFreeIndex].materialName + " has been added to inventory!");
            nextFreeIndex++;

        }
    }

    public void RemoveFromInventory(CraftMaterial materialToRemove, int amountToRemove)
    {

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].materialName == materialToRemove.materialName)
                {
                    Debug.Log(materialToRemove.materialName + " is already in inventory!");
                    if (inventory[i].currentAmount - amountToRemove > 0)
                    {
                        inventory[i].currentAmount -= amountToRemove;
                        return;
                    }
                    else
                    {
                        for(int j = i;  j < inventory.Length; j++)
                        {
                            if (inventory[j + 1] != null)
                            {
                                inventory[j] = inventory[j + 1];
                            }
                            else {
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

    public CraftMaterial[] GetInventory() {
        return inventory;
    }

    public int GetCurrentInventorySize()
    {
        return nextFreeIndex;
    }

    public int GetMaxInventorySize()
    {
        return inventory.Length;
    }

    public int GetMaterialAmount(CraftMaterial specifiedMaterial)
    {
        for(int i = 0; i < nextFreeIndex; i++)
        {
            if(specifiedMaterial.materialName == inventory[i].materialName)
            {
                return inventory[i].currentAmount;
            }
        }
        return 0;
   
    }

}

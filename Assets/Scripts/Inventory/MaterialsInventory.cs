using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialsInventory : MonoBehaviour, SaveSystemInterface
{
    // Start is called before the first frame update
    CraftMaterial[] inventory = new CraftMaterial[50];
    int nextFreeIndex = 0;
    void Start()
    {
        //Will later initialize this variable with save data.
        
        Debug.Log("Inventory size: " + nextFreeIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData(ref SaveData data)
    {
        
        for(int index = 0; index < inventory.Length; index++) { 
            if(inventory[index] != null)
            {
                var saveData = data.materialInventory[index];
                saveData = new CraftMaterialSaveData();
                saveData.materialName = inventory[index].materialName;
                saveData.currentAmount = inventory[index].currentAmount;
                saveData.maxMaterialAmount = inventory[index].maxMaterialAmount;
                saveData.dropRate = inventory[index].dropRate;
                saveData.dropAmount = inventory[index].dropAmount;
                data.materialInventory[index] = saveData;
            }
            else
            {
                break;
            }

        }
    }
    
        
    public void LoadData(SaveData data)
    {
        var materialList = GameObject.Find("MaterialsList").GetComponent<MaterialList>();
        for (int index = 0; index < inventory.Length; index++)
        {
            if (data.materialInventory[index] != null && data.materialInventory[index].materialName != "")
            {
                inventory[index] = new CraftMaterial();
                inventory[index].materialName = data.materialInventory[index].materialName;
                inventory[index].currentAmount = data.materialInventory[index].currentAmount;
                inventory[index].maxMaterialAmount = data.materialInventory[index].maxMaterialAmount;
                inventory[index].dropRate = data.materialInventory[index].dropRate;
                inventory[index].dropAmount = data.materialInventory[index].dropAmount;
                inventory[index].materialTexture = materialList.ReturnTexture(inventory[index].materialName);

                nextFreeIndex++;
            }
            else
            {
                break;
            }

        }
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

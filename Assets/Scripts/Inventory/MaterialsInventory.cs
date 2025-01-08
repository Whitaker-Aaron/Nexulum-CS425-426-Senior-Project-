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
    CraftMaterial[] totalInventory = new CraftMaterial[150];
    int nextFreeIndex = 0;
    int totalNextFreeIndex = 0;
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
                saveData.maxTotalMaterialAmount = inventory[index].maxTotalMaterialAmount;
                saveData.currentTotalAmount = inventory[index].currentTotalAmount;
                saveData.maxMaterialAmount = inventory[index].maxMaterialAmount;
                saveData.dropRate = inventory[index].dropRate;
                saveData.dropAmount = inventory[index].dropAmount;
                data.materialInventory[index] = saveData;
            }
            else
            {
                var saveData = new CraftMaterialSaveData();
                saveData.materialName = "";
                saveData.currentTotalAmount = 0;
                saveData.currentAmount = 0;
                saveData.maxTotalMaterialAmount = 0;
                saveData.maxMaterialAmount = 0;
                saveData.dropRate = 0.0f;
                saveData.dropAmount = 0;
                data.materialInventory[index] = saveData;
            }

        }

        for (int index = 0; index < totalInventory.Length; index++)
        {
            if (totalInventory[index] != null)
            {
                var saveData = data.totalMaterialInventory[index];
                saveData = new CraftMaterialSaveData();
                saveData.materialName = totalInventory[index].materialName;
                saveData.currentTotalAmount = totalInventory[index].currentTotalAmount;
                saveData.currentAmount = totalInventory[index].currentAmount;
                saveData.maxMaterialAmount = totalInventory[index].maxMaterialAmount;
                saveData.maxTotalMaterialAmount = totalInventory[index].maxTotalMaterialAmount;
                saveData.dropRate = totalInventory[index].dropRate;
                saveData.dropAmount = totalInventory[index].dropAmount;
                data.totalMaterialInventory[index] = saveData;
            }
            else
            {
                var saveData = new CraftMaterialSaveData();
                saveData.materialName = "";
                saveData.currentTotalAmount = 0;
                saveData.currentAmount = 0;
                saveData.maxTotalMaterialAmount = 0;
                saveData.maxMaterialAmount = 0;
                saveData.dropRate = 0.0f;
                saveData.dropAmount = 0;
                data.totalMaterialInventory[index] = saveData;
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
                inventory[index].currentTotalAmount = data.materialInventory[index].currentTotalAmount;
                inventory[index].maxMaterialAmount = data.materialInventory[index].maxMaterialAmount;
                inventory[index].maxTotalMaterialAmount = data.materialInventory[index].maxTotalMaterialAmount;
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

        for (int index = 0; index < totalInventory.Length; index++)
        {
            if (data.totalMaterialInventory[index] != null && data.totalMaterialInventory[index].materialName != "")
            {
                totalInventory[index] = new CraftMaterial();
                totalInventory[index].materialName = data.totalMaterialInventory[index].materialName;
                totalInventory[index].currentTotalAmount = data.totalMaterialInventory[index].currentTotalAmount;
                totalInventory[index].currentAmount = data.totalMaterialInventory[index].currentAmount;
                totalInventory[index].maxMaterialAmount = data.totalMaterialInventory[index].maxMaterialAmount;
                totalInventory[index].maxTotalMaterialAmount = data.totalMaterialInventory[index].maxTotalMaterialAmount;
                totalInventory[index].dropRate = data.totalMaterialInventory[index].dropRate;
                totalInventory[index].dropAmount = data.totalMaterialInventory[index].dropAmount;
                totalInventory[index].materialTexture = materialList.ReturnTexture(totalInventory[index].materialName);

                totalNextFreeIndex++;
            }
            else
            {
                break;
            }

        }
    }


    public void AddToInventory(CraftMaterial materialToAdd, int amount)
    {
        bool itemFound = false;
        for(int i =0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].materialName == materialToAdd.materialName)
                {
                    Debug.Log(materialToAdd.materialName + " is already in inventory!");
                    if (!(inventory[i].currentAmount + amount > inventory[i].maxMaterialAmount))
                    {
                        inventory[i].currentAmount += amount;
                        Debug.Log("Amount of " + materialToAdd.materialName + " inside inventory: " + inventory[i].currentAmount);
                    }
                    else
                    {
                        inventory[i].currentAmount = inventory[i].maxMaterialAmount;
                    }

                    itemFound = true;
                }

            }
            
        }
        if (!itemFound)
        {
            inventory[nextFreeIndex] = materialToAdd;
            if(amount >= inventory[nextFreeIndex].maxMaterialAmount)
            {
                inventory[nextFreeIndex].currentAmount = inventory[nextFreeIndex].maxMaterialAmount;
                
            }
            else
            {
                inventory[nextFreeIndex].currentAmount = amount;
            }
            Debug.Log(inventory[nextFreeIndex].materialName + " has been added to inventory!");
            nextFreeIndex++;

        }
    }

    public void AddToTotalInventory(CraftMaterial materialToAdd, int amount)
    {
        bool itemFound = false;
        for (int i = 0; i < totalInventory.Length; i++)
        {
            if (totalInventory[i] != null)
            {
                if (totalInventory[i].materialName == materialToAdd.materialName)
                {
                    Debug.Log(materialToAdd.materialName + " is already in inventory!");
                    if (!(totalInventory[i].currentTotalAmount + amount > totalInventory[i].maxTotalMaterialAmount))
                    {
                        totalInventory[i].currentTotalAmount += amount;
                       
                    }
                    else
                    {
                        totalInventory[i].currentTotalAmount = totalInventory[i].maxTotalMaterialAmount;
                    }

                    itemFound = true;
                }

            }

        }
        if (!itemFound)
        {
            totalInventory[totalNextFreeIndex] = materialToAdd;
            totalInventory[totalNextFreeIndex].currentTotalAmount = amount;
            Debug.Log(totalInventory[totalNextFreeIndex].materialName + " has been added to total inventory!");
            totalNextFreeIndex++;

        }
    }

    public void ClearInventory()
    {
        Debug.Log("Clearing inventory from materials inventory");
        Array.Clear(inventory, 0, inventory.Length);
        nextFreeIndex = 0;
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

    public void RemoveFromTotalInventory(CraftMaterial materialToRemove, int amountToRemove)
    {

        for (int i = 0; i < totalInventory.Length; i++)
        {
            if (totalInventory[i] != null)
            {
                if (totalInventory[i].materialName == materialToRemove.materialName)
                {
                    Debug.Log(materialToRemove.materialName + " is already in inventory!");
                    if (totalInventory[i].currentTotalAmount - amountToRemove > 0)
                    {
                        totalInventory[i].currentTotalAmount -= amountToRemove;
                        return;
                    }
                    else
                    {
                        for (int j = i; j < totalInventory.Length; j++)
                        {
                            if (totalInventory[j + 1] != null)
                            {
                                totalInventory[j] = totalInventory[j + 1];
                            }
                            else
                            {
                                totalInventory[j] = null;
                                totalNextFreeIndex--;
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

    public CraftMaterial[] GetTotalInventory()
    {
        return totalInventory;
    }

    public int GetCurrentInventorySize()
    {
        return nextFreeIndex;
    }

    public int GetCurrentTotalInventorySize()
    {
        return totalNextFreeIndex;
    }

    public int GetMaxInventorySize()
    {
        return inventory.Length;
    }

    public int GetMaxTotalInventorySize()
    {
        return totalInventory.Length;
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

    public int GetTotalMaterialAmount(CraftMaterial specifiedMaterial)
    {
        for (int i = 0; i < totalNextFreeIndex; i++)
        {
            if (specifiedMaterial.materialName == totalInventory[i].materialName)
            {
                return totalInventory[i].currentAmount;
            }
        }
        return 0;
    }

    public CraftMaterial[] GetFirstThreeMat()
    {
        CraftMaterial[] mats = new CraftMaterial[3];
        for(int i = 0; i < 3; i++)
        {
            if (inventory[i] != null)
            {
                mats[i] = inventory[i];
                Debug.Log(mats[i].materialName);
            }
            else
            {
                Debug.Log(i + " is null");
                mats[i] = null;
            }
        }
        return mats;
    }

}

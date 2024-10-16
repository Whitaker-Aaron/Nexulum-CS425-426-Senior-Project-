using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Start is called before the first frame update
    public int maxPlayerHealth;
    public int playerHealth;
    public CraftMaterialSaveData[] materialInventory;

    public SaveData()
    {
        maxPlayerHealth = 100;
        playerHealth = 100;
        materialInventory = new CraftMaterialSaveData[50];
    }
}

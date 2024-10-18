using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftMaterialSaveData
{
    public string materialName;
    public int currentAmount;
    public int maxMaterialAmount;
    //Please enter a value from 0-1 when creating Craft Material objects. 
    public float dropRate;
    //Value that determines the max amount an enemy can drop of item.
    public int dropAmount;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft Material", fileName = "newCraftMaterial")]
public class CraftMaterial : ScriptableObject
{
    public string materialName;
    public int currentAmount;
    public int maxMaterialAmount;
    public int currentTotalAmount;
    public int maxTotalMaterialAmount;
    //Please enter a value from 0-1 when creating Craft Material objects. 
    public float dropRate;
    //Value that determines the max amount an enemy can drop of item.
    public int dropAmount;
    public Texture materialTexture;
}

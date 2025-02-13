using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "CraftRecipe", fileName = "newCraftRecipe")]
public class CraftRecipe : ScriptableObject
{

    [SerializedDictionary("Material", "Amount")]
    public SerializedDictionary<CraftMaterial, int> requiredMaterials;
    public CraftTypes type;
    public string recipeName;
    public float shopCost;
    public bool hasCrafted = false;

    public enum CraftTypes
    {
        Weapon,
        Item,
        Rune
    }


}

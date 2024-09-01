using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft Material", fileName = "newCraftMaterial")]
public class CraftMaterial : ScriptableObject
{
    public string materialName;
    public int currentAmount;
    public int maxMaterialAmount;
    public Texture materialTexture;
}

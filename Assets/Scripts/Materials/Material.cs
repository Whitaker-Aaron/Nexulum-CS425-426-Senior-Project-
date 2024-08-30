using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Material", fileName = "newMaterial")]
public class Item : ScriptableObject
{
    public string materialName;
    public int currentAmount;
    public int maxMaterialAmount;
    public Texture materialTexture;
}

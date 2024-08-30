using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "newItem")]
public class Item : ScriptableObject
{
    public string itemName;
    public int currentAmount;
    public int maxStackAmount;
    public Texture itemTexture;
}
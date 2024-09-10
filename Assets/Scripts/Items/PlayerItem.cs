using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "PlayerItem", fileName = "newItem")]
public class PlayerItem : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] ItemType itemType;
    [SerializeField] Texture itemTexture;
    [SerializeField] public CraftRecipe itemRecipe;



    public enum ItemType
    {
        Projectile,
        Stat,
    }
}

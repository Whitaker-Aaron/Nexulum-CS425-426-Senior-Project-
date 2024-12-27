using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "PlayerItem", fileName = "newItem")]
public class PlayerItem : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public ItemType itemType;
    [SerializeField] public string itemDescription;
    [SerializeField] public int itemAmount;
    [SerializeField] public int maxItemAmount;
    [SerializeField] public float statAmount;
    [SerializeField] Texture itemTexture;
    [SerializeField] public CraftRecipe itemRecipe;



    public enum ItemType
    {
        Projectile,
        Stat,
        Health
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rune", fileName = "newRune")]
public class Rune : ScriptableObject
{
    public string runeName;
    public string runeDescription;
    public string runeEffect;
    public Texture runeTexture;
    public RuneType runeType;
    public CraftRecipe runeRecipe; 
    
    //more variables needed for type of rune and effect
    public enum RuneType {
        Weapon,
        Health,
        Defense,
        Buff,
        Projectile
    }
}
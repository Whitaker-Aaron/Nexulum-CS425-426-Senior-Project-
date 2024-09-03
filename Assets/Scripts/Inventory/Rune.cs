using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rune", fileName = "newRune")]
public class Rune : ScriptableObject
{
    public string runeName;
    public int currentAmount;
    public int maxStackAmount;
    public Texture runeTexture;
    
    //more variables needed for type of rune and effect
}
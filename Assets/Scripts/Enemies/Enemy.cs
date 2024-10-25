using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "newEnemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public int baseHealth;
    public int droppedExperience;
    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "WeaponClass", fileName = "newClass")]
public class WeaponClass : ScriptableObject
{
    
    public WeaponBase currentWeapon;
    public WeaponBase.weaponClassTypes classType;

    public float totalExp;
    public float numSkillPoints;
    public int currentLvl; 

    [SerializedDictionary("Experience", "Level")]
    public SerializedDictionary<float, int> levelUnlocks;

    public void updateExperience(float enemyExp)
    {
        totalExp += enemyExp;
        foreach (var item in levelUnlocks)
        {
            if (totalExp >= item.Key && currentLvl < item.Value)
            {
                currentLvl = item.Value;
                numSkillPoints += 1;
            }
        }
    }
    

}


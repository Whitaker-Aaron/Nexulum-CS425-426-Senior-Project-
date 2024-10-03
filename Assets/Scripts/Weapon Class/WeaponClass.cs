using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponClass", fileName = "newClass")]
public class WeaponClass : ScriptableObject
{
    
    public WeaponBase currentWeapon;
    public WeaponBase.weaponClassTypes classType;

}


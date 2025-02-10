using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsList : MonoBehaviour
{
    [SerializedDictionary("WeaponName", "Weapon")]
    public SerializedDictionary<string, WeaponBase> weaponLookup;

    public WeaponBase ReturnWeapon(string weaponName)
    {
        return weaponLookup[weaponName];
    }

}

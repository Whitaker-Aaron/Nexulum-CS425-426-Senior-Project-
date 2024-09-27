using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "WeaponBase", fileName = "newWeapon")]
public class WeaponBase : ScriptableObject
{
    [SerializeField] public string weaponName;
    [SerializeField] public string weaponDescription;
    [SerializeField] public int weaponAttack;
    [SerializeField] public weaponClassTypes weaponClassType;
    [SerializeField] public GameObject weaponMesh;
    [SerializeField] public CraftRecipe weaponRecipe;



    public enum weaponClassTypes
    {
        Knight,
        Gunner,
        Engineer
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInventory : MonoBehaviour
{
    WeaponBase[] inventory = new WeaponBase[100];
    int nextFreeIndex = 0;
    //public WeaponBase equippedWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(WeaponBase weaponToAdd)
    {
        if(nextFreeIndex != inventory.Length) {
            inventory[nextFreeIndex] = weaponToAdd;
            nextFreeIndex++;
        }
        
    }


}

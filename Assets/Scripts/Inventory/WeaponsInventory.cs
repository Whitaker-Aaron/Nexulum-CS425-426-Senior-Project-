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
            Debug.Log(weaponToAdd.weaponName + " added to inventory!");
            inventory[nextFreeIndex] = weaponToAdd;
            nextFreeIndex++;
        }


        for(int i = 0; i < nextFreeIndex; i++)
        {
            Debug.Log("Inventory slot " + i + ": " + inventory[i].weaponName);
        }

        
    }

    public WeaponBase[] GetInventory()
    {
        return inventory;
    }


}

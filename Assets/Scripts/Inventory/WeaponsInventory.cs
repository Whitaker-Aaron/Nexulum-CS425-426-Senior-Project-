using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInventory : MonoBehaviour, SaveSystemInterface
{
    WeaponBase[] inventory = new WeaponBase[100];
    WeaponsList weaponsList;
    int nextFreeIndex = 0;
    //public WeaponBase equippedWeapon;
    // Start is called before the first frame update
    void Start()
    {
        weaponsList = GameObject.Find("WeaponsList").GetComponent<WeaponsList>();
    }

    private void Awake()
    {
        
    }

    public void SaveData(ref SaveData data)
    {
        for (int index = 0; index < nextFreeIndex; index++)
        {
            data.weaponInventory[index] = inventory[index].weaponName;
        }
    }

    public void LoadData(SaveData data)
    {
         
        for (int index = 0; index < inventory.Length; index++)
        {
            if (data.weaponInventory[index] != "" && data.weaponInventory[index] != null)
            {
                AddToInventory(weaponsList.ReturnWeapon(data.weaponInventory[index]));
            }
            
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Start is called before the first frame update
    public int maxPlayerHealth;
    public int playerHealth;
    public string equippedWeapon;
    public string[] equippedRunes;

    public CraftMaterialSaveData[] materialInventory;
    public CraftMaterialSaveData[] totalMaterialInventory;
    public WeaponClassSaveData[] weaponClasses;

    public string[] weaponInventory;
    public string[] runeInventory;

    public SaveData()
    {
        maxPlayerHealth = 100;
        playerHealth = 100;
        equippedWeapon = "Dagger";

        materialInventory = new CraftMaterialSaveData[50];
        totalMaterialInventory = new CraftMaterialSaveData[150];
        weaponClasses = new WeaponClassSaveData[3];
        weaponClasses[0] = new WeaponClassSaveData();
        weaponClasses[1] = new WeaponClassSaveData();
        weaponClasses[2] = new WeaponClassSaveData();

        weaponInventory = new string[100];
        weaponInventory[0] = "Dagger";
        weaponInventory[1] = "Rifle";
        weaponInventory[2] = "Pistol";

        runeInventory = new string[100];
        runeInventory[0] = "Fire";
        runeInventory[1] = "Ice";
        runeInventory[2] = "Earth";

        equippedRunes = new string[3];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Start is called before the first frame update
    public bool isNewFile = true;
    public int maxPlayerHealth;
    public int playerHealth;
    public int florentineAmount;
    public string equippedWeapon;
    public string[] equippedRunes;

    public CraftMaterialSaveData[] materialInventory;
    public CraftMaterialSaveData[] totalMaterialInventory;
    public WeaponClassSaveData[] weaponClasses;
    public List<RoomSaveData> roomData = new List<RoomSaveData>();
    public CraftRecipeSaveData[] allRecipes;
    public CraftRecipeSaveData[] accessibleRecipes;


    public string[] weaponInventory;
    public string[] runeInventory;
    public PlayerItemSaveData[] itemInventory;

    public SaveData()
    {
        maxPlayerHealth = 100;
        playerHealth = 100;
        florentineAmount = 0;
        equippedWeapon = "Dagger";

        materialInventory = new CraftMaterialSaveData[50];
        totalMaterialInventory = new CraftMaterialSaveData[150];
        allRecipes = new CraftRecipeSaveData[150];
        accessibleRecipes = new CraftRecipeSaveData[150];

        //roomData = new RoomSaveData[50];
        weaponClasses = new WeaponClassSaveData[3];
        weaponClasses[0] = new WeaponClassSaveData();
        weaponClasses[1] = new WeaponClassSaveData();
        weaponClasses[2] = new WeaponClassSaveData();

        weaponInventory = new string[100];
        weaponInventory[0] = "Dagger";
        weaponInventory[1] = "Rifle";
        weaponInventory[2] = "Pistol";

        weaponClasses[0].currentWeapon = weaponInventory[0];
        weaponClasses[1].currentWeapon = weaponInventory[1];
        weaponClasses[2].currentWeapon = weaponInventory[2];

        runeInventory = new string[100];
        runeInventory[0] = "Fire";
        runeInventory[1] = "Ice";
        runeInventory[2] = "Earth";
        runeInventory[4] = "Regen";

        itemInventory = new PlayerItemSaveData[100];
        for(int i = 0; i < itemInventory.Length; i++)
        {
            itemInventory[i] = new PlayerItemSaveData();
        }

        equippedRunes = new string[3];
    }
}

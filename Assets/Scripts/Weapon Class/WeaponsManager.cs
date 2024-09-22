using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject weaponsList;
    GameObject weaponInventory;
    GameObject weaponPrefab;
    CharacterBase characterReference;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        weaponInventory = GameObject.Find("WeaponsInventory");
        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        Instantiate(weaponPrefab, characterReference.hand);
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
        weaponInventory.GetComponent<WeaponsInventory>().AddToInventory(weaponToAdd);
    }

    public void ChangeWeapon(WeaponBase newWeapon)
    {
        characterReference.GetWeaponClass().currentWeapon = newWeapon;
    }

    public bool FindWeaponAndAdd(string weaponName)
    {
        var weapons = GameObject.Find("weaponEquipList").GetComponent<WeaponEquipList>().allWeapons;
        Debug.Log("Inside Weapons Manager FindWeaponAndAdd");
        if (weapons != null && weapons.Count > 0)
        {
            Debug.Log("Inside Weapons Manager if statement");
            foreach (var weapon in weapons)
            {
                if (weaponName == weapon.weaponName)
                {
                    AddToInventory(weapon);
                    return true;
                }
            }
        }
        return false;
        
    }
}

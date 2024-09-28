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
    GameObject currentWeapon;
    GameObject currentTool;
    GameObject toolPrefab;
    GameObject currentShield, shieldPrefab;
    CharacterBase characterReference;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        weaponInventory = GameObject.Find("WeaponsInventory");
        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
            currentWeapon = Instantiate(weaponPrefab, characterReference.wrist);
        if(characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            toolPrefab = characterReference.engineerTool.weaponMesh;
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentTool = Instantiate(toolPrefab, characterReference.leftHand);
        }
        if(characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentShield = Instantiate(shieldPrefab, characterReference.leftForearm);
        }
            
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
        //characterReference.GetWeaponClass().currentWeapon = newWeapon;
        characterReference.UpdateWeapon(newWeapon);
        GameObject.Destroy(currentWeapon);
        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        if (newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
            currentWeapon = Instantiate(weaponPrefab, characterReference.wrist);
        if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentTool = Instantiate(toolPrefab, characterReference.leftHand);
        }
        if(characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentShield = Instantiate(shieldPrefab, characterReference.leftForearm);
        }
            

    }

    public WeaponBase CurrentlyEquipped() 
    {
        return characterReference.GetWeaponClass().currentWeapon;
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

    public WeaponBase[] GetWeaponsInventory()
    {
        return weaponInventory.GetComponent<WeaponsInventory>().GetInventory();
    }
    
}

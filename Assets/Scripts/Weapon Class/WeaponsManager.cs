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
        //DontDestroyOnLoad(this.gameObject);
        

        //AddToInventory(characterReference.equippedWeapon);
            
    }

    public void Initialize()
    {
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        weaponInventory = GameObject.Find("WeaponsInventory");
        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        shieldPrefab = characterReference.knightShield.weaponMesh;
        GameObject inputManager = GameObject.Find("InputandAnimationManager");


        Debug.Log("Current weapon type on load: " + characterReference.equippedWeapon.weaponClassType);
        if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.wrist);
            GameObject.FindGameObjectWithTag("projectileManager").GetComponent<projectileManager>().updateProjectileDamage("bulletPool", characterReference.gunnerObject.baseAttack + characterReference.equippedWeapon.weaponAttack);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(0, 1);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(2, 1);
        }

        else if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            toolPrefab = characterReference.engineerTool.weaponMesh;
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentTool = Instantiate(toolPrefab, characterReference.leftHand);
            GameObject.FindGameObjectWithTag("projectileManager").GetComponent<projectileManager>().updateProjectileDamage("pistolPool", characterReference.gunnerObject.baseAttack + characterReference.equippedWeapon.weaponAttack);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(0, 2);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(1, 2);
        }
        else if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            //Debug.Log(inputManager);
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentShield = Instantiate(shieldPrefab, characterReference.leftForearm);
            characterReference.equippedWeapon.weaponMesh.GetComponent<swordCombat>().updateDamage(characterReference.knightObject.baseAttack + characterReference.equippedWeapon.weaponAttack);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(1, 0);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(2, 0);
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

        switch (characterReference.equippedWeapon.weaponClassType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                Destroy(currentShield);
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                Destroy(currentTool);
                break;
        }

        //characterReference.GetWeaponClass().currentWeapon = newWeapon;
        characterReference.UpdateWeapon(newWeapon);
        Destroy(currentWeapon);

        GameObject inputManager = GameObject.FindGameObjectWithTag("inputManager");

        weaponPrefab = characterReference.equippedWeapon.weaponMesh;
        if (newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.wrist);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(0, 1);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(2, 1);
        }
        if (characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            toolPrefab = characterReference.engineerTool.weaponMesh;
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentTool = Instantiate(toolPrefab, characterReference.leftHand);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(0, 2);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(1, 2);
        }
        if(characterReference.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            currentWeapon = Instantiate(weaponPrefab, characterReference.hand);
            currentShield = Instantiate(shieldPrefab, characterReference.leftForearm);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(1, 0);
            inputManager.GetComponent<playerAnimationController>().changeClassLayer(2, 0);
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

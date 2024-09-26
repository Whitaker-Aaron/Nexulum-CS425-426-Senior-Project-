using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CharacterBase : MonoBehaviour
{
    //Will remove serialize field later. Here for testing purposes. Will have to be handled by lifetime 
    //managers.

    [SerializeField] public Rune[] equippedRunes;
    [SerializeField] public WeaponBase equippedWeapon;
    [SerializeField] public WeaponBase engineerTool;
    //[SerializeField] public RuneInt runeInt;
     WeaponClass weaponClass;
     MaterialsInventory materialInventory;
     RuneInventory runesInventory;
     WeaponsInventory weaponsInventory;
     ItemsInventory itemsInventory;

    [SerializeField] GameObject masterInput;
    Slider healthBar;
    Slider healthBorder;

    public bool invul = false;
    


    private RuneInt runeInt;

    //weapon spawn
    public Transform hand;

    //Player Health System
    public int maxHealth = 100;
    public int playerHealth;

    //Knight attackpoint transform - NEEDED FOR MASTERINPUT - Spencer
    public Transform swordAttackPoint;
    public Transform toolAttackPoint;

    private void Awake()
    {
        runeInt = GetComponent<RuneInt>();

        runeInt.apply();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = maxHealth; 

        for(int i = 0; i < equippedRunes.Length; i++)
        {
            Debug.Log("Currently equipped with " + equippedRunes[i].name + " rune.");
        }

        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        //healthBorder = GameObject.Find("HealthBorder").GetComponent<Slider>();

        healthBar.value = playerHealth;
        //healthBorder.value = playerHealth;

        healthBar.maxValue = maxHealth;
        //healthBorder.maxValue = maxHealth;



    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public WeaponClass GetWeaponClass()
    {
        return weaponClass;
    }

    public void UpdateWeapon(WeaponBase newWeapon)
    {
        Debug.Log("Inside Character Base");
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            Debug.Log("Newly equipped weapon is of type knight");
            masterInput.GetComponent<masterInput>().changeSword(newWeapon);
            equippedWeapon = newWeapon;
        }
        //Need to update for both his weapon pistol and his tool
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            Debug.Log("Newly equipped weapon is of type Engineer");
            masterInput.GetComponent<masterInput>().changeTool(newWeapon);
            equippedWeapon = newWeapon;
        }
        
    }

    

    public void ApplyRuneLogicToWeapon()
    {
        for(int i = 0;  i < equippedRunes.Length; i++)
        {
            if (equippedRunes[i].runeType == Rune.RuneType.Weapon)
            {
                //Implement logic to apply rune logic to currently equipped weapon.
            }

        }

    }

    public GameObject GetMasterInput()
    {
        return masterInput;
    }

    public void takeDamage(int damage)
    {

        if (!invul)
        {
            playerHealth -= damage;
            healthBar.value = playerHealth;
        }
        
        
        print("Player health: " + playerHealth);
    }

    
}

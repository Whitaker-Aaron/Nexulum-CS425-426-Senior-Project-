using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CharacterBase : MonoBehaviour
{
    //Will remove serialize field later. Here for testing purposes. Will have to be handled by lifetime 
    //managers.

    [SerializeField] public Rune[] equippedRunes = new Rune[3];
    [SerializeField] public WeaponBase equippedWeapon;
    [SerializeField] public WeaponBase engineerTool;
    [SerializeField] public WeaponBase knightShield;

    [SerializeField] public WeaponClass knightObject;
    [SerializeField] public WeaponClass gunnerObject;
    [SerializeField] private WeaponClass engineerObject;

    //[SerializeField] public RuneInt runeInt;
     public WeaponClass weaponClass;

     MaterialsInventory materialInventory;
     RuneInventory runesInventory;
     WeaponsInventory weaponsInventory;
     ItemsInventory itemsInventory;

    [SerializeField] GameObject masterInput;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider delayedHealthBar;

    public bool invul = false;

    public bool bubbleShield = false;
    

    private RuneInt runeInt;

    //weapon spawn
    public Transform hand, wrist, leftHand, leftForearm;

    //Player Health System
    int maxHealth;
    int playerHealth;

    //Knight attackpoint transform - NEEDED FOR MASTERINPUT - Spencer
    public Transform swordAttackPoint;
    public Transform toolAttackPoint;

    private void Awake()
    {
        
    }

   

    // Start is called before the first frame update
    void Start()
    {
        runeInt = GetComponent<RuneInt>();
        runeInt.Apply();

        maxHealth = 100;
        playerHealth = 100;

        Debug.Log("Current Player Health: " + playerHealth);



        //healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        //delayedHealthBar = GameObject.Find("DelayedHealthBar").GetComponent<Slider>();

        healthBar.value = playerHealth;
        delayedHealthBar.value = playerHealth;

        healthBar.maxValue = maxHealth;
        delayedHealthBar.maxValue = maxHealth;

        EquipClass(equippedWeapon.weaponClassType);
        weaponClass.currentWeapon = equippedWeapon;

        

    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public WeaponClass GetWeaponClass()
    {
        return weaponClass;
    }

    public void EquipClass(WeaponBase.weaponClassTypes type)
    {
        switch (type)
        {
            case WeaponBase.weaponClassTypes.Knight:
                weaponClass = knightObject;
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                weaponClass = gunnerObject;
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                weaponClass = engineerObject;
                break;
        }
    }

    public void UpdateWeapon(WeaponBase newWeapon)
    {
        Debug.Log("Inside Character Base");
        weaponClass.currentWeapon = newWeapon;
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Knight)
        {
            Debug.Log("Newly equipped weapon is of type Knight");
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
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
        {
            equippedWeapon = newWeapon;
        }
        
    }

    public void UpdateClass(WeaponBase.weaponClassTypes newClass)
    {
        EquipClass(newClass);
        masterInput.GetComponent<masterInput>().currentClass = newClass;
        //Debug.Log(weaponClass.currentWeapon);
        GameObject.Find("WeaponManager").GetComponent<WeaponsManager>().ChangeWeapon(weaponClass.currentWeapon);
        //UpdateWeapon(weaponClass.currentWeapon);
        runeInt.ChangeClass(newClass);
    }


    public void UpdateRunes(Rune runeToEquip, int position)
    {
        runeInt.Remove();
        equippedRunes[position] = runeToEquip;
        runeInt.Apply();
    }
    

    public GameObject GetMasterInput()
    {
        return masterInput;
    }

    public void takeDamage(int damage)
    {
        if (bubbleShield)
            return;
        if (!invul)
        {
            playerHealth -= damage;
            StartCoroutine(updateHealthBarsNegative());
        }
        


        print("Player health: " + playerHealth);
    }

    public void restoreHealth(int amount)
    {
        if (playerHealth + amount >= maxHealth) {
            playerHealth = maxHealth;
        }
        else
        {
            playerHealth += amount;
        }
        StartCoroutine(updateHealthBarsPositive());

    }

    public IEnumerator animateHealth()
    {
        
        float reduceVal = 150f;
        while (healthBar.value != playerHealth)
        {
            if (Mathf.Abs(healthBar.value - playerHealth) <= 5)
            {
                healthBar.value = playerHealth;
            }
            else if (playerHealth < healthBar.value)
            {
                healthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                healthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }

    public IEnumerator animateDelayedHealth()
    {
        float reduceVal = 150f;
        while (delayedHealthBar.value != playerHealth)
        {
            if (Mathf.Abs(delayedHealthBar.value - playerHealth) <= 5)
            {
                delayedHealthBar.value = playerHealth;
            }
            else if(playerHealth < delayedHealthBar.value)
            {
                delayedHealthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                delayedHealthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }


    public IEnumerator updateHealthBarsNegative()
    {
        StopCoroutine(animateHealth());
        yield return animateHealth();
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
    }

    public IEnumerator updateHealthBarsPositive()
    {
        StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(animateHealth());
        yield return animateHealth();
    }
}

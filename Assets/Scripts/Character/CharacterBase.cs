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
    //[SerializeField] public RuneInt runeInt;
     WeaponClass weaponClass;

     MaterialsInventory materialInventory;
     RuneInventory runesInventory;
     WeaponsInventory weaponsInventory;
     ItemsInventory itemsInventory;

    [SerializeField] GameObject masterInput;

    Slider healthBar;
    Slider delayedHealthBar;

    public bool invul = false;

    public bool bubbleShield = false;
    

    private RuneInt runeInt;

    //weapon spawn
    public Transform hand, wrist, leftHand, leftForearm;

    //Player Health System
    public int maxHealth = 100;
    public int playerHealth;

    //Knight attackpoint transform - NEEDED FOR MASTERINPUT - Spencer
    public Transform swordAttackPoint;
    public Transform toolAttackPoint;

    private void Awake()
    {
        runeInt = GetComponent<RuneInt>();
        runeInt.Apply();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = maxHealth; 

        

        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        delayedHealthBar = GameObject.Find("DelayedHealthBar").GetComponent<Slider>();

        healthBar.value = playerHealth;
        delayedHealthBar.value = playerHealth;

        healthBar.maxValue = maxHealth;
        delayedHealthBar.maxValue = maxHealth;



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
            if (Mathf.Abs(healthBar.value - playerHealth) <= 1)
            {
                healthBar.value = playerHealth;
            }
            else if (playerHealth < delayedHealthBar.value)
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
            if (Mathf.Abs(delayedHealthBar.value - playerHealth) <= 1)
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
        yield return animateHealth();
        yield return new WaitForSeconds(0.2f);
        yield return animateDelayedHealth();
    }

    public IEnumerator updateHealthBarsPositive()
    {
        yield return animateDelayedHealth();
        yield return new WaitForSeconds(0.2f);
        yield return animateHealth();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CharacterBase : MonoBehaviour, SaveSystemInterface
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
    public CharacterStat characterStats;

    LifetimeManager lifetimeManager;


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
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();

        


        

    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void SaveData(ref SaveData data)
    {
        data.playerHealth = playerHealth;
        data.maxPlayerHealth = maxHealth;

        data.equippedWeapon = equippedWeapon.weaponName;

        for(int index = 0; index < 3; index++) {
            switch (index)
            {
                case 0:
                    data.weaponClasses[index].currentWeapon = knightObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = knightObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = knightObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = knightObject.currentLvl;
                    break;
                case 1:
                    data.weaponClasses[index].currentWeapon = gunnerObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = gunnerObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = gunnerObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = gunnerObject.currentLvl;
                    break;
                case 2:
                    data.weaponClasses[index].currentWeapon = engineerObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = engineerObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = engineerObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = engineerObject.currentLvl;
                    break;
            }
        }

        Debug.Log("Saved player health: " + data.playerHealth);

    }

    public void InitializeHealth()
    {
        healthBar.value = playerHealth;
        delayedHealthBar.value = playerHealth;

        healthBar.maxValue = maxHealth;
        delayedHealthBar.maxValue = maxHealth;
    }

    public void LoadData(SaveData data)
    {
        Debug.Log("Tracking load error");

        var weapons = GameObject.Find("WeaponsList").GetComponent<WeaponsList>();

        playerHealth = data.playerHealth;
        maxHealth = data.maxPlayerHealth;

        equippedWeapon = weapons.ReturnWeapon(data.equippedWeapon);

        

        if (data.weaponClasses[0].currentWeapon != null)
        {
            
            for (int index = 0; index < 3; index++)
            {
                switch (index)
                {
                    case 0:
                        knightObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        knightObject.totalExp = data.weaponClasses[index].totalExp;
                        knightObject.numSkillPoints = data.weaponClasses[index].numSkillPoints;
                        knightObject.currentLvl = data.weaponClasses[index].currentLvl;
                        break;
                    case 1:
                        gunnerObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        gunnerObject.totalExp = data.weaponClasses[index].totalExp;
                        gunnerObject.numSkillPoints = data.weaponClasses[index].numSkillPoints;
                        gunnerObject.currentLvl = data.weaponClasses[index].currentLvl;
                        break;
                    case 2:
                        engineerObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        engineerObject.totalExp = data.weaponClasses[index].totalExp;
                        engineerObject.numSkillPoints = data.weaponClasses[index].numSkillPoints;
                        engineerObject.currentLvl = data.weaponClasses[index].currentLvl;
                        break;
                }
            }
        }

        

        EquipClass(equippedWeapon.weaponClassType);
        weaponClass.currentWeapon = equippedWeapon;

        runeInt = GetComponent<RuneInt>();
        runeInt.ChangeClass(equippedWeapon.weaponClassType);

        InitializeHealth();


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
            if(playerHealth - damage <= 0)
            {
                playerHealth = 0;
            }
            else
            {
                playerHealth -= damage;
            }
            StartCoroutine(updateHealthBarsNegative());
            
        }

        


        print("Player health: " + playerHealth);
    }

    public void Death()
    {
        lifetimeManager.OnDeath();

        StopCoroutine(animateHealth());
        StopCoroutine(animateDelayedHealth());

        playerHealth = maxHealth;
        healthBar.value = maxHealth;
        delayedHealthBar.value = maxHealth;
        
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
        if(delayedHealthBar.value == 0)
        {
            Death();
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

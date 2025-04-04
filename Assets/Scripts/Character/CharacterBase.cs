using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;




public class CharacterBase : MonoBehaviour, SaveSystemInterface
{
    //Will remove serialize field later. Here for testing purposes. Will have to be handled by lifetime 
    //managers.

    [SerializeField] public Rune[] equippedRunes = new Rune[3];
    [SerializeField] public WeaponBase equippedWeapon;
    [SerializeField] public WeaponBase engineerTool;
    [SerializeField] public WeaponBase knightShield;
    [SerializeField] public WeaponBase gunnerRocketPod;

    [SerializeField] public WeaponClass knightObject;
    [SerializeField] public WeaponClass gunnerObject;
    [SerializeField] public WeaponClass engineerObject;

    [SerializeField] public GameObject shakeEffect;

    //[SerializeField] public RuneInt runeInt;
    public WeaponClass weaponClass;
    public CharacterStat characterStats;
    public ProgressionChecks progressionChecks;
    public Coroutine curStopVel;
    PhysicMaterial physicMat;
    Rigidbody rigidbody;
    float florentineAmount;

    bool lowHealthReached = false;
    

    public Vector3 lastGroundLocation;

    public RoomInformation targetRoom;

    Coroutine currFallCountdown;

    //MANAGERS
    LifetimeManager lifetimeManager;
    UIManager uiManager;
    WeaponsManager weaponsManager;
    AudioManager audioManager;

    masterInput masterInput;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider delayedHealthBar;

    public bool invul = false;
    public bool bubbleShield = false;

    public bool transitioningRoom = false;
    public bool transitionedRoom = false;
    public bool teleporting = false;
    public bool inDialogueBox = false;
    public bool inEvent = false;
    public bool isDying = false;
    public bool isTouchingGround = false;
    public bool isGettingKnockbacked;
    public bool activatingFall = false;
    public bool usingTerminal = false;
    public RoomSpawnObject teleportSpawnObject;

    


    int collisionCounter = 0;
    int groundCounter = 0;
    public int floatingPlatformCounter = 0;
    public int wallCollisionCounter = 0;
    public int voidWallCounter = 0;
    public int enemyCollisionCounter = 0;
    float yPOSVal = 0f;
    private RuneInt runeInt;

    //weapon spawn
    public Transform hand, wrist, leftHand, leftForearm;

    //Player Health System
    public int maxHealth;
    int playerHealth;
    int defense = 0;

    //Knight attackpoint transform - NEEDED FOR MASTERINPUT - Spencer
    public Transform swordAttackPoint;
    public Transform toolAttackPoint;

    private void Awake()
    {
        runeInt = GameObject.FindGameObjectWithTag("runeManager").GetComponent<runeIntController>();
        physicMat = GetComponent<CapsuleCollider>().material;
        masterInput = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        rigidbody = GetComponent<Rigidbody>();
        progressionChecks = new ProgressionChecks();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected on player");
        //masterInput.GetComponent<masterInput>().StopDash();
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if(collision.gameObject.tag == "ground" || collision.gameObject.tag == "MovingPlatform")
        {
            Debug.Log("touching ground");
            
            groundCounter++;
            
        }

        if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("Touching wall");
            wallCollisionCounter++;
            

                //yPOSVal = gameObject.transform.position.y;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            enemyCollisionCounter++;
        }



        }

    public void ResetGroundCounter()
    {
        groundCounter = 0;
        enemyCollisionCounter = 0;
        voidWallCounter = 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (masterInput.GetComponent<masterInput>().characterColliding)
        //{
        //    if((collisionCounter - 1) > -1)
        //    {
        //        collisionCounter -= 1;
        //    }
        //    
        //}
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "MovingPlatform")
        {
            Debug.Log("No longer touching ground");
            if (groundCounter - 1 > -1)
            {
                groundCounter--;

            }
            //lastGroundLocation = gameObject.transform.position;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (enemyCollisionCounter - 1 > -1) enemyCollisionCounter--;


        }

        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("stopped touching wall");
            if(wallCollisionCounter - 1 > -1) wallCollisionCounter--;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VoidWall")
        {

            voidWallCounter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RestorePoint")
        {
            Debug.Log("No longer restore point");
            lastGroundLocation = transform.position;
        }
        if (other.gameObject.tag == "VoidWall")
        {
            Debug.Log("stopped touching wall");
            if (voidWallCounter - 1 > -1) voidWallCounter--;

        }

    }



    // Start is called before the first frame update
    void Start()
    {
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        weaponsManager = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayLevelUpParticle()
    {
        var particleSys = transform.Find("LevelUpParticle");
        if (particleSys != null)
        {
            particleSys.transform.SetParent(null, true);
            particleSys.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            particleSys.GetComponentInChildren<ParticleSystem>().Play();
        }
        StartLevelUpExplosion();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.scale = new Vector3(1, 1, 1);
        if(wallCollisionCounter >= 2)
        {
            
               physicMat.dynamicFriction = 0f;
            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, yPOSVal, gameObject.transform.position.z);
        }
        else
        {
               physicMat.dynamicFriction = 0.5f;
        }
        if (groundCounter <= 0)
        {
            activatingFall = true;
            if (isTouchingGround && !transitioningRoom) currFallCountdown = StartCoroutine(startFallCountdown());
            
        }
        else if(voidWallCounter >= 1)
        {
            if (isTouchingGround && !transitioningRoom) currFallCountdown = StartCoroutine(startFallCountdown());
            activatingFall = true;
            isTouchingGround = false;

        }
        else
        {
            if (masterInput.gameObject.activeSelf && !isTouchingGround && !transitioningRoom)
            {
                if(currFallCountdown != null) StopCoroutine(currFallCountdown);
                masterInput.DisableFallAnimation();
            }
            activatingFall = false;
            isTouchingGround = true;
        }

        if (isGettingKnockbacked || enemyCollisionCounter >= 1)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        //Debug.Log(enemyCollisionCounter);
    }

    public void ResetToGround()
    {
        transform.position = lastGroundLocation;
    }
    
    public IEnumerator startFallCountdown()
    {
        yield return new WaitForSeconds(0.25f);
        isTouchingGround = false;
        if (masterInput.gameObject.activeSelf && !isTouchingGround && activatingFall && floatingPlatformCounter < 1) masterInput.ActivateFallAnimation();

    }

    public void AddFlorentine(float amount)
    {
        if(florentineAmount + amount <= 9999)
        {
            florentineAmount += amount;
            Debug.Log("Character now has " +  florentineAmount + " florentine");
        }
        else
        {
            florentineAmount = 9999;
        }
        uiManager.UpdateFlorentine((int)florentineAmount, "Up");
        
    }

    public void StartLevelUpExplosion()
    {
        GameObject.Find("EffectsManager").GetComponent<EffectsManager>().getFromPool(("levelUpExplosion"), this.transform.position, Quaternion.identity, false, false);
        GameObject.Find("EffectsManager").GetComponent<EffectsManager>().getFromPool(("levelUpLightball"), this.transform.position, Quaternion.identity, true, false);
    }

    public void RemoveFlorentine(float amount)
    {
        if(florentineAmount - amount > 0)
        {
            florentineAmount -= amount;
        }
        else
        {
            florentineAmount = 0;
        }
        uiManager.UpdateFlorentine((int)florentineAmount, "Down");

    }

    public float GetFlorentine()
    {
        return florentineAmount;
    }

    public IEnumerator MoveForward()
    {
        var changeAmount = new Vector3(0.0f, 0.0f, 2.5f);
        while (transitioningRoom)
        {
            transform.position += changeAmount * Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public IEnumerator MoveBackward()
    {
        var changeAmount = new Vector3(0.0f, 0.0f, -2.5f);
        while (transitioningRoom)
        {
            transform.position += changeAmount * Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void SaveData(ref SaveData data)
    {
        data.isNewFile = false;
        data.playerHealth = playerHealth;
        data.maxPlayerHealth = maxHealth;
        data.florentineAmount = (int)florentineAmount;

        data.equippedWeapon = equippedWeapon.weaponName;

        for(int index = 0; index < 3; index++) {
            switch (index)
            {
                case 0:
                    data.weaponClasses[index].currentWeapon = knightObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = knightObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = knightObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = knightObject.currentLvl;
                    data.weaponClasses[index].baseAttack = knightObject.baseAttack;
                    break;
                case 1:
                    data.weaponClasses[index].currentWeapon = gunnerObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = gunnerObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = gunnerObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = gunnerObject.currentLvl;
                    data.weaponClasses[index].baseAttack = gunnerObject.baseAttack;
                    break;
                case 2:
                    data.weaponClasses[index].currentWeapon = engineerObject.currentWeapon.weaponName;
                    data.weaponClasses[index].totalExp = engineerObject.totalExp;
                    data.weaponClasses[index].numSkillPoints = engineerObject.numSkillPoints;
                    data.weaponClasses[index].currentLvl = engineerObject.currentLvl;
                    data.weaponClasses[index].baseAttack = engineerObject.baseAttack;
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
        florentineAmount = data.florentineAmount;
        uiManager.UpdateFlorentine(data.florentineAmount);

        equippedWeapon = weapons.ReturnWeapon(data.equippedWeapon);

        

        if (data.weaponClasses[0].currentWeapon != null && !data.isNewFile)
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
        else if (data.isNewFile)
        {
            knightObject.totalExp = 0f;
            knightObject.numSkillPoints = 0f;
            knightObject.currentLvl = 1;
            knightObject.baseAttack = 30;

            gunnerObject.totalExp = 0f;
            gunnerObject.numSkillPoints = 0f;
            gunnerObject.currentLvl = 1;
            gunnerObject.baseAttack = 5;

            engineerObject.totalExp = 0f;
            engineerObject.numSkillPoints = 0f;
            engineerObject.currentLvl = 1;
            engineerObject.baseAttack = 5;

            for (int index = 0; index < 3; index++)
            {
                switch (index)
                {
                    case 0:
                        knightObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        break;
                    case 1:
                        gunnerObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        break;
                    case 2:
                        engineerObject.currentWeapon = weapons.ReturnWeapon(data.weaponClasses[index].currentWeapon);
                        break;
                }
            }

        }

        

        EquipClass(equippedWeapon.weaponClassType);
        weaponClass.currentWeapon = equippedWeapon;

        //runeInt = GetComponent<RuneInt>();
        runeInt.ChangeClass(equippedWeapon.weaponClassType);

        InitializeHealth();


    }

    public WeaponClass GetWeaponClass()
    {
        return weaponClass;
    }

    public void AddExperienceToClass(float enemyExp)
    {
        bool leveledUp = weaponClass.updateExperience(enemyExp);
        if (leveledUp)
        {
            uiManager.UpdateExperienceLevel(weaponClass.classType, weaponClass.currentLvl);
            uiManager.StartLevelUpText();
        }
        else uiManager.UpdateExperienceBar(weaponClass.totalExp);


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
                masterInput.instance.changeTool(engineerTool);
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
            masterInput.instance.changeSword(newWeapon);
            equippedWeapon = newWeapon;
            equippedWeapon.weaponMesh.GetComponent<swordCombat>().updateDamage(knightObject.baseAttack + equippedWeapon.weaponAttack);
        }
        //Need to update for both his weapon pistol and his tool
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
        {
            Debug.Log("Newly equipped weapon is of type Engineer");
            masterInput.instance.changeTool(newWeapon);
            equippedWeapon = newWeapon;
            equippedWeapon.weaponType = equippedWeapon.weaponMesh.GetComponent<weaponType>();
            //GameObject.FindGameObjectWithTag("projectileManager").GetComponent<projectileManager>().updateProjectileDamage("pistolPool", gunnerObject.baseAttack + newWeapon.weaponAttack);
        }
        if(newWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner)
        {
            equippedWeapon = newWeapon;
            equippedWeapon.weaponType = equippedWeapon.weaponMesh.GetComponent<weaponType>();
            //GameObject.FindGameObjectWithTag("projectileManager").GetComponent<projectileManager>().updateProjectileDamage("bulletPool", gunnerObject.baseAttack + newWeapon.weaponAttack);

        }

        
        //masterInput.instance.updateWeapon(equippedWeapon.weaponType);
    }

    public void UpdateClass(WeaponBase.weaponClassTypes newClass)
    {
        EquipClass(newClass);
        masterInput.instance.currentClass = newClass;
        //Debug.Log(weaponClass.currentWeapon);
        weaponsManager.ChangeWeapon(weaponClass.currentWeapon);

        if (newClass == WeaponBase.weaponClassTypes.Engineer)
            masterInput.instance.changeTool(engineerTool);


        uiManager.UpdateClass(newClass, weaponClass.currentLvl, true);
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
        return masterInput.instance.gameObject;
    }

    public IEnumerator StopVelocity(float time)
    {
        
        yield return new WaitForSeconds(time);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isGettingKnockbacked = false;
    }

    public void takeDamage(int damage, Vector3 forwardDir)
    {
        if (bubbleShield)
            return;
        if (!invul)
        {
            audioManager.PlaySFX("DamageTaken");
            if (forwardDir != Vector3.zero)
            {
                if (!isGettingKnockbacked)
                {
                    transform.gameObject.GetComponent<Rigidbody>().AddForce((forwardDir.normalized) * 15f, ForceMode.VelocityChange);
                    if (curStopVel != null)
                    {
                        StopCoroutine(curStopVel);
                    }
                    curStopVel = StartCoroutine(StopVelocity(0.25f));
                    isGettingKnockbacked = true;
                }
                
               
                
            }
            if (playerHealth - damage <= 0)
            {
                playerHealth = 0;
            }
            else
            {
                playerHealth -= damage;
            }
            StartCoroutine(updateHealthBarsNegative());
            
        }

        


        //print("Player health: " + playerHealth);
    }

    public void Death()
    {
        invul = true;
        isDying = true;
        lifetimeManager.OnDeath();
        masterInput.instance.pausePlayerInput();

        StopCoroutine(animateHealth());
        StopCoroutine(animateDelayedHealth());

        
        
    }


    public void RecoverFromDeath()
    {
        playerHealth = maxHealth;
        healthBar.value = maxHealth;
        isDying = false;
        RestoreLowHealth();

        enemyCollisionCounter = 0;
        wallCollisionCounter = 0;
        delayedHealthBar.value = maxHealth;
        invul = false;
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

    public void buffPlayer(bool choice, string name, int amount)
    {
        if(name == "attack")
        {
            if(choice)
            {
                switch(equippedWeapon.weaponClassType)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        knightObject.baseAttack += amount;
                        Debug.Log("attack buffed to: " +  knightObject.baseAttack);
                        equippedWeapon.weaponMesh.GetComponent<swordCombat>().updateDamage(knightObject.baseAttack + equippedWeapon.weaponAttack);
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:

                        break;
                    case WeaponBase.weaponClassTypes.Engineer:

                        break;
                }
            }
            else
            {
                switch (equippedWeapon.weaponClassType)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        knightObject.baseAttack -= amount;
                        Debug.Log("attack de-buffed to: " + knightObject.baseAttack);
                        equippedWeapon.weaponMesh.GetComponent<swordCombat>().updateDamage(knightObject.baseAttack + equippedWeapon.weaponAttack);
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:

                        break;
                    case WeaponBase.weaponClassTypes.Engineer:

                        break;
                }
            }
        }
    }

    //Rune functions

    public void changeDefenseStat(int amount)
    {
        print("Changing defense from " + defense + " to " + (defense + amount));
        defense += amount;
    }


    //Health

    public void ApplyLowHealth()
    {
        var color = new Color();
        ColorUtility.TryParseHtmlString("#F7315D", out color);
        healthBar.fillRect.GetComponent<Image>().color = color;
        uiManager.ShowCriticalText();
        audioManager.PlaySFX("LowHealth");
        lowHealthReached = true;
    }

    public void RestoreLowHealth()
    {
        var color = new Color();
        ColorUtility.TryParseHtmlString("#31F7A9", out color);
        healthBar.fillRect.GetComponent<Image>().color = color;
        uiManager.HideCriticalText();
        lowHealthReached = false;
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

            if (healthBar.value <= 30)
            {
                if(!lowHealthReached) ApplyLowHealth();
                
                
            }
            else
            {
                if (lowHealthReached) RestoreLowHealth();
            }

            if (healthBar.value == 0)
            {
                GameObject currentShake = Instantiate(shakeEffect, gameObject.transform.position, Quaternion.identity);
                currentShake.GetComponent<ParticleSystem>().Play();
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
        if(delayedHealthBar.value == 0 && !isDying)
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


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;


public class EnemyFrame : MonoBehaviour
{
    //List that takes prefabs of Craft Material objects to spawn on enemy death.
    [SerializeField] GameObject[] materialList;
    [SerializeField] GameObject enemyHealth;

    //TODO: NEED TO INTERFACE ENEMY TYPE 
    //[SerializeField] 
    //enemyMinionCombat enemyType;
    enemyInt enemyType;
    [SerializeField] Enemy enemyReference;

    [SerializeField] EnemyStateManager movementReference;
    [SerializeField] EnemyLOS enemyLOSref;

    GameObject enemyUIRef;
    public GameObject healthRef;
    CharacterBase character;

    UIManager uiManager;

    //PLAYER HEALTH - Spencer
    private int health;
    private int maxHealth;
    

    public Vector3 initialPos;

    public bool dmgOverTimeActivated = false;
    public bool isMiniboss = false;
    bool takingDmgOT = false;
    bool dying = false;
    bool healthBarDeathAnimating = false;

    // Damage and Damage Types - Aisling
    public bool onDamaged = false; // True on hit, used for state machine logic to aggro enemies
    public DamageSource source;

    public bool collidingWithPlayer = false;

    public float effectTickInterval = 15.0f;
    public int iceStackMax = 4;
    public IceDamage iceEffect;
    public bool statusImmunity_Ice = false;
    public bool statusImmunity_Fire = false;
    public bool statusImmunity_Earth = false;
    public bool statusImmunity_Electric = false;
    public bool statusImmunity_Water = false;
    public bool statusImmunity_Wind = false;
    private GameObject tempEffectObj;
    public GameObject iceStackEffect;
    [HideInInspector] public GameObject iceStackEffectRef;
    // public GameObject iceFrozenEffect;
    // [HideInInspector] public GameObject iceFrozenEffectRef;

    //Enemy animation for taking hits
    EnemyAnimation anim;
    Vector3 zeroDir;
    Slider enemyHealthBar;
    Slider delayedEnemyHealthBar;

    public Coroutine curStopVel;



    private void Awake()
    {
        //enemyType = transform.GetComponent<enemyMinionCombat>();
    }


    // Start is called before the first frame update
    void Start()
    {
     
        zeroDir = Vector3.zero;
        health = enemyReference.baseHealth;
        maxHealth = enemyReference.baseHealth;
        initialPos = transform.position;
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //Slider enemyHealthBar = enemyHealth.GetComponentInChildren<Slider>();
        //Slider delayedEnemyHealthBar = delayedEnemyHealth.GetComponent<Slider>()
        var sliders = enemyHealth.GetComponentsInChildren<Slider>();
        Debug.Log(sliders.Length);

        movementReference = GetComponent<EnemyStateManager>();
        enemyLOSref = GetComponent<EnemyLOS>();
        
        anim = GetComponent<EnemyAnimation>();
        enemyUIRef = GameObject.Find("DynamicEnemyUI");

        if (!enemyReference.isInvincible)
        {
            healthRef = Instantiate(enemyHealth);
            healthRef.transform.SetParent(enemyUIRef.transform, false);

            enemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().health;
            delayedEnemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().delayedHealth;

            enemyHealthBar.maxValue = maxHealth;
            delayedEnemyHealthBar.maxValue = maxHealth;

            enemyHealthBar.value = health;
            delayedEnemyHealthBar.value = health;
        }
        

        

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (enemyReference != null) enemyType = gameObject.GetComponent<enemyInt>().getType();
        
        
        // Status effects - Aisling
        iceEffect = new IceDamage(movementReference, iceStackMax, this);
        InvokeRepeating("TickIceEffect", 0.0f, effectTickInterval); // Decreases current ice stacks by 1 every effectTickInterval seconds until 0

        // Visual aura for statuses (ice)
        iceStackEffectRef = InstantiateEffectHere(iceStackEffect, false);
        // iceFrozenEffectRef = InstantiateEffectHere(iceFrozenEffect, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (collision.gameObject.tag == "Player")
        {
            collidingWithPlayer = true;

        }
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collidingWithPlayer = false;

        }
    }

    public void DeactivateHealthBar()
    {
        if(healthRef != null)
        {
            healthRef.gameObject.SetActive(false);
        }
        
    }

    public void ActivateHealthBar()
    {
        if(healthRef != null)
        {
            healthRef.gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!enemyReference.isInvincible)
        {
            healthRef.transform.position = new Vector3(this.transform.position.x - 1f,
            this.transform.position.y + 2f + enemyReference.healthBarOffset, this.transform.position.z);
        }
        
    }

    


    private void OnDestroy()
    {
        StopCoroutine(dmgOverTime(0, 0, 0, DamageType.Sword));
    }

    //take damage function with given damage paramater - Spencer
    public void takeDamage(int damage, Vector3 forwardDir, DamageSource targetSource, DamageType damageType)
    {
        if (enemyReference.isInvincible) return;
        if (!enemyType.isActive) return;
        
        // Damage info for state - Aisling
        onDamaged = true;
        source = targetSource;

        Debug.LogWarning("onDamaged is " + onDamaged);

        Debug.Log("Taken damage of type " + damageType);
        switch(damageType)
        {
            case DamageType.Ice: // Ice dmg - Aisling
                if (!statusImmunity_Ice)
                {
                    iceEffect.AddStacks(1);
                    Debug.Log("Current stacks: " + iceEffect.GetCurrentStacks());
                    iceEffect.execute();
                }
                break;
        }
        
        if(enemyReference != null)
        {
            Debug.Log("Enemy was attacked");
            Debug.Log("Enemy attacking?" + enemyType.isAttacking);
            if (!enemyType.isAttacking)
            {
                if(anim != null)
                    anim.takeHit();
            }
            if(true)
            {
                Vector3 forceVector = new Vector3(5.0f, 0.0f, 5.0f);
                if (forwardDir != Vector3.zero)
                {
                    Debug.Log("Normalized enemy knockback: " + forwardDir.normalized);
                    transform.gameObject.GetComponent<Rigidbody>().AddForce((forwardDir.normalized) * 15, ForceMode.Impulse);
                    if (curStopVel != null)
                    {
                        StopCoroutine(curStopVel);
                    }
                    transform.GetComponent<CapsuleCollider>().isTrigger = true;
                    curStopVel = StartCoroutine(StopVelocity(0.15f));
                }

                
            }
            print("Health is: " + health + " Dmg taken is: " + damage);
            if (health - damage <= 0 && !dying)
            {
                health = 0;
                dying = true;
                //StartCoroutine(updateHealthBarsNegative());
                StartCoroutine(death());

            }

            else if (!dying)
            {
                health -= damage;
                StartCoroutine(updateHealthBarsNegative());
            }
        }
        

        

    }
    

    public IEnumerator StopVelocity(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetComponent<CapsuleCollider>().isTrigger = false;
    }

    public void resetPosition()
    {
        transform.position = initialPos;
    }

    

    public IEnumerator dmgOverTime(int dmg, float statusTime, float dmgTime, DamageType dmgType)
    {

        float endTime = Time.time + statusTime;

        Debug.Log("Starting dmg over time at: " + Time.time + " Until: " + endTime);

        // Continue applying damage over time until the statusTime expires
        while (Time.time < endTime)
        {
            // Apply damage once per dmgTime interval
            if(this == null)
            {
                yield break;
            }
            takeDamage(dmg, Vector3.zero, EnemyFrame.DamageSource.AOE, dmgType);
            uiManager.DisplayDamageNum(gameObject.transform, dmg);
            Debug.Log("Damage taken: " + dmg + " at time: " + Time.time);

            // Wait for the next damage tick
            yield return new WaitForSeconds(dmgTime);
        }

        // Once the effect duration ends, reset flags and exit the coroutine
        Debug.Log("Finished dmgOverTime at: " + Time.time);
        dmgOverTimeActivated = false;
        takingDmgOT = false;

        yield break;
        
    }


    public void restoreHealth(int amount)
    {
        if (health + amount >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
        StartCoroutine(updateHealthBarsPositive());

    }

    public IEnumerator animateHealth()
    {
        Debug.Log("Inside animate health");
        float reduceVal = 250f;
        while (enemyHealthBar.value != health)
        {
            if (Mathf.Abs(enemyHealthBar.value - health) <= 5)
            {
                enemyHealthBar.value = health;
            }
            else if (health < enemyHealthBar.value)
            {
                enemyHealthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                enemyHealthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }

    public IEnumerator animateDelayedHealth()
    {
        float reduceVal = 250f;
        while (delayedEnemyHealthBar.value != health)
        {
            if (Mathf.Abs(delayedEnemyHealthBar.value - health) <= 5)
            {
                delayedEnemyHealthBar.value = health;
            }
            else if (health < delayedEnemyHealthBar.value)
            {
                delayedEnemyHealthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                delayedEnemyHealthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }


    public IEnumerator updateHealthBarsNegative()
    {
        //StopCoroutine(animateHealth());
        yield return animateHealth();
        yield return new WaitForSeconds(0.2f);
        //StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
        if(dying) healthBarDeathAnimating = false;
    }

    public IEnumerator updateHealthBarsPositive()
    {
        StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(animateHealth());
        yield return animateHealth();
    }
   
    public IEnumerator waitThenForceKill()
    {
        yield return new WaitForSeconds(1.5f);
        healthBarDeathAnimating = false;
    }

    //death function and roll loot - Spencer
    private IEnumerator death()
    {
        healthBarDeathAnimating = true;
        StartCoroutine(updateHealthBarsNegative());
        while (healthBarDeathAnimating)
        {
            yield return null;
        }
        
        if (transform.GetComponentInChildren<ParticleSystem>() != null)
        {

            Debug.Log("enemy has particle system");
            var particleSys = transform.Find("DeathParticle");
            if(particleSys != null)
            {
                particleSys.transform.SetParent(null, true);
                particleSys.transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
                particleSys.GetComponentInChildren<ParticleSystem>().Play();
            }
            
        }
        else Debug.Log("enemy does not have particle system");
        if (collidingWithPlayer)
        {
            if(character.enemyCollisionCounter > 0) character.enemyCollisionCounter--;
        }
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("EnemyDeath");
        enemyType.onDeath();
        for (int i = 0; i < materialList.Length; i++)
        {
            var craftMat = materialList[i].GetComponent<OverworldMaterial>();
            Debug.Log("Craft material drop rate: ");
            Debug.Log(craftMat.material.dropRate);
            
            for (int j = 0; j < craftMat.material.dropAmount; j++)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= craftMat.material.dropRate)
                {
                    Instantiate(materialList[i], new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z), Quaternion.identity);
                }
            }
            


        }
        character.AddExperienceToClass(enemyReference.droppedExperience);
        removeHealth();
        Destroy(this.gameObject);
        yield break;
    }

    public void removeHealth()
    {
        Destroy(healthRef);
    }

    

    private void TickIceEffect() // Tick ice effect - Aisling
    {
        if (iceEffect.GetCurrentStacks() > 0)
        {
            iceEffect.AddStacks(-1);
            iceEffect.execute();
            Debug.Log("Current stacks: " + iceEffect.GetCurrentStacks());
        }
    }

    public float GetStacksOfDType(DamageType type)
    {
        switch (type)
        {
            case DamageType.Ice:
                return iceEffect.currentStacks;
                break;
            default:
                Debug.Log("Requested stacks of status effect (DType) does not exist.");
                return -1;
                break;
        }
    }

    // Instantiates set visual effect for statuses, returns gameobject to be used to ref the effect
    private GameObject InstantiateEffectHere(GameObject effect, bool enabled)
    {
        if (effect != null)
        {
            Vector3 pos = this.transform.position;
            pos.y += this.transform.localScale.y;
            Quaternion rot = Quaternion.Euler(new Vector3(90, 90, 0));
            tempEffectObj = Instantiate(effect, pos, rot, this.transform);
        }
        else
        {
            Debug.LogWarning("Visual status effect " + effect.name + " is null");
        }

        tempEffectObj.SetActive(enabled);
        return tempEffectObj;
    }

    public Enemy GetEnemy()
    {
        return enemyReference;
    }

    public enum DamageSource
    {
        Player,
        Enemy,
        AOE,
    }

    public enum DamageType
    {
        Sword,
        Projectile,
        Explosion,
        Fire,
        Ice,
        Earth,
        Electric,
        Wind,
        Water
    }

}

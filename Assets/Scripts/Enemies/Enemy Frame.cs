
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class EnemyFrame : MonoBehaviour
{
    //List that takes prefabs of Craft Material objects to spawn on enemy death.
    [SerializeField] GameObject[] materialList;
    [SerializeField] GameObject enemyHealth;

    //TODO: NEED TO INTERFACE ENEMY TYPE 
    [SerializeField] enemyMinionCombat enemyType;
    [SerializeField] Enemy enemyReference;

    GameObject enemyUIRef;
    public GameObject healthRef;
    CharacterBase character;

    //PLAYER HEALTH - Spencer
    private int health;
    private int maxHealth;
    

    Vector3 initialPos;

    public bool dmgOverTimeActivated = false;
    bool takingDmgOT = false;
    bool dying = false;

    public bool onDamaged = false; // True on hit, used for state machine logic to aggro enemies on hit - Aisling
    public DamageSource source;

    //Enemy animation for taking hits
    EnemyAnimation anim;
    Vector3 zeroDir;
    Slider enemyHealthBar;
    Slider delayedEnemyHealthBar;


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
        
        
        anim = GetComponent<EnemyAnimation>();
        enemyUIRef = GameObject.Find("DynamicEnemyUI");

        healthRef = Instantiate(enemyHealth);
        healthRef.transform.SetParent(enemyUIRef.transform);

        enemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().health;
        delayedEnemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().delayedHealth;

        enemyHealthBar.value = health;
        delayedEnemyHealthBar.value = health;

        enemyHealthBar.maxValue = maxHealth;
        delayedEnemyHealthBar.maxValue = maxHealth;

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        healthRef.transform.position = new Vector3(this.transform.position.x - 1f, this.transform.position.y + 2f, this.transform.position.z);
    }

    //take damage function with given damage paramater - Spencer
    public void takeDamage(int damage, Vector3 forwardDir, DamageSource targetSource, DamageType damageType)
    {

        switch(damageType)
        {
            case DamageType.Ice:
                print("slow enemy");
                break;
        }

        // Damage info for state - Aisling
        onDamaged = true;
        source = targetSource;

        Debug.Log("Enemy was attacked");
        Debug.Log("Enemy attacking?" + enemyType.isAttacking);
        if (!enemyType.isAttacking)
        {
            Vector3 forceVector = new Vector3(5.0f, 0.0f, 5.0f);
            if (forwardDir != Vector3.zero)
            {
                transform.gameObject.GetComponent<Rigidbody>().AddForce((forwardDir.normalized) * 10, ForceMode.VelocityChange);
                StartCoroutine(StopVelocity(0.15f));
            }

            anim.takeHit();
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
    

    public IEnumerator StopVelocity(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
            takeDamage(dmg, Vector3.zero, EnemyFrame.DamageSource.AOE, dmgType);
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
    }

    public IEnumerator updateHealthBarsPositive()
    {
        StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(animateHealth());
        yield return animateHealth();
    }
   

    //death function and roll loot - Spencer
    private IEnumerator death()
    {
        yield return (StartCoroutine(updateHealthBarsNegative()));
        for (int i = 0; i < materialList.Length; i++)
        {
            var craftMat = materialList[i].GetComponent<OverworldMaterial>();
            Debug.Log("Craft material drop rate: ");
            Debug.Log(craftMat.material.dropRate);
            for (int j = 0; j < craftMat.material.dropAmount; j++)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= craftMat.material.dropRate)
                {
                    Instantiate(materialList[i], new Vector3(transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.y + 2.5f, transform.position.z + UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
                }
            }
            


        }
        character.AddExperienceToClass(enemyReference.droppedExperience);
        Destroy(healthRef);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        /*
        //Debug.Log("Enemy collided with: ");
        if (collision.gameObject.tag == "Bullet")
        { for (int i = 0; i < materialList.Length; i++)
            {
                var craftMat = materialList[i].GetComponent<OverworldMaterial>();
                Debug.Log("Craft material drop rate: ");
                Debug.Log(craftMat.material.dropRate);
                for (int j = 0; j < craftMat.material.dropAmount; j++)
                {
                    if (UnityEngine.Random.Range(0.0f, 1.0f) <= craftMat.material.dropRate)
                    {
                        Instantiate(materialList[i], new Vector3(transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.y + 2.5f, transform.position.z + UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
                    }
                }



            }
            Destroy(this.gameObject);
            //Instantiate(materialList[0], new Vector3(transform.position.x - 0.3f, transform.position.y + 2.5f, transform.position.z - 0.3f), Quaternion.identity);

        }
        */
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
        Electric,
        Wind
    }

}

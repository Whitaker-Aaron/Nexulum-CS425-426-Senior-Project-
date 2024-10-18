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
    [SerializeField] Enemy enemyReference;

    GameObject enemyUIRef;
    GameObject healthRef;

    //PLAYER HEALTH - Spencer
    public int health = 200;
    public int maxHealth = 200;

    Vector3 initialPos;

    public bool dmgOverTimeActivated = false;
    bool dying = false;

    //Enemy animation for taking hits
    EnemyAnimation anim;
    Slider enemyHealthBar;
    Slider delayedEnemyHealthBar;



    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
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
    public void takeDamage(int damage)
    {
        anim.takeHit();
        print("Health is: " + health + " Dmg taken is: " + damage);
        if (health - damage <= 0 && !dying)
        {
            health = 0;
            dying = true;
            //StartCoroutine(updateHealthBarsNegative());
            StartCoroutine(death());

        }

        else if(!dying)
        {
            health -= damage;
            StartCoroutine(updateHealthBarsNegative());
        }
            
        

    }

    public void resetPosition()
    {
        transform.position = initialPos;
    }

    

    public IEnumerator dmgOverTime(int dmg, float statusTime, float dmgTime)
    {
        float currentTime = Time.time;
        while (currentTime + statusTime > Time.time)
        {
            takeDamage(dmg);
            yield return new WaitForSeconds(dmgTime);
        }
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
        float reduceVal = 150f;
        while (enemyHealthBar.value != health)
        {
            if (Mathf.Abs(enemyHealthBar.value - health) <= 1)
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
        float reduceVal = 150f;
        while (delayedEnemyHealthBar.value != health)
        {
            if (Mathf.Abs(delayedEnemyHealthBar.value - health) <= 1)
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

}

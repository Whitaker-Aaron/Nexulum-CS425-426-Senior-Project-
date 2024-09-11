using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFrame : MonoBehaviour
{
    //List that takes prefabs of Craft Material objects to spawn on enemy death.
    [SerializeField] GameObject[] materialList;
    [SerializeField] Enemy enemyReference;

    //PLAYER HEALTH - Spencer
    private int health = 100;

    //Enemy animation for taking hits
    EnemyAnimation anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<EnemyAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //take damage function with given damage paramater - Spencer
    public void takeDamage(int damage)
    {
        anim.takeHit();
        print("Health is: " + health + " Dmg taken is: " + damage);
        if (health - damage <= 0)
            death();
        else
            health -= damage;
        
    }

    //dmg over time function - Spencer
    public IEnumerator dmgOverTime(int dmg, float statusTime, float dmgTime)
    {
        float currentTime = Time.time;
        while(currentTime + statusTime > Time.time)
        {
            takeDamage(dmg);
            yield return new WaitForSeconds(dmgTime);
        }
        yield break;
    }

    //death function and roll loot - Spencer
    private void death()
    {
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
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
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
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFrame : MonoBehaviour
{
    //List that takes prefabs of Craft Material objects to spawn on enemy death.
    [SerializeField] GameObject[] materialList;
    [SerializeField] Enemy enemyReference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy collided with: ");
        if(collision.gameObject.tag == "Bullet")
        {   for(int i = 0;  i < materialList.Length; i++)
            {
                var craftMat = materialList[i].GetComponent<OverworldMaterial>();
                Debug.Log("Craft material drop rate: ");
                Debug.Log(craftMat.material.dropRate);
                for(int j = 0; j < craftMat.material.dropAmount; j++)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    //List that takes prefabs of Craft Material objects to spawn on enemy death.
    [SerializeField] GameObject[] materialList;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(materialList[1], new Vector3(transform.position.x + 2, transform.position.y + 1.5f, transform.position.z + 2), Quaternion.identity);
        Instantiate(materialList[0], new Vector3(transform.position.x - 2, transform.position.y + 1.5f, transform.position.z + 2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

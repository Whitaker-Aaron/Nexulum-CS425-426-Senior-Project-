using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindWithTag("Player").transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

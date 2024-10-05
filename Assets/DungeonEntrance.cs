using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "Player")
        {
            Debug.Log("Player detected");
            GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().GoToScene("MainPlayTest");
        }
    }


}
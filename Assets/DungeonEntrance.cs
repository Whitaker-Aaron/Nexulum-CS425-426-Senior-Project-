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
            var character = other.GetComponent<CharacterBase>();
            if (!character.transitioningRoom)
            {
                //character.transitioningRoom = true;
                StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().GoToScene(2));
            }
            
        }
    }




}

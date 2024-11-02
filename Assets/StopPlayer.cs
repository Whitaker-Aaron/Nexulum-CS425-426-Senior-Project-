using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayer : MonoBehaviour
{
    bool exited = false;
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
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Character colliding with invisible walls");
            collision.gameObject.GetComponent<CharacterBase>().GetMasterInput().GetComponent<masterInput>().dashSpeed = 0.5f;
            
        }
    }

}

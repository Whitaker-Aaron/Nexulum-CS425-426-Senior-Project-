using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicHud : MonoBehaviour
{
    GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player"); 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = new Vector3(character.transform.position.x - 1f, character.transform.position.y + 2f, character.transform.position.z); 
    }
}

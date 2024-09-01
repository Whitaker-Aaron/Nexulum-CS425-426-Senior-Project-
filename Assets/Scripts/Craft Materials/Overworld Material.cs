using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OverworldMaterial : MonoBehaviour
{
    [SerializeField] float verticalFloatRange;
    [SerializeField] float floatSpeed;

    //Needs to have a reference to an existing CraftMaterial so when we add to inventory, we can pass this object over. 
    [SerializeField] CraftMaterial material; 
    //[SerializeField] newCraft
    float originalPos;
    float offset = 0.5f;


    bool descending = false;
    public BoxCollider2D body;
    // Start is called before the first frame update
    void Start()
    {
         originalPos = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!descending)
        {
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, verticalFloatRange, transform.position.z),  floatSpeed * Time.deltaTime);
            if(transform.position.y >= verticalFloatRange - offset)
            {
                descending = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPos, transform.position.z), floatSpeed * Time.deltaTime);
            if (transform.position.y <= originalPos + offset)
            {
                descending = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Colliding with: ");
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
        {
            //TODO: Add logic that passes the material reference object into materials inventory on pickup. 
            Destroy(this.gameObject);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Inside collision");
    }
}

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
    [SerializeField] GameObject scrollManager;

    //Needs to have a reference to an existing CraftMaterial so when we add to inventory, we can pass this object over. 
    [SerializeField]  public CraftMaterial material; 
    //[SerializeField] newCraft
    float originalPos;
    float offset = 0.5f;


    bool descending = false;
    // Start is called before the first frame update
    void Start()
    {
        scrollManager = GameObject.Find("ScrollManager");
        originalPos = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!descending)
        {
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPos + verticalFloatRange, transform.position.z),  floatSpeed * Time.deltaTime);
            if(transform.position.y >= (originalPos + verticalFloatRange) - offset)
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
        
       // Debug.Log("Colliding with: ");
       // Debug.Log(collision.gameObject.tag);
       // if(collision.gameObject.tag == "Player")
        //{
        //    var scrollRef = scrollManager.GetComponent<MaterialScrollManager>();
        //    scrollRef.AddToMaterialsInventory(this.material);
        //    scrollRef.UpdateScroll(this.material.materialTexture, this.material.materialName);
        //    if(GameObject.FindGameObjectWithTag("MainMenu") != null)
        //    {
        //        GameObject.Find("MenuManager").GetComponent<MenuManager>().AddToCurrentInventory(this.material);
        //    }
            
        //    Destroy(this.gameObject);
        //}
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with: ");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")

        {
            var scrollRef = scrollManager.GetComponent<MaterialScrollManager>();
            scrollRef.AddToMaterialsInventory(this.material);
            scrollRef.UpdateScroll(this.material.materialTexture, this.material.materialName);
            if (GameObject.FindGameObjectWithTag("MainMenu") != null)
            {
                GameObject.Find("MenuManager").GetComponent<MenuManager>().AddToCurrentInventory(this.material);
            }

            Destroy(this.gameObject);
        }
    }
}

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
    AudioManager audioManager;

    //Needs to have a reference to an existing CraftMaterial so when we add to inventory, we can pass this object over. 
    [SerializeField]  public CraftMaterial material; 
    //[SerializeField] newCraft
    float originalPos;
    bool isCollectible = false;
    float offset = 0.5f;
    float yHeightPeakOffset = 2.75f;
    float yDesiredOffset = 10.0f;
    Vector3 desiredHorizontalOffset;


    bool descending = false;
    // Start is called before the first frame update
    void Start()
    {
        scrollManager = GameObject.Find("ScrollManager");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        originalPos = transform.position.y-1;
        desiredHorizontalOffset = new Vector3(transform.position.x + UnityEngine.Random.Range(-2.0f, 2.0f), 1, transform.position.z + UnityEngine.Random.Range(-2.0f, 2.0f));
        StartCoroutine(animatePop());
        StartCoroutine(animateHorizontal());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if(!descending)
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
        }*/
    }

    public IEnumerator animateHorizontal()
    {
        while(transform.position.x != desiredHorizontalOffset.x && transform.position.z != desiredHorizontalOffset.z)
        {
            var desiredPos = new Vector3(desiredHorizontalOffset.x, transform.position.y, desiredHorizontalOffset.z);
            transform.position = Vector3.Slerp(transform.position, desiredPos, 5.0f*Time.deltaTime);
            if (Mathf.Abs(transform.position.x - desiredPos.x) <= 0.075f && Mathf.Abs(transform.position.z - desiredPos.z) <= 0.075f) transform.position = desiredPos;
            yield return null;
        }
        yield break;
    }

    public IEnumerator animatePop()
    {
        var ogPos = transform.position;
        
        float deAccel = 26.4f;
        float rate = 12.0f;
        while(transform.position.y != (ogPos.y + yHeightPeakOffset))
        {

            if (rate - (deAccel * Time.deltaTime) <= 0.3f) rate = 0.30f;
            else rate -= deAccel*Time.deltaTime;
            var desiredPeakPos = new Vector3(transform.position.x, ogPos.y + yHeightPeakOffset, transform.position.z);
            Debug.Log("Rate: " + rate);
            transform.position = Vector3.MoveTowards(transform.position, desiredPeakPos, (rate * Time.deltaTime));
            if(Mathf.Abs(transform.position.y - desiredPeakPos.y) <= 0.05f) transform.position = desiredPeakPos;
            yield return null;

        }
        isCollectible = true;
        float accel = 21.4f;
        while (transform.position.y != ogPos.y-1)
        {
            rate += accel * Time.deltaTime;
            var desiredOgPos = new Vector3(transform.position.x, ogPos.y-1, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, desiredOgPos, (rate * Time.deltaTime));
            if (Mathf.Abs(transform.position.y - (ogPos.y-1)) <= 0.075f) transform.position = desiredOgPos;
            yield return null;

        }
        StartCoroutine(animateLoop());
        yield break;
    }

    public IEnumerator animateLoop()
    {
        
        while (true)
        {
            if (!descending)
            {

                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPos + verticalFloatRange, transform.position.z), floatSpeed * Time.deltaTime);
                if (transform.position.y >= (originalPos + verticalFloatRange) - offset)
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
            yield return null;
        }
        //yield break;
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
        if (other.gameObject.tag == "Player" && isCollectible)

        {
            audioManager.PlaySFX("MaterialCollect");
            var scrollRef = scrollManager.GetComponent<MaterialScrollManager>();
            scrollRef.AddToMaterialsInventory(this.material, 1);
            scrollRef.UpdateScroll(this.material.materialTexture, this.material.materialName);
            if (GameObject.FindGameObjectWithTag("MainMenu") != null)
            {
                GameObject.Find("MenuManager").GetComponent<MenuManager>().AddToCurrentInventory(this.material);
            }
            Destroy(this.gameObject);
        }
    }
}

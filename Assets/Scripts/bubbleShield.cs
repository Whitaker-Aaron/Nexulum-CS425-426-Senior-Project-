using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleShield : MonoBehaviour
{
    public float pushForce = 5f; // Adjust the force as needed
    // Start is called before the first frame update
    void Start()
    {
        float lifeTime = GameObject.FindGameObjectWithTag("inputManager").GetComponent<classAbilties>().bubbleTime;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has a Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !other.CompareTag("Player") && !other.CompareTag("ground"))
        {
            // Calculate the direction away from the center of the shield
            Vector3 direction = (other.transform.position - transform.position).normalized;

            // Apply force to push the object away from the shield
            rb.AddForce(direction * pushForce, ForceMode.Impulse);
        }
    }
}

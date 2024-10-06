using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    GameObject explosionEffect;

    public float explosionRadius = 3f;
    public int damage;
    public 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void explode()
    {
        GameObject currentExplosion = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        currentExplosion.GetComponent<ParticleSystem>().Play();
        Destroy(currentExplosion, 2f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            explode();
        }
        else
            explode();
    }
}

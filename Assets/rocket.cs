using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    public GameObject explosionEffect;

    public float explosionRadius = 3f;
    public int damage, directHitDamage;
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
        Collider[] hitEnemies = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);
        foreach (Collider collider in hitEnemies)
        {
            if(collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero);
            }
        }
        Destroy(currentExplosion, 2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, explosionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(directHitDamage, Vector3.zero);
            explode();
        }
        else
            explode();
    }
}

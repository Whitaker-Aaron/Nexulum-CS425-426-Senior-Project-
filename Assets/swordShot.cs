using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordShot : MonoBehaviour
{
    public float lifeTime = 7f;
    public int damage;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

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
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

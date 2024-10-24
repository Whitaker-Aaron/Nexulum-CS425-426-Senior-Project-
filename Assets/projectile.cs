using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{

    public float speed = 10f;
    public const float maxLifeTime = 3f;
    private float lifeTime;
    public int damage;
    Rigidbody rb;
    public LayerMask enemy;

    //fire rune vars
    bool gunnerFire = false;
    public float fireRad = .4f;
    public int fireDamage = 5;
    //public GameObject fireEffect;


    private void OnEnable()
    {
        lifeTime = maxLifeTime;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveProj();
        handleTime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (gunnerFire)
            {
                collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);

                Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, fireRad, enemy);
                foreach(Collider c in enemies)
                {
                    print("Fire hit");
                    c.GetComponent<EnemyFrame>().takeDamage(fireDamage);
                }
                resetProjectile();
                returnToPool();
            }
            else
            {
                collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);
                resetProjectile();
                returnToPool();
            }
        }
        else
        {
            resetProjectile();
            returnToPool();
        }
    }

    void resetProjectile()
    {
        

        // Reset any other projectile properties
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  // Reset velocity
            rb.angularVelocity = Vector3.zero;  // Reset rotation
        }
    }

    void returnToPool()
    {
        projectileManager.Instance.returnProjectile(gameObject);
    }


    void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void handleTime()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
        {
            returnToPool();
        }
    }


    //----------RUNE EFFECTS--------------
    public void fireGunnerRune()
    {
        gunnerFire = true;
    }
}

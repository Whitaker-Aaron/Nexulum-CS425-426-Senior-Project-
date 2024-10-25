using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectile : MonoBehaviour
{

    public float speed = 10f;
    public const float maxLifeTime = 3f;
    private float lifeTime;
    public int damage;
    Rigidbody rb;
    public LayerMask enemy;
    masterInput input;

    bool hitEnemy = false;
    public float bufferDistance;
    Vector3 hitPoint = Vector3.zero;
    bool stop = false;

    //fire rune vars
    ///bool gunnerFire = false;
    //public float fireRad = .4f;
    //public int fireDamage = 5;
    //public GameObject fireEffect;


    private void OnEnable()
    {
        stop = false;
        lifeTime = maxLifeTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            hitPoint = hit.point;
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hitEnemy = true;
                hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);
            }
            
        }
    }

    private void OnDisable()
    {
        stop = true;
        hitEnemy = false;
        Vector3 hitPoint = Vector3.zero;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this);
        input = GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop)
        {
            moveProj();
            handleTime();
        }


    }

    private void FixedUpdate()
    {
        if (hitPoint != Vector3.zero || hitEnemy)
        {
            checkDistance();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        stop = true;
        resetProjectile();
        returnToPool();
    }

    void checkDistance()
    {
        float step = speed * Time.fixedDeltaTime;
        float distanceToHit = Vector3.Distance(transform.position, hitPoint);

        if (distanceToHit <= step || distanceToHit <= bufferDistance)
        {
            EffectsManager.instance.getFromPool("bulletHitPool", hitPoint);
            // We've reached the hit point, stop the projectile
            stop = true;

            /*
            if(gunnerFire)
            {
                gunnerFire = false;

                Collider[] enemies = Physics.OverlapSphere(hitPoint, fireRad, enemy);
                foreach (Collider enemy in enemies)
                {
                    enemy.gameObject.GetComponent<EnemyFrame>().takeDamage(fireDamage);
                }
            }
            */
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
        if(input.currentClass == WeaponBase.weaponClassTypes.Gunner)
            projectileManager.Instance.returnProjectile(gameObject);
        else if(input.currentClass == WeaponBase.weaponClassTypes.Engineer)
            projectileManager.Instance.returnProjectile2(gameObject);
        else
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
    /*
    public void fireGunnerRune()
    {
        gunnerFire = true;
    }
    */
}

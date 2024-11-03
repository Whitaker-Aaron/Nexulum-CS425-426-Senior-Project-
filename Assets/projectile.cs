using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private CharacterBase playerBase;

    public float speed = 10f;
    public const float maxLifeTime = 3f;
    private float lifeTime;
    public int damage = 0;
    Rigidbody rb;
    public LayerMask enemy;
    masterInput input;

    bool hitEnemy = false;
    bool hitPlayer = false;
    public float bufferDistance;
    Vector3 hitPoint = Vector3.zero;
    bool stop = false;

    public string poolName = null;


    //fire rune vars
    ///bool gunnerFire = false;
    //public float fireRad = .4f;
    //public int fireDamage = 5;
    //public GameObject fireEffect;


    public void setName(string name)
    {
        poolName = name;
    }


    private void OnEnable()
    {
        stop = false;
        lifeTime = maxLifeTime;

        int layerMask = LayerMask.GetMask("Default", "Enemy", "ground");

        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask) && poolName != "enemyMagePoolOne")
        {
            hitPoint = hit.point;


            //player projectile conditions
            if (hit.collider.gameObject.tag == "Enemy" && poolName != "enemyMagePoolOne")
            {
                hitEnemy = true;
                hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);
            }

            //enemy mage projectile conditions
            if(hit.collider.gameObject.tag == "Enemy" && poolName == "enemyMagePoolOne")
            {
                //hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);
            }
            else if(hit.collider.gameObject.tag == "Player" && poolName == "enemyMagePoolOne")
            {
                //hit.collider.gameObject
            }
            
        }

    }

    private void OnDisable()
    {
        stop = true;
        hitEnemy = false;
        Vector3 hitPoint = Vector3.zero;
        poolName = null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this);
        input = GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>();
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        switch(playerBase.equippedWeapon.weaponClassType)
        {
            case WeaponBase.weaponClassTypes.Knight:
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                damage = playerBase.gunnerObject.baseAttack + playerBase.equippedWeapon.weaponAttack;
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                damage = playerBase.engineerObject.baseAttack + playerBase.equippedWeapon.weaponAttack;
                break;
        }
        
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
        if ((hitPoint != Vector3.zero || hitEnemy) && poolName != "enemyMagePoolOne")
        {
            checkDistance();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (poolName != "enemyMagePoolOne")
            return;

        if (collision.gameObject.tag == "Player" && poolName == "enemyMagePoolOne")
        {
            collision.gameObject.GetComponent<CharacterBase>().takeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy" && poolName == "enemyMagePoolOne")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage);
        }

        playEffect(gameObject.transform.position);
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
            playEffect(hitPoint);
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

    void playEffect(Vector3 position)
    {
        if((position != null || position != Vector3.zero) && poolName != "enemyMagePoolOne")
        {
            print("First if running");
            switch (poolName)
            {
                case "bulletPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position);
                    break;
                case "pistolPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position);
                    break;
                case "turretPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position);
                    break;
                case "dronePool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position);
                    break;
                case "tankPool":
                    EffectsManager.instance.getFromPool("tankHitPool", position);
                    break;
            }
        }
        else
        {
            print("else ran");
            switch (poolName)
            {
                case "enemyMagePoolOne":
                    print("Playing mage hit");
                    EffectsManager.instance.getFromPool("mageHitOne", position);
                    break;
            }
        }
        
    }

    void returnToPool()
    {
        if(poolName == "bulletPool")
            projectileManager.Instance.returnProjectile("bulletPool", gameObject);
        else if(poolName == "pistolPool")
            projectileManager.Instance.returnProjectile("pistolPool", gameObject);
        else if(poolName == "turretPool")
            projectileManager.Instance.returnProjectile("turretPool", gameObject);
        else if (poolName == "dronePool")
            projectileManager.Instance.returnProjectile("turretPool", gameObject);
        else if (poolName == "tankPool")
            projectileManager.Instance.returnProjectile("tankPool", gameObject);
        else if (poolName == "enemyMagePoolOne")
            projectileManager.Instance.returnProjectile("enemyMagePoolOne", gameObject);
        else
            projectileManager.Instance.returnProjectile("bulletPool", gameObject);
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

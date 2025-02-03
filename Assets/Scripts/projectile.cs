using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private CharacterBase playerBase;
    private masterInput masterInput;

    public float speed = 10f;
    public const float maxLifeTime = 3f;
    private float lifeTime;
    public int damage = 0;
    Rigidbody rb;
    public LayerMask enemy;
    masterInput input;
    UIManager uiManager;

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
        GetDamage();
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

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
                int updatedDamage = damage;
                if(playerBase.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner && Vector3.Distance(playerBase.gameObject.transform.position, hitPoint) > masterInput.instance.shootingRange)
                {
                    updatedDamage = damage / masterInput.instance.gunnerDmgMod;
                    
                }
                else if(playerBase.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer && Vector3.Distance(playerBase.gameObject.transform.position, hitPoint) > masterInput.instance.shootingRange)
                {
                    updatedDamage = damage / masterInput.instance.engrDmgMod;
                    
                }

                hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(updatedDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                uiManager.DisplayDamageNum(hit.collider.gameObject.transform, updatedDamage);

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
        GetDamage();
        
        
    }

    public void GetDamage()
    {
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        switch (playerBase.equippedWeapon.weaponClassType)
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
        Debug.Log(collision.gameObject.name);
        if (poolName != "enemyMagePoolOne")
            return;
        if (collision.gameObject.tag == "material") return;

        Vector3 knockBackDir = GameObject.FindGameObjectWithTag("Player").transform.position - collision.transform.position;
        if (collision.gameObject.tag == "Player" && poolName == "enemyMagePoolOne")
        {
            if(classAbilties.instance.earthBool == true && classAbilties.instance.bubble == true)
            {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                gameObject.transform.forward = -gameObject.transform.forward;
                return;
            }
            else
                collision.gameObject.GetComponent<CharacterBase>().takeDamage(damage, knockBackDir);
        }
        if (collision.gameObject.tag == "Enemy" && poolName == "enemyMagePoolOne")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
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
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  
            rb.angularVelocity = Vector3.zero; 
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
                    EffectsManager.instance.getFromPool("bulletHitPool", position, Quaternion.identity);
                    break;
                case "pistolPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position, Quaternion.identity);
                    break;
                case "revolverPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position, Quaternion.identity);
                    break;
                case "turretPool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position, Quaternion.identity);
                    break;
                case "dronePool":
                    EffectsManager.instance.getFromPool("bulletHitPool", position, Quaternion.identity);
                    break;
                case "tankPool":
                    EffectsManager.instance.getFromPool("tankHitPool", position, Quaternion.identity);
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
                    EffectsManager.instance.getFromPool("mageHitOne", position, Quaternion.identity);
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
        else if (poolName == "revolverPool")
            projectileManager.Instance.returnProjectile("revolverPool", gameObject);
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class projectile : MonoBehaviour
{
    protected CharacterBase playerBase;
    protected masterInput masterInput;
    protected enemyProjectileDamage enemyProjectileDamage;
    public GameObject parent;

    public float speed = 10f;
    public float maxLifeTime = 3f;
    protected float lifeTime;
    public int damage = 0;
    Rigidbody rb;
    public LayerMask enemy;
    protected LayerMask ignore;
    protected int layerMask;
    masterInput input;
    protected UIManager uiManager;

    protected bool hitEnemy = false;
    protected bool hitPlayer = false;
    public float bufferDistance;
    protected Vector3 hitPoint = Vector3.zero;
    protected bool stop = false;

    public string poolName = null;

    protected string bulletHitEffect;

    protected bool counting = false;


    //fire rune vars
    ///bool gunnerFire = false;
    //public float fireRad = .4f;
    //public int fireDamage = 5;
    //public GameObject fireEffect;


    public void setName(string name)
    {
        poolName = name;
    }

    public void setParent(GameObject parentNew)
    {
        parent = parentNew;
    }


    private void OnEnable()
    {
        if(playerBase == null)
            playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        stop = false;
        lifeTime = maxLifeTime;
        //Invoke(nameof(returnToPool), lifeTime);
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        counting = true;
        //GetDamage();

        /*
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
        */

    }

    private void OnDisable()
    {
        stop = true;
        //hitEnemy = false;
        Vector3 hitPoint = Vector3.zero;
        //poolName = null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(this);
        input = GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>();
        //GetDamage();
        ignore = LayerMask.GetMask("Material");//"Default", "Enemy", "ground");
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        enemyProjectileDamage = masterInput.instance.gameObject.GetComponent<enemyProjectileDamage>();

    }

    public void GetDamage(string name)
    {
        if(name == "Player")
        {
            playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

            switch (playerBase.equippedWeapon.weaponClassType)
            {
                case WeaponBase.weaponClassTypes.Knight:
                    damage = playerBase.equippedWeapon.weaponMesh.GetComponent<swordCombat>().projectileDamage;
                    break;
                case WeaponBase.weaponClassTypes.Gunner:
                    damage = playerBase.gunnerObject.baseAttack + playerBase.equippedWeapon.weaponAttack;
                    break;
                case WeaponBase.weaponClassTypes.Engineer:
                    damage = playerBase.engineerObject.baseAttack + playerBase.equippedWeapon.weaponAttack;
                    break;
            }
        }
        else if(name.StartsWith("Ability-"))
        {
            string[] parts = name.Split('-', 2);
            print(parts[0]);
            print(parts[1]);
            switch(parts[1])
            {
                case null:
                    Debug.LogError("cant get damage for proj in: " + poolName);
                    return;

                case "Turret":
                    damage = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().engineerObject.turretAttack;
                    break;
                case "swordShot":
                    damage = classAbilties.instance.swordShotDamage;
                    break;
            }
        }
        else
        {
            
            switch(name)
            {
                case "Archer":
                    damage = enemyProjectileDamage.instance.getDamage("Archer");
                    break;
                case "Mage":
                    damage = enemyProjectileDamage.instance.getDamage("Mage");
                    break;
            }
            
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    private void FixedUpdate()
    {
        if(counting)
        {
            handleTime();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //onHit(collision);
        //returnToPool();
        /*Debug.Log(collision.gameObject.name);
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
        */
    }

    protected void resetProjectile()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  
            rb.angularVelocity = Vector3.zero; 
        }
        returnToPool();
        hitEnemy = false;
        hitPlayer = false;

    }

    protected void playEffect(Vector3 position)
    {
        if(position != null || position != Vector3.zero)// && poolName != "enemyMagePoolOne")
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("BulletImpact");
            EffectsManager.instance.getFromPool(bulletHitEffect, position, Quaternion.identity, false, false);
            resetProjectile();
            
            //print("First if running");
            /*
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
            }*/
        }
            
        
    }

    protected void returnToPool()
    {
        stop = true;
        counting = false;
        projectileManager.Instance.returnProjectile(poolName, gameObject);
        /*
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
        */
    }

    void handleTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            returnToPool();
        }
    }

    protected abstract void moveProj();

    //{
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //}



    //----------RUNE EFFECTS--------------
    /*
    public void fireGunnerRune()
    {
        gunnerFire = true;
    }
    */
}

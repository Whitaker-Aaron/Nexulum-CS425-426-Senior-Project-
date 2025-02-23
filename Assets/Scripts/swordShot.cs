using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class swordShot : projectile
{
    //public float lifeTime = 7f;
    //public int damage;

    public bool isIce = false;
    public float iceRadius = 2f;
    public int iceDamage = 0;

    //string poolName = null;

    bool explode = false;
    public float explodeRadius = 2f;
    //UIManager uiManager;

    private void Awake()
    {
        //waitReturn();
        GetDamage("swordShot");
        bulletHitEffect = "swordShotHit";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!stop)
        {
            moveProj();
        }
    }

    private void OnEnable()
    {
        stop = false;
        //StartCoroutine(waitReturn());
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        GetDamage("Ability-swordShot");
        bulletHitEffect = "swordShotHit";
        counting = true;
    }

    private void OnDisable()
    {
        explode = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    protected override void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        //print("Colliding with: " + other.name);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            uiManager.DisplayDamageNum(other.gameObject.transform, damage);
            //other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (isIce)
                iceExplode();
            else if (explode)
            {
                Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, explodeRadius);

                foreach (Collider c in enemies)
                {
                    if (c.gameObject.tag == "Enemy")
                    {
                        //print("slow down the enemy");
                        c.gameObject.GetComponent<EnemyFrame>().takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                        uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                    }
                }
                //EffectsManager.instance.getFromPool("swordShotExplodeHit", gameObject.transform.position, Quaternion.identity, false, false);
                playEffect(gameObject.transform.position);
                //resetProjectile();
            }
            else
            {
                playEffect(gameObject.transform.position);
                //EffectsManager.instance.getFromPool("swordShotHit", gameObject.transform.position, Quaternion.identity, false, false);
                //resetProjectile();
            }

        }
        else if (isIce)
        {
            iceExplode();
        }
        else if (explode)
        {
            Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, explodeRadius);

            foreach (Collider c in enemies)
            {
                if (c.gameObject.tag == "Enemy")
                {
                    //print("slow down the enemy");
                    c.gameObject.GetComponent<EnemyFrame>().takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                    uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                }
            }
            EffectsManager.instance.getFromPool("swordShotExplodeHit", gameObject.transform.position, Quaternion.identity, false, false);
            //resetProjectile();
        }
        else
        {
            playEffect(gameObject.transform.position);
            //EffectsManager.instance.getFromPool("swordShot", gameObject.transform.position, Quaternion.identity, false, false);
            //resetProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void iceExplode()
    {
        Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, iceRadius);

        foreach(Collider c in enemies)
        {
            if (c.gameObject.tag == "Enemy")
            {
                //print("slow down the enemy");
                c.gameObject.GetComponent<EnemyFrame>().takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                uiManager.DisplayDamageNum(c.gameObject.transform, damage);
            }
        }
        EffectsManager.instance.getFromPool("swordShotIce", gameObject.transform.position, Quaternion.identity, false, false);
        resetProjectile();
    }

    public void activateExplosion()
    {
        explode = true;
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, explodeRadius);
    }
}

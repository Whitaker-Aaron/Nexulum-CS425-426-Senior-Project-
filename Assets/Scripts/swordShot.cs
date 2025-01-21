using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class swordShot : MonoBehaviour
{
    public float lifeTime = 7f;
    public int damage;

    public bool isIce = false;
    public float iceRadius = 2f;
    public int iceDamage = 0;

    string poolName = null;

    bool explode = false;
    public float explodeRadius = 2f;
    UIManager uiManager;

    private void Awake()
    {
        //waitReturn();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(waitReturn());
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            uiManager.DisplayDamageNum(collision.gameObject.transform, damage);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (isIce)
                iceExplode();
            else if(explode)
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
                EffectsManager.instance.getFromPool("swordShotExplodeHit", gameObject.transform.position);
                returnToPool();
            }
            else
            {
                EffectsManager.instance.getFromPool("swordShotHit", gameObject.transform.position);
                returnToPool();
            }
                
        }
        else if(isIce)
        {
            iceExplode();
        }
        else if(explode)
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
            EffectsManager.instance.getFromPool("swordShotExplodeHit", gameObject.transform.position);
            returnToPool();
        }
        else
        {
            EffectsManager.instance.getFromPool("swordShotHit", gameObject.transform.position);
            returnToPool();
        }
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
        EffectsManager.instance.getFromPool("swordShotIceHit", gameObject.transform.position);
        returnToPool();
    }

    public void activateExplosion()
    {
        explode = true;
    }

    void returnToPool()
    {
        if(isIce)
        {
            projectileManager.Instance.returnProjectile(poolName, this.gameObject);
        }
        else
        {
            projectileManager.Instance.returnProjectile(poolName, this.gameObject);
        }
        
    }

    IEnumerator waitReturn()
    {
        yield return new WaitForSeconds(lifeTime);
        returnToPool();
        yield break;
    }

    public void setName(string name)
    {
        poolName = name;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, explodeRadius);
    }
}

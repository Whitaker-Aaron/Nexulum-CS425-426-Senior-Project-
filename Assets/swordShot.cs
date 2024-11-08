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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (isIce)
                iceExplode();
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
            }
        }
        EffectsManager.instance.getFromPool("swordShotIceHit", gameObject.transform.position);
        returnToPool();
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
}

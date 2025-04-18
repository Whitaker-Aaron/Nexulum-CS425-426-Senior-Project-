using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    public GameObject rocketFireAura;

    public float explosionRadius = 3f;
    public int damage, directHitDamage;
    public int fireDmg;
    UIManager uiManager;
    public float fireT, fireR, fireRadius, abilityTime;

    public bool fireB = false;

    public LayerMask enemyMask;


    // Start is called before the first frame update
    void Start()
    {
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void explode()
    {
        //GameObject currentExplosion = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        //currentExplosion.GetComponent<ParticleSystem>().Play();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("ExplosionHit");

        if (fireB)
        {
            EffectsManager.instance.getFromPool("rocketFireCircle", gameObject.transform.position, Quaternion.identity, false, false);
            GameObject tempAura = Instantiate(rocketFireAura, gameObject.transform.position, Quaternion.identity);
            tempAura.GetComponent<areaOfEffect>().startCheck(gameObject.transform.position, fireRadius, fireDmg, fireT, fireR, abilityTime);
        }
        else
        {
            EffectsManager.instance.getFromPool("rocketHit", gameObject.transform.position, Quaternion.identity, false, false);
        }
        
        
        Collider[] hitEnemies = Physics.OverlapSphere(gameObject.transform.position, explosionRadius, enemyMask);
        bool hitBoss = false;
        foreach (Collider collider in hitEnemies)
        {
            if(collider.gameObject.tag == "Boss" && !hitBoss)
            {
                //print("slow down the enemy");
                collider.gameObject.GetComponent<golemBoss>().takeDamage(damage);
                uiManager.DisplayDamageNum(collider.gameObject.transform, damage);
                hitBoss = true;
            }
            if(collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Explosion);
                uiManager.DisplayDamageNum(collider.gameObject.transform, damage, 60f, 1f);
            }
        }
        hitBoss = false;
        //Destroy(currentExplosion, 2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, explosionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Boss")
        {
            explode();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(directHitDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            explode();
        }
        else
            explode();
    }

    public void increaseBlastRadius(float amount)
    {
        explosionRadius += amount;
    }
}

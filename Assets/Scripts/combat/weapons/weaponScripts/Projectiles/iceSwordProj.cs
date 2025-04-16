using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceSwordProj : projectile
{
    private int count = 0;
    [SerializeField] private int maxHitCount;

    private Collider[] hitObjs; 
    private void FixedUpdate()
    {
        //if (hitPoint != Vector3.zero)// || hitEnemy)

        if (!stop)
        {
            moveProj();
        }
    }

    private void Awake()
    {
        bulletHitEffect = "iceSwordProjHit";
        GetDamage("Player");
    }

    private void OnEnable()
    {
        stop = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        //RaycastHit hit;
        GetDamage("Player");

        hitObjs = new Collider[maxHitCount];

        if (playerBase == null)
            playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    private void OnDisable()
    {
        count = 0;
        
        hitObjs = null;
    }

    protected override void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || (other.gameObject.GetComponent<projectile>() != null && other.gameObject.GetComponent<projectile>().poolName == "iceSwordProjPool"))
            return;

        // Handle boss part damage
        if (other.gameObject.tag == "bossPart")
        {
            int updatedDamage = damage;
            if (Vector3.Distance(playerBase.gameObject.transform.position, other.transform.position) > masterInput.instance.shootingRange)
            {
                updatedDamage = damage / masterInput.instance.engrDmgMod;
            }
            
            if (other.gameObject.GetComponent<bossPart>() != null)
            {
                other.gameObject.GetComponent<bossPart>().takeDamage(updatedDamage);
                uiManager.DisplayDamageNum(other.gameObject.transform, updatedDamage);
            }
            
            playEffect(gameObject.transform.position);
            count++;
            
            // If we've reached max hit count, stop the projectile
            if (count >= maxHitCount)
            {
                stop = true;
            }
            return;
        }
        
        // Handle boss damage
        if (other.gameObject.tag == "Boss")
        {
            int updatedDamage = damage;
            if (Vector3.Distance(playerBase.gameObject.transform.position, other.transform.position) > masterInput.instance.shootingRange)
            {
                updatedDamage = damage / masterInput.instance.engrDmgMod;
            }
            
            if (other.gameObject.GetComponent<golemBoss>() != null)
            {
                other.gameObject.GetComponent<golemBoss>().takeDamage(updatedDamage);
                uiManager.DisplayDamageNum(other.gameObject.transform, updatedDamage);
            }
            
            playEffect(gameObject.transform.position);
            count++;
            
            // If we've reached max hit count, stop the projectile
            if (count >= maxHitCount)
            {
                stop = true;
            }
            return;
        }

        // Handle regular enemy damage
        if (count == maxHitCount - 1)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                uiManager.DisplayDamageNum(other.gameObject.transform, damage);
                playEffect(gameObject.transform.position);
                stop = true;
            }
            else
            {
                playEffect(gameObject.transform.position);
                stop = true;
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            count++;
            other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            playEffect(gameObject.transform.position);
            
            // Play hit sound effect
            GameObject.Find("AudioManager")?.GetComponent<AudioManager>()?.PlaySFX("BulletImpact");
        }
        else
        {
            playEffect(gameObject.transform.position);
            count++;
            
            // If we've hit something that's not an enemy, boss, or boss part, increment count
            // If we've reached max hit count, stop the projectile
            if (count >= maxHitCount)
            {
                stop = true;
            }
        }
    }
}

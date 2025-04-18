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
    
    // Track the boss that was directly hit to avoid hitting it again in the explosion
    private GameObject directlyHitBoss = null;

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
        directlyHitBoss = null;
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
        // Reset the directly hit boss tracking variable
        directlyHitBoss = null;
        
        // Make sure uiManager is initialized
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        
        //print("Colliding with: " + other.name);
        if (other.gameObject.tag == "Enemy")
        {
            // Try to play sound effect with null check
            AudioManager audioManager = GameObject.Find("AudioManager")?.GetComponent<AudioManager>();
            if (audioManager != null) audioManager.PlaySFX("SwordShotExplosion");
            
            // Apply damage to the enemy
            EnemyFrame enemyFrame = other.gameObject.GetComponent<EnemyFrame>();
            if (enemyFrame != null)
            {
                enemyFrame.takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                if (uiManager != null) uiManager.DisplayDamageNum(other.gameObject.transform, damage);
            }
            
            if (isIce)
                iceExplode();
            else if (explode)
            {
                explodeEffect();
            }
            else
            {
                playEffect(gameObject.transform.position);
            }
        }
        else if (other.gameObject.tag == "bossPart")
        {
            // Try to play sound effect with null check
            AudioManager audioManager = GameObject.Find("AudioManager")?.GetComponent<AudioManager>();
            if (audioManager != null) audioManager.PlaySFX("SwordShotExplosion");
            
            // Apply damage to the boss part
            bossPart bPart = other.gameObject.GetComponent<bossPart>();
            if (bPart != null)
            {
                bPart.takeDamage(damage);
                if (uiManager != null) uiManager.DisplayDamageNum(other.gameObject.transform, damage);
            }
            
            if (isIce)
                iceExplode();
            else if (explode)
            {
                explodeEffect();
            }
            else
            {
                playEffect(gameObject.transform.position);
            }
        }
        else if (other.gameObject.tag == "Boss")
        {
            // Store the directly hit boss to avoid hitting it again in the explosion
            directlyHitBoss = other.gameObject;
            
            // Try to play sound effect with null check
            AudioManager audioManager = GameObject.Find("AudioManager")?.GetComponent<AudioManager>();
            if (audioManager != null) audioManager.PlaySFX("SwordShotExplosion");
            
            // Apply damage to the boss
            EnemyFrame enemyFrame = other.gameObject.GetComponent<EnemyFrame>();
            if (enemyFrame != null)
            {
                enemyFrame.takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                if (uiManager != null) uiManager.DisplayDamageNum(other.gameObject.transform, damage);
            }
            
            if (isIce)
                iceExplode();
            else if (explode)
            {
                explodeEffect();
            }
            else
            {
                playEffect(gameObject.transform.position);
            }
        }
        else if (isIce)
        {
            iceExplode();
        }
        else if (explode)
        {
            explodeEffect();
        }
        else
        {
            playEffect(gameObject.transform.position);
        }
    }
    
    // Helper method to handle explosion effects and damage
    private void explodeEffect()
    {
        Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, explodeRadius);

        foreach (Collider c in enemies)
        {
            if (c != null && c.gameObject != null)
            {
                // Skip the boss that was directly hit
                if (directlyHitBoss != null && c.gameObject == directlyHitBoss)
                    continue;
                    
                if (c.gameObject.tag == "bossPart")
                {
                    bossPart bPart = c.gameObject.GetComponent<bossPart>();
                    if (bPart != null)
                    {
                        bPart.takeDamage(damage);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                    }
                }
                else if (c.gameObject.tag == "Enemy")
                {
                    EnemyFrame enemyFrame = c.gameObject.GetComponent<EnemyFrame>();
                    if (enemyFrame != null)
                    {
                        enemyFrame.takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                    }
                }
                else if (c.gameObject.tag == "Boss")
                {
                    EnemyFrame enemyFrame = c.gameObject.GetComponent<EnemyFrame>();
                    if (enemyFrame != null)
                    {
                        enemyFrame.takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                    }
                }
            }
        }
        
        playEffect(gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void iceExplode()
    {
        // Make sure uiManager is initialized
        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        
        Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, iceRadius);

        foreach(Collider c in enemies)
        {
            if(c != null && c.gameObject != null)
            {
                // Skip the boss that was directly hit
                if (directlyHitBoss != null && c.gameObject == directlyHitBoss)
                    continue;
                    
                if(c.gameObject.tag == "bossPart")
                {
                    //print("slow down the enemy");
                    bossPart bPart = c.gameObject.GetComponent<bossPart>();
                    if(bPart != null)
                    {
                        bPart.takeDamage(iceDamage);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, iceDamage);
                    }
                }
                else if (c.gameObject.tag == "Boss")
                {
                    //print("slow down the boss");
                    golemBoss boss = c.gameObject.GetComponent<golemBoss>();
                    if(boss != null)
                    {
                        boss.takeDamage(iceDamage);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, iceDamage);
                    }
                    else
                    {
                        // Try with EnemyFrame if golemBoss component is not found
                        EnemyFrame enemyFrame = c.gameObject.GetComponent<EnemyFrame>();
                        if(enemyFrame != null)
                        {
                            enemyFrame.takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                            if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, iceDamage);
                        }
                    }
                }
                else if (c.gameObject.tag == "Enemy")
                {
                    //print("slow down the enemy");
                    EnemyFrame enemyFrame = c.gameObject.GetComponent<EnemyFrame>();
                    if(enemyFrame != null)
                    {
                        enemyFrame.takeDamage(iceDamage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                        if (uiManager != null) uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                    }
                }
            }
        }
        
        if(EffectsManager.instance != null)
        {
            EffectsManager.instance.getFromPool("swordShotIce", gameObject.transform.position, Quaternion.identity, false, false);
        }
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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class teslaTower : MonoBehaviour
{

    GameObject tower1, tower2, wall;//, tempWall;
    public const float towerMaxHealth = 200f;
    float tower1Health, tower2Health = towerMaxHealth;
    bool destroyed1, destroyed2 = false;
    private int key = 0;

    public int damage = 35;
    public float shockTime = 5f;
    public float stunTime = 2f;

    public int iceDamage = 5;
    public float iceRadius;
    public float iceHitRate = 1f;
    bool iceAttacking = false;
    bool enableIce = false;


    public LayerMask enemyLayer;

    IEnumerator iceStorm()
    {
        if (iceAttacking)
            yield break;

        iceAttacking = true;

        Collider[] enemies = Physics.OverlapSphere(tower1.transform.position + Vector3.up, iceRadius, enemyLayer);
        Collider[] enemies2 = Physics.OverlapSphere(tower2.transform.position + Vector3.up, iceRadius, enemyLayer);

        // Process enemies from first tower
        foreach (Collider c in enemies)
        {
            if (c.CompareTag("Boss"))
            {
                golemBoss boss = c.gameObject.GetComponent<golemBoss>();
                if (boss != null)
                {
                    boss.takeDamage(iceDamage);
                    UIManager.instance.DisplayDamageNum(c.gameObject.transform, iceDamage);
                }
            }
            else if (c.CompareTag("bossPart"))
            {
                bossPart part = c.gameObject.GetComponent<bossPart>();
                if (part != null)
                {
                    part.takeDamage(iceDamage);
                    UIManager.instance.DisplayDamageNum(c.gameObject.transform, iceDamage);
                }
            }
            else if (c.CompareTag("Enemy"))
            {
                EnemyFrame enemy = c.gameObject.GetComponent<EnemyFrame>();
                if (enemy != null)
                {
                    enemy.takeDamage(iceDamage, Vector3.zero, EnemyFrame.DamageSource.AOE, EnemyFrame.DamageType.Ice);
                    UIManager.instance.DisplayDamageNum(c.gameObject.transform, iceDamage);
                }
            }
        }
        
        // Process enemies from second tower
        foreach (Collider b in enemies2)
        {
            if (b.CompareTag("Boss"))
            {
                golemBoss boss = b.gameObject.GetComponent<golemBoss>();
                if (boss != null)
                {
                    boss.takeDamage(iceDamage);
                    UIManager.instance.DisplayDamageNum(b.gameObject.transform, iceDamage);
                }
            }
            else if (b.CompareTag("bossPart"))
            {
                bossPart part = b.gameObject.GetComponent<bossPart>();
                if (part != null)
                {
                    part.takeDamage(iceDamage);
                    UIManager.instance.DisplayDamageNum(b.gameObject.transform, iceDamage);
                }
            }
            else if (b.CompareTag("Enemy"))
            {
                EnemyFrame enemy = b.gameObject.GetComponent<EnemyFrame>();
                if (enemy != null)
                {
                    enemy.takeDamage(iceDamage, Vector3.zero, EnemyFrame.DamageSource.AOE, EnemyFrame.DamageType.Ice);
                    UIManager.instance.DisplayDamageNum(b.gameObject.transform, iceDamage);
                }
            }
        }
        
        yield return new WaitForSeconds(iceHitRate);
        iceAttacking = false;

        yield break;
    }

    public void setIce(bool choice)
    {
        enableIce = choice;

    }

    public void setParents()
    {
        tower1.GetComponent<teslaBase>().teslaParent = gameObject;
        tower2.GetComponent<teslaBase>().teslaParent = gameObject;
        wall.GetComponent<teslaWall>().teslaParent = gameObject;
    }

    public void attackEnemy(Collider enemy)
    {
        if (enemy.CompareTag("Boss"))
        {
            golemBoss temp = enemy.gameObject.GetComponent<golemBoss>();
            if (temp != null)
            {
                temp.takeDamage(damage);
                UIManager.instance.DisplayDamageNum(enemy.transform, damage);
            }
        }
        else if (enemy.CompareTag("bossPart"))
        {
            bossPart temp = enemy.gameObject.GetComponent<bossPart>();
            if (temp != null)
            {
                temp.takeDamage(damage);
                UIManager.instance.DisplayDamageNum(enemy.transform, damage);
            }
        }
        else if (enemy.CompareTag("Enemy"))
        {
            EnemyFrame temp = enemy.gameObject.GetComponent<EnemyFrame>();
            if (temp != null)
            {
                temp.takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Explosion);
            }
        }
    }

    public void takeDamage1(float damage)
    {
        if (tower1Health - damage <= 0)
            destroyOne();
        else
            tower1Health -= damage;
    }

    public void takeDamage2(float damage)
    {
        if (tower2Health - damage <= 0)
            destroyTwo();
        else
            tower2Health -= damage;
    }

    void destroyOne()
    {
        if(!destroyed2 && wall != null)
        {
            //tempWall = wall;
            Destroy(wall);
        }
        
        destroyed1 = true;
        Destroy(tower1);
    }

    void destroyTwo()
    {
        if(!destroyed1 && wall != null)
        {
            //tempWall = wall;
            Destroy(wall);
        }
        destroyed2 = true;
        Destroy(tower2);
    }

    public void assignVars(GameObject tower, GameObject towerTwo, GameObject wallObj)
    {
        tower1 = tower;
        tower2 = towerTwo;
        wall = wallObj;
    }

    public void replaceTower(int num, GameObject newTower)
    {
        if (num == 1)
        {
            tower1 = newTower;
            tower1Health = towerMaxHealth;
        }
        else
        {
            tower2 = newTower;
            tower2Health = towerMaxHealth;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed1 && destroyed2)
            Destroy(gameObject);

        if (enableIce)
            StartCoroutine(iceStorm());
        else
        {
            StopCoroutine(iceStorm());
            tower1.gameObject.GetComponent<teslaBase>().iceEffect.GetComponent<ParticleSystem>().Stop();
            tower2.gameObject.GetComponent<teslaBase>().iceEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void assignKey(int num)
    {
        key = num;
    }

    public int getKey()
    {
        return key;
    }
    
    // Upgrade methods for skill tree
    public void increaseDamage(int amount)
    {
        Debug.Log("Tesla tower damage increased by: " + amount);
        damage += amount;
        
        // If ice mode is enabled, also increase ice damage
        if (enableIce)
        {
            iceDamage += amount / 2; // Increase ice damage by half the amount
        }
    }
    
    public void increaseIceDamage(int amount)
    {
        Debug.Log("Tesla tower ice damage increased by: " + amount);
        iceDamage += amount;
    }
    
    public void increaseRange(float amount)
    {
        Debug.Log("Tesla tower range increased by: " + amount);
        iceRadius += amount;
    }
    
    public void increaseHitRate(float amount)
    {
        Debug.Log("Tesla tower hit rate increased by: " + amount);
        // Lower value means faster hits
        iceHitRate -= amount;
        if (iceHitRate < 0.1f)
            iceHitRate = 0.1f; // Minimum hit rate
    }
    
    public void increaseStunTime(float amount)
    {
        Debug.Log("Tesla tower stun time increased by: " + amount);
        stunTime += amount;
    }
    
    public void increaseShockTime(float amount)
    {
        Debug.Log("Tesla tower shock time increased by: " + amount);
        shockTime += amount;
    }
    
    public void applyUpgrades()
    {
        // Apply any stored upgrades from classAbilities
        if (classAbilties.instance != null)
        {
            increaseDamage(classAbilties.instance.teslaDamageUpgrade);
            // Could also apply other upgrades here if they're stored in classAbilities
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(tower1.transform.position + Vector3.up, iceRadius);
    }
}

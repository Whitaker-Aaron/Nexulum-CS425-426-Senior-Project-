using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslaTower : MonoBehaviour
{

    GameObject tower1, tower2, wall;//, tempWall;
    public const float towerMaxHealth = 200f;
    float tower1Health, tower2Health = towerMaxHealth;
    bool destroyed1, destroyed2 = false;

    public int damage = 35;
    public float shockTime = 5f;
    public float stunTime = 2f;

    public void setParents()
    {
        tower1.GetComponent<teslaBase>().teslaParent = gameObject;
        tower2.GetComponent<teslaBase>().teslaParent = gameObject;
        wall.GetComponent<teslaWall>().teslaParent = gameObject;
    }

    public void attackEnemy(Collider enemy)
    {
        EnemyFrame temp = enemy.gameObject.GetComponent<EnemyFrame>();
        temp.takeDamage(damage);
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
            tower1Health -= damage;
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
        
        
    }
}

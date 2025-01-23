using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class turretCombat : MonoBehaviour
{
    public GameObject turretGun;
    private int key = 0;

    //Enemy layer
    public LayerMask Enemy;

    //attack variables
    public float attackRadius = 4f;
    public float turnSpeed = 2f;
    public Vector3 leftRotation, rightRotation;
    bool turningLeft = true;
    bool turningRight = false;
    bool turnWait, stopTurn, shooting, switchSpawn = false;
    public float turnWaitTime = .75f;
    public float bulletSpeed = 20f;
    public float fireRate = .75f;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLeft, bulletSpawnRight;

    //Turret Health
    public const int maxHealth = 300;
    int health = maxHealth;

    //repair vars
    bool playerInRange = false;
    public float repairRange = 1f;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(turretGun.transform.position, attackRadius);
    }

    public void takeDamage(int damage)
    {

        if (health - damage <= 0)
        {
            destroyTurret();
        }
        else
            health -= damage;

        Debug.Log("turret health is: " + health);
    }

    public void destroyTurret()
    {
        print("calling destroy turret with key: " + key);
        classAbilties.instance.removeTower(key);
    }

    public void repair(int amount)
    {
        if (!playerInRange)
            return;

        if (health + amount >= maxHealth)
            health = maxHealth;
        else
            health += amount;

        Debug.Log("Health is now: " + health);
    }

    void checkPlayerRange()
    {
        Collider[] player = Physics.OverlapSphere(gameObject.transform.position, repairRange);
        foreach (Collider collider in player)
        {
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("Player can repair");
                playerInRange = true;
                GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>().canRepair = true;
                GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>().assignRepair(gameObject);
            }
            else
            {
                playerInRange = false;
                GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>().canRepair = false;
            }
        }
    }

    

    IEnumerator turn(bool left, bool right)
    {
        //print("starting turn " + left + " " + right);
        turnWait = true;
        //turretGun.transform.rotation = Quaternion.Slerp(turretGun.transform.rotation, Quaternion.Euler(angle.x, angle.y, angle.z), turnSpeed * Time.deltaTime);
        yield return new WaitForSeconds(turnWaitTime);
        turningLeft = left;
        turningRight = right;
        turnWait = false;
        yield break;
    }

    IEnumerator shoot()
    {
        shooting = true;
        switchSpawn = !switchSpawn;
        if(switchSpawn)
        {
            GameObject bullet = projectileManager.Instance.getProjectile("pistolPool", bulletSpawnLeft.position, bulletSpawnLeft.rotation);
            //var bullet = Instantiate(bulletPrefab, bulletSpawnLeft.position, bulletSpawnLeft.rotation);
            //bullet.GetComponent<Rigidbody>().velocity = bulletSpawnLeft.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRate);
            shooting = false;
            yield break;
        }
        else
        {
            GameObject bullet = projectileManager.Instance.getProjectile("pistolPool", bulletSpawnRight.position, bulletSpawnRight.rotation);
            yield return new WaitForSeconds(fireRate);
            shooting = false;
            yield break;
        }
        
    }

    void detectEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(turretGun.transform.position, attackRadius, Enemy);
        if (enemiesInRange.Length == 0)
            stopTurn = false;
        foreach (Collider enemy in enemiesInRange)
        {
            //print("enemy in radius");
            //print("forward: " + turretGun.transform.forward);
            Vector3 directionToEnemy = (enemy.transform.position - turretGun.transform.position).normalized;//turretGun.transform.InverseTransformPoint(enemy.transform.position);
            directionToEnemy.y = 0;

            //print("direction: " + directionToEnemy);
            float angleToEnemy = Vector3.SignedAngle(Vector3.forward, directionToEnemy, Vector3.up);
            if (angleToEnemy < 0)
                angleToEnemy += 360;
            //angleToEnemy = normalizeAngle(angleToEnemy);
            print("angle to enemy: " + angleToEnemy);
            if (isEnemyInRange(angleToEnemy))
            {
                Vector3 temp = new Vector3(enemy.transform.position.x, 0.5f, enemy.transform.position.z);
                bulletSpawnLeft.LookAt(temp);
                bulletSpawnRight.LookAt(temp);
                print("enemy in angle");
                stopTurn = true;
                turretGun.transform.LookAt(turretGun.transform.position + directionToEnemy, Vector3.up);
                if(!shooting)
                    StartCoroutine(shoot());
            }
            
        }
    }

    bool isEnemyInRange(float angleToEnemy)
    {
        float leftAngle = normalizeAngle(leftRotation.y);
        float rightAngle = normalizeAngle(rightRotation.y);

        print("enemy: " + angleToEnemy + " left: " + leftAngle + " Right: " + rightAngle);
        if (leftAngle < rightAngle)
        {
            return angleToEnemy >= leftAngle && angleToEnemy <= rightAngle;
        }
        else
        {
            return angleToEnemy >= leftAngle || angleToEnemy <= rightAngle;
        }
    }

    float normalizeAngle(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }

    private void Awake()
    {

        leftRotation = new Vector3(0, normalizeAngle(gameObject.transform.rotation.eulerAngles.y) + leftRotation.y, 0);
        rightRotation = new Vector3(0, normalizeAngle(gameObject.transform.rotation.eulerAngles.y) + rightRotation.y, 0);
        print("Forward andgle:" + normalizeAngle(gameObject.transform.rotation.eulerAngles.y));
        print("Left andgle:" + leftRotation.y);
        print("Right andgle:" + rightRotation.y);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    void Update()
    {
        detectEnemies();
        checkPlayerRange();

        if(Input.GetKeyDown(KeyCode.P))
        {
            takeDamage(50);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (turningLeft && !turningRight)// && turretGun.transform.rotation != Quaternion.Euler(leftRotation.x, leftRotation.y, leftRotation.z))
        {
            if (stopTurn)
                return;
            if(!turnWait)
                StartCoroutine(turn(false, true));
            turretGun.transform.rotation = Quaternion.Lerp(turretGun.transform.rotation, Quaternion.Euler(leftRotation.x, leftRotation.y, leftRotation.z), turnSpeed * Time.deltaTime);
            //print("turning left");
        }
        //else
          //  StartCoroutine(turn(false, true));
        if (turningRight && !turningLeft)// && turretGun.transform.rotation != Quaternion.Euler(rightRotation.x, rightRotation.y, rightRotation.z))
        {
            if (stopTurn)
                return;
            if (!turnWait)
                StartCoroutine(turn(true, false));
            turretGun.transform.rotation = Quaternion.Lerp(turretGun.transform.rotation, Quaternion.Euler(rightRotation.x, rightRotation.y, rightRotation.z), turnSpeed * Time.deltaTime);
            //print("turning right");
        }
        //else
          //  StartCoroutine(turn(true, false));
    }


    public void assignKey(int num)
    {
        key = num;
    }

    public int getKey()
    {
        return key;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class turretCombat : MonoBehaviour
{
    public GameObject turretGun;

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
    public int health = 300;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(turretGun.transform.position, attackRadius);
    }

    public void takeDamage(int damage)
    {
        if (health - damage <= 0)
            destroy();
        else
            health -= damage;
    }

    void destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator turn(bool left, bool right)
    {
        print("starting turn " + left + " " + right);
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
            var bullet = Instantiate(bulletPrefab, bulletSpawnLeft.position, bulletSpawnLeft.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnLeft.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRate);
            shooting = false;
            yield break;
        }
        else
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnRight.position, bulletSpawnRight.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnRight.forward * bulletSpeed;
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

            Vector3 directionToEnemy = enemy.transform.position - turretGun.transform.position;
            float angleToEnemy = Vector3.Angle(turretGun.transform.forward, directionToEnemy);

            if(isEnemyInRange(angleToEnemy))
            {
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
        float enemyAngle = normalizeAngle(angleToEnemy);

        if (leftAngle < rightAngle)
        {
            return enemyAngle >= leftAngle && enemyAngle <= rightAngle;
        }
        else
        {
            return enemyAngle >= leftAngle || enemyAngle <= rightAngle;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        detectEnemies();
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
}

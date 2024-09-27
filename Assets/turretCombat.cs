using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretCombat : MonoBehaviour
{
    public GameObject turretGun;

    public LayerMask Enemy;

    public float attackRadius = 4f;
    public float turnSpeed = 2f;
    public Vector3 leftRotation, rightRotation;
    bool turningLeft = true;
    bool turningRight = false;
    bool turnWait = false;
    public float turnWaitTime = .75f;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(turretGun.transform.position, attackRadius);
    }

    //use Quaternion.LookRotation(Vector3);


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

    void detectEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(turretGun.transform.position, attackRadius, Enemy);

        foreach (Collider enemy in enemiesInRange)
        {
            Vector3 directionToEnemy = enemy.transform.position - turretGun.transform.position;
            float angleToEnemy = Vector3.Angle(turretGun.transform.forward, directionToEnemy);

            if(isEnemyInRange(angleToEnemy))
            {
                print("enemy in angle");
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
            if(!turnWait)
                StartCoroutine(turn(false, true));
            turretGun.transform.rotation = Quaternion.Lerp(turretGun.transform.rotation, Quaternion.Euler(leftRotation.x, leftRotation.y, leftRotation.z), turnSpeed * Time.deltaTime);
            //print("turning left");
        }
        //else
          //  StartCoroutine(turn(false, true));
        if (turningRight && !turningLeft)// && turretGun.transform.rotation != Quaternion.Euler(rightRotation.x, rightRotation.y, rightRotation.z))
        {
            if (!turnWait)
                StartCoroutine(turn(true, false));
            turretGun.transform.rotation = Quaternion.Lerp(turretGun.transform.rotation, Quaternion.Euler(rightRotation.x, rightRotation.y, rightRotation.z), turnSpeed * Time.deltaTime);
            //print("turning right");
        }
        //else
          //  StartCoroutine(turn(true, false));
    }
}

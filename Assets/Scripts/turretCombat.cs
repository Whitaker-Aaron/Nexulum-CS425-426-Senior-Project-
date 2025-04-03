using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;

//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class turretCombat : MonoBehaviour
{
    public GameObject turretGun;
    private int key = 0;

    //Enemy layer
    public LayerMask Enemy;
    public LayerMask playerLayer;

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
    public GameObject muzzleEffect;
    private GameObject[] leftEffects = new GameObject[3];
    private GameObject[] rightEffects = new GameObject[3];
    int effectCount = 0;

    //Turret Health
    public const int maxHealth = 300;
    int health = maxHealth;

    //repair vars
    bool playerInRange = false;
    public float repairRange = 1f;
    bool assigned = false;

    //effect
    public GameObject inRangeEffect;
    private GameObject effect;

    [SerializeField] bool isFire = false;
    [SerializeField] GameObject fireEffect;
    public float flameDistance, flameRadius, fireDmgRate;
    public int fireDmg;
    private Collider[] enemiesInRange;

    private void OnEnable()
    {
        effect = Instantiate(inRangeEffect, gameObject.transform.position, Quaternion.identity);
        effect.transform.parent = gameObject.transform;
        effect.SetActive(false);
    }


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

        Collider[] player = Physics.OverlapSphere(gameObject.transform.position, repairRange, playerLayer);
        if (player == null)
        {
            return;
        }
        if (player.Length > 0 && player[0].gameObject.tag == "Player" && !assigned)
        {
            assigned = true;
            Debug.Log("Player can repair");
            playerInRange = true;
            //collider.gameObject.GetComponent<masterInput>().canRepair = true;
            masterInput.instance.assignRepair(gameObject, key);

            effect.SetActive(true);
            effect.transform.position = gameObject.transform.position;
            effect.GetComponent<ParticleSystem>().Play();

        }
        if(player.Length == 0)
        {
            playerInRange = false;
            assigned = false;
            //GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>().canRepair = false;
            GameObject.FindGameObjectWithTag("inputManager").GetComponent<masterInput>().unassignRepair(gameObject, key);
            effect.SetActive(false);
            effect.GetComponent<ParticleSystem>().Stop();
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

    public int turretFireDmg;
    public float turretFireTime, turretFireRate;

    IEnumerator shoot()
    {
        if (shooting)
            yield break;

        if(isFire)
        {
            shooting = true;
            if(!fireEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                fireEffect.SetActive(true);
                fireEffect.GetComponent<ParticleSystem>().Play();
            }

            Vector3 startPoint = turretGun.transform.position + turretGun.transform.forward * .4f;
            Vector3 endPoint = startPoint + turretGun.transform.forward * flameDistance;

            Collider[] enemies = Physics.OverlapCapsule(startPoint, endPoint, flameRadius, Enemy);

            foreach(Collider c in enemies)
            {
                //direct damage
                c.GetComponent<EnemyFrame>().takeDamage(fireDmg, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Fire);
                UIManager.instance.DisplayDamageNum(c.transform, fireDmg);

                //damage over time from fire
                if (!c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated)
                {
                    c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated = true;
                    StartCoroutine(c.gameObject.GetComponent<EnemyFrame>().dmgOverTime(turretFireDmg, turretFireTime, turretFireRate, EnemyFrame.DamageType.Fire));
                }
            }
            //yield return new WaitUntil(() => { return enemiesInRange.Length == 0; });
            yield return new WaitForSeconds(fireDmgRate);
            if(enemiesInRange.Length == 0)
                fireEffect.GetComponent<ParticleSystem>().Stop();
            shooting = false;
            yield break;

        }
        else
        {
            shooting = true;
            switchSpawn = !switchSpawn;
            effectCount = effectCount % 3;
            if (switchSpawn)
            {
                GameObject bullet = projectileManager.Instance.getProjectile("turretPool", bulletSpawnLeft.position, bulletSpawnLeft.rotation);
                leftEffects[effectCount].GetComponent<ParticleSystem>().Play();
                //var bullet = Instantiate(bulletPrefab, bulletSpawnLeft.position, bulletSpawnLeft.rotation);
                //bullet.GetComponent<Rigidbody>().velocity = bulletSpawnLeft.forward * bulletSpeed;
                yield return new WaitForSeconds(fireRate);
                shooting = false;
                effectCount++;
                yield break;
            }
            else
            {
                GameObject bullet = projectileManager.Instance.getProjectile("turretPool", bulletSpawnRight.position, bulletSpawnRight.rotation);
                rightEffects[effectCount].GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(fireRate);
                shooting = false;
                effectCount++;
                yield break;
            }
            
        }

        
        
    }


    void detectEnemies()
    {
        enemiesInRange = Physics.OverlapSphere(turretGun.transform.position, attackRadius, Enemy);
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
            else
            {
                fireEffect.GetComponent<ParticleSystem>().Stop();
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
        fireEffect.GetComponent<ParticleSystem>().Stop();
        fireEffect.SetActive(false);

        for(int i = 0; i < 3; i++)
        {
            leftEffects[i] = Instantiate(muzzleEffect, bulletSpawnLeft);
            leftEffects[i].GetComponent<ParticleSystem>().Stop();
            leftEffects[i].gameObject.transform.localPosition = Vector3.zero;
            rightEffects[i] = Instantiate(muzzleEffect, bulletSpawnRight);
            rightEffects[i].GetComponent<ParticleSystem>().Stop();
            rightEffects[i].gameObject.transform.localPosition = Vector3.zero;
        }
        //print("Forward andgle:" + normalizeAngle(gameObject.transform.rotation.eulerAngles.y));
        //print("Left andgle:" + leftRotation.y);
        //print("Right andgle:" + rightRotation.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        //isFire = false;
        fireEffect.GetComponent<ParticleSystem>().Stop();
        fireEffect.SetActive(false);
    }

    void Update()
    {
        detectEnemies();
        checkPlayerRange();

        if(Input.GetKeyDown(KeyCode.P))
        {
            takeDamage(50);
        }

        if (enemiesInRange.Length == 0)
            fireEffect.GetComponent<ParticleSystem>().Stop();

        if (!isFire && fireEffect.GetComponent<ParticleSystem>().isPlaying)
        {
            fireEffect.GetComponent<ParticleSystem>().Stop();
            fireEffect.SetActive(false);
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

    public void activateFire(bool choice)
    {
        print("activateFire() in TurCom");
        isFire = choice;
        if(isFire)
        {
            print("fire is true");
            fireEffect.SetActive(true);
            //fireEffect.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.Red;

        Vector3 startPoint = turretGun.transform.position + turretGun.transform.forward * .4f;

        // Calculate end position based on turret's forward direction
        Vector3 endPoint = startPoint + turretGun.transform.forward * flameDistance;

        // Draw the capsule wireframe
        Gizmos.DrawWireSphere(startPoint, flameRadius);
        Gizmos.DrawWireSphere(endPoint, flameRadius);

        // Draw the cylindrical body with connecting lines
        //Gizmos.DrawLine(point1 + Vector3.right * radius, point2 + Vector3.right * radius);
        //Gizmos.DrawLine(point1 - Vector3.right * radius, point2 - Vector3.right * radius);
        //Gizmos.DrawLine(point1 + Vector3.forward * radius, point2 + Vector3.forward * radius);
        //Gizmos.DrawLine(point1 - Vector3.forward * radius, point2 - Vector3.forward * radius);
    }

}

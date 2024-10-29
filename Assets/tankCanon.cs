using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tankCanon : MonoBehaviour
{
    public Transform projectileSpawn;
    public float fireRate = 1f;
    public float fireAngleThreshold = 5f; // Angle tolerance to fire
    public float turnSpeed = 3f;
    public LayerMask enemyLayer;  // Layer for enemies
    public float detectionRadius = 10f;  // Detection radius for enemies
    private Transform targetEnemy;
    private bool isShooting = false;

    private void Update()
    {
        FindClosestEnemy();  // Constantly look for the closest enemy in range

        if (targetEnemy != null)
        {
            AimAtEnemy();
        }
    }

    private void FindClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            // Find the closest enemy
            targetEnemy = FindClosest(enemiesInRange);
        }
        else
        {
            targetEnemy = null;
        }
    }

    private Transform FindClosest(Collider[] enemies)
    {
        Transform closest = enemies[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closest = enemy.transform;
                closestDistance = distance;
            }
        }

        return closest;
    }

    private void AimAtEnemy()
    {
        if (targetEnemy == null) return;

        // Get direction to enemy
        Vector3 directionToTarget = (targetEnemy.position - transform.position).normalized;
        directionToTarget.y = 0f;  // Ignore height differences for aiming

        // Rotate cannon smoothly towards enemy
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        // Check if the cannon is within firing angle
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget <= fireAngleThreshold && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        while (targetEnemy != null)
        {
            // Get a projectile from the pool
            GameObject bullet = projectileManager.Instance.getProjectile("tankPool", projectileSpawn.position, projectileSpawn.rotation);

            // Fire projectile
            yield return new WaitForSeconds(fireRate);  // Wait for the next shot based on fire rate
        }

        isShooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = other.transform; // Set the target enemy when entering trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (targetEnemy == other.transform) // Check if exiting the target enemy
            {
                targetEnemy = null;  // Stop shooting if the enemy exits range
            }
        }
    }
    /*
    public Transform projectileSpawn;
    public float fireRate = 1f;
    public float fireAngleThreshold = 5f; // Angle tolerance to fire
    public float turnSpeed = 3f;
    public LayerMask enemyLayer;  // Layer for enemies
    public float detectionRadius = 10f;  // Detection radius for enemies
    private Transform targetEnemy;
    private bool isShooting = false;

    private void Update()
    {
        FindEnemy();  // Constantly look for enemies in range

        if (targetEnemy != null)
        {
            AimAtEnemy();
        }
    }

    private void FindEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            targetEnemy = enemiesInRange[0].transform;
        }
        else
        {
            targetEnemy = null;
        }
    }

    private void AimAtEnemy()
    {
        if (targetEnemy == null) return;

        // Get direction to enemy
        Vector3 directionToTarget = (targetEnemy.position - transform.position).normalized;
        directionToTarget.y = 0f;  // Ignore height differences for aiming

        // Rotate cannon smoothly towards enemy
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        // Check if the cannon is within firing angle
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget <= fireAngleThreshold && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        while (targetEnemy != null)
        {
            // Get a projectile from the pool
            GameObject bullet = projectileManager.Instance.getProjectile("tankPool", projectileSpawn.position, projectileSpawn.rotation);

            // Fire projectile
            yield return new WaitForSeconds(fireRate);  // Wait for the next shot based on fire rate
        }

        isShooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = null;  // Stop shooting if the enemy exits range
        }
    }
    */
    /*
    private Transform target;
    public float targetRadius = 10f;
    public float turnSpeed = 3f;
    public float fireRate = 1f;
    public float fireAngleThreshold = 5f;
    [SerializeField]
    private Transform projectileSpawn;
    public LayerMask enemyLayer;

    private bool canLook = false;
    private bool isShooting = false;
    private NavMeshAgent tankAgent; // Reference to the tank's movement

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        tankAgent = GetComponentInParent<NavMeshAgent>(); // Tank movement reference
    }

    void Update()
    {
        CheckEnemyInRange();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    void CheckEnemyInRange()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, targetRadius, enemyLayer);

        if (enemies.Length > 0)
        {
            Collider closestEnemy = GetClosestEnemy(enemies);

            if (closestEnemy != null)
            {
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, targetRadius, enemyLayer))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        canLook = true;
                        target = closestEnemy.transform;

                        if (!isShooting)
                        {
                            StartCoroutine(Shoot());
                        }
                    }
                    else
                    {
                        canLook = false;
                        target = null;
                    }
                }
            }
        }
        else
        {
            canLook = false;
            target = null;
            isShooting = false;
        }
    }

    void LookAtEnemy(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    Collider GetClosestEnemy(Collider[] enemies)
    {
        Collider closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }
        return closest;
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        while (canLook && target != null)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Slightly slow down the tank if it's aiming at an enemy to prevent abrupt stops
            if (tankAgent != null && angleToTarget <= fireAngleThreshold)
            {
                tankAgent.speed *= 0.5f; // Slow down the tank just a little
            }

            if (angleToTarget <= fireAngleThreshold)
            {
                if (tankAgent != null)
                {
                    tankAgent.isStopped = true; // Stop the tank briefly while shooting
                }

                GameObject bullet = projectileManager.Instance.getProjectile("tankPool", projectileSpawn.position, projectileSpawn.rotation);

                yield return new WaitForSeconds(fireRate);

                if (tankAgent != null)
                {
                    tankAgent.isStopped = false; // Resume movement after shooting
                    tankAgent.speed *= 2f; // Restore original speed
                }
            }
            else
            {
                LookAtEnemy(target.position); // Keep aiming at the enemy
            }

            yield return null;
        }

        isShooting = false;
    }
    */
}

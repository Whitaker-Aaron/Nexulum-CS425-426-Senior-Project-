using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tankCannon : MonoBehaviour
{

    private Transform target;  // The target to aim at (e.g., enemies or player)
    public float targetRadius = 10f;  // Adjusted detection radius
    public float turnSpeed = 3f;
    public float fireRate = 1f;  // Reduced fire rate for quicker shooting
    public float fireAngleThreshold = 5f;  // Maximum angle difference to allow shooting
    [SerializeField]
    private Transform projectileSpawn;
    public LayerMask enemyLayer;

    private bool canLook = false;
    private bool isShooting = false;  // Ensure shooting happens at correct intervals

    void FixedUpdate()
    {
        // Rotate the cannon to face the target if it has a target
        if (canLook && target != null)
        {
            LookAtEnemy(target.position);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CheckEnemyInRange();
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection radius for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    void CheckEnemyInRange()
    {
        // Use OverlapSphere to detect enemies within the targetRadius
        Collider[] enemies = Physics.OverlapSphere(transform.position, targetRadius, enemyLayer);

        if (enemies.Length > 0)
        {
            Collider closestEnemy = GetClosestEnemy(enemies);

            if (closestEnemy != null)
            {
                // Perform a line-of-sight check with Raycast to ensure the enemy is not obstructed
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, targetRadius, enemyLayer))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        canLook = true;
                        target = closestEnemy.transform;

                        // Start shooting coroutine if not already shooting
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
                else
                {
                    // Fallback: if no valid Raycast hit, but enemy is still within detection range
                    float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                    if (distance <= targetRadius)
                    {
                        // If within range and no obstruction, still track the enemy
                        canLook = true;
                        target = closestEnemy.transform;

                        if (!isShooting)
                        {
                            StartCoroutine(Shoot());
                        }
                    }
                }
            }
        }
        else
        {
            // No enemies found
            canLook = false;
            target = null;
            isShooting = false; // Stop shooting when no enemies are detected
        }
    }

    void LookAtEnemy(Vector3 position)
    {
        // Rotate the cannon horizontally to face the enemy, ignoring the y-axis
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0f;  // Keep the cannon level

        // Calculate the target rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the cannon towards the enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    Collider GetClosestEnemy(Collider[] enemies)
    {
        Collider closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            // Calculate distance to each enemy
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
        isShooting = true; // Set the shooting flag to true

        while (canLook && target != null)
        {
            // Ensure the cannon is facing the enemy before shooting
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= fireAngleThreshold)  // Check if the cannon is facing the enemy
            {
                // Fire the projectile
                GameObject bullet = projectileManager.Instance.getProjectile("tankPool", projectileSpawn.position, projectileSpawn.rotation);

                // Wait for fireRate duration before allowing the next shot
                yield return new WaitForSeconds(fireRate);
            }
            else
            {
                // Wait a small amount of time before checking again
                yield return null;
            }
        }

        isShooting = false;  // Reset shooting flag when no longer targeting an enemy
    }
}

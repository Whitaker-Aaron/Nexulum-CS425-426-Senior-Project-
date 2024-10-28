using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneCombat : MonoBehaviour
{

    public Transform missileSpawn;
    public Transform bulletSpawn;
    public float fireRate;
    public float missileFireRate;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float detectionRange;
    public LayerMask enemy;
    public float bulletSpeed;
    public bool shooting = false;

    Collider getClosestEnemy(Collider[] enemies)
    {
        Collider closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider c in enemies)
        {
            float distance = Vector3.Distance(transform.position, c.transform.position);
            if (distance < closestDistance)
            {
                closest = c;
                closestDistance = distance;
            }
        }
        return closest;
    }

    IEnumerator shoot()
    {
        shooting = true;
        GameObject bullet = projectileManager.Instance.getProjectile("dronePool", bulletSpawn.position, bulletSpawn.rotation);
        yield return new WaitForSeconds(fireRate);
        shooting = false;
        yield break;
    }

    void smoothLook(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;  // Keep the y-axis at 0
        Quaternion targetRot = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, gameObject.GetComponent<droneFollow>().rotationSpeed * Time.deltaTime);


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, enemy);

        if (enemies.Length > 0)
        {
            gameObject.GetComponent<droneFollow>().isAiming = true;
            Collider closestEnemy = getClosestEnemy(enemies);
            if (closestEnemy != null)
            {
                print("Looking at enemy");
                Vector3 temp = new Vector3(closestEnemy.transform.position.x, 0.5f, closestEnemy.transform.position.z);
                bulletSpawn.transform.LookAt(temp);
                smoothLook(closestEnemy.transform);
                if (!shooting)
                    StartCoroutine(shoot());
            }
        }
        else
        {
            gameObject.GetComponent<droneFollow>().isAiming = false;
        }
    }
}

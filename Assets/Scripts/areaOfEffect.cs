using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaOfEffect : MonoBehaviour
{

    bool check = false;
    public float checkTime = 5f;
    int enemyLayer;

    Vector3 position;
    float radius, damageT, damageR;
    int fireDamage;

    private Dictionary<GameObject, Coroutine> burningEnemies = new Dictionary<GameObject, Coroutine>();

    public void startCheck(Vector3 pos, float rad, int fireDmg, float dmgTime, float dmgRate, float abilityTime)
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        position = pos;
        radius = rad;
        damageT = dmgTime;
        damageR = dmgRate;
        fireDamage = fireDmg;
        check = true;

        Destroy(gameObject, abilityTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(position, radius);
    }

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        burningEnemies.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (check)
        {
            // Get enemies within the radius
            Collider[] enemiesInRange = Physics.OverlapSphere(position, radius, enemyLayer);

            HashSet<GameObject> currentEnemies = new HashSet<GameObject>();

            foreach (Collider c in enemiesInRange)
            {
                GameObject enemy = c.gameObject;

                // Add enemy to set of current enemies
                currentEnemies.Add(enemy);

                // If enemy is not already burning, apply burning effect
                if (!burningEnemies.ContainsKey(enemy))
                {
                    // Start the damage-over-time coroutine and store it in the dictionary
                    Coroutine burnCoroutine = StartCoroutine(ApplyBurnEffect(enemy));
                    burningEnemies.Add(enemy, burnCoroutine);
                }
            }

            // Remove enemies that are no longer in range
            List<GameObject> enemiesToRemove = new List<GameObject>();

            foreach (var enemy in burningEnemies.Keys)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    // Stop applying the effect immediately, but let the burning continue on its own
                    enemiesToRemove.Add(enemy);
                }
            }

            // Remove out-of-range enemies from the dictionary
            foreach (var enemy in enemiesToRemove)
            {
                burningEnemies.Remove(enemy);
            }
        }
    }


    private IEnumerator ApplyBurnEffect(GameObject enemy)
    {
        EnemyFrame enemyFrame = enemy.GetComponent<EnemyFrame>();

        // Apply damage-over-time to the enemy
        if (enemyFrame != null && !enemyFrame.dmgOverTimeActivated)
        {
            enemyFrame.dmgOverTimeActivated = true;
            yield return enemyFrame.StartCoroutine(enemyFrame.dmgOverTime(fireDamage, damageT, damageR, EnemyFrame.DamageType.Fire));

            // Ensure that the burning effect ends after the duration
            enemyFrame.dmgOverTimeActivated = false;
        }
    }
}

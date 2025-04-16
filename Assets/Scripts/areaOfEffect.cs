using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaOfEffect : MonoBehaviour
{

    bool check = false;
    public float checkTime = 5f;

    Vector3 position;
    float radius, damageT, damageR;
    int fireDamage;

    private Dictionary<GameObject, Coroutine> burningEnemies = new Dictionary<GameObject, Coroutine>();

    public void startCheck(Vector3 pos, float rad, int fireDmg, float dmgTime, float dmgRate, float abilityTime)
    {
        // We no longer need the enemy layer mask since we're checking tags directly
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
            // Get entities within the radius - we'll check for both enemies and bosses
            Collider[] entitiesInRange = Physics.OverlapSphere(position, radius);

            HashSet<GameObject> currentEntities = new HashSet<GameObject>();

            foreach (Collider c in entitiesInRange)
            {
                GameObject entity = c.gameObject;
                
                // Only process entities with Enemy or Boss tag
                if (entity.CompareTag("Enemy") || entity.CompareTag("Boss"))
                {
                    // Add entity to set of current entities
                    currentEntities.Add(entity);

                    // If entity is not already burning, apply burning effect
                    if (!burningEnemies.ContainsKey(entity))
                    {
                        // Start the damage-over-time coroutine and store it in the dictionary
                        Coroutine burnCoroutine = StartCoroutine(ApplyBurnEffect(entity));
                        burningEnemies.Add(entity, burnCoroutine);
                    }
                }
            }

            // Remove entities that are no longer in range
            List<GameObject> entitiesToRemove = new List<GameObject>();

            foreach (var entity in burningEnemies.Keys)
            {
                if (!currentEntities.Contains(entity))
                {
                    // Stop applying the effect immediately, but let the burning continue on its own
                    entitiesToRemove.Add(entity);
                }
            }

            // Remove out-of-range entities from the dictionary
            foreach (var entity in entitiesToRemove)
            {
                burningEnemies.Remove(entity);
            }
        }
    }


    private IEnumerator ApplyBurnEffect(GameObject entity)
    {
        // Check the tag of the entity
        if (entity.CompareTag("Enemy"))
        {
            // Handle regular enemy
            EnemyFrame enemyFrame = entity.GetComponent<EnemyFrame>();

            // Apply damage-over-time to the enemy
            if (enemyFrame != null && !enemyFrame.dmgOverTimeActivated)
            {
                enemyFrame.dmgOverTimeActivated = true;
                yield return enemyFrame.StartCoroutine(enemyFrame.dmgOverTime(fireDamage, damageT, damageR, EnemyFrame.DamageType.Fire));

                // Ensure that the burning effect ends after the duration
                enemyFrame.dmgOverTimeActivated = false;
            }
        }
        else if (entity.CompareTag("Boss"))
        {
            // Handle boss enemy
            golemBoss boss = entity.GetComponent<golemBoss>();
            
            if (boss != null)
            {
                // Apply damage to the boss at regular intervals for the duration
                float endTime = Time.time + damageT;
                
                while (Time.time < endTime)
                {
                    boss.takeDamage(fireDamage);
                    
                    // Wait for the next damage tick
                    yield return new WaitForSeconds(damageR);
                }
            }
        }
    }
}

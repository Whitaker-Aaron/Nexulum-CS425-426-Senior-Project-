using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GraveDigger : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    private bool _isAttacking;
    private bool _isActive;
    public bool isActive
    {
        get { return _isActive; }
        set
        {
            if (_isActive != value)  // Only set if the value is different
            {
                _isActive = value;
                // Do the other necessary actions
            }
        }
    }
    public LayerMask Player;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    public int attackDamage = 20;
    private float timeOffset;

    int curSkeletonCounter = 0;
    public GameObject skeletonPrefab1; // First skeleton prefab
    public GameObject skeletonPrefab2; // Second skeleton prefab
    public GameObject smokeEffectPrefab; // Smoke effect prefab

    private float firstSpawnDelay = 5f; // Initial delay before first spawn
    private float spawnInterval = 15f; // Time in seconds between spawns

    private bool isSpawning = true;
    public bool canAttack = true;
    [SerializeField] bool useFirstSpawnDelay = true;

    Coroutine curSpawnRoutine;
    private void Awake()
    {
        // Set active state immediately
        //isActive = true;
        
        // Automatically find the Player if not set in Inspector
        if (player == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            }
            canAttack = true;
        }

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyHead!");
        }
    }

    void Start()
    {
        // Ensure the spawning routine is started in Start as well as OnEnable
        if (curSpawnRoutine == null)
        {
            curSpawnRoutine = StartCoroutine(SpawnSkeletonsRoutine());
        }
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
        }
    }

    public void OnEnable()
    {
        curSpawnRoutine = StartCoroutine(SpawnSkeletonsRoutine());
    }

    public void OnDisable()
    {
        if (curSpawnRoutine != null)
        {
            StopCoroutine(curSpawnRoutine);
            curSpawnRoutine = null;
        }
    }
    
    // Public method to force activation from outside
    public void ForceActivate()
    {
        //isActive = true;
        if (curSpawnRoutine == null)
        {
            curSpawnRoutine = StartCoroutine(SpawnSkeletonsRoutine());
        }
    }

    IEnumerator SpawnSkeletonsRoutine()
    {
        // First spawn after initial delay
        float waitTime = 0f;
        float maxWaitTime = 10f; // Maximum time to wait for activation before proceeding anyway
        
        while (!isActive && waitTime < maxWaitTime)
        {
            Debug.Log("Grave digger not active");
            //waitTime += Time.deltaTime;
            yield return null;
        }
        
        // If we've waited too long, force activation to prevent being stuck
        if (!isActive && waitTime >= maxWaitTime)
        {
            Debug.Log("Grave digger activation timeout - forcing active state");
            //isActive = true;
        }
        
        if(useFirstSpawnDelay) yield return new WaitForSeconds(firstSpawnDelay);
        SpawnSkeletons();

        // Continue spawning at regular intervals
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnSkeletons();
            yield return null;
        }
    }

    void SpawnSkeletons()
    {
        // Check if prefabs are assigned
        if (skeletonPrefab1 == null || skeletonPrefab2 == null)
        {
            Debug.LogError("Skeleton prefabs are not assigned!");
            return;
        }
        
        // Allow spawning if counter is less than limit OR if this is called from onDeath (emergency spawn)
        bool forceSpawn = curSkeletonCounter == 0 && !isSpawning; // Emergency spawn condition
        
        if (curSkeletonCounter < 4 || forceSpawn)
        {
            Vector3 spawnPosition1 = new Vector3(transform.position.x + 5f, transform.position.y + 1f, transform.position.z);
            Vector3 spawnPosition2 = new Vector3(transform.position.x - 5f, transform.position.y + 1f, transform.position.z);

            // Spawn smoke effect at both locations
            if (smokeEffectPrefab != null)
            {
                Instantiate(smokeEffectPrefab, spawnPosition1, Quaternion.identity);
                Instantiate(smokeEffectPrefab, spawnPosition2, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Smoke effect prefab is not assigned!");
            }

            // Spawn skeletons
            GameObject skeleton1 = Instantiate(skeletonPrefab1, spawnPosition1, Quaternion.identity);
            GameObject skeleton2 = Instantiate(skeletonPrefab2, spawnPosition2, Quaternion.identity);
            skeleton1.transform.parent = this.transform.parent;
            skeleton2.transform.parent = this.transform.parent;
            if (skeleton1.GetComponent<enemyMinionCombat>() != null) skeleton1.GetComponent<enemyMinionCombat>().tempEnemy = true;
            if (skeleton2.GetComponent<enemyMinionCombat>() != null) skeleton2.GetComponent<enemyMinionCombat>().tempEnemy = true;

            // Warp skeletons to ensure proper positioning
            if (skeleton1.GetComponent<NavMeshAgent>() != null)
                skeleton1.GetComponent<NavMeshAgent>().Warp(skeleton1.transform.position);
            if (skeleton2.GetComponent<NavMeshAgent>() != null)
                skeleton2.GetComponent<NavMeshAgent>().Warp(skeleton2.transform.position);
                
            curSkeletonCounter += 2;
            Debug.Log("GraveDigger spawned skeletons. Total count: " + curSkeletonCounter);
        }
        else
        {
            Debug.Log("Skeleton counter limit reached: " + curSkeletonCounter);
        }
    }

    public void onDeath()
    {
        isSpawning = false;
        // Ensure we've spawned at least one set of skeletons before dying
        if (curSkeletonCounter == 0)
        {
            Debug.Log("GraveDigger dying without spawning - forcing one spawn");
            SpawnSkeletons();
        }
    }

    public enemyInt getType()
    {
        return this;
    }

    public bool isAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)
            {
                _isAttacking = value;
            }
        }
    }

    public IEnumerator attackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }

    void attackPlayer()
    {
        if (!canAttack) return;
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach (Collider player in playerInRange)
        {
            //attack player commands
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if (player.tag == "Player") {
                playerRef.takeDamage(attackDamage, knockBackDir);
                StartCoroutine(attackCooldown());
            };
            Debug.Log(player.tag);
            
        }

    }
}

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

    public GameObject skeletonPrefab1; // First skeleton prefab
    public GameObject skeletonPrefab2; // Second skeleton prefab
    public GameObject smokeEffectPrefab; // Smoke effect prefab

    private float firstSpawnDelay = 5f; // Initial delay before first spawn
    private float spawnInterval = 45f; // Time in seconds between spawns

    private bool isSpawning = true;
    public bool canAttack = true;

    Coroutine curSpawnRoutine;
    private void Awake()
    {
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
        StopCoroutine(curSpawnRoutine);
        curSpawnRoutine = null;
    }

    IEnumerator SpawnSkeletonsRoutine()
    {
        // First spawn after initial delay
        while (!isActive)
        {
            Debug.Log("Grave digger not active");
            yield return null;
        }
        yield return new WaitForSeconds(firstSpawnDelay);
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
        if (skeletonPrefab1 != null && skeletonPrefab2 != null)
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
                Debug.LogError("Smoke effect prefab is not assigned!");
            }

            // Spawn skeletons
            GameObject skeleton1 = Instantiate(skeletonPrefab1, spawnPosition1, Quaternion.identity);
            GameObject skeleton2 = Instantiate(skeletonPrefab2, spawnPosition2, Quaternion.identity);
            skeleton1.transform.parent = this.transform.parent;
            skeleton2.transform.parent = this.transform.parent;
            if (skeleton1.GetComponent<enemyMinionCombat>() != null) skeleton1.GetComponent<enemyMinionCombat>().tempEnemy = true;
            if (skeleton2.GetComponent<enemyMinionCombat>() != null) skeleton2.GetComponent<enemyMinionCombat>().tempEnemy = true;

            // Warp skeletons to ensure proper positioning
            skeleton1.GetComponent<NavMeshAgent>().Warp(skeleton1.transform.position);
            skeleton2.GetComponent<NavMeshAgent>().Warp(skeleton2.transform.position);
        }
        else
        {
            Debug.LogError("Skeleton prefabs are not assigned!");
        }
    }

    public void onDeath()
    {
        isSpawning = false;
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

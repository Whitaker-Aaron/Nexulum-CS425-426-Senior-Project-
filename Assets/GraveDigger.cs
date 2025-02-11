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
    public LayerMask Player;
    public float attackRange = .5f;
    public int attackDamage = 20;
    private float timeOffset;

    public GameObject skeletonPrefab1; // First skeleton prefab
    public GameObject skeletonPrefab2; // Second skeleton prefab
    private float spawnInterval = 30f; // Time in seconds between spawns

    private bool isSpawning = true;

    void Start()
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
        }

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyHead!");
        }

        StartCoroutine(SpawnSkeletonsRoutine());
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
        }
    }

    IEnumerator SpawnSkeletonsRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnSkeletons();
        }
    }

    void SpawnSkeletons()
    {
        if (skeletonPrefab1 != null && skeletonPrefab2 != null)
        {
            Vector3 spawnPosition1 = new Vector3(transform.position.x + 5f, transform.position.y + 1f, transform.position.z);
            Vector3 spawnPosition2 = new Vector3(transform.position.x - 5f, transform.position.y + 1f, transform.position.z);

            Instantiate(skeletonPrefab1, spawnPosition1, Quaternion.identity);
            Instantiate(skeletonPrefab2, spawnPosition2, Quaternion.identity);
            skeletonPrefab1.GetComponent<NavMeshAgent>().Warp(skeletonPrefab1.transform.position);
            skeletonPrefab2.GetComponent<NavMeshAgent>().Warp(skeletonPrefab2.transform.position);

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

    void attackPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach (Collider player in playerInRange)
        {
            if (player.CompareTag("Player"))
            {
                // Ensure we get the CharacterBase component
                playerRef = player.GetComponent<CharacterBase>();
                if (playerRef != null)
                {
                    Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
                    playerRef.takeDamage(attackDamage, knockBackDir);
                }
                else
                {
                    Debug.LogError("CharacterBase component not found on Player!");
                }
            }
        }
    }
}

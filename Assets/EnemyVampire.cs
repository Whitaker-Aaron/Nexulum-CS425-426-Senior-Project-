using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyVampire : MonoBehaviour, enemyInt
{
    public Transform player;
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    private bool _isAttacking;
    public LayerMask Player;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    public int attackDamage = 20;
    private float timeOffset;

    public GameObject batPrefab1;
    public GameObject batPrefab2;
    public GameObject batPrefab3;
    public GameObject batPrefab4;
    public GameObject smokeEffectPrefab;

    private float firstSpawnDelay = 5f;
    private float spawnInterval = 40f;

    private bool isSpawning = true;
    public bool canAttack = true;

    void Start()
    {
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
            Debug.LogError("EnemyStateManager not found on EnemyVampire!");
        }

        StartCoroutine(SpawnBatsRoutine());
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
        }
    }

    IEnumerator SpawnBatsRoutine()
    {
        yield return new WaitForSeconds(firstSpawnDelay);
        SpawnBats();

        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBats();
        }
    }

    void SpawnBats()
    {
        if (batPrefab1 != null && batPrefab2 != null && batPrefab3 != null && batPrefab4 != null)
        {
            Vector3 spawnPosition1 = new Vector3(transform.position.x + 5f, transform.position.y + 2f, transform.position.z);
            Vector3 spawnPosition2 = new Vector3(transform.position.x - 5f, transform.position.y + 2f, transform.position.z);
            Vector3 spawnPosition3 = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 5f);
            Vector3 spawnPosition4 = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 5f);

            if (smokeEffectPrefab != null)
            {
                Instantiate(smokeEffectPrefab, spawnPosition1, Quaternion.identity);
                Instantiate(smokeEffectPrefab, spawnPosition2, Quaternion.identity);
                Instantiate(smokeEffectPrefab, spawnPosition3, Quaternion.identity);
                Instantiate(smokeEffectPrefab, spawnPosition4, Quaternion.identity);

            }
            else
            {
                Debug.LogError("Smoke effect prefab is not assigned!");
            }

            GameObject bat1 = Instantiate(batPrefab1, spawnPosition1, Quaternion.identity);
            GameObject bat2 = Instantiate(batPrefab2, spawnPosition2, Quaternion.identity);
            GameObject bat3 = Instantiate(batPrefab3, spawnPosition3, Quaternion.identity);
            GameObject bat4 = Instantiate(batPrefab4, spawnPosition4, Quaternion.identity);

            bat1.GetComponent<NavMeshAgent>().Warp(bat1.transform.position);
            bat2.GetComponent<NavMeshAgent>().Warp(bat2.transform.position);
            bat3.GetComponent<NavMeshAgent>().Warp(bat3.transform.position);
            bat4.GetComponent<NavMeshAgent>().Warp(bat4.transform.position);
        }
        else
        {
            Debug.LogError("Bat prefabs are not assigned!");
        }
    }

    public void onDeath() { isSpawning = false; }

    public enemyInt getType() { return this; }

    public bool isAttacking
    {
        get { return _isAttacking; }
        set { if (_isAttacking != value) _isAttacking = value; }
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

        if (playerInRange.Length > 0)
        {
            foreach (Collider player in playerInRange)
            {
                if (player.CompareTag("Player"))
                {
                    Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
                    playerRef.takeDamage(attackDamage, knockBackDir);
                    StartCoroutine(attackCooldown());
                    break;
                }
            }
        }
    }
}

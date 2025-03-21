using UnityEngine;
using System.Collections;

public class EnemyBat : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    public Transform frontDirection; // Reference to the front direction object
    CharacterBase playerRef;

    public LayerMask Player;

    public int attackDamage = 20;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool _isAttacking;

    // Add necessary variables for movement
    public float diveSpeed = 5f;
    public float climbSpeed = 2f;
    private bool isDiving = false;
    private bool isFlying = true;

    private Vector3 originalPosition;
    private bool isReturning = false;

    // Animator
    private Animator animator;

    // NavMeshAgent
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false; // Disable the NavMeshAgent to allow manual movement
        }

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

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyBat!");
        }

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on EnemyBat!");
        }

        transform.position = new Vector3(transform.position.x, playerRef.transform.position.y + 2f, transform.position.z);
        originalPosition = transform.position;

    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
            StartCoroutine(AttackRoutine());

            if (isReturning)
            {
                FaceOriginalPosition();
            }
            else
            {
                FacePlayer();
            }
        }
    }

    public void onDeath() { }

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

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds before diving

            if (player != null && !isDiving && !isReturning)
            {
                isDiving = true;
                animator.SetTrigger("StartGliding");

                Vector3 playerPosition = player.position;
                if (Vector3.Distance(transform.position, playerPosition) > 20f)
                {
                    break;
                }

                while (Vector3.Distance(transform.position, playerPosition) > 1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerPosition, diveSpeed * Time.deltaTime);
                    yield return null;
                }

                animator.SetTrigger("StopGliding");
                yield return new WaitForSeconds(1.5f); // Pause for 1 second after reaching the player

                isDiving = false;
                isReturning = true;

                while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, originalPosition, climbSpeed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(5f);
                isReturning = false;
            }
        }
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

    private void FacePlayer()
    {
        if (frontDirection != null && player != null)
        {
            Vector3 directionToPlayer = (player.position - frontDirection.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void FaceOriginalPosition()
    {
        // Calculate direction to the original position
        Vector3 directionToOriginal = (originalPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToOriginal);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

}
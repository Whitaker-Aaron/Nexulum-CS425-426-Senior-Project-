using UnityEngine;
using System.Collections;
using static UnityEngine.GridBrushBase;

public class EnemyBat : MonoBehaviour, enemyInt
{
    public Transform player;
    private GameObject playerObj;
    public Transform attackPoint;
    public Transform frontDirection; // Reference to the front direction object
    CharacterBase playerRef;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Coroutine attackRoutineInstance = null;

    public LayerMask Player;

    public int attackDamage = 20;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool _isAttacking;
    private bool _isActive;

    // Add necessary variables for movement
    public float diveSpeed = 5f;
    public float climbSpeed = 2f;
    private bool isDiving = false;
    private bool isFlying = true;
    private bool isReturning = false;
    private bool faceplayer = true;

    private bool rotatingAroundPlayer = false;
    public float rotationSpeed = 20f;
    private float rotationDuration = 3f;
    private int rotationDirection;

    public float routineCooldown = 3f;

    private Vector3 originalPosition;


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

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on EnemyBat!");
        }

        originalPosition = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
            if (attackRoutineInstance == null)
            {
                attackRoutineInstance = StartCoroutine(AttackRoutine());
            }
        }

        if (faceplayer)
        {
            FacePlayer();
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
        yield return new WaitForSeconds(routineCooldown); // Wait before diving

        if (player != null && !isDiving && !isReturning)
        {
            if (Vector3.Distance(transform.position, player.position) > 20f)
            {
                Debug.Log("Player is too far away. Cancelling attack routine.");
                attackRoutineInstance = null;
                yield break;
            }

            Vector3 playerPosition = player.position;
            Debug.Log("New player position stored: " + playerPosition);

            rotationDuration = Random.Range(1.5f, 4f); // Set random rotation duration
            StartCoroutine(RotateAroundPlayer());

            yield return new WaitForSeconds(rotationDuration); // Wait for rotation to finish

            originalPosition = transform.position;
            animator.SetTrigger("StartGliding");

            isDiving = true;
            faceplayer = false;

            playerPosition = player.position;

            while (Vector3.Distance(transform.position, playerPosition) > 1f && transform.position.y > playerRef.transform.position.y + 0.3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, diveSpeed * Time.deltaTime);
                yield return null;
            }

            animator.SetTrigger("StopGliding");

            isDiving = false;
            yield return new WaitForSeconds(0.6f);
            isReturning = true;

            while (Vector3.Distance(transform.position, originalPosition) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, climbSpeed * Time.deltaTime);
                FaceOriginalPosition();
                yield return null;
            }

            faceplayer = true;
            isReturning = false;
        }

        attackRoutineInstance = null;
    }

    private void OnDisable()
    {
        attackRoutineInstance = null;
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
        if (!isReturning && frontDirection != null && player != null)
        {
            Debug.Log("FacePlayer is executing!");

            Vector3 directionToPlayer = (player.position - frontDirection.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
        }
    }



    private void FaceOriginalPosition()
    {
        if (isReturning)
        {
            Vector3 directionToOriginal = (originalPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToOriginal);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    private IEnumerator RotateAroundPlayer()
    {
        rotatingAroundPlayer = true;
        rotationDirection = Random.value < 0.5f ? -1 : 1;

        float randomizedRotationDuration = Random.Range(1.5f, 4f); // Random duration between 1.5s and 4s
        float elapsed = 0f;

        while (elapsed < randomizedRotationDuration)
        {
            elapsed += Time.deltaTime;
            if (player != null)
            {
                transform.RotateAround(player.position, Vector3.up, rotationSpeed * rotationDirection * Time.deltaTime);
            }
            yield return null;
        }

        rotatingAroundPlayer = false;
    }


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
}


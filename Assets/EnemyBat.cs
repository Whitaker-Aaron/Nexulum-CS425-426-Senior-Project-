using UnityEngine;
using System.Collections;
using static UnityEngine.GridBrushBase;

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
    private bool _isActive;

    // Add necessary variables for movement
    public float diveSpeed = 5f;
    public float climbSpeed = 2f;
    private bool isDiving = false;
    private bool isFlying = true;
    private bool isReturning = false;
    private bool finishedAttack = true;

    private bool rotatingAroundPlayer = false;
    private float rotationSpeed = 20f;
    private float rotationDuration = 3f;
    private int rotationDirection;

    private Vector3 originalPosition;

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
        }

        if (finishedAttack)
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
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds before diving

            if (player != null && !isDiving && !isReturning)
            {
                isDiving = true;

                finishedAttack = false;

                StartCoroutine(RotateAroundPlayer());

                yield return new WaitForSeconds(rotationDuration);

                originalPosition = transform.position;
                animator.SetTrigger("StartGliding");

                Vector3 playerPosition = player.position;
                if (Vector3.Distance(transform.position, playerPosition) > 20f)
                {
                    break;
                }

                while (Vector3.Distance(transform.position, playerPosition) > 1f && transform.position.y > playerRef.transform.position.y + 0.3f)
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
                    FaceOriginalPosition(); // Continuously call this to ensure rotation is applied
                    yield return null;
                }

                yield return new WaitForSeconds(5f);
                finishedAttack = true;
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
        if (!isReturning && frontDirection != null && player != null)
        {
            Vector3 directionToPlayer = (player.position - frontDirection.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    private void FaceOriginalPosition()
    {
        if (isReturning)
        {
            Vector3 directionToOriginal = (originalPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToOriginal);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f &&
                Vector3.Distance(transform.position, originalPosition) <= 0.1f)
            {
                isReturning = false; // Mark return as completed
                FacePlayer(); // Smoothly start looking at the player again
            }
        }
    }
    private IEnumerator RotateAroundPlayer()
    {
        rotatingAroundPlayer = true;
        rotationDirection = Random.value < 0.5f ? -1 : 1;

        float elapsed = 0f;

        while (elapsed < rotationDuration)
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


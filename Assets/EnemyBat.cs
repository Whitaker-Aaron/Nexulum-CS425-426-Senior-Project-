using UnityEngine;
using System.Collections;

public class EnemyBat : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    public LayerMask Player;

    public int attackDamage = 20;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool _isAttacking;

    // Add necessary variables for movement
    public float diveSpeed = 3f;
    public float climbSpeed = 2f;
    private bool isDiving = false;
    private bool isFlying = true;

    // Animator
    private Animator animator;

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

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyBat!");
        }

        // Get the playerRef to access player's components
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        animator = GetComponent<Animator>(); // Get reference to the animator
        if (animator == null)
        {
            Debug.LogError("Animator not found on EnemyBat!");
        }

        StartCoroutine(DiveAttackRoutine()); // Start the dive attack routine
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
        }

        if (isDiving)
        {
            DiveMovement(); // Handle dive movement
        }
        else if (isFlying)
        {
            FlyMovement(); // Handle flying movement
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
                    break; // Prevent triggering multiple cooldowns per frame
                }
            }
        }
    }

    // Coroutine to handle dive attack every 15 seconds
    private IEnumerator DiveAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f); // Wait for 15 seconds
            StartDiveAttack(); // Trigger the dive attack
            yield return new WaitUntil(() => !isDiving); // Wait until diving is done
        }
    }

    // Start the dive attack
    private void StartDiveAttack()
    {
        animator.SetTrigger("StartGliding"); // Trigger Armature_Air-Gliding animation
        isDiving = true;
        isFlying = false;
    }

    // Handle dive movement towards the player
    private void DiveMovement()
    {
        Vector3 targetPosition = new Vector3(player.position.x, 0.2f, player.position.z); // Target Y position of 0.2
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, diveSpeed * Time.deltaTime);

        // When the bat reaches y value of 0.2, switch to flying
        if (transform.position.y <= 0.2f)
        {
            StopDive();
        }
    }

    // Stop the dive and start flying up
    private void StopDive()
    {
        animator.SetTrigger("StopGliding"); // Transition to flying state
        isDiving = false;
        isFlying = true;
    }

    // Handle flying movement
    private void FlyMovement()
    {
        // Fly upwards to y = 4.5
        if (transform.position.y < 4.5f)
        {
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
        }
        else
        {
            // If it reaches 4.5, continue flying
            animator.SetTrigger("StartFlying"); // Ensure the flying animation continues
        }
    }
}

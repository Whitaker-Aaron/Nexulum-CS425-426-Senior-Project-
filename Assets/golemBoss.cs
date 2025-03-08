using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class golemBoss : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float attackCooldown = 2f;
    public float attackRange = 5f;
    public int maxAttacks = 3;
    public float turnSpeed = 15f; // Adjusted turn speed for smoother turning
    public float detectionRadius = 7f; // Detection range
    public float maxAngle = 4f;

    private NavMeshAgent agent;
    private bool isAttacking = false;
    private bool isRecovering = false;
    private bool isTurning = false;

    public LayerMask playerLayer; // Player layer for detection



    //Health
    public int MAXHEALTH;
    private int health;

    //attack effects
    [SerializeField]
    private List<GameObject> attackList = new List<GameObject>();
    private List<Vector3> atckPos = new List<Vector3>();


    //Damage
    public float atkRng1;
    public int atkDmg1, atkDmg2, atkDmg3, atkDmg4, atkDmg5;

    //-----------------Main Functions------------------------------

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return;

        // Check if player is within detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        bool playerDetected = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerDetected = true;
                break;
            }
        }

        if (!playerDetected)
        {
            animator.SetFloat("Forward", 0); // Stop movement animation
            return; // Stop processing if player is out of range
        }

        if (isAttacking || isRecovering)
        {
            agent.isStopped = true;
            return;
        }

        // Calculate the angle to the player
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        // If the angle exceeds the threshold, start turning
        if (Mathf.Abs(angle) > maxAngle && !isTurning)
        {
            StartCoroutine(TurnInPlace(angle));
            return; // Don't move while turning
        }

        // Move towards player if not turning
        if (!isTurning)
        {
            gameObject.transform.LookAt(player.position, Vector3.up);
            agent.isStopped = false;
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Forward", speed / agent.speed); // Normalize speed for animation
            animator.SetFloat("Turn", 0); // Reset turn animation when moving
        }

        // Attack if in range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            StartCoroutine(AttackSequence());
        }
    }




    //-----------------Turning------------------------------

    IEnumerator TurnInPlace(float totalAngle)
    {
        isTurning = true;
        agent.isStopped = true;
        animator.SetFloat("Forward", 0); // No movement during turning
        animator.SetFloat("Turn", Mathf.Sign(totalAngle)); // Update turn animation based on angle direction

        float remainingAngle = Mathf.Abs(totalAngle);
        float turnDirection = Mathf.Sign(totalAngle);

        // Rotate until the remaining angle is small enough
        while (remainingAngle > 1) // Small buffer to stop rotation
        {
            float turnStep = turnSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * turnDirection * turnStep);
            remainingAngle -= turnStep;
            yield return null;
        }

        // After turning, set the rotation to face the player
        transform.rotation = Quaternion.LookRotation(transform.forward);
        isTurning = false;
    }







    //-----------------ATTACKING------------------------------


    IEnumerator AttackSequence()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("Attack", true);
        animator.SetFloat("Forward", 0);

        int attackCount = Random.Range(1, maxAttacks + 1);


        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCooldown);

        isRecovering = true;
        yield return new WaitForSeconds(1f);

        isRecovering = false;
        isAttacking = false;
        agent.isStopped = false;
    }




    //IEnumerator checkAttack(float attackTime,)





    //-----------------Draw Gizmos------------------------------


    // Draw Gizmo Sphere to visualize detection range
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, atkRng1);
    }



}

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

    [SerializeField] GameObject weapon;

    public Vector3 weaponOffset;
    public float weaponRad;
    public int weaponDmg;
    public bool hitPlayer = false;
    public float hitPlayerCooldown;


    //Health
    public int MAXHEALTH;
    private int health;
    bool bossDying = false;

    //attack effects
    [SerializeField]
    private List<Vector3> attackList = new List<Vector3>();
    private List<Vector3> atckPos = new List<Vector3>();

    public float jumpSpeed, jumpLength, jumpCooldown, longJumpSpeed, longJumpLength, longJumpCooldown;
    public bool canJump = true, canLongJump = true;


    //Damage
    public float atkRng1, atkRng2, atkRng3, atkRng4, atkRng5;
    public int atkDmg1, atkDmg2, atkDmg3, atkDmg4, atkDmg5;


    //effects
    [SerializeField] GameObject slash1, slash2, slashSlam, slashSlam1, slashSlam2, jumpSlam;
    public float slash1Time = 1f;
    public float slamRadius = 2.5f, slamTime1, slamTime2;


    //-----------------Main Functions------------------------------

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        slash1.GetComponent<ParticleSystem>().Stop();
        health = MAXHEALTH;
        animator.SetBool("death", false);
    }

    void Update()
    {
        if (bossDying)
            return;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return;

        //if (hitPlayer)
            //StartCoroutine(hitPlayerWait());

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
        checkWeaponArea();
        checkAttackAreas();
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
            //StartCoroutine(AttackSequence());
        }

        
        
    }

    IEnumerator hitPlayerWait()
    {
        yield return new WaitForSeconds(hitPlayerCooldown);
        hitPlayer = false;
        yield break;
    }


    public void takeDamage(int damage)
    {
        if (health - damage > 0)
            health -= damage;
        else
            StartCoroutine(bossDeath());
    }

    public IEnumerator bossDeath()
    {
        if (bossDying)
            yield break;

        bossDying = true;

        animator.SetBool("death", true);
        animator.Play("death");
        animator.SetBool("death", false);
        yield break;
    }


    IEnumerator slamArea()
    {

        yield return new WaitForSeconds(slamTime1);

        yield break;
    }
    public Vector3 slamOffset;
    IEnumerator slamArea2()
    {

        yield return new WaitForSeconds(slamTime2);
        jumpSlam.GetComponent<ParticleSystem>().Play();
        Collider[] temp = Physics.OverlapSphere(gameObject.transform.position + slamOffset, slamRadius);
        yield break;
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

    void checkAttackAreas()
    {
        if (isAttacking || isRecovering || !canJump)
            return;

        for(int i = 0; i < attackList.Count; i++)
        {
            switch(i)
            {
                case 0:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng1)
                        StartCoroutine(AttackSequence());
                    break;

                case 1:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng2 && canJump)
                        StartCoroutine(JumpAttack(jumpSpeed, jumpLength));
                    break;
                    
                case 2:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng3 && canLongJump)
                        StartCoroutine(longJumpAttack(longJumpSpeed, longJumpLength));
                    break;
            }
        }
    }

    void checkWeaponArea()
    {
        if (!isAttacking || hitPlayer || isRecovering)
            return;

        Debug.Log("golem: checkingWeapon");
        Collider[] player = Physics.OverlapSphere(weapon.GetComponent<golemBossWeapon>().headPosition.transform.position + weaponOffset, weaponRad, playerLayer);
        foreach(Collider p in player)
        {
            hitPlayer = true;
            p.GetComponent<CharacterBase>().takeDamage(weaponDmg, gameObject.transform.forward);
            UIManager.instance.DisplayDamageNum(p.transform, weaponDmg);
            StartCoroutine(hitPlayerWait());
        }
    }

    IEnumerator longJumpAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canLongJump)
            yield break;

        canLongJump = false;
        isAttacking = true;
        agent.isStopped = true;

        Debug.Log("golem: Starting longJump Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("jumpAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("jumpAttack");
        animator.SetBool("jumpAttack", false);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        Debug.Log("golem: long Jump Attack Complete, Entering Recovery");

        isRecovering = true;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");

        isRecovering = false;
        isAttacking = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(longJumpCooldown);

        //animator.SetTrigger("ReturnToIdle"); // Use a trigger for animation transition
        canLongJump = true;
        yield break;
    }

    IEnumerator JumpAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canJump)
            yield break;

        canJump = false;
        isAttacking = true;
        agent.isStopped = true;

        Debug.Log("golem: Starting Jump Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("jump", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("jumpAtck");
        animator.SetBool("jump", false);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        Debug.Log("golem: Jump Attack Complete, Entering Recovery");

        isRecovering = true;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");

        isRecovering = false;
        isAttacking = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(jumpCooldown);

        //animator.SetTrigger("ReturnToIdle"); // Use a trigger for animation transition
        canJump = true;
    }


    IEnumerator AttackSequence()
    {
        if (isAttacking)
            yield break;

        isAttacking = true;
        agent.isStopped = true;
        
        animator.SetFloat("Forward", 0);

        int attackCount = Random.Range(1, maxAttacks + 1);

        switch(attackCount)
        {
            case 1:
                animator.SetBool("Attack", true);
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack1");
                animator.SetBool("Attack", false);
                yield return new WaitForSeconds(slash1Time);
                slash1.GetComponent<ParticleSystem>().Play();
                break;
            case 2:
                animator.SetBool("Attack2", true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack2");
                animator.SetBool("Attack2", false);
                break;
            case 3:
                animator.SetBool("Attack3", true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack3");
                animator.SetBool("Attack3", false);
                break;
        }
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
        
        //yield return new WaitForSeconds(attackCooldown);

        isRecovering = true;
        yield return new WaitForSeconds(1f);

        isRecovering = false;
        isAttacking = false;
        hitPlayer = false;
        agent.isStopped = false;
    }




    //IEnumerator checkAttack(float attackTime,)





    //-----------------Draw Gizmos------------------------------


    // Draw Gizmo Sphere to visualize detection range
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(weapon.GetComponent<golemBossWeapon>().headPosition.transform.position + weaponOffset, weaponRad);
        Gizmos.DrawWireSphere(gameObject.transform.position + slamOffset, slamRadius);
        Gizmos.color = Color.green;
        for (int i = 0; i < attackList.Count; i++)
        {
            Vector3 vec = attackList[i];
            switch (i)
            {
                case 0:
                    //Gizmos.DrawWireSphere(gameObject.transform.position + vec + Vector3.up + gameObject.transform.forward, atkRng1);
                    Gizmos.DrawWireSphere(gameObject.transform.position + transform.rotation * (vec + Vector3.up + Vector3.forward), atkRng1);

                    break;
                case 1:
                    Gizmos.DrawWireSphere(gameObject.transform.position + transform.rotation * (vec + Vector3.up + Vector3.forward), atkRng2);
                    break;
                case 2:
                    Gizmos.DrawWireSphere(gameObject.transform.position + transform.rotation * (vec + Vector3.up + Vector3.forward) , atkRng3);
                    break;
                case 3:
                    Gizmos.DrawWireSphere(gameObject.transform.position + vec + Vector3.up, atkRng4);
                    break;
                case 4:
                    Gizmos.DrawWireSphere(gameObject.transform.position + vec + Vector3.up, atkRng5); // Use atkRng5 if available
                    break;
                default:
                    Gizmos.DrawWireSphere(gameObject.transform.position + vec + Vector3.up, atkRng4); // Fallback if index > 4
                    break;
            }
        }

    }



}

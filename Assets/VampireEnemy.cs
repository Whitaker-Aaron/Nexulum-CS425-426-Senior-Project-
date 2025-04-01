using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireEnemy : MonoBehaviour, enemyInt
{
    public Transform player;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public LayerMask Player;

    public int attackDamage = 20;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool _isAttacking;
    private bool _isActive;

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
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on Vampire!");
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= 3.0f)
        {
            animator.SetBool("Kick", true);
            animator.SetBool("Kick", false);
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
                    break;
                }
            }
        }
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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SocialPlatforms.Impl;

public class EnemySlimeCombat : MonoBehaviour, enemyInt
{
    bool canAttack = true;
    public Transform attackPoint;
    public float attackRange = .5f;
    //public bool isAttacking => isAttacking;
    public LayerMask Player;
    //public enemySword sword;
    EnemyAnimation anim;
    EnemyBehavior enemy;
    [SerializeField] GameObject smallSlimeRef;
    CharacterBase playerRef;
    //EnemyLOS los;
    //EnemyStateManager stateManager;
    public int attackDamage = 20;
    private bool _isAttacking;

    private Vector3 position = Vector3.zero;
    private Animator animator;
    private NavMeshAgent agent;

    public bool isAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)  // Only set if the value is different
            {
                _isAttacking = value;
                // Do the other necessary actions
            }
        }
    }
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

    private void OnEnable()
    {
        isAttacking = false;
        canAttack = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        attackPlayer();
        
    }

    private void FixedUpdate()
    {
        checkMovement();
    }

    public enemyInt getType()
    {
        return this;
    }

    void attackPlayer()
    {
        if (!canAttack) return;
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);
        foreach (Collider player in playerInRange)
        {
            //attack player commands
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if(player.tag == "Player" ) playerRef.takeDamage(attackDamage, knockBackDir);
            if(canAttack) StartCoroutine(attackCountdown());
            Debug.Log(player.tag);
        }

    }

    public IEnumerator attackCountdown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;


    }

    public void onDeath()
    {
        if(smallSlimeRef != null)
        {
            for(int i =0; i < 2; i++)
            {
                var smallSlime = Instantiate(smallSlimeRef);
                smallSlime.transform.position = new Vector3(this.transform.position.x + Random.Range(-0.25f, 0.25f), this.transform.position.y + 1f, this.transform.position.z + Random.Range(-0.25f, 0.25f));
                smallSlime.GetComponent<NavMeshAgent>().Warp(smallSlime.transform.position);
                smallSlime.GetComponent<EnemyLOS>().ChangeTarget(playerRef.gameObject);
                smallSlime.transform.parent = this.transform.parent;
                smallSlime.GetComponent<EnemyFrame>().initialPos = GetComponent<EnemyFrame>().initialPos;
                //smallSlime.GetComponent<EnemyFrame>().healthRef.transform.SetParent(GameObject.Find("DynamicEnemyUI").transform);
            }
            
        }
        
    }

    public float animDistance = .3f;

    void checkMovement()
    {
        float speed = agent.velocity.magnitude; // Get speed of NavMeshAgent
        bool isMoving = speed > 0.1f; // If speed is small, consider it idle

        if (isMoving != animator.GetBool("moving")) // Update only when needed
        {
            animator.SetBool("moving", isMoving);

            if (isMoving)
            {
                animator.CrossFade("forward", 0.2f); // Smooth transition
            }
            else
            {
                animator.CrossFade("idle", 0.2f);
            }
        }
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.FilePathAttribute;

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

    private void OnEnable()
    {
        isAttacking = false;
        canAttack = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        attackPlayer();
    }

    public enemyInt getType()
    {
        return this;
    }

    void attackPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach (Collider player in playerInRange)
        {
            //attack player commands
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if(player.tag == "Player") playerRef.takeDamage(attackDamage, knockBackDir);

        }
        
    }

    public void onDeath()
    {
        if(smallSlimeRef != null)
        {
            for(int i =0; i < 2; i++)
            {
                var smallSlime = Instantiate(smallSlimeRef);
                smallSlime.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
                smallSlime.GetComponent<NavMeshAgent>().Warp(smallSlime.transform.position);
                smallSlime.GetComponent<EnemyLOS>().ChangeTarget(playerRef.gameObject);
            }
            
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

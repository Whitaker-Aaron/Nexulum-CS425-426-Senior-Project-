using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyMinionCombat : MonoBehaviour, enemyInt
{
    bool canAttack = true;

    bool checkingAttack = false;
    public Transform attackPoint;
    public float attackRange = .5f;
    //public bool isAttacking => isAttacking;
    public LayerMask Player;
    public enemySword sword;
    EnemyAnimation anim;
    EnemyBehavior enemy;
    //EnemyLOS los;
    //EnemyStateManager stateManager;
    public int attackDamage = 20;
    public float attackCooldown = 2f;

    bool attackCooldownBool = false;
    [SerializeField] public bool tempEnemy = false;

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
        attackCooldownBool = false;
    }

    private void OnDisable()
    {
        if(tempEnemy) Destroy(this.gameObject);
    }

    private void Awake()
    {
        isActive = true;
    }


    public enemyInt getType()
    {
        return this;
    }

    void attackPlayer()
    {
        if(checkingAttack) return;
        StartCoroutine(attackWait());
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach(Collider player in playerInRange)
        {
            //attack player commands
            Debug.Log("Starting attack");
            canAttack = false;
            if(sword != null) sword.activateAttack(true, attackDamage, this.gameObject);
            isAttacking = true;
            anim.minionAttack();
            enemy.pauseMovement(anim.getAnimationTime());
            StartCoroutine(wait(anim.getAnimationTime(), anim));
        }
    }

    IEnumerator attackWait()
    {
        if (attackCooldownBool) yield break;
        attackCooldownBool = true;
        yield return new WaitForSeconds(attackCooldown);
        attackCooldownBool = false;
        //canAttack = true;
    }

    IEnumerator disableAttack(float time)
    {
        Debug.Log("Animation time: " + time);
        yield return new WaitForSeconds(time);
        Debug.Log("isAttacking disabled");
        isAttacking = false;
        checkingAttack = false;
        sword.activateAttack(false, attackDamage, this.gameObject);
    }

    IEnumerator wait(float time, EnemyAnimation anim)
    {
        if(checkingAttack) yield break;

        checkingAttack = true;
        yield return StartCoroutine(disableAttack(time));
        //anim.Stop();
        //yield return new WaitForSeconds(5.0f);
        Debug.Log("Able to attack again");
        canAttack = true;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<EnemyAnimation>();
        enemy = GetComponent<EnemyBehavior>();
        //los = GetComponent<EnemyLOS>();
        //stateManager = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && isActive && !attackCooldownBool) {
            attackPlayer();
        }
        else
        {
            //sword.isAttacking = false;
        }
        
    }

    public void onDeath()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

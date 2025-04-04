using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyMinionCombat : MonoBehaviour, enemyInt
{
    bool canAttack = true;
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
    }

    private void OnDisable()
    {
        if(tempEnemy) Destroy(this.gameObject);
    }


    public enemyInt getType()
    {
        return this;
    }

    void attackPlayer()
    {
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

    IEnumerator disableAttack(float time)
    {
        Debug.Log("Animation time: " + time);
        yield return new WaitForSeconds(2.5f);
        Debug.Log("isAttacking disabled");
        isAttacking = false;
        sword.activateAttack(false, attackDamage, this.gameObject);
    }

    IEnumerator wait(float time, EnemyAnimation anim)
    {
        yield return StartCoroutine(disableAttack(time));
        //anim.Stop();
        yield return new WaitForSeconds(5.0f);
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
        if (canAttack && isActive) {
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

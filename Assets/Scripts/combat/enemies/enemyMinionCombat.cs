using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyMinionCombat : MonoBehaviour
{
    bool canAttack = true;
    public Transform attackPoint;
    public float attackRange = .5f;
    public bool isAttacking = false;
    public LayerMask Player;
    public enemySword sword;
    EnemyAnimation anim;
    EnemyBehavior enemy;
    EnemyLOS los;
    public int attackDamage = 20;



    
    void attackPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach(Collider player in playerInRange)
        {
            //attack player commands
            Debug.Log("Starting attack");
            canAttack = false;
            sword.activateAttack(true, attackDamage);
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
        sword.activateAttack(false, attackDamage);
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
        los = GetComponent<EnemyLOS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && los.isTargetSpotted)
            attackPlayer();
        else
        {
            //sword.isAttacking = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

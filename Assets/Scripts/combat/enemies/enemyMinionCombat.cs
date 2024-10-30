using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyMinionCombat : MonoBehaviour
{
    bool canAttack = true;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask Player;
    public enemySword sword;
    EnemyAnimation anim;
    EnemyBehavior enemy;
    public int attackDamage = 20;



    
    void attackPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach(Collider player in playerInRange)
        {
            //attack player commands
            canAttack = false;
            sword.activateAttack(true, attackDamage);
            anim.minionAttack();
            enemy.pauseMovement(anim.getAnimationTime());
            StartCoroutine(wait(anim.getAnimationTime(), anim));
        }
    }

    IEnumerator disableAttack(float time)
    {
        yield return new WaitForSeconds(time / 1.1f);
        sword.activateAttack(false, attackDamage);
    }

    IEnumerator wait(float time, EnemyAnimation anim)
    {
        StartCoroutine(disableAttack(time));
        //anim.Stop();
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<EnemyAnimation>();
        enemy = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.FilePathAttribute;

public class EnemyHead : MonoBehaviour, enemyInt
{
    bool canAttack = true;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask Player;
    EnemyAnimation anim;
    EnemyBehavior enemy;
    CharacterBase playerRef;
    public int attackDamage = 20;
    private bool _isAttacking;
    private bool canMove = false;

    public float floatSpeed = 2f; // Speed of floating effect
    public float floatHeight = 0.5f; // Height of floating effect
    private Vector3 startPos;

    public bool isAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)
            {
                _isAttacking = value;
            }
        }
    }

    private void OnEnable()
    {
        isAttacking = false;
        canAttack = true;
    }

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        startPos = transform.position;
    }

    void Update()
    {
        attackPlayer();
        ApplyFloatingEffect();
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
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if (player.tag == "Player") playerRef.takeDamage(attackDamage, knockBackDir);
            Debug.Log(player.tag);
        }
    }

    public void onDeath()
    {
        // Unkillable enemy
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void ApplyFloatingEffect()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour, enemyInt
{
    bool canAttack = true;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask Player;
    CharacterBase playerRef;
    public int attackDamage = 20;
    private bool _isAttacking;
    private bool canMove = true; // Enemy stops moving when looked at

    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    private Vector3 startPos;

    public Transform playerVision; // This is the GameObject that rotates with the player
    public float visionAngle = 60f; // How wide the player can "see"

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

        if (playerRef != null)
        {
            playerVision = GameObject.Find("PlayerVision").transform; // Assign manually if needed
        }
    }

    void Update()
    {
        CheckIfPlayerIsLooking();

        if (canMove)
        {
            ApplyFloatingEffect();
        }
    }

    void CheckIfPlayerIsLooking()
    {
        if (playerVision == null) return;

        Vector3 directionToEnemy = (transform.position - playerVision.position).normalized;
        float dotProduct = Vector3.Dot(playerVision.forward, directionToEnemy);

        bool isInVision = dotProduct > Mathf.Cos(visionAngle * Mathf.Deg2Rad / 2);

        if (isInVision)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerVision.position, directionToEnemy, out hit))
            {
                if (hit.collider.gameObject == gameObject) // If the enemy is in direct sight
                {
                    canMove = false;
                    return;
                }
            }
        }

        canMove = true; // Resume movement if not looked at
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void ApplyFloatingEffect()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

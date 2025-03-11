using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyBat : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    public LayerMask Player;

    public int attackDamage = 20;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool _isAttacking;

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

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyBat!");
        }

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    void Update()
    {
        if (player != null)
        {
            attackPlayer();
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
                    break; // Prevent triggering multiple cooldowns per frame
                }
            }
        }
    }
}

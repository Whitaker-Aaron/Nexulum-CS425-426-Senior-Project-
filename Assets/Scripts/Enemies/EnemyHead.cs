using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyHead : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    public LayerMask Player;
    private Vector3 startPos;
    private float lastYPosition; // Store last valid floating position

    public int attackDamage = 20;
    public float visionDistance = 10f;
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    public float attackRange = .5f;
    public float attackCooldownTime = 2f;
    private float timeOffset;

    public bool canAttack = true;
    private bool canMove = true;
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
            Debug.LogError("EnemyStateManager not found on EnemyHead!");
        }

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();

        startPos = transform.position;
        lastYPosition = startPos.y;
        StartCoroutine(StartFloatingAfterDelay(0.5f));
    }

    IEnumerator StartFloatingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        timeOffset = Time.time;
    }

    void Update()
    {
        if (player != null)
        {
            CheckIfPlayerIsLooking();
            attackPlayer();
        }

        if (estate != null)
        {
            estate.movementPaused = !canMove;
        }

        if (canMove)
        {
            ApplyFloatingEffect();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, lastYPosition, transform.position.z);
        }
    }

    void CheckIfPlayerIsLooking()
    {
        RaycastHit hit;
        Vector3 origin = masterInput.instance.bulletSpawn.position;
        Vector3 direction = masterInput.instance.bulletSpawn.forward;

        if (Physics.Raycast(origin, direction, out hit, visionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                canMove = false;
                Debug.Log("Player is looking at the enemy! Freezing movement.");
                return;
            }
        }
        canMove = true;
    }

    void ApplyFloatingEffect()
    {
        if (timeOffset == 0) return;

        float elapsedTime = (Time.time - timeOffset);
        float newY = startPos.y + Mathf.Sin(elapsedTime * floatSpeed) * floatHeight;
        lastYPosition = newY;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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

        foreach (Collider player in playerInRange)
        {
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if (player.tag == "Player")
            {
                playerRef.takeDamage(attackDamage, knockBackDir);
                StartCoroutine(attackCooldown());
            }
            Debug.Log(player.tag);
        }
    }
}

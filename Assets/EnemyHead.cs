using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyHead : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;

    public float visionDistance = 10f; // How far the player can "look"
    private bool canMove = true;
    private bool _isAttacking;
    public LayerMask Player;
    public float attackRange = .5f;
    private Vector3 startPos;
    private float lastYPosition; // Store last valid floating position
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    private float timeOffset;

    void Start()
    {
        // Automatically find the Player if not set in Inspector
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
        }

        estate = GetComponent<EnemyStateManager>();
        if (estate == null)
        {
            Debug.LogError("EnemyStateManager not found on EnemyHead!");
        }

        startPos = transform.position;
        lastYPosition = startPos.y; // Initialize lastYPosition
        StartCoroutine(StartFloatingAfterDelay(0.5f)); // Delay floating for a smoother start
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

        // Ensure estate is valid before accessing it
        if (estate != null)
        {
            estate.movementPaused = !canMove;
        }

        // Only apply floating effect if movement is allowed
        if (canMove)
        {
            ApplyFloatingEffect();
        }
        else
        {
            // Maintain last valid Y position to prevent spazzing
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

        // Only reset movement if the player is no longer looking
        canMove = true;
    }

    void ApplyFloatingEffect()
    {
        if (timeOffset == 0) return; // Prevent floating before the delay is over

        float elapsedTime = (Time.time - timeOffset);
        float newY = startPos.y + Mathf.Sin(elapsedTime * floatSpeed) * floatHeight;
        lastYPosition = newY; // Store last valid position before freezing
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void onDeath()
    {
        // Unkillable enemy
    }

    public enemyInt getType()
    {
        return this;
    }

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

    void attackPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(attackPoint.position, attackRange, Player);

        foreach (Collider player in playerInRange)
        {
            //attack player commands
            Vector3 knockBackDir = playerRef.transform.position - gameObject.transform.position;
            if (player.tag == "Player") playerRef.takeDamage(attackDamage, knockBackDir);
            Debug.Log(player.tag);

        }

    }
}
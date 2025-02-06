using UnityEngine;

public class EnemyHead : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    public float visionDistance = 10f; // How far the player can "look"
    private bool canMove = true;
    private bool _isAttacking;

    private Vector3 startPos;
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;

    void Start()
    {
        // Automatically find the Player if not set in Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            }
        }

        startPos = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            CheckIfPlayerIsLooking();
        }

        if (canMove)
        {
            ApplyFloatingEffect();
        }
    }

    void CheckIfPlayerIsLooking()
    {
        RaycastHit hit;

        // Cast a Ray from the Player in their forward direction
        if (Physics.Raycast(player.position, player.forward, out hit, visionDistance))
        {
            // If the Ray hits THIS enemy, freeze movement
            if (hit.collider.gameObject == gameObject)
            {
                canMove = false;
                Debug.Log("Player is looking at the enemy! Freezing movement.");
                return;
            }
        }

        // If not hit, allow movement again
        canMove = true;
    }

    void ApplyFloatingEffect()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
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

}

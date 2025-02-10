using Unity.VisualScripting;
using UnityEngine;

public class GraveDigger : MonoBehaviour, enemyInt
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

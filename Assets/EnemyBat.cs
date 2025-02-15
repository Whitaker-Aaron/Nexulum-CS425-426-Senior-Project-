using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyBat : MonoBehaviour, enemyInt
{
    public Transform player; // Reference to the player's transform
    private EnemyStateManager estate;
    private GameObject playerObj;
    public Transform attackPoint;
    CharacterBase playerRef;

    private Animator animator;  // Reference to Animator component

    private bool _isAttacking;
    public LayerMask Player;
    public float attackRange = .5f;
    public int attackDamage = 10;

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

    private void OnEnable()
    {
        isAttacking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        animator = GetComponent<Animator>(); // Get the Animator component

        if (animator != null)
        {
            animator.Play("YourAnimationName"); // Replace with your actual animation state name
        }
        else
        {
            Debug.LogWarning("Animator not found on EnemyBat!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackPlayer();
        if (animator != null)
        {
            animator.Play("YourAnimationName"); // Ensure animation keeps playing
        }
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

    }
}

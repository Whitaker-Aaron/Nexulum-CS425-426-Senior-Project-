using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class tankMovement : MonoBehaviour
{


    /*
    private GameObject player;
    public float followRad = 4f;
    public float rotationSpeed = 5f;
    public float stopDistance = 1f;
    public float minDistance = 2f;

    private NavMeshAgent agent;
    public Vector3 offset;
    private float originalSpeed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            originalSpeed = agent.speed;
        }
    }

    void Update()
    {
        if (agent == null || player == null) return;

        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= minDistance)
        {
            agent.isStopped = true;  // Stop if too close to the player
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);

            float slowDownFactor = Mathf.Clamp(distanceToPlayer / followRad, 0.5f, 1f);
            agent.speed = originalSpeed * slowDownFactor;
        }

        RotateTankBody();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void RotateTankBody()
    {
        Vector3 direction = agent.velocity.normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RespawnOnPlayer();
    }

    private void RespawnOnPlayer()
    {
        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset;
        agent.Warp(targetPosition);
    }
    */

    private GameObject player;
    public float followRad = 4f;    // Radius to follow player
    public float rotationSpeed = 5f; // Speed of rotation
    public float stopDistance = 1f;  // Distance to stop from player
    public float minDistance = 2f;   // Minimum distance before tank stops moving

    private NavMeshAgent agent;
    public Vector3 offset;           // Offset from player position
    private float originalSpeed;      // Original speed of the agent

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep the tank across scenes
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component

        if (agent != null)
        {
            originalSpeed = agent.speed; // Store the original speed
        }
    }

    void Update()
    {
        if (agent == null || player == null) return; // Ensure agent and player are valid

        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset; // Calculate target position
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); // Distance to player

        if (distanceToPlayer <= minDistance)
        {
            agent.isStopped = true; // Stop if too close to the player
        }
        else
        {
            agent.isStopped = false; // Enable movement
            agent.SetDestination(targetPosition); // Set destination to target position

            // Slow down the tank as it approaches the follow radius
            float slowDownFactor = Mathf.Clamp(distanceToPlayer / followRad, 0.5f, 1f);
            agent.speed = originalSpeed * slowDownFactor; // Adjust speed based on distance
        }

        RotateTankBody(); // Handle tank rotation
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loading event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from scene loading event
    }

    private void RotateTankBody()
    {
        Vector3 direction = agent.velocity.normalized; // Get the direction of movement
        if (direction != Vector3.zero) // Check if there is movement
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Calculate target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Smoothly rotate towards the movement direction
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RespawnOnPlayer(); // Position the tank on scene load
    }

    private void RespawnOnPlayer()
    {
        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset; // Calculate the new position
        agent.Warp(targetPosition); // Instantaneously move the tank to the new position
    }
}

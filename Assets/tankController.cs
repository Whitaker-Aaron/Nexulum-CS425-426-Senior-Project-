using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class tankController : MonoBehaviour
{

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
        //gameObject.transform.parent = player.transform;
        agent = gameObject.GetComponent<NavMeshAgent>();

        //offset = new Vector3(0, 0, -followRad);
        if (agent != null)
        {
            originalSpeed = agent.speed;  // Store the original speed
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null || player == null) return;

        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= minDistance)
        {
            agent.isStopped = true;  // Stop the tank if too close
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);

            // Adjust speed without permanently modifying it
            float slowDownFactor = Mathf.Clamp(distanceToPlayer / followRad, 0.5f, 1f);
            agent.speed = originalSpeed * slowDownFactor;  // Adjust speed dynamically
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
        // Get the direction the agent is moving in
        Vector3 direction = agent.velocity.normalized;
        if (direction != Vector3.zero)
        {
            // Calculate the desired rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the tank body towards the movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            Vector3 targetPosition = player.transform.position + player.transform.rotation * offset;
            agent.SetDestination(targetPosition);

            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);
        }
    }

    // This method is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        // Reposition the AI tank based on the player's new position and offset
        RespawnOnPlayer();
    }

    private void RespawnOnPlayer()
    {
        // Calculate the new position for the AI tank based on the player's position and offset
        Vector3 targetPosition = player.transform.position + player.transform.rotation * offset;

        // Move the tank directly to the new position
        agent.Warp(targetPosition);  // Use Warp to avoid pathfinding and instantly move the AI

        Debug.Log("AI Tank repositioned to offset from player.");
    }

}

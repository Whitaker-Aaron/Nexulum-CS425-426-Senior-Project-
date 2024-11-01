// State manager for enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    // ----------------------------------------------
    // Adjustable in-editor settings for behaviors
    // ----------------------------------------------

    public float movementSpeed = 2; // Movement speed of enemy
    public float engagementRange = 1; // How close, from target, the enemy will get to the target (radius)

    // ----------------------------------------------
    // Components
    // ----------------------------------------------

    public NavMeshAgent agent;

    public EnemyLOS enemyLOS;

    // ----------------------------------------------
    // State objects and state-related variables
    // ----------------------------------------------

    // Current state
    private EnemyState currentState;

    // Concrete states
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemySearchState searchState = new EnemySearchState();

    // Toggleable bool in editor to enable debug logging for state switching
    public bool enableStateDebugLogs = false;

    // ----------------------------------------------
    // Methods
    // ----------------------------------------------

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLOS = GetComponent<EnemyLOS>();

        agent.speed = movementSpeed;
        //agent.stoppingDistance = engagementRange;

        ChangeState(idleState);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.RunState(this);
        }
        else
        {
            CustomDebugLog("currentState is null");
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public void CustomDebugLog(string log)
    {
        if (enableStateDebugLogs == true)
        {
            Debug.Log(log);
        }
    }

    // Move to a given Vector3 position. Bools for enabling movement with pathfinding (NavMesh) and prediction
    // Pathfinding-less and predicted movement will be supported at a later date, for now this function can only cover movement with pathfinding
    public void MoveTo(Vector3 position, bool enablePathfinding = false, bool enablePrediction = false)
    {
        if (enablePathfinding)
        {
            agent.SetDestination(position);
        }
        else
        {
            CustomDebugLog("Movement without pathfinding not supported yet--please toggle 'enablePathfinding' to true");
        }
    }

    // Overloaded MoveTo, allows stoppingDist
    public void MoveTo(Vector3 position, float stoppingDist, bool enablePathfinding = false, bool enablePrediction = false)
    {
        float distanceToPos = Vector3.Distance(enemyLOS.selfPos, position);

        if (enablePathfinding)
        {
            if (distanceToPos < engagementRange) // Enemy is too close
            {
                Vector3 awayDirection = (enemyLOS.selfPos - position).normalized; // Get direction away from player
                Vector3 awayPos = (enemyLOS.selfPos + awayDirection);
                agent.SetDestination(awayPos);
            }
            else // Enemy is too far away
            {
                agent.SetDestination(position);
            }
        }
        else
        {
            CustomDebugLog("Movement without pathfinding not supported yet--please toggle 'enablePathfinding' to true");
        }
    }
}
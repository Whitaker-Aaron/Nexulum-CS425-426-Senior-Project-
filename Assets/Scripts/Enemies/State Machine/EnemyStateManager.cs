// State manager for enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour, IStateMachine
{
    // ----------------------------------------------
    // Adjustable in-editor settings for behaviors
    // ----------------------------------------------

    public float defaultMovementSpeed = 2f; // Movement speed of enemy
    public float engagementRange = 1f; // How close, from target, the enemy will get to the target (radius). Set with SetEngagementRange(float range)

    // Debugging
    public float currentSpeed;

    // Movement pausing
    public bool movementPaused = false;
    // public float waitTime = 0;

    // ----------------------------------------------
    // Components
    // ----------------------------------------------

    public NavMeshAgent agent;
    public EnemyLOS enemyLOS;
    public EnemyFrame enemyFrame;

    // ----------------------------------------------
    // State objects and state-related variables
    // ----------------------------------------------

    // Current state
    private EnemyState currentState;

    // Concrete states
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemySearchState searchState = new EnemySearchState();

    public bool enableStateDebugLogs = false;

    // ----------------------------------------------
    // Effects
    // ----------------------------------------------

    public bool isFrozen = false;

    // ----------------------------------------------
    // Methods
    // ----------------------------------------------

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLOS = GetComponent<EnemyLOS>();
        enemyFrame = GetComponent<EnemyFrame>();

        if (enemyLOS == null)
        {
            Debug.LogError("enemyLOS is null");
        }

        currentSpeed = defaultMovementSpeed;

        ChangeState(idleState);
    }

    public void Update()
    {
        agent.speed = currentSpeed;

        if (currentState != null)
        {
            currentState.RunState();
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
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public void CustomDebugLog(string log)
    {
        if (enableStateDebugLogs == true)
        {
            Debug.Log("SM Debug: " + log);
        }
    }

    // Move to a given Vector3 position. Bools for enabling movement with pathfinding (NavMesh) and prediction
    // Pathfinding-less and predicted movement will be supported at a later date, for now this function can only cover movement with pathfinding
    public void MoveTo(Vector3 position, bool enablePrediction = false)
    {
        Vector3 directionToPos = (position - enemyLOS.selfPos).normalized;
        agent.SetDestination(position);
    }

    // Overloaded MoveTo, enforces engagement range from point
    public void MoveTo(Vector3 position, float stoppingDist, bool enablePrediction = false)
    {
        float distanceToPos = Vector3.Distance(enemyLOS.selfPos, position);
        Vector3 directionToPos = (position - enemyLOS.selfPos).normalized;

        if (distanceToPos < engagementRange) // Enemy is too close
        {
            Vector3 awayDirection = (enemyLOS.selfPos - position).normalized; // Get direction away from player
            Vector3 awayPos = (enemyLOS.selfPos + awayDirection); // Get the position, away from the player, to go to
            agent.SetDestination(awayPos);
        }
    }

    public void SetEngagementRange(float range)
    {
        engagementRange = range;
    }

    public void LookAt(GameObject lookat)
    {
        float rotationSpeed = 2f;
        Vector3 headingtolookat = lookat.transform.position - transform.position;

        var rotationtolookat = Quaternion.LookRotation(headingtolookat);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationtolookat, rotationSpeed * Time.deltaTime);
    }

    public string GetCurrentTargetTag()
    {
        return enemyLOS.currentTarget.tag;
    }

    public string TargetSpotted()
    {
        return enemyLOS.TargetSpotted();
    }

    public void ResetEnemyState()
    {
        ChangeState(idleState);
    }

    public string GetCurrentStateName() // Returns name (string) of current state
    {
        return currentState.stateName;
    }

    public EnemyState GetCurrentState() // Returns the state object of the current state
    {
        return currentState;
    }

    public void PauseMovementFor(float seconds)
    {
        StartCoroutine(StopFor(seconds));
    }

    private IEnumerator StopFor(float s)
    {
        if (!movementPaused)
        {
            movementPaused = true;
            yield return new WaitForSeconds(s);
            movementPaused = false;
        }
    }
}
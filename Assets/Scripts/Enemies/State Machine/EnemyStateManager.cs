// State manager for enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyMovementAgent enemyAgent;

    // Current state - actively running state
    private EnemyState currentState;

    // Initial state
    public EnemyState initialState;

    // Concrete states
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemySearchState searchState = new EnemySearchState();

    // Editable bool in editor to enable debug logging for state switching
    public bool enableStateDebugLogs = false;

    public void Start()
    {
        enemyAgent = GetComponent<EnemyMovementAgent>();

        ChangeInitialState(idleState);

        ChangeState(initialState);
    }

    public void Update()
    {
        // Runs the RunState() function of the current state
        currentState.RunState(this);
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

    public void ChangeInitialState(EnemyState newInitial)
    {
        initialState = newInitial;
    }

    public void CustomDebugLog(string log)
    {
        if (enableStateDebugLogs == true)
        {
            Debug.Log(log);
        }
    }
}
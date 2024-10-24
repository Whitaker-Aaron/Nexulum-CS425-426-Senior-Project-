// State manager for enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // Reference to EnemyBehavior so individual states can retrieve and send information back to EnemyBehavior
    // No need to see this in inspector
    [HideInInspector] public EnemyBehavior enemyBehaviorRef;

    // Current state - actively running state
    private EnemyState currentState;

    // Initial state - may not be actively running
    public EnemyState initialState;

    // Concrete states
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemySearchState searchState = new EnemySearchState();

    // Editable bool in editor to enable debug logging for state switching
    public bool enableStateDebugLogs = false;

    // Delay used to slow down state updates, since they don't necessarily need to occur every frame (for performance)
    public IEnumerator updateDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Start()
    {
        // Grab reference to EnemyBehavior
        enemyBehaviorRef = GetComponent<EnemyBehavior>();

        ChangeInitialState(idleState);

        ChangeState(initialState);
    }

    public void Update()
    {
        updateDelay(0.2f);

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
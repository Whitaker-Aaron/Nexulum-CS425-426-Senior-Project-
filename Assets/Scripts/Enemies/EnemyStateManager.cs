// State manager for enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{

    // Reference to EnemyBehavior so individual states have access to behavior variables/functions
    [HideInInspector]
    public EnemyBehavior enemyBehaviorRef;

    // Holder of the current state
    private EnemyState currentState;

    // Default state
    public EnemyState defaultState; // Can be idle, patrol, etc - set in inspector

    // Holders for specific states (idle, chase) - set in inspector
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;

    // Delay used to slow down state updates, since they don't necessarily need to occur every frame (for performance)
    public IEnumerator updateDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Start()
    {
        // Grab reference to EnemyBehavior
        enemyBehaviorRef = GetComponent<EnemyBehavior>();

        idleState = GetComponent<EnemyIdleState>();
        chaseState = GetComponent<EnemyChaseState>();

        ChangeDefaultState(idleState);

        ChangeState(defaultState);

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

    public void ChangeDefaultState(EnemyState newDefault)
    {
        defaultState = newDefault;
    }
}
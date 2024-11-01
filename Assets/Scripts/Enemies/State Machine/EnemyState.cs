using UnityEngine;

public abstract class EnemyState
{
    // Reference the state manager is needed so the state can switch to another state within itself
    public abstract void EnterState(EnemyStateManager stateContext);
    public abstract void RunState(EnemyStateManager stateContext);
    public abstract void ExitState(EnemyStateManager stateContext);
}
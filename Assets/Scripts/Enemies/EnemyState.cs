using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    // Reference the state manager so each state has context as to what is happening - Aisling
    public abstract void EnterState(EnemyStateManager stateContext);
    public abstract void RunState(EnemyStateManager stateContext);
    public abstract void ExitState(EnemyStateManager stateContext);
}
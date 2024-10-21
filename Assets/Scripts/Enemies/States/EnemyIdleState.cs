using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext) {
        Debug.Log("Entered idle state");
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyBehaviorRef.TargetSpotted())
        {
            // Changes to chase state if the target is spotted - Aisling
            stateContext.ChangeState(stateContext.chaseState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        Debug.Log("Exited idle state");
    }
}
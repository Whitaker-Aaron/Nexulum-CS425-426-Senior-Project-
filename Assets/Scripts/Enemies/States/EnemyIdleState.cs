using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext) {
        stateContext.CustomDebugLog("Entered idle state");
        // Enemy should be stationary while idling
        stateContext.enemyBehaviorRef.agent.isStopped = true;
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyBehaviorRef.TargetSpotted())
        {
            // Changes to chase state if the target is spotted - Aisling
            stateContext.enemyBehaviorRef.isTargetSpotted = true;
            stateContext.ChangeState(stateContext.chaseState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        stateContext.CustomDebugLog("Exited idle state");
    }
}
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext) {
        stateContext.CustomDebugLog("Entered idle state");
        // Enemy should be stationary while idling
        stateContext.agent.isStopped = true;
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyLOS.TargetSpotted())
        {
            // Changes to chase state if the target is spotted - Aisling
            stateContext.ChangeState(stateContext.chaseState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        stateContext.CustomDebugLog("Exited idle state");
    }
}
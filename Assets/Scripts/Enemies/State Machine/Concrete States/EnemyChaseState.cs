using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        stateContext.CustomDebugLog("Entered chase state");
        // Enemy is going to be moving in order to chase the target
        stateContext.agent.isStopped = false;
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyLOS.TargetSpotted())
        {
            // Chases the target as long as they are spotted
            stateContext.MoveTo(stateContext.enemyLOS.targetPos, stateContext.engagementRange, true, false);
        }
        else
        {
            stateContext.ChangeState(stateContext.searchState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        stateContext.enemyLOS.lastKnownTargetPos = stateContext.enemyLOS.targetPos;
        stateContext.CustomDebugLog("Exited chase state with last known target position at " + stateContext.enemyLOS.lastKnownTargetPos);
    }
}
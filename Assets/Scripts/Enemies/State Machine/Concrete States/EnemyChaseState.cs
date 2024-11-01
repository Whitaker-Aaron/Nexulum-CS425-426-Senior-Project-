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
        if (stateContext.enemyAgent.enemyLOS.TargetSpotted())
        {
            // Chases the target as long as they are spotted
            stateContext.enemyAgent.SetDestination(stateContext.enemyAgent.enemyLOS.TargetPos);
        }
        else
        {
            stateContext.ChangeState(stateContext.searchState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        stateContext.enemyAgent.enemyLOS.isTargetSpotted = false;
        stateContext.lastKnownTargetPos = stateContext.enemyAgent.enemyLOS.TargetPos;
        stateContext.CustomDebugLog("Exited chase state with last known target position at " + stateContext.enemyAgent.enemyLOS.lastKnownTargetPos);
    }
}
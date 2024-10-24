using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        stateContext.CustomDebugLog("Entered chase state");
        // Enemy is going to be moving in order to chase the target
        stateContext.enemyBehaviorRef.agent.isStopped = false;
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyBehaviorRef.TargetSpotted())
        {
            // Chases the target as long as they are spotted
            stateContext.enemyBehaviorRef.agent.SetDestination(stateContext.enemyBehaviorRef.target.transform.position);
        }
        else
        {
            stateContext.ChangeState(stateContext.searchState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        stateContext.enemyBehaviorRef.isTargetSpotted = false;
        stateContext.enemyBehaviorRef.lastKnownTargetPos = stateContext.enemyBehaviorRef.target.transform.position;
        stateContext.CustomDebugLog("Exited chase state with last known target position at " + stateContext.enemyBehaviorRef.lastKnownTargetPos);
    }
}
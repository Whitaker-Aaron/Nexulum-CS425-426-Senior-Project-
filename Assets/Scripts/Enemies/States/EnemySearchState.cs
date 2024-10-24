using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchState : EnemyState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        stateContext.CustomDebugLog("Entered search state");
    }
    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyBehaviorRef.TargetSpotted())
        {
            stateContext.ChangeState(stateContext.chaseState);
        }
        else
        {
            stateContext.enemyBehaviorRef.agent.SetDestination(stateContext.enemyBehaviorRef.lastKnownTargetPos);
            if (stateContext.enemyBehaviorRef.transform.position == stateContext.enemyBehaviorRef.lastKnownTargetPos)
            {
                stateContext.updateDelay(stateContext.enemyBehaviorRef.timeToWaitInSearch);
                stateContext.ChangeState(stateContext.idleState);
            }
        }
    }
    public override void ExitState(EnemyStateManager stateContext)
    {
        stateContext.CustomDebugLog("Exited search state");
    }
}

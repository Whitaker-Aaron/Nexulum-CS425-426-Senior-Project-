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
        if (stateContext.enemyAgent.enemyLOS.TargetSpotted())
        {
            stateContext.ChangeState(stateContext.chaseState);
        }
        else
        {
            stateContext.enemyAgent.agent.SetDestination(stateContext.enemyAgent.enemyLOS.lastKnownTargetPos);
            if (stateContext.enemyAgent.transform.position == stateContext.enemyAgent.enemyLOS.lastKnownTargetPos)
            {
                stateContext.ChangeState(stateContext.idleState);
            }
        }
    }
    public override void ExitState(EnemyStateManager stateContext)
    {
        stateContext.CustomDebugLog("Exited search state");
    }
}

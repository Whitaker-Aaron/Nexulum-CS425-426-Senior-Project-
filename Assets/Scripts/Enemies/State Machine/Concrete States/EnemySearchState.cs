using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Search";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        // Enemy should be able to move if searching, unless forcibly stopped
        stateContext.agent.isStopped = stateContext.movementPaused;

        // Get players current position (called only once here in Start()) amd set to the last known target position
        UpdateLastKnownTargetPosition();

        // Move to the last known target position
        stateContext.MoveTo(stateContext.enemyLOS.lastKnownTargetPos, false);
        stateContext.CustomDebugLog("Moving to " + stateContext.enemyLOS.lastKnownTargetPos + " from " + stateContext.enemyLOS.selfPos);
    }

    public override void RunState()
    {
        stateContext.agent.isStopped = stateContext.movementPaused;
        
        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag())
        {
            stateContext.ChangeState("Chase");
        }
        else
        {
            if (stateContext.EnemyIsAtPosition(stateContext.enemyLOS.lastKnownTargetPos))
            {
                stateContext.CustomDebugLog("Arrived at last known player position, switching to idle.");
                stateContext.ChangeState("Idle");
            }
        }
    }

    public override void ExitState()
    {
        stateContext.CustomDebugLog("Exited " + stateName + " state");
    }

    private void UpdateLastKnownTargetPosition() // Updates last known target position in enemyLOS
    {
        if (stateContext.enemyLOS.GetCurrentTargetObject() != null)
        {
            Vector3 ESS_targetPosition = stateContext.enemyLOS.GetCurrentTargetPosition();
            stateContext.enemyLOS.SetLastKnownTargetPosition(ESS_targetPosition);
        }
        else
        {
            stateContext.CustomDebugLogError("EnemySearchState.cs UpdateLastKnownTargetPosition() - Current target was found null.");
        }
    }
}

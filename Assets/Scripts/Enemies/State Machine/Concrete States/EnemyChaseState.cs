public class EnemyChaseState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Chase";

        stateContext.CustomDebugLog("Entered " + stateName + " state");
        stateContext.MoveTo(stateContext.enemyLOS.targetPos, stateContext.engagementRange, false);

        stateContext.agent.isStopped = false;
    }

    public override void RunState()
    {
        
        if (false) {
            
        }
        else
        {
            stateContext.ChangeState(stateContext.searchState);
        }
    }

    public override void ExitState() {
        stateContext.enemyLOS.lastKnownTargetPos = stateContext.enemyLOS.targetPos;
        stateContext.CustomDebugLog("Exited " + stateName + " state with last known target position at " + stateContext.enemyLOS.lastKnownTargetPos);
    }
}
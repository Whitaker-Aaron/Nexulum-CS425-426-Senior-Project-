public class EnemyChaseState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Chase";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        stateContext.agent.isStopped = false;
    }

    public override void RunState()
    {
        

        stateContext.MoveTo(stateContext.enemyLOS.targetPos, stateContext.engagementRange, true, false);

        if (!stateContext.enemyLOS.TargetSpotted()) {
            stateContext.ChangeState(stateContext.searchState);
        }
    }

    public override void ExitState() {
        stateContext.enemyLOS.lastKnownTargetPos = stateContext.enemyLOS.targetPos;
        stateContext.CustomDebugLog("Exited " + stateName + " state with last known target position at " + stateContext.enemyLOS.lastKnownTargetPos);
    }
}
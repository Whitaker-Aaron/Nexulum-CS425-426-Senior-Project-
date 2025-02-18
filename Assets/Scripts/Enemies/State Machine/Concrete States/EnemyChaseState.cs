
public class EnemyChaseState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Chase";

        stateContext.CustomDebugLog("Entered " + stateName + " state");
        stateContext.MoveTo(stateContext.enemyLOS.targetPos, stateContext.engagementRange, false);

        // Enemy should be able to move if chasing, unless forcibly stopped
        stateContext.agent.isStopped = stateContext.movementPaused;
    }

    public override void RunState()
    {
        stateContext.agent.isStopped = stateContext.movementPaused;

        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag()) {
            stateContext.MoveTo(stateContext.enemyLOS.targetPos, stateContext.engagementRange, false);
        }
        else
        {
            stateContext.ChangeState(stateContext.GetStateOfName("Search"));
        }
    }

    public override void ExitState() {
        stateContext.enemyLOS.lastKnownTargetPos = stateContext.enemyLOS.targetPos;
        stateContext.CustomDebugLog("Exited " + stateName + " state with last known target position at " + stateContext.enemyLOS.lastKnownTargetPos);
    }
}
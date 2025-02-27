public class EnemySearchState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Search";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        // Enemy should be able to move if searching, unless forcibly stopped
        // stateContext.agent.isStopped = stateContext.movementPaused;

        stateContext.MoveTo(stateContext.enemyLOS.lastKnownTargetPos, false);
    }

    public override void RunState()
    {
        // stateContext.agent.isStopped = stateContext.movementPaused;
        
        base.OnDamaged();
        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag())
        {
            stateContext.ChangeState(stateContext.chaseState);
        }
        else
        {
            if (stateContext.transform.position == stateContext.enemyLOS.lastKnownTargetPos)
            {
                stateContext.ChangeState(stateContext.idleState);
            }
        }
    }

    public override void ExitState()
    {
        stateContext.CustomDebugLog("Exited " + stateName + " state");
    }
}

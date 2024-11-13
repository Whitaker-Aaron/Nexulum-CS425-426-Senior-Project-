public class EnemySearchState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Search";

        stateContext.CustomDebugLog("Entered " + stateName + " state");
    }

    public override void RunState()
    {
        stateContext.MoveTo(stateContext.enemyLOS.lastKnownTargetPos, false);

        if (stateContext.enemyLOS.TargetSpotted() || base.OnDamaged())
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

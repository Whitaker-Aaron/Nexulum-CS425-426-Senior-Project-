public class EnemyIdleState : EnemyNeutralState
{
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Idle";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        // Enemy should be stationary while idling
        stateContext.agent.isStopped = true;
    }

    public override void RunState()
    {
        base.OnDamaged();
        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag())
        {
            // Changes to chase state if the target is spotted - Aisling
            stateContext.ChangeState(stateContext.chaseState);
        }
    }

    public override void ExitState()
    {
        stateContext.CustomDebugLog("Exited " + stateName + " state");
    }
}
using UnityEngine;

public class EnemyChaseState : EnemyState
{

    public override void EnterState(EnemyStateManager stateContext)
    {
        Debug.Log("Entered chase state");
    }

    public override void RunState(EnemyStateManager stateContext)
    {
        if (stateContext.enemyBehaviorRef.TargetSpotted())
        {
            // Chases the target as long as they are spotted
            stateContext.enemyBehaviorRef.agent.isStopped = false;
            stateContext.enemyBehaviorRef.agent.SetDestination(stateContext.enemyBehaviorRef.target.transform.position);
        }
        else
        {
            stateContext.ChangeState(stateContext.defaultState);
        }
    }

    public override void ExitState(EnemyStateManager stateContext) {
        Debug.Log("Exited chase state");
    }
}
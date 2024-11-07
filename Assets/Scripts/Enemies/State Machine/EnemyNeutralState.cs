// Super state for more common functionalities of child states - Aisling

public abstract class EnemyNeutralState : EnemyState
{
    protected virtual void OnDamaged()
    {
        if (stateContext.enemyFrame.onDamaged)
        {
            stateContext.enemyFrame.onDamaged = false; // Reset onDamaged

            stateContext.CustomDebugLog("Enemy damaged by " + stateContext.enemyFrame.source);
            switch (stateContext.enemyFrame.source)
            {
                case EnemyFrame.DamageSource.Player:
                    // Target player
                    stateContext.enemyLOS.ChangeTarget(stateContext.enemyLOS.player);

                    // Look at, change state
                    stateContext.LookAt(stateContext.enemyLOS.currentTarget);
                    stateContext.ChangeState(stateContext.chaseState);
                    break;
            }
        }
    }

    protected virtual void OnFrozen()
    {
        //
    }

    protected virtual void OnParalyzed()
    {
        //
    }
}
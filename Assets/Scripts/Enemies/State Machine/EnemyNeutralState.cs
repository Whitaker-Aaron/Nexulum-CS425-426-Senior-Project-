// Super state for more common functionalities of basic states - Aisling

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
                    stateContext.ChangeState(stateContext.chaseState);
                    break;
            }
        }
    }
}
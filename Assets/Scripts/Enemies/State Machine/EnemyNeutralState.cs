// Super state for more common functionalities of child states - Aisling

public abstract class EnemyNeutralState : EnemyState
{
    protected virtual bool OnDamaged()
    {
        bool damagedActionReady = false;

        if (stateContext.enemyFrame.onDamaged)
        {
            stateContext.enemyFrame.onDamaged = false; // Reset onDamaged

            stateContext.CustomDebugLog("Enemy damaged by " + stateContext.enemyFrame.source);
            switch (stateContext.enemyFrame.source)
            {
                case EnemyFrame.DamageSource.Player:
                    // Target player
                    stateContext.enemyLOS.ChangeTarget(stateContext.enemyLOS.player);
                    stateContext.enemyLOS.isTargetSpotted = true;

                    damagedActionReady = true;
                    break;
            }
        }

        return damagedActionReady;
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
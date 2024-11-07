// Super class for more common functionalities of certain states - Aisling

public abstract class EnemyNeutralState : EnemyState
{
    protected virtual void OnDamaged()
    {
        if (stateContext.enemyFrame.onDamaged)
        {
            // Chase target (typically player) on damage taken
            // Future update: get what hit the enemy
            stateContext.enemyFrame.onDamaged = false; // Reset onDamaged
            stateContext.LookAt(stateContext.enemyLOS.currentTarget);
            stateContext.ChangeState(stateContext.chaseState);
        }
    }
}
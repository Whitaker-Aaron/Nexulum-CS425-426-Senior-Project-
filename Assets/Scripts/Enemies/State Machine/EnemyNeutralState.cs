// Super state for more common functionalities of basic states - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyNeutralState : EnemyState
{
    protected virtual void OnDamaged()
    {
        if (stateContext.enemyFrame.onDamaged)
        {
            Debug.Log("EnemyNeutralState.cs - Enemy damaged by " + stateContext.enemyFrame.source);
            switch (stateContext.enemyFrame.source)
            {
                case EnemyFrame.DamageSource.Player:
                    // Target player
                    GameObject player = GameObject.FindWithTag("Player");
                    stateContext.enemyLOS.ChangeTarget(player);
                    stateContext.enemyLOS.SetLastKnownTargetPosition(stateContext.enemyLOS.GetCurrentTargetPosition()); // Ensure LKTP is updated
                    stateContext.ChangeState("Search"); // Search will go to the target regardless of if its in LoS
                    break;
            }
            stateContext.enemyFrame.onDamaged = false; // Reset onDamaged
        }
    }
}
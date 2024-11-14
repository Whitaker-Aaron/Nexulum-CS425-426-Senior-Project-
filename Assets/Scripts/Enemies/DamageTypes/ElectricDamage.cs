// Handler for electric damage interaction with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDamage : DamageTypeHandler
{
    EnemyStateManager movementRef;

    public ElectricDamage(EnemyStateManager movementRef, float effectThreshold, float maxValue, float decayRate)
    {
        this.movementRef = movementRef;
        this.currentValue = 0;
        this.effectThreshold = effectThreshold;
        this.maxValue = maxValue;
        this.decayRate = decayRate;
    }

    public void executeEffect()
    {
        Debug.Log("Executing electric effect against enemy");

        if (currentValue > effectThreshold)
        {
            movementRef.ChangeMovementSpeed(0);
        }
    }
}
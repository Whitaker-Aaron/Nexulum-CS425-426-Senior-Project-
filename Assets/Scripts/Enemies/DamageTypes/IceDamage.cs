// Handler for ice damage interaction with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : DamageTypeHandler
{
    EnemyStateManager movementRef;
    float speedDecrement;
    float baseSpeed;

    public IceDamage(EnemyStateManager movementRef, float effectThreshold, float maxValue, float speedDecrement, float decayRate)
    {
        this.movementRef = movementRef;
        this.currentValue = 0;
        this.effectThreshold = effectThreshold;
        this.maxValue = maxValue;
        this.speedDecrement = speedDecrement;
        this.decayRate = decayRate;
    }

    public void execute() {
        Debug.Log("Executing ice effect against enemy");

        baseSpeed = movementRef.defaultMovementSpeed;

        if (currentValue < effectThreshold)
        {
            // Effect is impacting the enemy, but not yet reached its final form
            float currentSpeed = movementRef.agent.speed;
            float appliedDebuffSpeed = currentSpeed - speedDecrement;

            // Change movement speed of enemy to the calculated debuffed speed
            movementRef.ChangeMovementSpeed(appliedDebuffSpeed);
        }
        else
        {
            // Effect is "maxed"
            movementRef.ChangeMovementSpeed(0);
        }
    }
}
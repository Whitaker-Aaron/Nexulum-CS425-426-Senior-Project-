// Handler for ice damage interaction with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : IType
{
    EnemyStateManager movementRef;
    float originalSpeed;

    public float currentStacks = 0;
    float maxStacks;

    public IceDamage(EnemyStateManager movementRef, int maxStacks)
    {
        if(movementRef != null) this.movementRef = movementRef;
        if(maxStacks != null) this.maxStacks = maxStacks;
        if(movementRef != null) this.originalSpeed = movementRef.defaultMovementSpeed;
    }

    public float GetCurrentStacks()
    {
        return currentStacks;
    }

    public void execute()
    {
        Debug.Log("Current stacks: " + currentStacks);
        Debug.Log("Max stacks: " + maxStacks);

        // Percent movement reduction
        float percentage = (currentStacks / maxStacks);
        Debug.Log("Percentage: " + percentage);
        float newSpeed = originalSpeed - (originalSpeed * percentage);

        movementRef.currentSpeed = newSpeed;

        Debug.Log("Current speed: " + movementRef.currentSpeed + " Calculated speed: " + newSpeed);

        if (currentStacks == maxStacks)
        {
            movementRef.isFrozen = true;
        }
        else
        {
            movementRef.isFrozen = false;
        }
    }

    public void AddStacks(int num)
    {
        if (currentStacks <= maxStacks)
        {
            if (num < 0 && currentStacks != 0)
            {
                currentStacks += num;
            }
            else if (currentStacks < maxStacks)
            {
                currentStacks += num;
            }
        }
    }

    public void IncreaseMaxStacks(int num)
    {
        maxStacks += num;

        if (maxStacks < 0)
        {
            maxStacks = 0;
        }
    }
}
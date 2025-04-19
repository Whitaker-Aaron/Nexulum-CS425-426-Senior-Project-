// Handler for ice damage interaction with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : IType
{
    EnemyStateManager movementRef;
    float previousSpeed; // Speed before iteration
    float initialSpeed; // Speed before any ice effects were applied at all
    float originalSpeed;

    public float currentStacks = 0;
    float maxStacks;
    public bool isFrozen = false;
    private bool resetNext = false;
    ParticleSystem iceParticleSystemRef;
    EnemyFrame enemyFrameRef;

    public IceDamage(EnemyStateManager movementRef, int maxStacks, ParticleSystem particleSystem, EnemyFrame frameRef)
    {
        if(movementRef != null) this.movementRef = movementRef;
        if(maxStacks != null) this.maxStacks = maxStacks;
        if(movementRef != null) this.originalSpeed = movementRef.defaultMovementSpeed;
        if(particleSystem != null) this.iceParticleSystemRef = particleSystem;
        if(frameRef != null) this.enemyFrameRef = frameRef;
    }

    public float GetCurrentStacks()
    {
        return currentStacks;
    }

    public void SetCurrentStacks(float stacks)
    {
        currentStacks = stacks;
    }

    public float GetMaxStacks()
    {
        return maxStacks;
    }

    public void SetMaxStacks(float stacks)
    {
        maxStacks = stacks;
    }

    public void SetInitialSpeed(float speed)
    {
        initialSpeed = speed;
    }

    public void execute()
    {

        if (isFrozen)
        {
            resetNext = true;
        }

        float percentage = (currentStacks / maxStacks);
        float newSpeed = originalSpeed -  (originalSpeed * percentage);

        movementRef.currentSpeed = newSpeed;

        // Debug.Log("Current ice stacks: " + currentStacks);
        // Debug.Log("Current speed: " + movementRef.currentSpeed + " Calculated speed: " + newSpeed);
        // Debug.Log("Is frozen? " + isFrozen);
        // Debug.Log("Previous speed: " + originalSpeed);
        // Debug.Log("Percentage: " + percentage);
    }

    public void AddStacks(float num)
    {
        if (currentStacks <= maxStacks) // Ensure current stacks are <= max
        {
            if (num < 0 && currentStacks != 0) // For adding negative stacks
            {
                currentStacks += num;
                isFrozen = false; // Always going to be false when reducing stacks since current <= max
                if (resetNext) // Means that previously the stacks were max/enemy was frozen, reset the current stacks
                {
                    currentStacks = 0;
                    resetNext = false;
                }
            }
            else if (currentStacks < maxStacks) // For adding positive stacks
            {
                currentStacks += num;
                if (currentStacks == maxStacks)
                {
                    isFrozen = true;
                    Debug.Log("Enemy is at max stacks and frozen.");
                }
                else
                {
                    isFrozen = false;
                }
            }
            else // If current stacks exceeds max somehow, set it to max
            {
                currentStacks = maxStacks;
            }
        }
        // Debug.Log("Stacks increased by " + num);
    }

    public void IncreaseMaxStacks(float num)
    {
        maxStacks += num;

        if (maxStacks < 0)
        {
            maxStacks = 0;
        }
    }
}
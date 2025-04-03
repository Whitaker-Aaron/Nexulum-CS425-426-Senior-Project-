// Handler for ice damage interaction with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : IType
{
    EnemyStateManager movementRef;
    float previousSpeed; // Speed before iteration
    float initialSpeed; // Speed before any ice effects were applied at all

    public float currentStacks = 0;
    float maxStacks;
    public bool isFreezeCooldownActive = false;
    private float cooldown = 2f;

    public IceDamage(EnemyStateManager movementRef, int maxStacks)
    {
        this.movementRef = movementRef;
        this.maxStacks = maxStacks;
        this.previousSpeed = movementRef.defaultMovementSpeed;
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

    public void SetFreezeCooldown(float cd)
    {
        cooldown = cd;
    }

    public void execute()
    {
        // Percent movement reduction
        float percentage = (currentStacks / maxStacks);
        // float newSpeed = initialSpeed -  (initialSpeed * percentage);
        float newSpeed = previousSpeed -  (previousSpeed * percentage);

        movementRef.currentSpeed = newSpeed;

        Debug.Log("Current speed: " + movementRef.currentSpeed + " Calculated speed: " + newSpeed);
        Debug.Log("Previous speed: " + previousSpeed);
        Debug.Log("Current stacks: " + currentStacks);
        Debug.Log("Percentage: " + percentage);
    }

    public void AddStacks(float num)
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
        Debug.Log("Stacks increased by " + num);
    }

    public void IncreaseMaxStacks(float num)
    {
        maxStacks += num;

        if (maxStacks < 0)
        {
            maxStacks = 0;
        }
    }

    public IEnumerator FreezeCooldown()
    {
        Debug.Log("Freeze cooldown start: " + isFreezeCooldownActive);
        while (isFreezeCooldownActive)
        {
            Debug.Log("Freeze cooldown true, starting cooldown");
            yield return new WaitForSeconds(cooldown);
            AddStacks(-1f * maxStacks);
            // movementRef.currentSpeed = initialSpeed;
            // previousSpeed = initialSpeed;
            isFreezeCooldownActive = false;
        }
    }
}
// Interface for handling what happens when elemental effects interact with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IType
{
    void check() { }
    void execute() { }
}

public class DamageTypeHandler : IType
{
    public float currentValue;
    public float effectThreshold;
    public float maxValue;
    public float decayRate;

    // Ensure currentValue is within bounds, run execute to handle what the effect should do
    public void check()
    {
        Debug.Log("Running check() in damage type handler");
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }

        if (currentValue < 0)
        {
            currentValue = 0;
        }

        execute();
    }

    void execute() { }

    public IEnumerator Decay()
    {
        while (currentValue != 0)
        {
            currentValue -= decayRate * Time.deltaTime;
            yield return null;
        }
    }
}
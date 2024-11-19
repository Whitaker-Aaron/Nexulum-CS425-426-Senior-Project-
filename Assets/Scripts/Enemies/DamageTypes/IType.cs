// Interface for handling what happens when elemental effects interact with enemies - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IType
{
    void execute() { }
    void AddStacks(int num) { }
    void IncreaseMaxStacks(int num) { }
}
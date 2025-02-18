// Base class for states in an EnemyStateManager state machine - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyState : ScriptableObject
{
    protected EnemyStateManager stateContext;
    public string stateName { get; set; }

    public EnemyState()
    {
        this.stateName = "";
    }

    public virtual void EnterState(EnemyStateManager stateContext) { }
    public virtual void RunState() { }
    public virtual void ExitState() { }
}
// Base class for states in an EnemyStateManager state machine - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Enemy State")]
public class EnemyState : ScriptableObject
{
    protected EnemyStateManager stateContext;
    public string stateName;
    public EnemyState()
    {
        this.stateName = "";
    }

    public virtual void EnterState(EnemyStateManager stateContext) { }
    public virtual void RunState() { }
    public virtual void ExitState() { }
}
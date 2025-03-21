// Base class for states in an EnemyStateManager state machine - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyPatrolState : EnemyNeutralState
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Patrol";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        stateContext.agent.isStopped = stateContext.movementPaused;
        stateContext.agent.autoBraking = false;
        stateContext.agent.stoppingDistance = 0;

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("EnemyPatrolState.cs - Enemy set to patrol but patrol route is null or of length 0. Back to idle.");
            stateContext.stayInIdle = true;
            stateContext.ChangeState("Idle");
        }

        stateContext.MoveTo(patrolPoints[currentPointIndex].position, false);
    }

    public override void RunState()
    {
        base.OnDamaged();
        stateContext.agent.isStopped = stateContext.movementPaused;

        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag()) // Chase if target is seen
        {
            stateContext.CustomDebugLog("EnemyPatrolState - Target spotted");
            stateContext.ChangeState("Chase");
        }

        if (stateContext.EnemyIsAtPosition(patrolPoints[currentPointIndex].position) && patrolPoints.Length != 1)
        {
            stateContext.CustomDebugLog("Enemy is at patrol point " + patrolPoints[currentPointIndex].position);
            MoveToNextPoint();
        }
    }

    public override void ExitState()
    {
        stateContext.CustomDebugLog("Exited " + stateName + " state");
        stateContext.agent.autoBraking = true;
    }

    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public void SetPatrolPoints(Transform[] newPoints)
    {
        patrolPoints = newPoints;
    }

    private void MoveToNextPoint()
    {
        if (patrolPoints != null || patrolPoints.Length != 0)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            stateContext.MoveTo(patrolPoints[currentPointIndex].position, false);
        }
        else
        {
            Debug.LogError("EnemyPatrolState.cs - Enemy set to patrol but patrol route is null or of length 0.");
            return;
        }
    }

    public void AppendPatrolPoint(Transform point)
    {
        if (patrolPoints.Length != 0){
            patrolPoints[patrolPoints.Length - 1] = point;
        }
        else
        {
            patrolPoints[0] = point;
        }
    }
}
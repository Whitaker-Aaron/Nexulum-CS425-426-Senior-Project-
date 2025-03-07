// Base class for states in an EnemyStateManager state machine - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

[Serializable]
public class EnemyPatrolState : EnemyNeutralState
{
    public Vector3[] patrolPoints;
    private Vector3 nextPatrolPosition;
    private int currentPointIndex = 0;
    private NavMeshPath path;
    public override void EnterState(EnemyStateManager stateContext)
    {
        this.stateContext = stateContext;
        this.stateName = "Patrol";

        stateContext.CustomDebugLog("Entered " + stateName + " state");

        stateContext.agent.isStopped = stateContext.movementPaused;

        nextPatrolPosition = patrolPoints[currentPointIndex];

        // Create patrol path
        path = new NavMeshPath();
        bool calculatePathBool = NavMesh.CalculatePath(stateContext.enemyLOS.selfPos, nextPatrolPosition, NavMesh.AllAreas, path);

        stateContext.CustomDebugLog("CalculatePath returned " + calculatePathBool);
        stateContext.CustomDebugLog("Is stopped? " + stateContext.agent.isStopped);
        stateContext.CustomDebugLog("Next position = " + nextPatrolPosition);
        stateContext.CustomDebugLog("Current point index = " + currentPointIndex);
        stateContext.CustomDebugLog("Path: " + path);
        stateContext.CustomDebugLog("Path Status: " + path.status);
        stateContext.CustomDebugLog("Agent enabled? " + stateContext.agent.enabled);

        // stateContext.MoveTo(nextPatrolPosition, false);
    }

    public override void RunState()
    {
        base.OnDamaged();

        if (stateContext.TargetSpotted() == stateContext.GetCurrentTargetTag()) // Chase if target is seen
        {
            stateContext.CustomDebugLog("EnemyPatrolState - Target spotted");
            stateContext.ChangeState("Chase");
        }

        if (stateContext.EnemyIsAtPosition(nextPatrolPosition)) // Patrol otherwise
        {
            stateContext.CustomDebugLog("Arrived at point " + nextPatrolPosition);
            IncrementCurrentPointIndex();
            SetnextPatrolPosition(patrolPoints[currentPointIndex]);
            stateContext.CustomDebugLog("Moving to next point " + nextPatrolPosition + " at index " + currentPointIndex);
            // stateContext.MoveTo(nextPatrolPosition, false);
        }
    }

    public override void ExitState()
    {
        stateContext.CustomDebugLog("Exited " + stateName + " state");
    }

    private Vector3 GetnextPatrolPosition()
    {
        return patrolPoints[currentPointIndex + 1];
    }

    private void SetnextPatrolPosition(Vector3 position)
    {
        nextPatrolPosition = position;
    }

    public Vector3[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public void SetPatrolPoints(Vector3[] points)
    {
        patrolPoints = points;
    }

    private void IncrementCurrentPointIndex() // Increments current point index by 1
    {
        int maxIndex = patrolPoints.Length - 1;
        if (currentPointIndex + 1 <= maxIndex) // If, after adding amount, the new index would be within array bounds
        {
            currentPointIndex += 1;
        }
        else // Otherwise wrap around, index only incremented by 1 in this fxn -> currentPointIndex can just be set to 0
        {
            currentPointIndex = 0;
        }
    }
}
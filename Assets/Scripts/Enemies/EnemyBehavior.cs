using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    public int detectionRange;
    public NavMeshAgent agent;
    public GameObject target;
    Vector3 selfPosition;
    Vector3 playerPosition;
    float distanceToPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        selfPosition = transform.position;
        playerPosition = target.transform.position;
        distanceToPlayer = (playerPosition - selfPosition).magnitude;

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(playerPosition);
        }
        else
        {
            agent.SetDestination(selfPosition);
        }
    }
}

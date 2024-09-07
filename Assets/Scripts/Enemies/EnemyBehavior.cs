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

    //animation function for getting direction, sends to animation interface
    private EnemyAnimation enemyAnim;
    private Vector3 CalculateMovementDirecton()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return (player.transform.position - transform.position).normalized;
    }


    void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        target = GameObject.FindWithTag("Player");
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

        //animation handling
        Vector3 movementDirection = CalculateMovementDirecton();
        enemyAnim.updateAnimation(movementDirection);



    }
}

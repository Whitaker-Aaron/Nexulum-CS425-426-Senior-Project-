using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAgent : MonoBehaviour
{
    // Navmesh agent component, used by states to execute NavMesh related movement
    public NavMeshAgent agent;

    // LOS component, states can check the enemy LOS
    public EnemyLOS enemyLOS;

    public float enemyWalkspeed = 1;
    public float enemySprintSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLOS = GetComponent<EnemyLOS>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

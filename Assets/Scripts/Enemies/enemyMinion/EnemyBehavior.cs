using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Animation Direction Script

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    private EnemyAnimation enemyAnim; //animation function for getting direction, sends to animation interface

    bool isMoving = true;
    bool paused = false;

    public IEnumerator pauseMovement(float time)
    {
        isMoving = false;
        yield return new WaitForSeconds(time);
        isMoving = true;
        yield break;
    }

    
    void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        target = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
    }

    void FixedUpdate()
    {
        if (!isMoving || paused)
            return;

        //animation handling
        Vector3 movementDirection = gameObject.GetComponent<NavMeshAgent>().velocity;//CalculateMovementDirecton();
       
        if (movementDirection.magnitude < 0.3f || movementDirection == Vector3.zero)
        {
            enemyAnim.updateAnimation(Vector3.zero);
        }
        else
        {
            enemyAnim.updateAnimation(movementDirection);
        }
    }

    
}

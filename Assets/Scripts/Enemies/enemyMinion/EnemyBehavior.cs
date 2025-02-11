using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Components
    //public NavMeshAgent agent;
    public GameObject target;
    //[HideInInspector] public EnemyStateManager enemyStateManager;
    private EnemyAnimation enemyAnim; //animation function for getting direction, sends to animation interface - Spencer

    //[HideInInspector] public bool isTargetSpotted = false;

    //// Enemy vision - Aisling
    //[SerializeField] private float detectionRange = 7;
    //[SerializeField] private float visionAngle = 90;

    //private Vector3 selfPos;
    //private Vector3 targetPos;
    //[HideInInspector] public Vector3 lastKnownTargetPos;

    //stop movement implementation for combat, simple bool control - Spencer
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

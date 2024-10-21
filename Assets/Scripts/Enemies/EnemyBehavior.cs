using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    // NavMesh agent and GameObject for target - Aisling
    public NavMeshAgent agent;
    public GameObject target;

    // Enemy vision - Aisling
    public float detectionRange = 7;
    public float visionAngle = 90;

    // State manager ref, allows state changes to be conducted in EnemyBehavior - Aisling
    [HideInInspector]
    public EnemyStateManager enemyStateManager;

    //animation function for getting direction, sends to animation interface - Spencer
    private EnemyAnimation enemyAnim;

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

    private Vector3 CalculateMovementDirecton()
    {
        GameObject player = GameObject.FindGameObjectWithTag(target.tag);
        return (player.transform.position - transform.position).normalized;
    }

    void Start()
    {
        enemyStateManager = GetComponent<EnemyStateManager>();
        enemyAnim = GetComponent<EnemyAnimation>();
        target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!isMoving || paused)
            return;

        //animation handling
        Vector3 movementDirection = CalculateMovementDirecton();
        enemyAnim.updateAnimation(movementDirection);
    }

    public bool TargetSpotted()
    {
        Vector3 selfpos = transform.position;
        Vector3 targetpos = target.transform.position;
        Vector3 headingtotarget = targetpos - selfpos;

        float distancetotarget = Vector3.Distance(targetpos, selfpos);
        float targetangle = Vector3.Angle(headingtotarget, transform.forward);

        RaycastHit hit;

        if ((distancetotarget <= detectionRange)) // Determine if target is within detection range
        {
            if (targetangle <= visionAngle) // Determine if target is in vision 'cone' (angle)
            {
                Physics.Raycast(origin: selfpos, direction: headingtotarget.normalized, hitInfo: out hit, maxDistance: detectionRange);
                if (hit.transform == target.transform)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}

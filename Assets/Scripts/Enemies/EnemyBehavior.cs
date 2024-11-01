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

    private Vector3 CalculateMovementDirecton()
    {
        if (target != null)
        {
            return (target.transform.position - transform.position).normalized;
        }
        else
        {
            return transform.position;
        }
    }

    void Awake()
    {
        target = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
    }

    void Update()
    {
        if (!isMoving || paused)
            return;

        //animation handling
        Vector3 movementDirection = CalculateMovementDirecton();
        enemyAnim.updateAnimation(movementDirection);
    }

    //    public void ChangeTarget(GameObject newTarget)
    //    {
    //        if (newTarget != null)
    //        {
    //            target = newTarget;
    //        }
    //        else
    //        {
    //            Debug.Log("New target cannot be null");
    //        }
    //    }

    //    public bool TargetSpotted()
    //    {
    //        if (target != null)
    //        {
    //            selfPos = transform.position;
    //            targetPos = target.transform.position;
    //            Vector3 headingtotarget = targetPos - selfPos;

    //            float distancetotarget = Vector3.Distance(targetPos, selfPos);
    //            float targetangle = Vector3.Angle(headingtotarget, transform.forward);

    //            RaycastHit hit;

    //            if ((distancetotarget <= detectionRange)) // Determine if target is within detection range
    //            {
    //                if (targetangle <= visionAngle) // Determine if target is in vision 'cone' (angle)
    //                {
    //                    Physics.Raycast(origin: selfPos, direction: headingtotarget.normalized, hitInfo: out hit, maxDistance: detectionRange); // Determine if target is obstructed
    //                    if (hit.transform == target.transform)
    //                    {
    //                        // smoothLook(target.transform);
    //                        return true;
    //                    }
    //                    else
    //                    {
    //                        return false;
    //                    }
    //                }
    //                else
    //                {
    //                    return false;
    //                }
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    // Spencer's smooth LookAt function from drone combat script, slight edit to work with enemies
    //    void smoothLook(Transform target)
    //    {
    //        Vector3 direction = (target.position - transform.position).normalized;
    //        direction.y = 0f;  // Keep the y-axis at 0
    //        Quaternion targetRot = Quaternion.LookRotation(direction);

    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime);
    //    }
}

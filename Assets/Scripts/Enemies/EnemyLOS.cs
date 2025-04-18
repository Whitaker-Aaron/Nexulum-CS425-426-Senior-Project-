// Enemy visual field - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLOS : MonoBehaviour
{
    // ----------------------------------------------
    // Targeting
    // ----------------------------------------------

    public GameObject currentTarget { get; set; }

    public bool isTargetSpotted = false;
    public bool canTarget = true;

    private LayerMask layers = LayerMask.GetMask();

    // ----------------------------------------------
    // Concrete targets
    // ----------------------------------------------

    public GameObject player;

    // ----------------------------------------------
    // Visual field variables
    // ----------------------------------------------

    // Scope of visual field (in-editor configurable)
    public float detectionRange = 7;
    [SerializeField] private int visionAngle = 90;

    // Enable xray vision
    [SerializeField] private bool canSeeThroughWalls = false;

    // Debugging
    [SerializeField] public float distancetotarget;

    [SerializeField] private Vector3 headingtotarget;
    public Vector3 myHeading;

    // ----------------------------------------------
    // Positions
    // ----------------------------------------------

    public Vector3 selfPos;
    public Vector3 targetPos;
    public Vector3 lastKnownTargetPos;

    // ----------------------------------------------
    // Methods
    // ----------------------------------------------

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ChangeTarget(player);
    }

    // Update is called once per frame
    void Update()
    {
        myHeading = transform.forward;
    }

    public void SetDetectionRange(float range)
    {
        detectionRange = range;
    }

    public void SetVisionAngle(int angle)
    {
        visionAngle = angle;
    }

    public void SetLastKnownTargetPosition(Vector3 position)
    {
        lastKnownTargetPos = position;
    }

    public float GetDistanceToTarget()
    {
        return distancetotarget;
    }

    public GameObject GetCurrentTargetObject()
    {
        return currentTarget;
    }

    public Vector3 GetCurrentTargetPosition()
    {
        return targetPos;
    }

    public Vector3 GetLastKnownTargetPosition()
    {
        return lastKnownTargetPos;
    }

    public float GetDistanceToLastKnownPos()
    {
        return Vector3.Distance(lastKnownTargetPos, selfPos);
    }

    public float GetDistanceToPosition(Vector3 position)
    {
        return Vector3.Distance(position, selfPos);
    }

    // ChangeTarget takes a gameobject (a new target) and switches the current target to the new target
    // Returns true if successful, returns false if the new target is null (a null target causes errors)
    public bool ChangeTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            currentTarget = newTarget;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Return the tag of the collider spotted
    public string TargetSpotted()
    {
        //if (!canTarget) return null;
        if (currentTarget != null)
        {
            selfPos = transform.position;
            targetPos = currentTarget.transform.position;
            headingtotarget = targetPos - selfPos;

            distancetotarget = Vector3.Distance(targetPos, selfPos);
            float targetangle = Vector3.Angle(headingtotarget, transform.forward);

            RaycastHit hit;
            //(targetangle <= visionAngle)
            //(distancetotarget <= detectionRange) && 
            if ((distancetotarget <= detectionRange) && canSeeThroughWalls)
            {
                isTargetSpotted = true;
                return currentTarget.tag;
            }
            else if ((distancetotarget <= detectionRange) && !canSeeThroughWalls)
            {
                Physics.Raycast(origin: selfPos, direction: headingtotarget.normalized, hitInfo: out hit, maxDistance: detectionRange); // Determine if target is obstructed
                //Debug.Log("Ray hit: " + hit.collider.tag);
                if (hit.transform == currentTarget.transform)
                {
                    isTargetSpotted = true;
                    return hit.collider.tag;
                }
                else
                {
                    isTargetSpotted = false;
                    return null;
                }
            }
            else
            {
                isTargetSpotted = false;
                return null;
            }
        }
        else
        {
            isTargetSpotted = false;
            return null;
        }
    }

    public bool TargetInDetectionRange()
    {
        selfPos = transform.position;
        targetPos = currentTarget.transform.position;

        distancetotarget = Vector3.Distance(targetPos, selfPos);

        if (currentTarget != null)
        {
            if (distancetotarget <= detectionRange)
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

    void OnDrawGizmosSelected()
    {
        if (isTargetSpotted)
        {
            Gizmos.DrawLine(selfPos, targetPos);
        }
    }
}
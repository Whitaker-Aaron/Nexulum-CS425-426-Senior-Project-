// Enemy visual field - Aisling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLOS : MonoBehaviour
{
    private GameObject currentTarget;

    public GameObject CurrentTarget
    {
        get
        {
            return currentTarget;
        }
    }

    public bool isTargetSpotted = false;

    [SerializeField] private float detectionRange = 7;
    [SerializeField] private float visionAngle = 90;

    private Vector3 selfPos;
    private Vector3 targetPos;

    public Vector3 TargetPos
    {
        get
        {
            return targetPos;
        }
    }

    public Vector3 lastKnownTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        ChangeTarget(GameObject.FindWithTag("Player"));
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

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

    public bool TargetSpotted()
    {
        if (currentTarget != null)
        {
            selfPos = transform.position;
            targetPos = currentTarget.transform.position;
            Vector3 headingtotarget = targetPos - selfPos;

            float distancetotarget = Vector3.Distance(targetPos, selfPos);
            float targetangle = Vector3.Angle(headingtotarget, transform.forward);

            RaycastHit hit;

            if ((distancetotarget <= detectionRange)) // Determine if target is within detection range
            {
                if (targetangle <= visionAngle) // Determine if target is in vision 'cone' (angle)
                {
                    Physics.Raycast(origin: selfPos, direction: headingtotarget.normalized, hitInfo: out hit, maxDistance: detectionRange); // Determine if target is obstructed
                    if (hit.transform == currentTarget.transform)
                    {
                        isTargetSpotted = true;
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
        else
        {
            return false;
        }
    }
}

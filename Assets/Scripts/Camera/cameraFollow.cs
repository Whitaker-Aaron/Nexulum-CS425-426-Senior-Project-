using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.0005f;
    public bool yAxisLocked = false;
    public bool lookAtLocked = false;
    public float lastYPos;
    public FollowMode followMode = FollowMode.Lerp;
    public Vector3 offset;

    bool found = false;
    public bool pauseFollow = false;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        found = true;
        lastYPos = transform.position.y;
    }

    public void PauseFollow()
    {
        pauseFollow = true;
    }
    public void UnpauseFollow()
    {
        pauseFollow = false;
    }

    //private void Update()
    //{
    //    if (!found)
    //    {
    //        if (GameObject.FindWithTag("Player") == null)
    //            return;
    //    }
    //    else
    //        return;
    //}

    void LateUpdate()
    {

        switch(followMode)
        {
            case FollowMode.Lerp:
                LerpFollow();
                break;
            case FollowMode.Exact:
                ExactFollow();
                break;
        }

    }

    void LerpFollow()
    {
        if (target == null)
            return;
        else if (!pauseFollow)
        {
            Vector3 desiredPosition;
            if (!yAxisLocked)
            {
                desiredPosition = target.position + offset;
                lastYPos = transform.position.y;
            }
            else
            {
                desiredPosition = new Vector3(target.position.x + offset.x, lastYPos, target.position.z + offset.z);
            }
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
            

        }
        if(!lookAtLocked) transform.LookAt(target);


    }

    void ExactFollow()
    {
        if(!pauseFollow) transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        if (!lookAtLocked) transform.LookAt(target);
    }

    public enum FollowMode
    {
        Lerp,
        Exact
    }


}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.0005f;
    public bool yAxisLocked = false;
    public float lastYPos;
    public Vector3 offset;

    bool found = false;
    bool pauseFollow = false;

    private void Start()
    {
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

    private void Update()
    {
        if (!found)
        {
            if (GameObject.FindWithTag("Player") == null)
                return;

            target = GameObject.FindWithTag("Player").transform;
            found = true;
        }
        else
            return;
    }

    void FixedUpdate()
    {

        if (target == null)
            return;
        else if(!pauseFollow)
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
        transform.LookAt(target);

    }


}



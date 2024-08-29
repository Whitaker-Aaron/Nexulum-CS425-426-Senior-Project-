using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = .125f;
    public Vector3 offset;

    bool found = false;

    private void Start()
    {
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
        else
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
            transform.LookAt(target);
        }

    }


}



using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 positionTarget;

    public float smoothSpeed = 0.0005f;
    public bool cameraPanning = false;
    public bool yAxisLocked = false;
    public bool panYAxisLocked = false;
    public bool lookAtLocked = false;
    public bool panLookAtLocked = false;
    public float lastYPos;
    public FollowMode followMode = FollowMode.Lerp;
    public Vector3 offset;

    bool found = false;
    public bool pauseFollow = false;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        found = true;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
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

    public void PauseLookAt()
    {
        lookAtLocked = true;
    }

    public void UnpauseLookAt()
    {
        lookAtLocked = false;
    }

    public void SetCameraMode(FollowMode mode)
    {
        followMode = mode;
    }

    public void StartPan(Vector3 positionToPanTo, bool lockY, bool lockLook, float rate)
    {
        panLookAtLocked = lockLook;
        panYAxisLocked = lockY;
        StartCoroutine(PanToPosition(positionToPanTo, rate));
    }

    public IEnumerator PanToPosition(Vector3 position, float rate)
    {
        if(cameraPanning)
        {
            yield break;
        }
        cameraPanning = true;
        target.transform.GetComponent<CharacterBase>().GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        float ogSpeed = smoothSpeed;
        CameraFollow.FollowMode ogFollowMode = followMode;
        positionTarget = position;
        smoothSpeed = rate;
        SetCameraMode(CameraFollow.FollowMode.PositionLerp);
        yield return new WaitForSeconds(2.5f);
        smoothSpeed = 0.05f;
        SetCameraMode(CameraFollow.FollowMode.Lerp);
        yield return new WaitForSeconds(2f);
        SetCameraMode(ogFollowMode);
        yield return new WaitForSeconds(0.25f);
        smoothSpeed = ogSpeed;
        cameraPanning = false;
        target.transform.GetComponent<CharacterBase>().GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        panLookAtLocked = false;
        panYAxisLocked = false;
        yield break;
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
                return;
            case FollowMode.Exact:
                ExactFollow();
                //LerpFollow();
                break;
        }
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);

    }
    private void FixedUpdate()
    {
        
        switch (followMode)
        {
            case FollowMode.Lerp:
                LerpFollow();
                break;
            case FollowMode.PositionLerp:
                PositionLerp();
                break;
            case FollowMode.Exact:
                return;
        }

    }

    void LerpFollow()
    {
        if (target == null)
            return;
        else if (!pauseFollow)
        {
            Vector3 desiredPosition;
            if (!yAxisLocked && !panYAxisLocked)
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
        //if(!lookAtLocked && !panLookAtLocked) transform.LookAt(target.position);
        if (!lookAtLocked && !panLookAtLocked)
        {
            Vector3 direction = (target.position - (transform.position + offset));
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            toRotation = Quaternion.Euler(toRotation.eulerAngles.x, toRotation.eulerAngles.y, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothSpeed*2);
        }



    }

    void ExactFollow()
    {
        if(!pauseFollow) transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        //if (!lookAtLocked && !panLookAtLocked) transform.LookAt(target.position);
        if (!lookAtLocked && !panLookAtLocked)
        {
            Vector3 direction = (target.position - (transform.position + offset));
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            toRotation = Quaternion.Euler(toRotation.eulerAngles.x, toRotation.eulerAngles.y, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothSpeed * 2);
        }
    }

    void PositionLerp()
    {
        if (positionTarget == null)
            return;
        else if (!pauseFollow)
        {
            Vector3 desiredPosition;
            if (!yAxisLocked && !panYAxisLocked)
            {
                desiredPosition = positionTarget + offset;
                lastYPos = transform.position.y;
            }
            else
            {
                desiredPosition = new Vector3(positionTarget.x + offset.x, lastYPos, positionTarget.z + offset.z);
            }
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;


        }
        if (!panLookAtLocked) transform.LookAt(positionTarget);
        //if (!panLookAtLocked)
        //{
        //    Vector3 direction = (positionTarget - transform.position).normalized;
        //    Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothSpeed);
        //}
        
    }

    void SetPositionTarget(Vector3 posTarget)
    {
        positionTarget = posTarget;
    }

    public enum FollowMode
    {
        Lerp,
        Exact,
        PositionLerp,
        PositionExact
    }


}



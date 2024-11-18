using System.Collections;
using System.Collections.Generic;
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

    public void StartPan(Vector3 positionToPanTo, bool lockY, bool lockLook)
    {
        panLookAtLocked = lockLook;
        panYAxisLocked = lockY;
        StartCoroutine(PanToPosition(positionToPanTo));
    }

    public IEnumerator PanToPosition(Vector3 position)
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
        smoothSpeed = 0.01f;
        SetCameraMode(CameraFollow.FollowMode.PositionLerp);
        yield return new WaitForSeconds(3.5f);
        smoothSpeed = 0.05f;
        SetCameraMode(CameraFollow.FollowMode.Lerp);
        yield return new WaitForSeconds(1f);
        SetCameraMode(ogFollowMode);
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
                break;
        }

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
        if(!lookAtLocked && !panLookAtLocked) transform.LookAt(target.position);


    }

    void ExactFollow()
    {
        if(!pauseFollow) transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        if (!lookAtLocked && !panLookAtLocked) transform.LookAt(target.position);
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



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 desiredPosition;
    [SerializeField] float speed;
    bool playerOnPlatform = false;
    private CameraFollow cameraRef;
    void Awake()
    {
        //startPosition = transform.position;   
        cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        transform.localPosition = startPosition;
        StartCoroutine(animatePlatform());
    }

    private void OnDisable()
    {
        transform.localPosition = startPosition;
        StopCoroutine(animatePlatform());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered platform");
            collision.gameObject.transform.parent = transform;
            playerOnPlatform = true;
            StartCoroutine(EnableExactFollowOnCamera());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has left platform");
            collision.gameObject.transform.parent = null;
            playerOnPlatform = false;
            StartCoroutine(EnableLerpFollowOnCamera());
            DontDestroyOnLoad(collision.gameObject);
            
        }
    }

    public IEnumerator EnableLerpFollowOnCamera()
    {
        yield return new WaitForSeconds(0.2f);
        if(!playerOnPlatform)
        {
            cameraRef.SetCameraMode(CameraFollow.FollowMode.Lerp);
        }
    }

    public IEnumerator EnableExactFollowOnCamera()
    {
        yield return new WaitForSeconds(0.2f);
        if (playerOnPlatform)
        {
            cameraRef.SetCameraMode(CameraFollow.FollowMode.Exact);
        }
    }

    public IEnumerator animatePlatform()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            while (transform.localPosition != desiredPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, desiredPosition, Time.deltaTime * speed);
                if(Mathf.Abs(transform.localPosition.magnitude - desiredPosition.magnitude) < 0.1)
                {
                    transform.localPosition = desiredPosition;
                }
                yield return null;
            }
            yield return new WaitForSeconds(5f);
            Debug.Log("Start pos: " + startPosition);
            Debug.Log("Current pos: " + transform.localPosition);
            while (transform.localPosition != startPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, Time.deltaTime * speed);
                if (Mathf.Abs(transform.localPosition.magnitude - startPosition.magnitude) < 0.1)
                {
                    transform.localPosition = startPosition;
                }
                yield return null;
            }
            yield return null;
        }
        
    }
}

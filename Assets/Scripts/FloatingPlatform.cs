using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 desiredPosition;
    [SerializeField] float speed;
    bool playerOnPlatform = false;
    private CameraFollow cameraRef;
    private CharacterBase character;
    void Awake()
    {
        //startPosition = transform.position;   
        cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    void Start()
    {
        
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
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered platform");
            collision.gameObject.transform.parent = transform;
            playerOnPlatform = true;
            //StartCoroutine(EnableExactFollowOnCamera());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            character.floatingPlatformCounter++;
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
            //StartCoroutine(EnableLerpFollowOnCamera());
            DontDestroyOnLoad(collision.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(character.floatingPlatformCounter > 0) character.floatingPlatformCounter--;
            StartCoroutine(EnableLerpFollowOnCamera());

        }
    }

    public IEnumerator EnableLerpFollowOnCamera()
    {
        yield return new WaitForSeconds(1.3f);
        if (!playerOnPlatform)
        {
            cameraRef.SetCameraMode(CameraFollow.FollowMode.Lerp);
        }
        yield break;
    }

    public IEnumerator EnableExactFollowOnCamera()
    {
        yield return new WaitForSeconds(0.25f);
        if (playerOnPlatform)
        {
            cameraRef.SetCameraMode(CameraFollow.FollowMode.Exact);
        }
        yield break;
    }

    public IEnumerator animatePlatform()
    {
        while (true)
        {
            //yield return new WaitForSeconds(2f);
            while (transform.localPosition != desiredPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, desiredPosition, Time.deltaTime * speed);
                if (Mathf.Abs(transform.localPosition.magnitude - desiredPosition.magnitude) < 0.025)
                {
                    transform.localPosition = desiredPosition;
                }
                yield return null;
            }
            //yield return new WaitForSeconds(2f);
            Debug.Log("Start pos: " + startPosition);
            Debug.Log("Current pos: " + transform.localPosition);
            while (transform.localPosition != startPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, Time.deltaTime * speed);
                if (Mathf.Abs(transform.localPosition.magnitude - startPosition.magnitude) < 0.025)
                {
                    transform.localPosition = startPosition;
                }
                yield return null;
            }
            yield return null;
        }

    }
}

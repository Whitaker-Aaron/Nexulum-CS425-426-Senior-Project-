using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject objectToRotateAround;
    bool playerOnPlatform = false;
    private CameraFollow cameraRef;
    private CharacterBase character;
    [SerializeField] bool rotate = true;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER ENTERED ROTATE AROUND COLLISION");
            collision.gameObject.transform.parent = transform.parent;
            //collision.gameObject.transform.SetParent(transform, false);
            playerOnPlatform = true;
            //StartCoroutine(EnableExactFollowOnCamera());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER ENTERED ROTATE AROUND");
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
            if (character.floatingPlatformCounter > 0) character.floatingPlatformCounter--;
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

    // Update is called once per frame
    void Update()
    {
        if(rotate) this.transform.RotateAround(objectToRotateAround.transform.position, new Vector3(0,1,0), 10 * Time.deltaTime);
    }
}

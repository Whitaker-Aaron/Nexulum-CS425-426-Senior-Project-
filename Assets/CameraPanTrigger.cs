using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPanTrigger : MonoBehaviour
{
    [SerializeField] GameObject objectToPanTo;
    [SerializeField] float panSpeed;
    [SerializeField] bool panYAxisLocked = false;
    [SerializeField] bool panLookAtLocked = false;
    CameraFollow camera;
    // Start is called before the first frame update

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(StartCameraPanThenDestroy());
        }
    }

    public IEnumerator StartCameraPanThenDestroy()
    {
        camera.panYAxisLocked = panYAxisLocked;
        camera.panLookAtLocked = panLookAtLocked;
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(camera.PanToPosition(objectToPanTo.transform.position, panSpeed));
        Destroy(this.gameObject);
    }
}

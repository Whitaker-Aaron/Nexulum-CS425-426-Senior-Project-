using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBuildObjct : MonoBehaviour
{
    [SerializeField] Vector3 desiredPos;
    [SerializeField] Vector3 startPos;
    Vector3 desiredUpPos;
    Vector3 desiredDownPos;
    float desiredRotation;
    bool rotationFinished = false;
    bool ascendFinished;
    // Start is called before the first frame update
    void Start()
    {
        rotationFinished = false;
        ascendFinished = false;
        desiredRotation = transform.rotation.eulerAngles.z + 90f;
        desiredUpPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        desiredDownPos = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator rotateObject()
    {
        var desiredRot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, desiredRotation);
        var desiredEuler = Quaternion.Euler(desiredRot.x, desiredRot.y, desiredRot.z);
        while (transform.rotation.z < desiredEuler.z)
        {
            var rotation = new Vector3(0, 0, 75 * Time.deltaTime);

            transform.Rotate(rotation);
            if(Mathf.Abs(transform.rotation.eulerAngles.z - desiredRotation) <= 0.25f)
            {
                break;
                transform.rotation = Quaternion.Euler(desiredRot.x, desiredRot.y, desiredRot.z);
            }
            yield return null;
        }
        rotationFinished = true;
        yield break;
    }

    public IEnumerator moveObjectUp()
    {
        while(transform.position.y < desiredUpPos.y)
        {
            transform.position = Vector3.Lerp(transform.position, desiredUpPos, Time.deltaTime * 2.5f);
            if(Mathf.Abs(transform.position.y - desiredUpPos.y) <= 0.025f){
                transform.position = desiredUpPos;
            }
            yield return null;
        }
        //ascendFinished = true;
        //yield break;
    }

    public IEnumerator moveObjectDown()
    {
        while (transform.position.y > desiredDownPos.y)
        {
            transform.position = Vector3.Lerp(transform.position, desiredDownPos, Time.deltaTime * 2.5f);
            if (Mathf.Abs(transform.position.y - desiredDownPos.y) <= 0.025f)
            {
                transform.position = desiredDownPos;
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator animateBuildObject()
    {
        //StartCoroutine(rotateObject());
        yield return new WaitForSeconds(0.15f);
        yield return StartCoroutine(moveObjectUp());
        //while(!rotationFinished && !ascendFinished)
        //{
        //    yield return null;
        //}
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(moveObjectDown());
        yield break;
    }
}

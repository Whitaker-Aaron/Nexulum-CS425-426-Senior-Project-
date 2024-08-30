using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
            //body.AddForce(0.0f, 0.0f, 500.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f);
        }
    }
}


using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Physics should be processed in the fixed update
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W)){
            body.position += new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
            body.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //body.AddForce(0.0f, 0.0f, 500.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.position -= new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
            body.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.position -= new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f);
            body.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            body.position += new Vector3(5.0f * Time.deltaTime, 0.0f, 0.0f);
            body.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

    }
}

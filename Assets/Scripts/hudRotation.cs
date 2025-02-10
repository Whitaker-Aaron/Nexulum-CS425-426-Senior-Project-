using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class hudRotation : MonoBehaviour
{
    [SerializeField] Vector3 rate;
    float lastUpdateTime = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*float currentTime = Time.unscaledTime;

        float deltaTime = currentTime - lastUpdateTime;

        lastUpdateTime = currentTime;
        //this.transform.Rotate(rate * Time.unscaledDeltaTime);
        Debug.Log(Time.unscaledDeltaTime);
        var rectTrans = this.GetComponent<RectTransform>();
        rectTrans.rotation = Quaternion.Euler(rectTrans.eulerAngles.x, rectTrans.eulerAngles.y, rectTrans.eulerAngles.z + (rate.z * Time.unscaledDeltaTime));
        */
    }

    private void Update()
    {
        float currentTime = Time.unscaledTime;

        float deltaTime = currentTime - lastUpdateTime;

        lastUpdateTime = currentTime;
        //this.transform.Rotate(rate * Time.unscaledDeltaTime);
        //Debug.Log(Time.unscaledDeltaTime);
        var rectTrans = this.GetComponent<RectTransform>();
        rectTrans.rotation = Quaternion.Euler(rectTrans.eulerAngles.x, rectTrans.eulerAngles.y, rectTrans.eulerAngles.z + (rate.z * Time.unscaledDeltaTime));
    }
}

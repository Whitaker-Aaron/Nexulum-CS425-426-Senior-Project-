using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDemoTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().DisplayThankYouScreen();
            Destroy(this.gameObject);
        }
    }
}

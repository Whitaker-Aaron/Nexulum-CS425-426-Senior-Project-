using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslaWall : MonoBehaviour
{
    public GameObject teslaParent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(teslaParent != null)
            {
                teslaParent.GetComponent<teslaTower>().attackEnemy(other);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

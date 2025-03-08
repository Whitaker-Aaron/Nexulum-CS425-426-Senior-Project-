using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golemPart : MonoBehaviour
{
    [SerializeField] GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    golemBoss getParent()
    {
        return parent.GetComponent<golemBoss>();
    }
}

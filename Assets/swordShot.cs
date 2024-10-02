using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordShot : MonoBehaviour
{
    public float lifeTime = 7f;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
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

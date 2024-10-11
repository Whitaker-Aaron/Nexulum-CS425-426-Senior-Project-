using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public string name;
    [SerializeField] public bool spawnPlayer;
    [SerializeField] public bool screenTransition;
    void Start()
    {
        
    }

    private void Awake()
    {
        if(spawnPlayer) GameObject.FindWithTag("Player").transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        if(screenTransition) StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

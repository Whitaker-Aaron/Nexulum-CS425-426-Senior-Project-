using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public string sceneName;
    [SerializeField] public string transitionTitle;
    [SerializeField] public bool spawnPlayer;
    [SerializeField] public bool screenTransition;
    [SerializeField] public Vector3 playerSpawnPos; 
    void Start()
    {
        if(sceneName == "BaseCamp")
        {
            var baseRoom = new RoomInformation();
            baseRoom.roomName = sceneName;
            baseRoom.isCheckpoint = true;
            GameObject.Find("RoomManager").GetComponent<RoomManager>().SetRoom(baseRoom);
        }
       
    }

    private void Awake()
    {
        if(spawnPlayer) GameObject.FindWithTag("Player").transform.position = playerSpawnPos;
        if(screenTransition) StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

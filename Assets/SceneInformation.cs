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
    [SerializeField] RoomInformation beginningRoom;
    [SerializeField] public Vector3 playerSpawnPos;
    [SerializeField] public GameObject initialSpawnLocation;
    void Start()
    {
        var roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        if (sceneName == "BaseCamp")
        {
            beginningRoom = new RoomInformation();
            beginningRoom.roomName = sceneName;
            beginningRoom.isCheckpoint = true;
            roomManager.SetRoom(beginningRoom);

        }
        else if(beginningRoom != null)
        {
            roomManager.SetRoom(beginningRoom);
        }
        

    }

    private void Awake()
    {
        
        if (spawnPlayer)
        {
            var player = GameObject.FindWithTag("Player");
            if (initialSpawnLocation != null)
            {
                player.transform.position = initialSpawnLocation.transform.position;
            }
            else
            {
                player.transform.position = playerSpawnPos;
            }
            
        }
        
        if(screenTransition) StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

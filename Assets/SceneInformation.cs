using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        
        

    }

    private void Awake()
    {
        var roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        var player = GameObject.FindWithTag("Player");
        if (sceneName == "BaseCamp")
        {
            beginningRoom = new RoomInformation();
            beginningRoom.roomName = sceneName;
            beginningRoom.isCheckpoint = true;
            roomManager.SetRoom(beginningRoom);

        }
        else if (beginningRoom != null)
        {
            roomManager.SetRoom(beginningRoom);
        }

        if (spawnPlayer && sceneName != "BaseCamp")
        {

            if (initialSpawnLocation != null)
            {
                player.transform.position = initialSpawnLocation.transform.position;
            }
            else
            {
                player.transform.position = playerSpawnPos;
            }
        }
        else if (sceneName == "BaseCamp" && player.GetComponent<CharacterBase>().transitioningRoom)
        {

            StartCoroutine(WaitThenStartCharacterMove(player));
        }
        
        if(screenTransition) StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    public IEnumerator WaitThenStartCharacterMove(GameObject character)
    {
        character.transform.position = initialSpawnLocation.transform.position;
        yield return new WaitForSeconds(1.5f);
        character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 180.0f, character.transform.rotation.z);
        StartCoroutine(character.GetComponent<CharacterBase>().MoveBackward());
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

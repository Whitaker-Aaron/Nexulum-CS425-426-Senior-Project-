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
    [SerializeField] SceneAudio sceneAudio;
    [SerializeField] public string beginningTrack;
    [SerializeField] FloorInformation floorInfo;
    AudioManager audioManager;
    RoomManager roomManager;
    CharacterBase characterRef;
    [SerializeField] public Vector3 playerSpawnPos;
    [SerializeField] public GameObject initialSpawnLocation;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //audioManager.StopLoop();
        if(beginningTrack != "" && beginningTrack != null)
        {
            audioManager.ChangeTrack(beginningTrack);
        }
        if (!characterRef.teleporting && roomManager.currentRoom != null && roomManager.currentRoom.floorEntrance)
        {
            audioManager.PlaySFX("Bell");
        }

        
        
    }

    private void Awake()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        characterRef = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();

        if (sceneName == "Floor1") characterRef.progressionChecks.setHasVisitedDungeon(true);

        if (sceneName == "BaseCamp")
        {
            beginningRoom = new RoomInformation();
            beginningRoom.roomName = sceneName;
            beginningRoom.isCheckpoint = true;
            roomManager.SetRoom(beginningRoom);

        }
        else if (beginningRoom != null && !characterRef.teleporting) 
        {
            
            roomManager.SetRoom(beginningRoom);
        }
        else if(characterRef.teleporting) SetCurrentRoomFromTeleport();

        if (spawnPlayer && sceneName != "BaseCamp")
        {

            if (initialSpawnLocation != null && !characterRef.teleporting)
            {
                characterRef.transform.position = initialSpawnLocation.transform.position;
            }
            else if (characterRef.teleporting)
            {
                characterRef.transform.position = characterRef.teleportSpawnObject.spawnPosition;

            }
            else
            {
                characterRef.transform.position = playerSpawnPos;
            }
        }
        else if (sceneName == "BaseCamp" && characterRef.transitioningRoom)
        {

            StartCoroutine(WaitThenStartCharacterMove(characterRef.gameObject));
        }
        else if(sceneName == "BaseCamp" && !characterRef.transitioningRoom & spawnPlayer)
        {
            characterRef.transform.position = playerSpawnPos;

        }
        

        if (screenTransition) StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartScene());
    }

    public IEnumerator WaitThenStartCharacterMove(GameObject character)
    {
        character.transform.position = initialSpawnLocation.transform.position;
        var cameraBehavior = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        var directionOffset = new Vector3(0.0f, 0.0f, -7.0f);
        cameraBehavior.PauseFollow();
        cameraBehavior.transform.position = character.transform.position + cameraBehavior.offset + directionOffset;
        yield return new WaitForSeconds(1.5f);
        character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 180.0f, character.transform.rotation.z);
        StartCoroutine(character.GetComponent<CharacterBase>().MoveBackward());
        yield break;
    }

    public void SetCurrentRoomFromTeleport()
    {
        if(floorInfo != null)
        {
            var rooms = floorInfo.GetRooms();
            foreach (var room in rooms)
            {
                var info = room.GetComponent<RoomInformation>();
                if(info.roomName == characterRef.teleportSpawnObject.roomName)
                {
                    roomManager.SetRoom(info);
                    break;
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

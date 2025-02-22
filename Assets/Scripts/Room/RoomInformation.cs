using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomInformation : MonoBehaviour
{
    [SerializeField] public bool isCheckpoint;
    [SerializeField] public string roomName;
    [SerializeField] GameObject enemies;
    [SerializeField] bool lockYAxis = false;
    [SerializeField] public Vector3 roomSpawnPoint;
    List<GameObject> allEnemies = new List<GameObject>();
    public List<GameObject> allLockedDoors = new List<GameObject>();
    public List<GameObject> allEventTriggers = new List<GameObject>();
    public bool firstVisit = true;
    public bool floorEntrance = false;
    public bool requiredEnemyRoom = false;
    public RoomPersistenceData roomData;

    GameObject character; 
    // Start is called before the first frame update

    private void Awake()
    {
        //roomData = new RoomPersistenceData();
        character = GameObject.FindGameObjectWithTag("Player");
        

        //allEnemies = new GameObject[enemies.transform.childCount];

        for(int i =0; i < enemies.transform.childCount; i++)
        {
            allEnemies.Add(enemies.transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        if (lockYAxis)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yAxisLocked = true;
            Debug.Log("Locking camera y-axis");
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yAxisLocked = false;
            Debug.Log("Unlocking camera y-axis");
        }
        ActivateEnemyHealthBars();
    }

    private void OnDisable()
    {
        DeactivateEnemyHealthBars();
    }

    private void Update()
    {
        for(int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] == null)
            {
                Debug.Log("Removing enemy from list");
                allEnemies.RemoveAt(i);
                i--;
            }
            
        }
    }

    private void Start()
    {
        if (floorEntrance)
        {
            
            StartCoroutine(WaitThenStartCharacterMove());
        }
        //InitializeDoors();
    }

    public void InitializeDoors()
    {
        if(allLockedDoors != null && allLockedDoors.Count > 0 && roomData.lockedDoors != null)
        {
            foreach (var door in allLockedDoors)
            {
                var doorScript = door.GetComponent<Door>();
                if(doorScript != null)
                {
                    doorScript.SetRoomInfo(this);
                    if (!roomData.lockedDoors[doorScript.doorGuid])
                    {
                        doorScript.UnlockDoor();
                    }
                }
            }
        }
    }

    public void InitializeTriggers()
    {
        if (allEventTriggers != null && allEventTriggers.Count > 0 && roomData.eventTriggers != null)
        {
            foreach (var trigger in allEventTriggers)
            {
                var triggerScript = trigger.GetComponent<EventTrigger>();
                if (triggerScript != null)
                {
                    triggerScript.SetRoomInfo(this);
                    if (roomData.eventTriggers[triggerScript.triggerGuid])
                    {
                        triggerScript.DisableTrigger();
                    }
                }
            }
        }
    }

    public void UpdateDoorState(string guid, bool state)
    {
        if(roomData.lockedDoors != null)
        {
            roomData.lockedDoors[guid] = state;
        }
    }

    public void UpdateTriggerState(string guid, bool state)
    {
        if (roomData.eventTriggers != null)
        {
            roomData.eventTriggers[guid] = state;
        }
    }

    public IEnumerator WaitThenStartCharacterMove()
    {
        
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(character.GetComponent<CharacterBase>().MoveForward());
        yield break;
    }

    public void OnTransition()
    {
        //character.transform.position = startPos.transform.position;
    }

    public List<GameObject> GetEnemies()
    {
        return allEnemies;
    }

    public void DeactivateEnemyHealthBars()
    {
       for(int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                allEnemies[i].GetComponent<EnemyFrame>().DeactivateHealthBar();
            }
            else
            {
                break;
            }
        }
    }

    public void ActivateEnemyHealthBars()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                allEnemies[i].GetComponent<EnemyFrame>().ActivateHealthBar();
            }
            else
            {
                break;
            }
        }
    }
}

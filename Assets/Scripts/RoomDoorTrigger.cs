using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class RoomDoorTrigger : MonoBehaviourID, EventTrigger
{
    // Start is called before the first frame update
    //GameObject[] enemies;
    [SerializeField] public List<GameObject> controlledDoors;
    [SerializeField] public List<GameObject> controlledEnemies;
    [SerializeField] public string triggerGuid { get; set; }
    [SerializeField] public string guid;
    
    public RoomInformation roomInfo { get; set; }
    public bool hasTriggered  { get; set; }
    bool doorsTriggered = false;
    void Start()
    {
        //enemies = transform.GetComponent<RoomInformation>().GetEnemies();
    }

    void OnEnable()
    {
        if(roomInfo != null)
        {
            var enemies = roomInfo.GetEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null && enemies[i].GetComponent<EnemyFrame>().isMiniboss)
                {
                    enemies[i].GetComponent<EnemyLOS>().canTarget = false;
                }
            }
        }
        
    }

    // Update is called once per frame
    /*void Update()
    {
        var enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        if (enemies.Count > 0)
        {
            //Debug.Log("There are still enemies");
        }
        else
        {
            //Debug.Log("All enemies killed");
            if (!doorsTriggered)
            {
                doorsTriggered = true;
                StartCoroutine(LockDoors());
            }
        }
    }*/

    private void Awake()
    {
        triggerGuid = guid;
        hasTriggered = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(LockDoors());
        }
    }

    public IEnumerator LockDoors()
    {
        if (controlledDoors != null)
        {
            for (int i = 0; i < controlledDoors.Count; i++)
            {
                Door door = controlledDoors[i].GetComponent<Door>();
                if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
                {
                    Debug.Log("IS DOOR LOCKED: " + door.isOpen);
                    if (door.isOpen)
                    {
                        door.isLocked = true;
                        yield return StartCoroutine(PanToDoor(door.transform.position));
                        door.ToggleDoor(true);
                        var enemies = roomInfo.GetEnemies();
                        if (roomInfo.requiredEnemyRoom) GameObject.Find("UIManager").GetComponent<UIManager>().ActivateEnemiesRemainingUI(enemies.Count);
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().ChangeTrack("Battle1");
                        UnlockEnemies();
                    }

                }
                yield return null;
            }

        }
        hasTriggered = true;
        //UpdateTriggerState();
        //Destroy(this.gameObject);
        yield break;
    }

    public void UnlockEnemies()
    {
        var enemies = roomInfo.GetEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<EnemyLOS>().canTarget = true;
                enemies[i].GetComponent<enemyInt>().isActive = true;
            }
        }
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < controlledDoors.Count; i++)
        {
            Door door = controlledDoors[i].GetComponent<Door>();
            if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
            {
                if (!door.isOpen)
                {
                    door.isLocked = false;
                    door.ToggleDoor();
                }

            }
        }
    }

    public IEnumerator PanToDoor(Vector3 pos)
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        yield return new WaitForSeconds(0.25f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().StartPan(pos, true, true, 0.05f);
    }

    public void SetRoomInfo(RoomInformation roomInfo_)
    {
        roomInfo = roomInfo_;
    }

    public void DisableTrigger()
    {
        UnlockDoors();
        Destroy(this.gameObject);
    }

    public void UpdateTriggerState()
    {
        Debug.Log(triggerGuid);
        roomInfo.UpdateTriggerState(triggerGuid, hasTriggered);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    //GameObject[] enemies;
    [SerializeField] public List<GameObject> controlledDoors;
    [SerializeField] public List<GameObject> roomTriggerObjects;
    bool doorsTriggered = false;
    bool encounterComplete = true;
    int prevEnemyCount;
    UIManager uiManager;
    void Start()
    {
        //enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        var enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        for(int i =0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<enemyInt>().isActive = false;
        }
        
        //prevEnemyCount = 0;
    }

    private void OnEnable()
    {
        CheckTriggers();
    }

    // Update is called once per frame
    void Update()
    {
        
        var enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        
        if (enemies.Count > 0)
        {
            encounterComplete = false;
            //Debug.Log("There are still enemies");

        }
        else
        {
            //Debug.Log("All enemies killed");
            if(!doorsTriggered)
            {
                if (!encounterComplete)
                {
                    encounterComplete = true;
                    var audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                    audioManager.ChangeTrack(GameObject.Find("SceneInformation").GetComponent<SceneInformation>().beginningTrack);
                    audioManager.PlaySFX("BattleComplete");
                }
                doorsTriggered = true;
                StartCoroutine(OpenDoors());
            }
        }
        if (prevEnemyCount != enemies.Count)
        {
            bool uiUpdated = uiManager.UpdateEnemiesRemainingUI(enemies.Count);
            if (uiUpdated) prevEnemyCount = enemies.Count;
        }
    }

    public void CheckTriggers()
    {
        bool activeTriggerFound = false;
        Debug.Log("Room Trigger Count: " + roomTriggerObjects.Count);
        for (int i = 0; i < roomTriggerObjects.Count; i++)
        {
            if (roomTriggerObjects[i] == null)
            {
                Debug.Log("Room trigger found");
                activeTriggerFound = true;
            }
        }
        if(activeTriggerFound) DeactivateEnemies();
    }

    public IEnumerator OpenDoors()
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        if (controlledDoors != null)
        {
            character.invul = true;
            for(int i = 0; i < controlledDoors.Count; i++)
            {
                Door door = controlledDoors[i].GetComponent<Door>();
                if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
                {
                    door.isLocked = false;
                    if (!door.isOpen)
                    {
                        yield return StartCoroutine(PanToDoorAndOpen(door.transform.position, door));
                        
                    }

                }
                yield return null;
            }

            character.invul = false;
            DeactivateEnemies();
            DeleteRoomTriggers();
        }
        yield break;
    }

    public void DeactivateEnemies()
    {
        var enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        encounterComplete = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if (enemies[i].GetComponent<EnemyFrame>() != null) enemies[i].GetComponent<EnemyFrame>().removeHealth();
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                i--;
            }
        }
    }

    public void DeleteRoomTriggers()
    {
        for(int i = 0; i < roomTriggerObjects.Count; i++)
        {
            if (roomTriggerObjects[i] != null)
            {
                roomTriggerObjects[i].GetComponent<EventTrigger>().hasTriggered = true;
                roomTriggerObjects[i].GetComponent<EventTrigger>().UpdateTriggerState();
                Destroy(roomTriggerObjects[i]);
            }
        }
    }
    public IEnumerator PanToDoorAndOpen(Vector3 pos, Door door)
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //if(character.)
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        yield return new WaitForSeconds(0.25f);
        door.ToggleDoor();
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").
            GetComponent<CameraFollow>().StartMultiPan(pos, true, true, 0.05f));
    }
}

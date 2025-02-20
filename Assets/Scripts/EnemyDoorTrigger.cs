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
    void Start()
    {
        //enemies = transform.GetComponent<RoomInformation>().GetEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        var enemies = transform.GetComponent<RoomInformation>().GetEnemies();
        if (enemies.Count > 0)
        {
            //Debug.Log("There are still enemies");
        }
        else
        {
            //Debug.Log("All enemies killed");
            if(!doorsTriggered)
            {
                doorsTriggered = true;
                StartCoroutine(OpenDoors());
            }
        }
    }

    public IEnumerator OpenDoors()
    {
        if (controlledDoors != null)
        {
            for(int i = 0; i < controlledDoors.Count; i++)
            {
                Door door = controlledDoors[i].GetComponent<Door>();
                if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
                {
                    door.isLocked = false;
                    if (!door.isOpen)
                    {
                        yield return StartCoroutine(PanToDoor(door.transform.position));
                        door.ToggleDoor();
                    }

                }
                yield return null;
            }
            DeleteRoomTriggers();
        }
        yield break;
    }

    public void DeleteRoomTriggers()
    {
        for(int i = 0; i < roomTriggerObjects.Count; i++)
        {
            if (roomTriggerObjects[i] != null)
            {
                Destroy(roomTriggerObjects[i]);
            }
        }
    }
    public IEnumerator PanToDoor(Vector3 pos)
    {
        var character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        //if(character.)
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        yield return new WaitForSeconds(0.25f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().StartPan(pos, true, true, 0.05f);
    }
}

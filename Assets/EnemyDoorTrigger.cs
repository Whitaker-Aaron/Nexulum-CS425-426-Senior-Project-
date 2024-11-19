using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    //GameObject[] enemies;
    [SerializeField] public GameObject controlledDoor;
    bool doorTriggered = false;
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
            if(!doorTriggered)
            {
                doorTriggered = true;
                OpenDoor();
            }
        }
    }

    public void OpenDoor()
    {
        if (controlledDoor != null)
        {
            Door door = controlledDoor.GetComponent<Door>();
            if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
            {
                door.isLocked = false;
                if (!door.isOpen)
                {
                    StartCoroutine(PanToDoor(door.transform.position));
                    door.ToggleDoor();
                }

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

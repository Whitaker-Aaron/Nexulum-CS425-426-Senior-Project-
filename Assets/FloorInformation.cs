using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] RoomList;
    CharacterBase characterRef;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        characterRef = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();
    }

    private void Start()
    {
        InitailizeRoomDoors();
        StartCoroutine(DeactivateRooms());

    }

    public void InitailizeRoomDoors()
    {
        if (RoomList.Length > 0)
        {
            foreach (var room in RoomList)
            {
                room.GetComponent<RoomInformation>().InitializeDoors();
            }
        }
    }

    public GameObject[] GetRooms()
    {
        return RoomList;
    }

    public IEnumerator DeactivateRooms()
    {
        Debug.Log("Deactivating rooms");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < RoomList.Length; i++)
        {
            if (!RoomList[i].GetComponent<RoomInformation>().floorEntrance && !characterRef.teleporting)
            {
                //RoomList[i].GetComponent<RoomInformation>().DeactivateEnemyHealthBars();
                RoomList[i].SetActive(false);
            }
            else if (characterRef.teleporting && characterRef.teleportSpawnObject.roomName != RoomList[i].GetComponent<RoomInformation>().roomName)
            {
                RoomList[i].SetActive(false);
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Start is called before the first frame update

    public RoomInformation currentRoom;

    void Start()
    {
        
    }

    public RoomInformation GetRoom()
    {
        return currentRoom;
    }

    public void SetRoom(RoomInformation newRoom)
    {
        currentRoom = newRoom;
    }

    public bool IsCheckpoint()
    {
        Debug.Log("Is checkpoint: " + currentRoom.isCheckpoint);
        return currentRoom.isCheckpoint;
    }

    
}

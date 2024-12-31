using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomSaveData
{
    public string roomName;
    public List<LockedDoorSaveData> lockedDoors = new List<LockedDoorSaveData>();
    
}

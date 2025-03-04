using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomSaveData
{
    public string roomName;
    public bool hasVisited;
    public bool isCheckpoint;
    public List<LockedDoorSaveData> lockedDoors = new List<LockedDoorSaveData>();
    public List<EventTriggerSaveData> eventTriggers = new List<EventTriggerSaveData>();
    public List<CollectiblesSaveData> collectibles = new List<CollectiblesSaveData>();
    
}

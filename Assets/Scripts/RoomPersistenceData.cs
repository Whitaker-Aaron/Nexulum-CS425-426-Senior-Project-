using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "RoomPersistenceData", fileName = "newRoomSpawnObject")]
public class RoomPersistenceData : ScriptableObject
{
    [SerializedDictionary("DoorGuid", "IsLocked")]
    public string roomName;
    public SerializedDictionary<string, bool> lockedDoors;
    public SerializedDictionary<string, bool> eventTriggers;
}

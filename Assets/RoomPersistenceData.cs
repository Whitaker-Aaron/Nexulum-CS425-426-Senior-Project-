using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "RoomPersistenceData", fileName = "newRoomSpawnObject")]
public class RoomPersistenceData : ScriptableObject
{
    [SerializedDictionary("DoorGuid", "IsLocked")]
    public SerializedDictionary<string, bool> lockedDoors;
    
}

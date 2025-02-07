using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomSpawnObject", fileName = "newRoomSpawnObject")]
public class RoomSpawnObject : ScriptableObject
{
    [SerializeField] public string roomName;
    [SerializeField] public string stylizedRoomName;
    [SerializeField] public Vector3 spawnPosition;
    [SerializeField] public int sceneNum;
}

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour, SaveSystemInterface
{
    // Start is called before the first frame update

    public RoomInformation currentRoom;
    public List<RoomPersistenceData> allRoomData;
    public List<RoomPersistenceData> allVisitedRoomsData;
    public List<RoomSpawnObject> currentRoomsCanTravelTo;

    void Start()
    {
        
    }

    public RoomInformation GetRoom()
    {
        return currentRoom;
    }

    public List<RoomSpawnObject> GetCheckpoints()
    {
        return currentRoomsCanTravelTo;
    }

    public void SetRoom(RoomInformation newRoom)
    {
        currentRoom = newRoom;
        if (newRoom.roomData != null && !newRoom.roomData.hasVisited)
        {
            newRoom.roomData.hasVisited = true;
            allVisitedRoomsData.Add(newRoom.roomData);

            if (newRoom.isCheckpoint && newRoom.roomData.spawnObject != null)
            {
                newRoom.roomData.isCheckpoint = newRoom.isCheckpoint;
                currentRoomsCanTravelTo.Add(newRoom.roomData.spawnObject);
            }
        }
    }

    public void SaveData(ref SaveData data)
    {
        Debug.Log("Saving room data");
        if(allRoomData != null)
        {
            for (int i = 0; i < allRoomData.Count; i++)
            {
                var tempRoomData = new RoomSaveData();
                tempRoomData.roomName = allRoomData[i].roomName;
                tempRoomData.hasVisited = allRoomData[i].hasVisited;
                tempRoomData.isCheckpoint = allRoomData[i].isCheckpoint;
                if (allRoomData[i].lockedDoors != null && allRoomData[i].lockedDoors.Count > 0)
                {
                    foreach (var door in allRoomData[i].lockedDoors)
                    {
                        var tempLockedDoorData = new LockedDoorSaveData();
                        tempLockedDoorData.doorGuid = door.Key;
                        tempLockedDoorData.isLocked = door.Value;
                        tempRoomData.lockedDoors.Add(tempLockedDoorData);
                    }
                }
                if (allRoomData[i].eventTriggers != null && allRoomData[i].eventTriggers.Count > 0)
                {
                    foreach (var trigger in allRoomData[i].eventTriggers)
                    {
                        var tempEventTriggerData = new EventTriggerSaveData();
                        tempEventTriggerData.triggerGuid = trigger.Key;
                        tempEventTriggerData.hasTriggered = trigger.Value;
                        tempRoomData.eventTriggers.Add(tempEventTriggerData);
                    }
                }
                if (i+1 <= data.roomData.Count && data.roomData[i] != null && data.roomData[i].roomName == tempRoomData.roomName) data.roomData[i] = tempRoomData;
                else data.roomData.Add(tempRoomData);

            }
                
        }
    }

    public void LoadData(SaveData data)
    {
        if (data.isNewFile && allRoomData != null && allRoomData.Count > 0)
        {
            for(int i =0; i< allRoomData.Count; i++)
            {
                //INITIALIZE ALL LOCKED DOORS TO LOCKED
                if (allRoomData[i].lockedDoors != null && allRoomData[i].lockedDoors.Count > 0)
                {
                    foreach (var doorKey in allRoomData[i].lockedDoors.Keys.ToList())
                    {
                        var doors = allRoomData[i].lockedDoors;
                        doors[doorKey] = true;
                    }
                }
                //INITIALIZE ALL EVENT TRIGGERS TO UNTRIGGERED
                if (allRoomData[i].eventTriggers != null && allRoomData[i].eventTriggers.Count > 0)
                {
                    foreach (var triggerKey in allRoomData[i].eventTriggers.Keys.ToList())
                    {
                        var trigger = allRoomData[i].eventTriggers;
                        trigger[triggerKey] = false;
                    }
                }

                //INITIALIZE ALL COLLECTIBLES TO UNCOLLECTED
                if (allRoomData[i].collectibles != null && allRoomData[i].collectibles.Count > 0)
                {
                    foreach (var collectibleKey in allRoomData[i].collectibles.Keys.ToList())
                    {
                        var collectible = allRoomData[i].collectibles;
                        collectible[collectibleKey] = false;
                    }
                }
                allRoomData[i].hasVisited = false;
            }
        }
        else if(allRoomData != null && allRoomData.Count > 0)
        {
            for (int i = 0; i < allRoomData.Count; i++)
            {  
                    for(int j =0; j < data.roomData.Count; j++)
                    {
                        if (data.roomData[j].roomName == allRoomData[i].roomName)
                        {
                        allRoomData[i].hasVisited = data.roomData[j].hasVisited;
                        allRoomData[i].isCheckpoint = data.roomData[j].isCheckpoint;
                        if (allRoomData[i].hasVisited == true) allVisitedRoomsData.Add(allRoomData[i]);
                        if (allRoomData[i].hasVisited == true && 
                            allRoomData[i].isCheckpoint && allRoomData[i].spawnObject != null) currentRoomsCanTravelTo.Add(allRoomData[i].spawnObject);

                        //LOAD LOCKED DOORS FROM SAVE DATA 
                        if (data.roomData[j].lockedDoors != null && data.roomData[j].lockedDoors.Count > 0 && 
                                allRoomData[i].lockedDoors != null && allRoomData[i].lockedDoors.Count > 0)
                            {
                                foreach (var door in data.roomData[j].lockedDoors)
                                {
                                    allRoomData[i].lockedDoors[door.doorGuid] = door.isLocked;
                                }
                            }

                            //LOAD EVENT TRIGGERS FROM SAVE DATA 
                            if (data.roomData[j].eventTriggers != null && data.roomData[j].eventTriggers.Count > 0 && 
                                allRoomData[i].eventTriggers != null && allRoomData[i].eventTriggers.Count > 0)
                            {
                                foreach (var trigger in data.roomData[j].eventTriggers)
                                {
                                    allRoomData[i].eventTriggers[trigger.triggerGuid] = trigger.hasTriggered;
                                }
                            }

                        //LOAD EVENT COLLECTIBLES FROM SAVE DATA 
                        if (data.roomData[j].collectibles != null && data.roomData[j].collectibles.Count > 0 &&
                            allRoomData[i].collectibles != null && allRoomData[i].collectibles.Count > 0)
                        {
                            foreach (var collectible in data.roomData[j].collectibles)
                            {
                                allRoomData[i].collectibles[collectible.collectibleGuid] = collectible.hasCollected;
                            }
                        }
                        break;
                        }
                    else continue;
                }
           
            }
        }
    }

        public bool IsCheckpoint()
    {
        Debug.Log("Is checkpoint: " + currentRoom.isCheckpoint);
        return currentRoom.isCheckpoint;
    }

    
}

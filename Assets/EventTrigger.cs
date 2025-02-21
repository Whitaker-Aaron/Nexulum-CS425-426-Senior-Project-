using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EventTrigger 
{
    public RoomInformation roomInfo { get; set; }
    public string triggerGuid { get; set; }
    public bool hasTriggered { get; set; }


    public void SetRoomInfo(RoomInformation roomInfo_);

    public void DisableTrigger();

    public void UpdateTriggerState();
}

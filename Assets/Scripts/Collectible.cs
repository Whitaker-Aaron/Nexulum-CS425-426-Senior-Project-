using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Collectible
{

    public RoomInformation roomInfo { get; set; }
    public string collectibleGuid { get; set; }
    public bool hasCollected { get; set; }


    public void SetRoomInfo(RoomInformation roomInfo_);

    public void DisableCollectible();

    public void UpdateCollectible();
    
}

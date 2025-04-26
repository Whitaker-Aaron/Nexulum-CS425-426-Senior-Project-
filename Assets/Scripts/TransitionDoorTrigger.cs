using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject controlledDoor;
    [SerializeField] RoomInformation attachedRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && controlledDoor != null )
        {
            var characterTargetRoom = other.GetComponent<CharacterBase>().targetRoom;
            if(characterTargetRoom == attachedRoom) OpenDoor();
        }
        
    }

    public void OpenDoor()
    {
        if (controlledDoor != null && controlledDoor.GetComponent<Door>().isLocked)
        {
            Door door = controlledDoor.GetComponent<Door>();
            if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
            {
                
                if (!door.isOpen && door.isLocked)
                {
                    Debug.Log("Inside transition door trigger");
                    door.isLocked = false;
                    door.ToggleDoor(true);
                }

            }
        }
    }
}

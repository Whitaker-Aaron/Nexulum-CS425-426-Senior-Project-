using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject controlledDoor;
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
        if(other.tag == "Player" && controlledDoor != null)
        {
            OpenDoor();
        }
        
    }

    public void OpenDoor()
    {
        if (controlledDoor != null)
        {
            Door door = controlledDoor.GetComponent<Door>();
            if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
            {
                door.isLocked = false;
                if (!door.isOpen)
                {
                    door.ToggleDoor();
                }

            }
        }
    }
}

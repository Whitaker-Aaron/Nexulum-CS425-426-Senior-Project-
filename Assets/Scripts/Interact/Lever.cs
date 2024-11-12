using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum LeverType { OneDoor, Multiple }

public class Lever : MonoBehaviour, i_Interactable
{
    [SerializeField] public LeverType type;
    public GameObject leverUI;

    [HideInInspector] public GameObject controlledDoor;  // Only used for OneDoor type
    [HideInInspector] public List<Door> doorList;        // Used only for Multiple type

    private Animator animator;
    private bool leverToggled;

    public void Start()
    {
        leverToggled = false;
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("No animator component found");
        }

        // Automatically assign doors based on LeverType at the start
        AssignDoors();
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Activating Lever");

        // Toggle lever animation and sound
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("LeverClick");
        leverToggled = !leverToggled;
        animator.SetBool("isToggled", leverToggled);

        if (type == LeverType.OneDoor)
        {
            if (controlledDoor != null)
            {
                Door door = controlledDoor.GetComponent<Door>();
                if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
                {
                    door.isLocked = false;
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("This lever cannot control the selected door type or no Door component found.");
                }
            }
            else
            {
                Debug.LogError("No controlled door assigned to this lever.");
            }
        }
        else if (type == LeverType.Multiple)
        {
            foreach (Door door in doorList)
            {
                if (door != null && (door.doorType == DoorType.Gate || door.doorType == DoorType.Wood))
                {
                    door.isLocked = false;
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("One or more doors cannot be controlled by this lever or are missing a Door component.");
                }
            }
        }

        return true;
    }

    public void ShowUI()
    {
        if (leverUI != null)
        {
            leverUI.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (leverUI != null)
        {
            leverUI.SetActive(false);
        }
    }

    // Method to automatically assign doors based on LeverType
    private void AssignDoors()
    {
        if (type == LeverType.OneDoor)
        {
            if (controlledDoor != null)
            {
                AddDoor(controlledDoor.GetComponent<Door>());
            }
            else
            {
                Debug.LogWarning("No controlled door assigned for OneDoor type lever.");
            }
        }
        else if (type == LeverType.Multiple)
        {
            foreach (Door door in doorList)
            {
                AddDoor(door);
            }
        }
    }

    // Method to add a door based on lever type
    public void AddDoor(Door door)
    {
        if (door == null)
        {
            Debug.LogWarning("Attempted to add a null door.");
            return;
        }

        if (type == LeverType.OneDoor)
        {
            controlledDoor = door.gameObject;
        }
        else if (type == LeverType.Multiple)
        {
            if (!doorList.Contains(door))
            {
                doorList.Add(door);
            }
        }
    }
}

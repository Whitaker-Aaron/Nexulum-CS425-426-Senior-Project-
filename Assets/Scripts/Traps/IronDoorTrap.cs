using System.Collections;
using UnityEngine;

public class IronDoorTrap : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float openSpeed = 0.5f;
    public float closeSpeed = 5f;

    [Header("Timing Settings")]
    public float openDelay = 1f;
    public float closeDelay = 5f;

    [Header("Damage Trigger Settings")]

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        StartCoroutine(DoorControlLoop());
    }

    private void Update()
    {
        float currentSpeed = isOpen ? openSpeed : closeSpeed;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, isOpen ? openRotation : closedRotation, Time.deltaTime * currentSpeed);
    }

    private IEnumerator DoorControlLoop()
    {
        while (true)
        {
            isOpen = true;
            yield return new WaitForSeconds(closeDelay);

            isOpen = false;
            yield return new WaitForSeconds(openDelay);
        }
    }

}

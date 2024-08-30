using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class lookAtCursor : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    private Camera mainCamera;

    //public GameObject pauseMenu;

    public bool run = false;


    private void Start()
    {
        // Cache the camera, Camera.main is an expensive operation.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (run == true)
            Aim();
        else
            return;
    }

    public void startAndStop(bool choice)
    {
        run = choice;
    }

    public Vector3 returnAim()
    {
        return transform.forward;
    }


    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }
}


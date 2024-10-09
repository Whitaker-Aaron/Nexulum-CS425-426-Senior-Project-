using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneAnimation : MonoBehaviour
{
    public Transform droneBody; // The drone model's transform
    public droneFollow movementScript; // Reference to the movement script
    private Animator animator; // Animator for the drone's tilt animations
    private Vector3 lastDronePosition; // Last known position for calculating velocity

    public float movementSmoothing = 0.1f; // Smoothing factor for animation
    public float idleThreshold = 0.1f; // Threshold for detecting if the drone is idle

    private float forwardAmount = 0f; // Smoothed forward value
    private float turnAmount = 0f; // Smoothed turn value

    private void Awake()
    {
        //lastDronePosition = droneBody.position;
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        AnimateDrone(movement);
    }

    void AnimateDrone(Vector3 movement)
    {
        Vector3 localMove = droneBody.gameObject.transform.InverseTransformDirection(movement);
        turnAmount = localMove.z;
        forwardAmount = localMove.x;

        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
}

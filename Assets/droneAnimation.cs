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
        lastDronePosition = droneBody.position;
        animator = droneBody.GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateDrone();
    }

    void AnimateDrone()
    {
        // Calculate the drone's movement direction in local space
        Vector3 droneMovementDirection = droneBody.InverseTransformDirection((droneBody.position - lastDronePosition) / Time.deltaTime);
        lastDronePosition = droneBody.position;

        // Calculate forward and turn amounts
        float targetForward = droneMovementDirection.z; // Use local Z axis for forward/backward movement
        float targetTurn = droneMovementDirection.x; // Use local X axis for turning left/right

        // Smooth the forward and turn values for a smoother transition
        forwardAmount = Mathf.Lerp(forwardAmount, targetForward, movementSmoothing);
        turnAmount = Mathf.Lerp(turnAmount, targetTurn, movementSmoothing);

        // Update animator parameters with smoothed values
        animator.SetFloat("Forward", forwardAmount, movementSmoothing, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, movementSmoothing, Time.deltaTime);

        // Determine if the drone is moving based on the magnitude of its local velocity
        bool isMoving = droneMovementDirection.magnitude > idleThreshold;

        // Update the animator with the isMoving state
        animator.SetBool("isMoving", isMoving);
    }
}

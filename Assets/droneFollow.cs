using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneFollow : MonoBehaviour
{
    Transform player; // The player transform
    public float followSpeed = 2f; // Speed at which the drone follows the player
    public Vector3 followOffset = new Vector3(3f, 3f, 3f); // The offset relative to the player
    public float rotationSpeed = 5f; // Speed at which the drone rotates

    private Vector3 lastPlayerPosition; // Used to calculate player movement direction
    private Vector3 playerMovementDirection; // Current movement direction of the player

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        FollowPlayer();
        SmoothLookAtPlayerMovement();
    }

    void FollowPlayer()
    {
        // Calculate the target position with the offset
        Vector3 targetPosition = player.position + followOffset;

        // Smoothly move the parent towards the player's position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void SmoothLookAtPlayerMovement()
    {
        // Calculate player movement direction
        playerMovementDirection = (player.position - lastPlayerPosition).normalized;

        // Only rotate the drone if the player is moving
        if (playerMovementDirection.magnitude > 0.1f)
        {
            // Only rotate on the Y-axis (to avoid messing up tilt animation)
            Vector3 flatDirection = new Vector3(playerMovementDirection.x, 0, playerMovementDirection.z);

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);

            // Smoothly rotate towards the player's movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        lastPlayerPosition = player.position; // Update the last player position
    }

    // Return the player's movement direction (for use in the animation script)
    public Vector3 GetPlayerMovementDirection()
    {
        return playerMovementDirection;
    }
}

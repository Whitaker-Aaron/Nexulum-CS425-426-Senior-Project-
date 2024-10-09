using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneFollow : MonoBehaviour
{
    Transform player; // The player transform
    public float followSpeed = 2f; // Speed at which the drone follows the player
    public Vector3 followOffset = new Vector3(3f, 3f, 3f); // The offset relative to the player
    public float maxDistance = 2f;
    public float rotationSpeed = 5f; // Speed at which the drone rotates

    private Vector3 lastPlayerPosition; // Used to calculate player movement direction
    private Vector3 playerMovementDirection; // Current movement direction of the player
    public bool isAiming = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerPosition = player.position;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        //SmoothLookAtPlayerMovement();
    }

    void FollowPlayer()
    {
        // Calculate the target position with the offset
        //Vector3 temp = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
        //Vector3 temp2 = new Vector3(player.transform.position.x, 1, player.transform.position.z);
        //float distance = Vector3.Distance(temp, temp2);

       // if(distance > maxDistance)
        //{
            //Vector3 dir = 
       // }
        //Transform pos = player.transform.InverseTransformDirection();


        Vector3 targetPosition = player.position + player.right * followOffset.x + player.up * followOffset.y + player.forward * followOffset.z;

        // Check if drone is farther than the follow distance
        if (Vector3.Distance(transform.position, player.position) > maxDistance || transform.position != targetPosition)
        {
            // Move the drone towards the target position smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            
        }
        // Calculate the direction to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Slow down the rotation with Slerp
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        if (isAiming)
            return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


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

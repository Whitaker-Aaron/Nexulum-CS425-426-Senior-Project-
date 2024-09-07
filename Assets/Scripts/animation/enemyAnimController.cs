using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnimController : MonoBehaviour
{

    public Transform target; // The target (e.g., player or destination) the enemy is moving towards

    private enemyAnimInterface animInt;
    private Vector3 previousPosition;
    private float previousRotationY;

    // Start is called before the first frame update
    void Start()
    {
        // Get the animation controller attached to this object
        animInt = GetComponent<enemyAnimInterface>();

        // Initialize previous position and rotation
        previousPosition = transform.position;
        previousRotationY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = (currentPosition - previousPosition).normalized;
        previousPosition = currentPosition;

        // Calculate forward amount (relative to object's forward direction)
        float forward = Vector3.Dot(movementDirection, transform.forward);

        // Calculate turning amount (how much the enemy is turning)
        float currentRotationY = transform.eulerAngles.y;
        float turn = Mathf.DeltaAngle(previousRotationY, currentRotationY) / Time.deltaTime;
        previousRotationY = currentRotationY;

        // Pass the calculated values to the animation controller
        animInt.SetMovement(forward, turn);
    }
}

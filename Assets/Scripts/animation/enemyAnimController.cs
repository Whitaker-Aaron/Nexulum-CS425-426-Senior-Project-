using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAnimController : MonoBehaviour, EnemyAnimation
{
    /*
    public Transform target; // The target (e.g., player or destination) the enemy is moving towards

    private enemyAnimInterface animInt;
    private Vector3 previousPosition;
    private float previousRotationY;
    private Animator animator;

    // Smoothing variables
    private float forwardVelocity = 0.0f;  // SmoothDamp velocity for forward value
    private float turnVelocity = 0.0f;     // SmoothDamp velocity for turn value
    public float smoothingTime = 0.1f;     // Time taken to smooth forward and turn values
    */
    private Animator animator;
    private Transform enemyTransform;

    private int forwardHash = Animator.StringToHash("Forward");
    private int turnHash = Animator.StringToHash("Turn");

    public void updateAnimation(Vector3 movementDirection)
    {
        if(movementDirection.magnitude > 1)
        {
            movementDirection.Normalize();
        }
        //converts from world space to local space
        Vector3 localDir = enemyTransform.InverseTransformDirection(movementDirection);
        float forwardAmount = localDir.x;
        float turnAmount = localDir.z;

        animator.SetFloat(forwardHash, forwardAmount);
        animator.SetFloat(turnHash, turnAmount);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyTransform = GetComponent<Transform>();
        /*
        // Get the animation controller attached to this object
        animInt = GetComponent<enemyAnimInterface>();
        animator = GetComponent<Animator>();

        // Initialize previous position and rotation
        previousPosition = transform.position;
        previousRotationY = transform.eulerAngles.y;
        */
    }

    private void Update()
    {
        /*
        // Calculate forward velocity based on movement direction
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = (currentPosition - previousPosition).normalized;
        previousPosition = currentPosition;

        // Calculate forward amount (relative to object's forward direction)
        float targetForward = Vector3.Dot(movementDirection, transform.forward);

        // Calculate turning amount (how much the enemy is turning)
        float currentRotationY = transform.eulerAngles.y;
        float targetTurn = Mathf.DeltaAngle(previousRotationY, currentRotationY) / Time.deltaTime;
        previousRotationY = currentRotationY;

        // Smoothly update the forward and turn values
        float forward = Mathf.SmoothDamp(animator.GetFloat("Forward"), targetForward, ref forwardVelocity, smoothingTime);
        float turn = Mathf.SmoothDamp(animator.GetFloat("Turn"), targetTurn, ref turnVelocity, smoothingTime);

        // Pass the smoothed values to the animation controller
        animInt.SetMovement(forward, turn);
        */
    }


}

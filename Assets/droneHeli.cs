using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneHeli : MonoBehaviour
{
    //MOVEMENT-------------------------
    
    Transform droneBody;
    Transform player;

    [Header("Movement settings")]
    public float followDistanceZ = 3f;
    public float followDistanceX = 3f;
    public float followHeight = 3f;
    public float followSpeed = 2f;

    Vector3 offset;
    

    //---------------------------------

    //TARGETING------------------------

    [Header("Targeting")]
    public Transform missileSpawn;
    public Transform bulletSpawn;
    public float fireRate;
    public float missileFireRate;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float detectionRange;
    public LayerMask enemy;
    public float bulletSpeed;
    bool shooting = false;

    Collider getClosestEnemy(Collider[] enemies)
    {
        Collider closest = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider c in enemies)
        {
            float distance = Vector3.Distance(transform.position, c.transform.position);
            if(distance < closestDistance)
            {
                closest = c;
                closestDistance = distance;
            }
        }
        return closest;
    }

    IEnumerator shoot()
    {
        shooting = true;
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * bulletSpeed;
        yield return new WaitForSeconds(fireRate);
        shooting = false;
        yield break;
    }

    //---------------------------------

    //ANIMATION------------------------

    [Header("Animation")]
    public float forwardAmount;
    public float turnAmount;
    public float rotationSpeed = 3f;
    public float tiltAmount = 20f;
    public GameObject animObj;
    Animator animator;
    Vector3 movement, lastPos;

    public void updateAnimation(Vector3 movementDirection)
    {
        Move(movementDirection);

        if (animator != null)
        {
            bool isMoving = movementDirection.magnitude > 0;
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void Move(Vector3 moving)
    {
        if (moving.magnitude > 1)
        {
            moving.Normalize();
        }

        this.movement = moving;

        convertMoveInput();
        updateAnimator();
    }

    void convertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(movement);
        turnAmount = localMove.z;

        forwardAmount = localMove.x;
    }
    void updateAnimator()
    {
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

        if (forwardAmount > 0 || turnAmount > 0)
        {
            animator.SetBool("isMoving", true); // Ensure moving animations
        }
        else
        {
            animator.SetBool("isMoving", false); // Stop movement animations
        }
    }

    void smoothLook(Transform target)
    {
        //Vector3 direction = (target.position - transform.position).normalized;
        //direction.y = 0f;
        //Quaternion targetRot = Quaternion.LookRotation(direction);
        //Debug.Log("Target rotation: " + targetRot.eulerAngles);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;  // Keep the y-axis at 0
        Quaternion targetRot = Quaternion.LookRotation(direction);

        

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        
    }


    //---------------------------------
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void Awake()
    {
        droneBody = gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(followDistanceX, followHeight, followDistanceZ);
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT------------------

        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        

        //--------------------------

        //TARGETING-----------------

        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, enemy);

        if(enemies.Length > 0 )
        {
            if (animator != null)
            {
                //animator.applyRootMotion = true;
            }
            Collider closestEnemy = getClosestEnemy(enemies);
            if(closestEnemy != null)
            {
                print("Looking at enemy");
                smoothLook(closestEnemy.transform);
                if(!shooting)
                    StartCoroutine(shoot());
            }
        }
        else
        {
            // No enemies, resume normal movement behavior
            animator.SetBool("isMoving", true);  // Ensure it stays moving when no enemies

            // Rotate back towards the player's movement direction (if necessary)
            Vector3 direction = (targetPos - transform.position).normalized;
            direction.y = 0f;  // Keep the Y-axis locked

            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // Enable root motion after resuming movement
            if (animator != null)
            {
                //animator.applyRootMotion = false;
            }
        }

        //--------------------------


        //ANIMATION-----------------


        // Calculate movement direction based on the change in position over time
        Vector3 movementDirection = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;

        // Update body tilt based on movement
        //applyTilt(movementDirection);

        // Always update animation based on movement
        updateAnimation(movementDirection);

        // Calculate movement direction
        //Vector3 movementDirection = (transform.position - lastPos) / Time.deltaTime;

        // Debug movement direction to track issues
        //Debug.Log("Movement Direction: " + movementDirection);

        // If there is significant movement, update last position and animate accordingly
        //if (movementDirection.magnitude > 0.01f)  // Check if it's moving
        //{
        //lastPos = transform.position;  // Update last position only when moving
        //Vector3 localMove = transform.InverseTransformDirection(movementDirection);
        // float vertical = Mathf.Clamp(movementDirection.z, -1f, 1f);
        //float horizontal = Mathf.Clamp(movementDirection.x, -1f, 1f);

        //movement = new Vector3(horizontal, 0, vertical);
        // updateAnimation(movementDirection);  // Continue updating the movement animation
        //}
        //else
        //{
        // If not moving, ensure animator reflects idle state
        // animator.SetBool("isMoving", false);
        //}

        //Vector3 movementDirection = (transform.position - lastPos) / Time.deltaTime;
        //lastPos = transform.position;

        //print(movement);
        //updateAnimation(movement);

        //--------------------------
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{
    public Rigidbody body;
    public float speed = 3;
    Vector2 move;
    public bool isMoving;
    public PlayerInputActions playerControl;

    public Animator animator;

    Vector3 lookPos, camForward, movement, moveInput;
    float forwardAmount, turnAmount;

    new Transform camera;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        playerControl = new PlayerInputActions();

        setupAnimator();
    }

    public void onMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");

        if (isMoving && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else if (!isMoving && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Move(Vector3 moving)
    {
        if (moving.magnitude > 1)
        {
            moving.Normalize();
        }

        this.moveInput = moving;

        convertMoveInput();
        updateAnimator();
    }

    void convertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.z;

        forwardAmount = localMove.x;
    }
    void updateAnimator()
    {
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    private void Start()
    {
        camera = Camera.main.transform;
    }


    private void FixedUpdate()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);


        if (camera != null)
        {
            camForward = Vector3.Scale(camera.up, new Vector3(1, 0, 1)).normalized;
            movement = vertical * camForward + horizontal * camera.right;
        }
        else
            movement = vertical * Vector3.forward + horizontal * Vector3.right;

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        Move(movement);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
            movePlayer();
        }
        else
        {
            isMoving = false;
        }

    }

    // Update is called once per frame
    void Update()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        animator.SetFloat("inputX", horizontal);
        animator.SetFloat("inputY", vertical);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //movePlayer();

        

        animator.SetFloat("inputX", horizontal);
        animator.SetFloat("inputY", vertical);



    }

   

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void setupAnimator()
    {
        animator = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animator)
            {
                animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
    

}


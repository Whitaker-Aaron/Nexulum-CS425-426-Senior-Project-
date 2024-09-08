/*
 * MASTER INPUT SCRIPT
 * Author: Spencer Garcia
 * Start Date: 9/7/2024
 * 
 * Description:
 * 
 * complete input manager for all player class types. Basic movement is all handled together for each class, 
 * but each class has its own set of inputs needed.
 * 
 * Checks logic from Character Base to collect the active player ID - NOTICE - need to update once implemented, using 
 * FindWithTag("Player") for now
 * 
 * 
 * Other Contributions:
 * Author - date - edit made
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class masterInput : MonoBehaviour
{
    //-------------VARIABLES------------

    //temporary player object - CHANGE TO CharacterBase ONCE IMPLEMENTED
    private GameObject player;
    [SerializeField] public WeaponBase.weaponClassTypes currentClass;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //basic general player movement
    public PlayerInputActions playerControl;
    private bool isAttacking = false;
    Vector2 move;
    Vector3 lookPos;
    public float speed = 3f;

    //animation variables
    private PlayerAnimation animationControl;
    Vector3 movement, camForward;
    new Transform camera;




    //--------------FUNCTIONS--------------


    //onMove is implemented through InputSystem in unity, context is the input
    public void onMove(InputAction.CallbackContext context)
    {
        if (isAttacking)
            return;
        move = context.ReadValue<Vector2>();
    }

    //actual player translation for FixedUpdate
    public void movePlayer()
    {
        if (isAttacking)
            return;
        Vector3 movement = new Vector3(move.x, 0, move.y);

        player.transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControl = new PlayerInputActions();
        animationControl = GetComponent<PlayerAnimation>();
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
            return;

        //player look at cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - player.transform.position;
        lookDir.y = 0;

        player.transform.LookAt(player.transform.position + lookDir, Vector3.up);
    }
    private void FixedUpdate()
    {
        if (isAttacking)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //universal player movement
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            movePlayer();
        }



        //animation

        movement = new Vector3(horizontal, 0, vertical);
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

        animationControl.updatePlayerAnimation(movement);
    }
}

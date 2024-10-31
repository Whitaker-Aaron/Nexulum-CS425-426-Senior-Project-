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
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class masterInput : MonoBehaviour
{
    //-------------VARIABLES------------

    private GameObject player;
    public WeaponBase.weaponClassTypes currentClass;

    

    //player
    CharacterBase character;
    GameObject projectedPlayer;

    //basic general player movement
    //public PlayerInputActions playerControl;
    private PlayerInput playerInput; 
    Vector2 move;
    Vector3 lookPos;
    Vector3 lookDir;
    Vector3 dashDistance;
    public float speed = 3f;
    bool isMoving = false;
    bool isDashing = false;
    bool isGamepadLooking = false;
    bool isMouseLooking = false;
    public bool characterColliding = false;
    public float minLookDistance = 1f;
    public LayerMask ground;

    bool stopVelocity = true;

    public bool abilityInUse = false;

    //animation variables
    private PlayerAnimation animationControl;
    Vector3 movement, camForward;
    new Transform camera;


    //Knight Combat Variables
    bool isAttacking = false;
    float cooldown = 1f;
    bool inputPaused = false;
    bool returningFromMenu = true;
    private static int noOfClicks = 0;
    private float lastClickedTime = 0;
    //[Header("Knight Variables")]
    public float cooldownTime = 2f;
    public float nextAttackTime = .3f;
    public float maxComboDelay = .7f;
    public float animTime = 0.5f;
    public float animTimeTwo = 0.5f;
    public float animTimeThree = 0.99f;
    GameObject sword;
    public float blockTime;
    public float blockSpeed;
    bool isBlocking = false;
    public Transform swordAttackPoint;
    public float swordAttackRadius;
    public LayerMask layer;
    public float dashSpeed = 3f;
    public float dashTime = .2f;

    GameObject staminaBar;
    GameObject staminaFill;
    GameObject staminaBorder; 

    float maxStaminaValue;

    public bool shootingSwords = false;

    public GameObject swordSlashPrefab, swordSlash2, swordSlash3;
    private GameObject SS1, SS2, SS3;



    //Gunner Variables

    //bullet
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    //combat
    bool isReloading = false;
    public float reloadTime = 2f;
    public float fireRateTime = .1f;
    bool canShoot = true;
    int bulletCount;
    public int magSize = 25;

    //rocket
    public bool shootingRocket = false;

    //laser
    public bool shootingLaser = false;

    //grenade
    public bool throwingGrenade = false;

    //RUNE VARS
    bool fireBullet = false;


    //Engineer variables

    //bullet
    public Transform pistolBulletSpawn;
    public GameObject pistolBulletObj;
    public float pistolBulletSpeed;


    //pistol combat
    bool pistolReloading = false;
    public float pistolReloadTime = 2f;
    public float pistolFireRateTime = .3f;
    bool canPistolShoot = true;
    int pistolBulletCount;
    public int pistolMagSize = 10;

    //melee combat
    public float engCooldown;
    public float engNextAttackTime = .3f;
    public float engMaxComboDelay = .7f;
    public float engAnimTime = 0.5f;
    public float engAnimTimeTwo = 0.5f;
    public float engAnimTimeThree = 0.99f;
    GameObject tool;
    GameObject pistol;

    public Transform toolAttackPoint;
    public float toolAttackRadius;

    //abilities
    public bool placing = false;

    //repair
    public bool canRepair = false;
    bool repairing = false;
    public float repairRate = 1f;
    GameObject repairObj;
    public int repairVal = 25;


    //--------------MAIN RUNNING FUNCTIONS--------------

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected on player");
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //dashCollision = true;
    }

   
    private void Awake()
    {
        

        staminaBar = GameObject.Find("StaminaBar");
        staminaFill = GameObject.Find("StaminaFill");
        staminaBorder = GameObject.Find("StaminaBorder");

        maxStaminaValue = staminaBar.GetComponent<Slider>().value;

        Vector4 staminaColor = staminaFill.GetComponent<Image>().color;
        staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, 0.0f);

        Vector4 staminaBorderFill = staminaBorder.GetComponent<Image>().color;
        staminaBorder.GetComponent<Image>().color = new Vector4(staminaBorderFill.x, staminaBorderFill.y, staminaBorderFill.z, 0.0f);

        playerInput = GetComponent<PlayerInput>();

    }

    private void OnEnable()
    {
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerControl = gameObject.GetComponent<PlayerInputActions>();

        SS1 = Instantiate(swordSlashPrefab);
        SS2 = Instantiate(swordSlash2);
        SS3 = Instantiate(swordSlash3);
        SS1.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        SS2.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        SS3.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        SS1.transform.SetParent(player.transform, false);
        SS2.transform.SetParent(player.transform, false);
        SS3.transform.SetParent(player.transform, false);
        SS1.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        SS2.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        SS3.transform.position = new Vector3(player.transform.position.x, .5f, player.transform.position.z);
        DontDestroyOnLoad(SS1);
        DontDestroyOnLoad(SS2);
        DontDestroyOnLoad(SS3);

        //playerControl = new PlayerInputActions();
        animationControl = GetComponent<PlayerAnimation>();
        camera = Camera.main.transform;

        character = player.GetComponent<CharacterBase>();
        projectedPlayer = player.transform.Find("ProjectedPlayer").gameObject;
        Debug.Log("Character: " + character.equippedWeapon.weaponClassType);
        currentClass = character.equippedWeapon.weaponClassType;
        Debug.Log("Character's current class from master input: " + currentClass);

        
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            sword = character.equippedWeapon.weaponMesh;
            swordAttackPoint = character.swordAttackPoint;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            pistol = character.equippedWeapon.weaponMesh;
            tool = character.engineerTool.weaponMesh;
            toolAttackPoint = character.toolAttackPoint;

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (stopVelocity)
            player.GetComponent<Rigidbody>().velocity = new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);


        //Vector2 lookInput = playerControl.player.mouseLook.ReadValue<Vector2>();

        
        //player look at cursor position
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, ground))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - player.transform.position;
        lookDir.y = 0;

        
        if(lookDir.magnitude > minLookDistance && !inputPaused)
            player.transform.LookAt(player.transform.position + lookDir, Vector3.up);
        else
            player.transform.LookAt(player.transform.position, Vector3.up);
        */



        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight))
            return;

        if (inputPaused) return;

        runLogic();

        returningFromMenu = false;
    }
    private void FixedUpdate()
    {

        player.transform.rotation = Quaternion.Euler(0.0f, player.transform.eulerAngles.y, 0.0f);
        



        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused)
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
        /*
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
        */
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (Camera.main != null)
        {
            // Get the camera's forward and right vectors, flattened on the XZ plane
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

            // Apply camera-relative movement, with possible offset correction
            movement = vertical * camForward + horizontal * camRight;
            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }
            animationControl.updatePlayerAnimation(movement);
            // Here we add an offset rotation to correct any small misalignment
            //Quaternion playerOffset = Quaternion.Euler(0, player.transform.eulerAngles.y - camera.transform.eulerAngles.y, 0);
            //movement = playerOffset * movement;
        }
        else
        {
            // Fallback to world space movement if no camera is available
            movement = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        if (movement.magnitude > 1)
        {
            movement.Normalize(); // Normalize to prevent faster diagonal movement
        }

        // Update player animation with the correct movement direction
        animationControl.updatePlayerAnimation(movement);
    }



    //--------------------USER DEFINED FUNCTIONS----------------------

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        if (inputPaused || isGamepadLooking)
            return;
        isMouseLooking = true;
        // Read mouse position input
        Vector2 mousePosition = context.ReadValue<Vector2>();

        // Perform raycast from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Raycast onto the ground
        if (Physics.Raycast(ray, out hit, 100, ground))
        {
            lookPos = hit.point; // Set look position to where the raycast hit the ground
        }

        // Calculate the direction to look at
        lookDir = Vector3.zero;
        if(player != null)
            lookDir = lookPos - player.transform.position;
        lookDir.y = 0;

        if (lookDir.magnitude > minLookDistance && !inputPaused)
        {
            player.transform.LookAt(player.transform.position + lookDir, Vector3.up); // Rotate towards the look direction
        }
        isMouseLooking = false;
    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        if (inputPaused || isMouseLooking)
            return;
        isGamepadLooking = true;

        // Get the right stick input from the gamepad
        Vector2 rightStickInput = context.ReadValue<Vector2>();

        if (rightStickInput != Vector2.zero)
        {
            // Use camera direction to calculate look direction
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Ignore Y axis to keep player level
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalize camera directions
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate look direction based on right stick input
            Vector3 lookDirection = (cameraRight * rightStickInput.x) + (cameraForward * rightStickInput.y);

            // Adjust the look position based on the right stick direction
            lookPos = player.transform.position + lookDirection * 10f; // Adjust distance as needed
        }

        // Calculate the direction to look at
        lookDir = lookPos - player.transform.position;
        lookDir.y = 0;

        if (lookDir.magnitude > minLookDistance && !inputPaused)
        {
            player.transform.LookAt(player.transform.position + lookDir, Vector3.up); // Rotate towards the look direction
            //player.transform.rotation = Quaternion.Euler(lookDir.)
        }
        isGamepadLooking = false;
    }

    //onMove is implemented through InputSystem in unity, context is the input
    public void onMove(InputAction.CallbackContext context)
    {
        if(inputPaused)//(isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused)
            return;
        move = context.ReadValue<Vector2>();
    }

    public void onDash(InputAction.CallbackContext context)
    {
        if (context.performed && isMoving && !characterColliding)
        {
            //dashStarted = true;
            if (!isDashing)
            {
                isDashing = true;
                dashSpeed = 3.0f;
                StartCoroutine(PlayerDash());
            }

        }
    }

    public void StopDash()
    {
        dashSpeed = 1.0f;
        isDashing = false;
    }



    //actual player translation for FixedUpdate
    public void movePlayer()
    {
        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused || (isAttacking && currentClass == WeaponBase.weaponClassTypes.Engineer))
            return;
        /*
        Vector3 movement = new Vector3(move.x, 0, move.y);

        if (movement.magnitude == 0)
            isMoving = false;
        else
            isMoving = true;

  
            if (currentClass == WeaponBase.weaponClassTypes.Knight && isBlocking)
                player.transform.Translate(movement * blockSpeed * Time.deltaTime, Space.World);
            else
                player.transform.Translate(movement * speed * dashSpeed * Time.deltaTime, Space.World);
        */
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        // Use input to move relative to camera's direction
        Vector3 movement = cameraForward * move.y + cameraRight * move.x;

        if (movement.magnitude == 0)
            isMoving = false;
        else
            isMoving = true;
        if (isDashing)
        {
            RaycastHit hit;
       

            projectedPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            projectedPlayer.transform.Translate(movement * speed * dashSpeed * Time.deltaTime, Space.World);
            // Does the ray intersect any objects excluding the player layer
            if(Physics.Linecast(player.transform.position, projectedPlayer.transform.position)){
                //Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Collision detected during dash");
                dashSpeed = 0.0f;
            }
        }

        // Apply movement based on class and whether the player is blocking
        if (currentClass == WeaponBase.weaponClassTypes.Knight && isBlocking)
        {
            player.transform.Translate(movement * blockSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            player.transform.Translate(movement * speed * dashSpeed * Time.deltaTime, Space.World);
        }



    }
    IEnumerator PlayerDash()
    {
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        dashSpeed = 1; 
        yield break;
        
    }

    public void pausePlayerInput()
    {
        inputPaused = true;
        animationControl.stop();

    }

    public void resumePlayerInput()
    {
        inputPaused = false;
        animationControl.stop();
    }



    //----------------Knight Functions------------------

    public void changeSword(WeaponBase newSword)
    {
        sword = newSword.weaponMesh;
    }
    IEnumerator wait(float animationTime)
    {
        //isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        if(currentClass == WeaponBase.weaponClassTypes.Knight)
            animationControl.resetKnight();
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
            animationControl.resetEngineer();
        //isAttacking = false;
        yield break;
    }

    IEnumerator waitAttack(float animationTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        //animationControl.resetKnight();
        isAttacking = false;
        yield break;
    }

    


    //--------------------Gunner functions-------------------

    IEnumerator shoot()
    {
        canShoot = false;
        while (playerInput.actions["Attack"].IsPressed() && bulletCount > 0 && isReloading == false)
        {
            bulletCount--;
            GameObject bullet = projectileManager.Instance.getProjectile("bulletPool", bulletSpawn.position, bulletSpawn.rotation); //Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            //if (fireBullet)

                //bullet.GetComponent<projectile>().fireGunnerRune();
            //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRateTime);
        }
        canShoot = true;
        yield break;
    }

    IEnumerator reload()
    {
        if (bulletCount == magSize)
            yield break;

        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletCount = magSize;
        isReloading = false;
        canShoot = true;
        yield break;
    }


    //----------------------Engineer Functions------------------------

    public void changeTool(WeaponBase newTool)
    {
        tool = newTool.weaponMesh;
    }

    IEnumerator pistolShoot()
    {
        canPistolShoot = false;
        while (playerInput.actions["Attack"].IsPressed() && pistolBulletCount > 0 && pistolReloading == false)
        {
            pistolBulletCount--;
            GameObject bullet = projectileManager.Instance.getProjectile("pistolPool", pistolBulletSpawn.position, pistolBulletSpawn.rotation);
            //bullet.GetComponent<Rigidbody>().velocity = pistolBulletSpawn.forward * pistolBulletSpeed;
            yield return new WaitForSeconds(pistolFireRateTime);
        }
        
        canPistolShoot = true;
        yield break;
    }

    IEnumerator pistolReload()
    {
        if (pistolBulletCount == pistolMagSize)
            yield break;
        else
        {
            canPistolShoot = false;
            pistolReloading = true;
            yield return new WaitForSeconds(pistolReloadTime);
            pistolBulletCount = pistolMagSize;
            pistolReloading = false;
            canPistolShoot = true;
        }
        yield break;
    }

    public void assignRepair(GameObject current)
    {
        repairObj = current;
    }

    void removeRepair()
    {
        repairObj = null;
    }

    IEnumerator repairWait()
    {
        if(repairObj != null)
        {
            print("repairing!");
            //repairing = false;
            repairObj.GetComponent<turretCombat>().repair(repairVal);
            yield return new WaitForSeconds(repairRate);
            //repairing = true;
            yield break;
        }
        else
            yield break;
    }

    //-----------------------------------------------------------------

    private void runLogic()
    {
        //KNIGHT LOGIC
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClicks = 0;
            }
            if (Time.time > lastClickedTime + nextAttackTime && isAttacking == false && !shootingSwords)//Time.time > cooldownTime && isAttacking == false)
            {
                if (playerInput.actions["attack"].triggered)
                {
                    print("click: " + noOfClicks);

                    lastClickedTime = Time.time;

                    noOfClicks++;
                    if (noOfClicks == 1)
                    {
                        
                        if (animationControl.getAnimationInfo().IsName("waitTwo") && animationControl.getAnimationInfo().normalizedTime > .99f)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        if (animationControl.getAnimationInfo().IsName("attackThree") && animationControl.getAnimationInfo().normalizedTime < animTimeThree)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer);
                        animationControl.knightAttackOne(animTime);
                        StartCoroutine(waitAttack(animTime * 2));
                        StartCoroutine(wait(animTime));
                        SS1.GetComponent<ParticleSystem>().Play();
                    }
                    noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

                    if (noOfClicks >= 2 && animationControl.getAnimationInfo().IsName("waitOne"))
                    {
                        
                        nextAttackTime = animTimeTwo;
                        sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer);
                        animationControl.knightAttackTwo(animTimeTwo);
                        StartCoroutine(wait(animTimeTwo));
                        StartCoroutine(waitAttack(animTimeTwo * 2));
                        SS2.GetComponent<ParticleSystem>().Play();
                    }

                    if (noOfClicks >= 3 && animationControl.getAnimationInfo().IsName("waitTwo"))
                    {
                        
                        nextAttackTime = animTimeThree;
                        noOfClicks = 0;
                        cooldownTime = Time.time + cooldown;
                        sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer);
                        animationControl.knightAttackThree();
                        StartCoroutine(wait(animTimeThree));
                        StartCoroutine(waitAttack(animTimeThree * 2));
                        nextAttackTime = animTime;
                        SS3.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        if (Time.time - lastClickedTime > maxComboDelay)
                            animationControl.resetKnight();
                    }
                }
            }

            if (playerInput.actions["RightClick"].triggered)
            {
                isBlocking = true;
                character.invul = true;

                Vector4 staminaColor = staminaFill.GetComponent<Image>().color;
                staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, 1.0f);

                Vector4 staminaBorderFill = staminaBorder.GetComponent<Image>().color;
                staminaBorder.GetComponent<Image>().color = new Vector4(staminaBorderFill.x, staminaBorderFill.y, staminaBorderFill.z, 0.51f);

                if (isMoving)
                    animationControl.blocking();
                else
                    StartCoroutine(animationControl.startKnightBlock(blockTime));
                StartCoroutine(StartStaminaCooldown());
            }
            if (playerInput.actions["RightClick"].WasReleasedThisFrame())
            {
                isBlocking = false;
                character.invul = false;

                if (isMoving)
                    StartCoroutine(animationControl.stopKnightBlock(0));
                else
                    StartCoroutine(animationControl.stopKnightBlock(blockTime));
            }

        }

        //GUNNER LOGIC
        if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingRocket && !shootingLaser && !throwingGrenade)
        {
            if (bulletCount <= 0 && !isReloading && bulletCount < magSize)
            {
                bulletCount = 0;
                canShoot = false;
                StartCoroutine(reload());
                animationControl.gunnerReload();
            }

            // Reload input check
            if (playerInput.actions["Reload"].triggered && bulletCount < magSize)
            {
                StartCoroutine(reload());
                animationControl.gunnerReload();
            }

            // Initialize shooting state
            bool shooting = false;

            // Check mouse input for shooting
            if (playerInput.actions["attack"].WasPressedThisFrame())
            {
                shooting = true;
            }
            else if (playerInput.actions["attack"].WasReleasedThisFrame())
            {
                shooting = false;
            }

            // Always check the trigger input regardless of the frame state
            float triggerValue = playerInput.actions["attack"].ReadValue<float>();
            //Debug.Log("Trigger Value: " + triggerValue); // Log the trigger value for debugging

            // Check if the trigger is pressed above the threshold
            if (triggerValue > 0.5f) // Adjust threshold if necessary
            {
                shooting = true;
            }

            // Check for shooting conditions
            if (shooting && !isReloading && bulletCount > 0 && canShoot)
            {
                //Debug.Log("Shooting..."); // Log when the shoot coroutine is called
                StartCoroutine(shoot());
            }
        }

        //Engineer Logic
        if (currentClass == WeaponBase.weaponClassTypes.Engineer && placing == false)
        {
            if (playerInput.actions["attack"].IsPressed() && pistolBulletCount <= 0 && !pistolReloading && pistolBulletCount < pistolMagSize && isAttacking == false)
            {
                pistolBulletCount = 0;
                canPistolShoot = false;
                StartCoroutine(pistolReload());
                animationControl.engineerReload();
            }

            if (playerInput.actions["Reload"].triggered && pistolBulletCount < pistolMagSize && !pistolReloading && isAttacking == false)
            {
                StartCoroutine(pistolReload());
                animationControl.engineerReload();
            }


            bool shooting = false;

            // Check mouse input for shooting
            if (playerInput.actions["attack"].WasPressedThisFrame())
            {
                shooting = true;
            }
            else if (playerInput.actions["attack"].WasReleasedThisFrame())
            {
                shooting = false;
            }

            // Always check the trigger input regardless of the frame state
            float triggerValue = playerInput.actions["attack"].ReadValue<float>();
            //Debug.Log("Trigger Value: " + triggerValue); // Log the trigger value for debugging

            // Check if the trigger is pressed above the threshold
            if (triggerValue > 0.5f) // Adjust threshold if necessary
            {
                shooting = true;
            }

            // Check for shooting conditions
            if (shooting && !pistolReloading && pistolBulletCount > 0 && canPistolShoot)
            {
                //Debug.Log("Shooting..."); // Log when the shoot coroutine is called
                StartCoroutine(pistolShoot());
            }

            //if (playerInput.actions["attack"].triggered && canPistolShoot && pistolBulletCount > 0 && isAttacking == false)
            //{
                //StartCoroutine(pistolShoot());
            //}

            if (canRepair)
            {
                if(Input.GetKeyDown(KeyCode.B) && !isAttacking && !pistolReloading)
                {
                    Debug.Log("Starting repair");
                    repairing = true;
                }
                if (repairing && Input.GetKeyUp(KeyCode.B))
                {
                    Debug.Log("Stop repair");
                    repairing = false;
                }
            }
            else
                removeRepair();

            if(repairing)
            {
                StartCoroutine(repairWait());
            }

            if (Time.time - lastClickedTime > engMaxComboDelay)
            {
                noOfClicks = 0;
            }
            if (Time.time > lastClickedTime + engNextAttackTime && isAttacking == false)// && Time.time > engCooldown)//&& isAttacking == false)
            {
                if (playerInput.actions["RightClick"].triggered)
                {
                    print("click: " + noOfClicks);

                    lastClickedTime = Time.time;

                    noOfClicks++;


                    if (noOfClicks == 1 && !animationControl.getAnimationInfo().IsName("engWaitTwo") && animationControl.getAnimationInfo().normalizedTime > engAnimTime)
                    {
                        if (animationControl.getAnimationInfo().IsName("engWaitTwo") && animationControl.getAnimationInfo().normalizedTime > .9f)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        if (animationControl.getAnimationInfo().IsName("engAttackThree") && animationControl.getAnimationInfo().normalizedTime > engAnimTimeThree)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        engNextAttackTime = engAnimTime;
                        StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(engAnimTime, swordAttackPoint, toolAttackRadius, layer));
                        animationControl.engAttackOne(engAnimTime);
                        StartCoroutine(waitAttack(engAnimTime * 2));
                        StartCoroutine(wait(engAnimTime));
                    }
                    noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

                    if (noOfClicks >= 2 && animationControl.getAnimationInfo().IsName("engWaitOne") && animationControl.getAnimationInfo().normalizedTime > engAnimTimeTwo * 2)
                    {
                        print("animate two");
                        engNextAttackTime = engAnimTimeTwo;
                        StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(engAnimTimeTwo, toolAttackPoint, toolAttackRadius, layer));
                        animationControl.engAttackTwo(engAnimTimeTwo);
                        StartCoroutine(wait(engAnimTimeTwo));
                        StartCoroutine(waitAttack(engAnimTimeTwo * 2));
                    }

                    if (noOfClicks >= 3 && animationControl.getAnimationInfo().IsName("engWaitTwo"))
                    {
                        print("animate three");
                        engNextAttackTime = engAnimTimeThree;
                        noOfClicks = 0;
                        engCooldown = Time.time + cooldown;
                        StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(engAnimTimeTwo, toolAttackPoint, toolAttackRadius, layer));
                        animationControl.engAttackThree();
                        StartCoroutine(wait(engAnimTimeThree));
                        StartCoroutine(waitAttack(engAnimTimeThree * 2));
                        engNextAttackTime = engAnimTime;

                    }
                    else
                    {
                        if (Time.time - lastClickedTime > engMaxComboDelay)
                            animationControl.resetEngineer();
                        if (noOfClicks >= 3)
                            noOfClicks = 0;
                    }

                }

            }



        }

        //Class ability Logic

        if (playerInput.actions["AbilityOne"].triggered && !abilityInUse)
        {
            abilityInUse = true;
            gameObject.GetComponent<classAbilties>().activateAbilityOne(currentClass);
            if(currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
            {
                StartCoroutine(abilityWait());
            }
            //StartCoroutine(abilityWait());
        }
        else if (playerInput.actions["AbilityTwo"].triggered && !abilityInUse)
        {
            abilityInUse = true;
            gameObject.GetComponent<classAbilties>().activateAbilityTwo(currentClass);
            //StartCoroutine(abilityWait());
            if (currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
            {
                StartCoroutine(abilityWait());
            }
        }
        else if (playerInput.actions["AbilityThree"].triggered && !abilityInUse)
        {
            abilityInUse = true;
            if (currentClass == WeaponBase.weaponClassTypes.Knight)
            {
                animationControl.knightShootSwords();
                shootingSwords = true;
            }
            gameObject.GetComponent<classAbilties>().activateAbilityThree(currentClass);
            //StartCoroutine(abilityWait());
            if (currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
            {
                StartCoroutine(abilityWait());
            }
        }
    }

    IEnumerator abilityWait()
    {
        yield return new WaitForSeconds(1f);
        abilityInUse = false;
        yield break;
    }


    private IEnumerator ReduceStaminaValue()
    {
        Slider slider = staminaBar.GetComponent<Slider>();
        while (true && slider != null)
        {
            slider.value -= 300 * Time.deltaTime;
            yield return null;

        }
        yield break;
    }

    private IEnumerator StartStaminaCooldown()
    {
        //IEnumerator coroutine = ReduceStaminaValue();
        //StartCoroutine("ReduceOpacity", scrollObj);
        //StartCoroutine(coroutine);
        //yield return new WaitForSeconds(10);
        //Debug.Log("Finished stamina coroutine");

        Slider slider = staminaBar.GetComponent<Slider>();
        while (slider.value > 0 && slider != null && isBlocking)
        {
            slider.value -= 300 * Time.deltaTime;
            yield return null;

        }

        if (isMoving)
            StartCoroutine(animationControl.stopKnightBlock(0));
        else
            StartCoroutine(animationControl.stopKnightBlock(blockTime));

        isBlocking = false;
        character.invul = false;

        StartCoroutine(RechargeStaminaBar());
        yield break;

    }

    private IEnumerator RechargeStaminaBar()
    {
        Slider slider = staminaBar.GetComponent<Slider>();
        while (slider.value < maxStaminaValue && slider != null && !isBlocking)
        {
            if (maxStaminaValue - slider.value < 10)
            {
                slider.value = maxStaminaValue;
                //Vector4 staminaColor = staminaFill.GetComponent<Image>().color;
                //staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, 0.0f);

                //Vector4 staminaBorderFill = staminaBorder.GetComponent<Image>().color;
                //staminaBorder.GetComponent<Image>().color = new Vector4(staminaBorderFill.x, staminaBorderFill.y, staminaBorderFill.z, 0.0f);
                StartCoroutine(ReduceStaminaOpacity());
            }
            else
            {
                slider.value += 300 * Time.deltaTime;
            }
            
            yield return null;
            
        }

        yield break;
    }

    private IEnumerator ReduceStaminaOpacity()
    {
        Vector4 staminaColor = staminaFill.GetComponent<Image>().color;
        Vector4 staminaBorderFill = staminaBorder.GetComponent<Image>().color;

        //staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, 0.0f);
        float reduceVal = 1f;
        while (staminaColor.w >= 0.0f && !isBlocking)
        {
            staminaColor = staminaFill.GetComponent<Image>().color;
            staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, (staminaColor.w - (reduceVal * Time.deltaTime)));

            staminaBorderFill = staminaBorder.GetComponent<Image>().color;
            staminaBorder.GetComponent<Image>().color = new Vector4(staminaBorderFill.x, staminaBorderFill.y, staminaBorderFill.z, (staminaBorderFill.w - (reduceVal * Time.deltaTime)));
            yield return null;
        }
        yield break;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(swordAttackPoint.position, swordAttackRadius);
        Gizmos.DrawWireSphere(toolAttackPoint.position, toolAttackRadius);
    }




    //-------------------RUNE FUNCTIONS----------------------

    public void activateFireRune(bool choice)
    {
        fireBullet = choice;
    }

    


}








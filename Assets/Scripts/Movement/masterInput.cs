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
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class masterInput : MonoBehaviour
{
    //-------------VARIABLES------------
    public static masterInput instance;

    private GameObject player;
    public WeaponBase.weaponClassTypes currentClass;
    



    //player
    CharacterBase character;
    GameObject projectedPlayer;
    private weaponType equippedWeapon;

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
    bool isGamepadActive= false;
    bool isKeyboardActive = false;
    public bool characterColliding = false;
    public float minLookDistance = 1f;
    public LayerMask ground;

    bool stopVelocity = true;

    public bool abilityInUse = false;

    bool usingSpellRunes = false;

    //animation variables
    private PlayerAnimation animationControl;
    Vector3 movement, camForward;
    new Transform camera;


    //Knight Combat Variables
    bool isAttacking = false;
    public bool bubble = false;
    float cooldown = 1f;
    public bool inputPaused = false;
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
    public float animHeavyTimeOne = 0.6f;
    public float animHeavyTimeTwo = 0.6f;
    GameObject sword;
    public float blockTime;
    public float blockSpeed;
    bool isBlocking = false;
    public Transform swordAttackPoint;
    public float swordAttackRadius;
    public LayerMask layer;
    public bool canDash = true;
    public float dashSpeed = 3f;
    public float dashTime = .2f;
    Coroutine dashCooldown;
    public float comboCooldown = 1.5f;

    GameObject staminaBar;
    GameObject staminaFill;
    GameObject staminaBorder;
    GameObject dashBar;

    float maxStaminaValue;
    float maxDashValue;

    public bool shootingSwords = false;

    public GameObject swordSlashPrefab, swordSlash2, swordSlash3;
    private GameObject SS1, SS2, SS3;//sword slash
    public GameObject ESP1, ESP2, ESP3;//engr slash prefab
    private GameObject ES1, ES2, ES3;
    private GameObject HS1, HS2, HS3;//heavy slash
    public GameObject HSP1, HSP2, HSP3;



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
    public float damageDropOffDistance = 5f;
    public int gunnerDmgMod;
    bool shooting = false;

    //rocket
    public bool shootingRocket = false;

    //laser
    public bool shootingLaser = false;
    public float shootingRange;

    //grenade
    public bool throwingGrenade = false;

    //RUNE VARS
    bool fireBullet = false;

    //line render
    private LineRenderer laserLine;
    bool pauseLaser = false;
    Color laserStartColor;


    //Engineer variables

    //bullet
    public Transform pistolBulletSpawn;
    public GameObject pistolBulletObj;
    public float pistolBulletSpeed;
    public int engrDmgMod;
    public float damageDropOffDistanceEngr = 8f;


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
    public bool spellsTriggered = false;
    GameObject[] towersToRepair;

    //repair
    public bool canRepair = false;
    bool repairing = false;
    public float repairRate = 1f;
    GameObject repairObj;
    public int repairVal = 25;

    UIManager uiManager;
    LifetimeManager lifetimeManager;
    AudioManager audioManager;


    //--------------MAIN RUNNING FUNCTIONS--------------

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected on player");
        //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //dashCollision = true;
    }

   
    private void Awake()
    {
        instance = this;

        staminaBar = GameObject.Find("StaminaBar");
        dashBar = GameObject.Find("DashBar");
        staminaFill = GameObject.Find("StaminaFill");
        staminaBorder = GameObject.Find("StaminaBorder");

        maxStaminaValue = staminaBar.GetComponent<Slider>().value;
        maxDashValue = dashBar.GetComponent<Slider>().value;

        Vector4 staminaColor = staminaFill.GetComponent<Image>().color;
        staminaFill.GetComponent<Image>().color = new Vector4(staminaColor.x, staminaColor.y, staminaColor.z, 0.0f);

        Vector4 staminaBorderFill = staminaBorder.GetComponent<Image>().color;
        staminaBorder.GetComponent<Image>().color = new Vector4(staminaBorderFill.x, staminaBorderFill.y, staminaBorderFill.z, 0.0f);

        playerInput = GetComponent<PlayerInput>();

        laserLine = gameObject.GetComponent<LineRenderer>();
        laserLine.enabled = false;
        laserStartColor = laserLine.startColor;

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        //laserLineRenderer.enabled = true;

        

    }

    private void OnEnable()
    {
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //playerControl = gameObject.GetComponent<PlayerInputActions>();
        towersToRepair = new GameObject[classAbilties.instance.turretMaxQuantity + classAbilties.instance.teslaMaxQuantity];

        activateSwordSlashes();
        //DontDestroyOnLoad(SS1);
        //DontDestroyOnLoad(SS2);
        //DontDestroyOnLoad(SS3);

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
            laserLine.enabled = true;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            tool = character.engineerTool.weaponMesh;
            toolAttackPoint = character.toolAttackPoint;
            laserLine.enabled = true;
        }
        equippedWeapon = character.equippedWeapon.weaponMesh.GetComponent<weaponType>();
        if(equippedWeapon != null)
            updateDistance(equippedWeapon.rangeModifier);
    }

    // Update is called once per frame
    void Update()
    {

        //if (stopVelocity)
            //player.GetComponent<Rigidbody>().velocity = new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);


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



        //if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight))
            //return;

        if (inputPaused) return;

        if (playerInput.actions["SwitchAbilities"].triggered)
        {
            if(!placing && !shootingLaser && !shootingRocket && 
                !throwingGrenade && !shootingSwords) spellsTriggered = !spellsTriggered;
            onSwitchToSpell();
            
        }

        if (animationControl != null)
            checkAnimationState();
        runLogic();

        returningFromMenu = false;
    }
    private void FixedUpdate()
    {
        bool playFootsteps = false;
        player.transform.rotation = Quaternion.Euler(0.0f, player.transform.eulerAngles.y, 0.0f);
        if (inputPaused)
        {
            if(!character.transitioningRoom) animationControl.updatePlayerAnimation(Vector3.zero);
            audioManager.PauseFootsteps("TestWalk");
            return;

        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        if (!character.transitioningRoom)  animationControl.updatePlayerAnimation(movement);

        
            

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //universal player movement
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            playFootsteps = true;
            movePlayer();
        }
        else
        {
            playFootsteps = false;
        }
        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight))
        {
            playFootsteps = false;
        }
        if(playFootsteps)
        {
            if(!audioManager.playingFootsteps && character.isTouchingGround && !isDashing) audioManager.PlayFootsteps("TestWalk");
        }
        else
        {
            audioManager.PauseFootsteps("TestWalk");
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
            if (!character.transitioningRoom) animationControl.updatePlayerAnimation(movement);
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
        if (!character.transitioningRoom) animationControl.updatePlayerAnimation(movement);
    }



    //--------------------USER DEFINED FUNCTIONS----------------------

    public PlayerAnimation GetAnimationControl()
    {
        return animationControl;
    }

    public bool getGamepadActive()
    {
        return isGamepadActive;
    }

    public void ActivateFallAnimation(){
        switch (currentClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                animationControl.falling("Knight");
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                animationControl.falling("Gunner");
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                animationControl.falling("Engineer");
                break;

        }
        
    }

    public void DisableFallAnimation()
    {
        switch (currentClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                animationControl.stopFall("Knight");
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                animationControl.stopFall("Gunner");
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                animationControl.stopFall("Engineer");
                break;

        }
    }

    public void OnActivateKeyboard(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isGamepadActive = false;
            isKeyboardActive = true;
        }
        
    }

    public void OnActivateGamepad(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isGamepadActive = true;
            isKeyboardActive = false;
        }
        
    }


    public void OnMouseLook(InputAction.CallbackContext context)
    {
        if (inputPaused)
            return;
        if (isGamepadLooking) isGamepadLooking = false;
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
        //isMouseLooking = false;
    }

    public void onSwitchToSpell()
    {
        if(shootingLaser || shootingRocket || shootingSwords || throwingGrenade ||placing)
        {
            print("cant switch, ability active");
            return;
        }
        uiManager.SwitchAbilityUI();
        int rCount = 0;
        foreach(Rune rune in character.equippedRunes)
        {
            if(rune != null)
            {
                if (rune.runeType == Rune.RuneType.Spell)
                    rCount++;
            }
            
            
        }
        if(rCount > 0)
        {
            //usingSpellRunes = !usingSpellRunes;
            print("usingSpellRunes = " + usingSpellRunes);
        }
        else
        {
            print("cant switch. no spell runes in inventory");
            return;
        }
        
    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        if (inputPaused)
            return;
        if (isMouseLooking) isMouseLooking = false;
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
            lookPos = player.transform.position + lookDirection * 5f; // Adjust distance as needed
        }

        // Calculate the direction to look at
        lookDir = lookPos - player.transform.position;
        lookDir.y = 0;

        if (lookDir.magnitude > minLookDistance && !inputPaused)
        {
            player.transform.LookAt(player.transform.position + lookDir, Vector3.up); // Rotate towards the look direction
            //player.transform.rotation = Quaternion.Euler(lookDir.)
        }
        //isGamepadLooking = false;
    }

    //onMove is implemented through InputSystem in unity, context is the input
    public void onMove(InputAction.CallbackContext context)
    {
        //if(inputPaused) return;//(isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused)
           
        move = context.ReadValue<Vector2>();
    }

    public void onDash(InputAction.CallbackContext context)
    {
        if (context.performed && isMoving && canDash && character.isTouchingGround && !characterColliding && !inputPaused)
        {
            //dashStarted = true;
            if (!isDashing)
            {
                audioManager.PauseFootsteps("TestWalk");
                audioManager.PlaySFX("Dash");
                EffectsManager.instance.getFromPool("playerDash", player.transform.position + new Vector3(0, .8f, 0), player.transform.rotation, true, false);
                uiManager.startBorderStretch();
                isDashing = true;
                dashSpeed = 4.5f;
                Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
                Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

                // Use input to move relative to camera's direction
                Vector3 movement = cameraForward * move.y + cameraRight * move.x;
                projectedPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                projectedPlayer.transform.Translate(movement * speed * 1.5f * dashSpeed * Time.deltaTime, Space.World);
                Vector3 targetDir = projectedPlayer.transform.position - player.transform.position;
                Debug.Log(targetDir);
                float angle = 0.0f;
                if (lifetimeManager.currentScene == "BaseCamp")
                {
                    angle = Vector3.Angle(targetDir, cameraForward + cameraRight);
                }
                else
                {
                    angle = Vector3.Angle(targetDir, cameraForward);
                }
                Debug.Log(angle);
                if (targetDir.x < 0) angle = -angle;
                StartCoroutine(uiManager.InstantiateSmear(angle, character.transform.position));
                StartCoroutine(PlayerDash());
            }

        }
    }

    public void StopDash()
    {
        dashSpeed = 1.0f;
        isDashing = false;
        uiManager.stopBorderStretch();
        //if (dashCooldown != null) StopCoroutine(dashCooldown);
        //dashCooldown = StartCoroutine(RechargeDashBar());
    }

    public void updateWeapon(weaponType newType)
    {
        print("updating weapon in MI");
        equippedWeapon = newType;
        StartCoroutine(updateWeaponWait());
        
    }

    IEnumerator updateWeaponWait()
    {
        yield return new WaitForSeconds(.3f);
        
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {
            print("calling reload in UWW");
            StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().equippedWeapon.weaponType.Reload());
            StartCoroutine(animationControl.gunnerReload(equippedWeapon.reloadTime));
            updateDistance(equippedWeapon.rangeModifier);
            yield break;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            StartCoroutine(equippedWeapon.Reload());
            StartCoroutine(animationControl.engineerReload(equippedWeapon.reloadTime));
            updateDistance(equippedWeapon.rangeModifier);
            yield break;
        }
        else
            yield break;
    }



    //actual player translation for FixedUpdate
    public void movePlayer()
    {
        //if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused || (isAttacking && currentClass == WeaponBase.weaponClassTypes.Engineer))
        //return
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
        //if(inputPaused && !isDashing) return;
        if (inputPaused) return;

        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        // Use input to move relative to camera's direction
        if (!isDashing)
        {
            movement = cameraForward * move.y + cameraRight * move.x;
        }

       
        

        if (movement.magnitude == 0)
            isMoving = false;
        else
            isMoving = true;
        if (isDashing)
        {
            RaycastHit hit;
       

            projectedPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            projectedPlayer.transform.Translate(movement * speed * 1.2f * dashSpeed * Time.deltaTime, Space.World);
            // Does the ray intersect any objects excluding the player layer
            if(Physics.Linecast(player.transform.position, projectedPlayer.transform.position, out RaycastHit hitInfo)){
                //Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow);
                if(hitInfo.collider.tag == "RestorePoint" || hitInfo.collider.tag == "MovingPlatform" || hitInfo.collider.tag == "Player" || hitInfo.collider.tag == "Trigger"){
                    Debug.Log(hitInfo.collider.tag);
                    Debug.Log("Ignoring collision");
                }
                else
                {
                    Debug.Log("Collision detected during dash: " + hitInfo.collider.name);
                    dashSpeed = 0.0f;
                }  
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
        //canDash = false;
        if (dashCooldown != null) StopCoroutine(dashCooldown);
        dashCooldown = StartCoroutine(RechargeDashBar());
        yield return new WaitForSeconds(0.2f);
        StopDash();
        //yield return new WaitForSeconds(0.15f);
        yield return new WaitForSeconds(0.25f);
        uiManager.DestroyOldestSmear();
        yield break;
        
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(0.3f);
        //canDash = true;
    }


    public void pausePlayerInput()
    {
        inputPaused = true;
        isMoving = false;
        isDashing = false;
        //uiManager.DestroyOldestSmear(true);
        //move = Vector2.zero;
        movement = Vector3.zero;
        animationControl.stop();

    }

    public void resumePlayerInput()
    {
        if (!character.inDialogueBox && !character.inEvent)
        {
            inputPaused = false;
            animationControl.stop();
        }
        
    }



    //----------------Knight Functions------------------

    public void changeSword(WeaponBase newSword)
    {
        sword = newSword.weaponMesh;
    }
    /*
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
    }*/

    /*
    IEnumerator wait(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        // Only reset if no further inputs were registered
        if (noOfClicks >= 0)
        {
            if (currentClass == WeaponBase.weaponClassTypes.Knight)
                animationControl.resetKnight();
            if (currentClass == WeaponBase.weaponClassTypes.Engineer)
                animationControl.resetEngineer();
        }
    }

    IEnumerator waitAttack(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        // Ensure isAttacking is only reset if no new input has occurred
        if (Time.time - lastClickedTime > engNextAttackTime)
        {
            isAttacking = false;
        }
    }*/

    

    


    //--------------------Gunner functions-------------------

    public void updateDistance(float modifier)
    {
        shootingRange = damageDropOffDistance * modifier;
    }

    void renderLine()
    {
        if(currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            //if(laserLine.enabled)
            laserLine.enabled = false;
            return;
        }
        else
        {
            if (equippedWeapon.isReloading || (isAttacking && animationControl.getAnimationInfo().normalizedTime < .99f) || (!animationControl.getAnimationInfo().IsName("Locomotion")))
            {
                laserLine.enabled = false;
                return;
            }

            if (!laserLine.enabled && pauseLaser == false)
                laserLine.enabled = true;

            //if(laserLine.enabled)
            if(currentClass == WeaponBase.weaponClassTypes.Gunner)
                laserLine.SetPosition(0, bulletSpawn.position);
            else
                laserLine.SetPosition(0, pistolBulletSpawn.position);
            //Debug.Log("rendering line with position: " + bulletSpawn);
            Ray ray;
            if (currentClass == WeaponBase.weaponClassTypes.Gunner)
                ray = new Ray(bulletSpawn.position, bulletSpawn.forward);
            else
                ray = new Ray(pistolBulletSpawn.position, pistolBulletSpawn.forward);
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("Default", "Enemy", "ground");

            if (Physics.Raycast(ray, out hit, 25f, layerMask))
            {
                //if (hit.point != null && laserLine.enabled)

                    laserLine.SetPosition(1, hit.point);


                if(hit.collider.gameObject.tag == "Enemy" && Vector3.Distance(player.transform.position, hit.point) > shootingRange)
                {
                    laserLine.startColor = Color.red;
                }
                else if(hit.collider.gameObject.tag == "Enemy" && Vector3.Distance(player.transform.position, hit.point) <= shootingRange)
                {
                    laserLine.startColor = Color.green;
                }
                else
                {
                    laserLine.startColor = laserStartColor;
                }
            }
            else
            {
                //if(laserLine.enabled)
                laserLine.startColor = laserStartColor;
                if (currentClass == WeaponBase.weaponClassTypes.Gunner)
                    laserLine.SetPosition(1, bulletSpawn.position + bulletSpawn.forward * 25f);
                else
                    laserLine.SetPosition(1, bulletSpawn.position + pistolBulletSpawn.forward * 25f);
            }
            //laserLine.startColor = Color.green;
            //laserLine.endColor = Color.green;
        }
        

    }

    /*IEnumerator shoot()
    {
        canShoot = false;
        while (playerInput.actions["Attack"].IsPressed() && bulletCount > 0 && isReloading == false)
        {
            audioManager.PlaySFX("Laser");
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

    void reload()
    {
        StartCoroutine(character.equippedWeapon.weaponMesh.GetComponent<weaponType>().Reload());

    }
    */

    //----------------------Engineer Functions------------------------
    /*IEnumerator waitShoot(float shootTime)
    {
        canPistolShoot = false;
        yield return new WaitForSeconds(shootTime);
        canPistolShoot = true;
        yield break;
    }*/
    public void changeTool(WeaponBase newTool)
    {
        tool = newTool.weaponMesh;
    }

    /*IEnumerator pistolShoot()
    {
        canPistolShoot = false;
        while (playerInput.actions["Attack"].IsPressed() && pistolBulletCount > 0 && pistolReloading == false && isAttacking == false)
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
        StartCoroutine(equippedWeapon.Reload());
        

        if (pistolBulletCount == pistolMagSize)
            yield break;
        else
        {
            canPistolShoot = false;
            pistolReloading = true;
            laserLine.enabled = false;
            pauseLaser = true;
            yield return new WaitForSeconds(pistolReloadTime);
            pistolBulletCount = pistolMagSize;
            pistolReloading = false;
            canPistolShoot = true;
            laserLine.enabled = true;
            pauseLaser = false;
        }
        yield break;
    }
    */

    public void assignRepair(GameObject current, int count)
    {
        canRepair = true;
        towersToRepair[count] = current;
    }

    public void unassignRepair(GameObject current, int count)
    {
        canRepair = false;
        towersToRepair[count] = null;
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
    /*
    private IEnumerator PerformAttack(float attackTime, GameObject effect, int attackStage)
    {
        effect.GetComponent<ParticleSystem>()?.Play();

        switch (attackStage)
        {
            case 1:
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, swordAttackPoint, toolAttackRadius, layer));
                animationControl.engAttackOne(attackTime);
                break;
            case 2:
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, toolAttackPoint, toolAttackRadius, layer));
                animationControl.engAttackTwo(attackTime);
                break;
            case 3:
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, toolAttackPoint, toolAttackRadius, layer));
                animationControl.engAttackThree();
                break;
        }

        StartCoroutine(wait(attackTime));        // Handles animation reset
        yield return StartCoroutine(waitAttack(attackTime * 2));  // Controls `isAttacking`
    }
    */
    public float attackTime = .5f;
    private bool canTrigger = true;

    private IEnumerator attackCooldown(float time)
    {
        canTrigger = false;
        yield return new WaitForSeconds(time);
        canTrigger = true;
    }

    private void checkAnimationState()
    {
        AnimatorStateInfo temp = animationControl.getAnimationInfo();
        if((temp.IsName("waitOne") || temp.IsName("waitTwo") || temp.IsName("waitThree")) && temp.normalizedTime >= 0.5f)
        {
            StartCoroutine(attackCooldown(0.25f));
        }
        if((temp.IsName("heavyWaitOne") || temp.IsName("heavyWaitTwo") || temp.IsName("heavyThree")) && temp.normalizedTime >= 0.7f)
        {
            StartCoroutine(attackCooldown(0.4f));
        }
    }

    private IEnumerator HandleComboAttack(float attackTime, int attackStage, bool isHeavy)
    {
        if (!canTrigger)
            yield break;
        AnimatorStateInfo temp = animationControl.getAnimationInfo();
        //effect.GetComponent<ParticleSystem>()?.Play();


        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            if (temp.IsName("engAttackOne") || temp.IsName("engAttackTwo") || temp.IsName("engAttackThree"))
                yield break;

            if (temp.IsName("Locomotion"))
            {
                animationControl.engAttackOne(animTime);
                audioManager.PlaySFX("Sword1");
                ES1.GetComponent<ParticleSystem>().Play();
                attackTime = engAnimTime;
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, toolAttackPoint, toolAttackRadius, layer));
            }
            if (temp.IsName("engWaitOne"))
            {
                animationControl.engAttackTwo(animTime);
                audioManager.PlaySFX("Sword2");
                ES2.GetComponent<ParticleSystem>().Play();
                attackTime = engAnimTimeTwo;
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, toolAttackPoint, toolAttackRadius, layer));
            }
            if (temp.IsName("engWaitTwo"))
            {
                animationControl.engAttackThree();
                audioManager.PlaySFX("Sword3");
                ES3.GetComponent<ParticleSystem>().Play();
                attackTime = engAnimTimeThree;
                StartCoroutine(tool.GetComponent<engineerTool>().activateAttack(attackTime, toolAttackPoint, toolAttackRadius, layer));
            }
            /*
            switch (attackStage)
            {
                case 1:
                    animationControl.engAttackOne(animTime);
                    ES1.GetComponent<ParticleSystem>().Play();
                    attackTime = engAnimTime;
                    break;
                case 2:
                    animationControl.engAttackTwo(animTime);
                    ES2.GetComponent<ParticleSystem>().Play();
                    attackTime = engAnimTimeTwo;
                    break;
                case 3:
                    animationControl.engAttackThree();
                    ES3.GetComponent<ParticleSystem>().Play();
                    attackTime = engAnimTimeThree;
                    break;
            }
            */
            StartCoroutine(wait(attackTime * 2));
            yield return StartCoroutine(waitAttack(attackTime * 2));
        }

        if (isHeavy && currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            if ((temp.IsName("heavyTwo") || temp.IsName("heavyOne")) && temp.normalizedTime <= .6f)
                yield break;
            //nextAttackTime = 0.6f;
            // Trigger Heavy Attack Animations and Effects
            temp = animationControl.getAnimationInfo();
            if (temp.IsName("Locomotion"))
            {
                animationControl.knightHeavyOne(animHeavyTimeOne);
                HS1.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword1");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, true, animHeavyTimeOne, 1));
            }
            if (temp.IsName("waitOne") || temp.IsName("heavyWaitOne"))
            {
                StopCoroutine(wait(attackStage));
                animationControl.knightHeavyTwo(animHeavyTimeTwo);
                HS2.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword2");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, true, animHeavyTimeOne, 2));
            }
            if (temp.IsName("waitTwo") || (temp.IsName("heavyWaitTwo") && temp.normalizedTime < .8f))
            {
                animationControl.knightHeavyThree();
                HS3.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword3");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, true, animHeavyTimeOne, 3));
            }
            /*
            switch (attackStage)
            {
                case 1:
                    animationControl.knightHeavyOne(animHeavyTimeOne);
                    HS1.GetComponent<ParticleSystem>().Play();
                    break;
                case 2:
                    animationControl.knightHeavyTwo(animHeavyTimeTwo);
                    HS1.GetComponent<ParticleSystem>().Play();
                    break;
                case 3:
                    animationControl.knightHeavyThree();
                    HS1.GetComponent<ParticleSystem>().Play();
                    break;
            }
            */
            
        }
        else
        {
            if (currentClass == WeaponBase.weaponClassTypes.Engineer)
                yield break;
            // Trigger Light Attack Animations and Effects
            temp = animationControl.getAnimationInfo();
            if (temp.IsName("Locomotion"))
            {
                animationControl.knightAttackOne(animTime);
                SS1.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword1");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, false, animTime, 1));
            }
            if (temp.IsName("waitOne") || temp.IsName("heavyWaitOne"))
            {
                animationControl.knightAttackTwo(animTimeTwo);
                SS2.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword2");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, false, animTime, 2));
            }
            if (temp.IsName("waitTwo") || (temp.IsName("heavyWaitTwo") && temp.normalizedTime < .8f))
            {
                animationControl.knightAttackThree();
                SS3.GetComponent<ParticleSystem>().Play();
                audioManager.PlaySFX("Sword3");
                StartCoroutine(sword.GetComponent<swordCombat>().activateAttack(swordAttackPoint, swordAttackRadius, layer, false, animTime, 3));
            }
            /*
            switch (attackStage)
            {
                case 1:
                    
                    animationControl.knightAttackOne(animTime);
                    SS1.GetComponent<ParticleSystem>().Play();
                    break;
                case 2:
                    animationControl.knightAttackTwo(animTimeTwo);
                    SS2.GetComponent<ParticleSystem>().Play();
                    break;
                case 3:
                    animationControl.knightAttackThree();
                    SS3.GetComponent<ParticleSystem>().Play();
                    break;
            }*/
            
        }

        // Wait for animation and reset logic
        StartCoroutine(wait(attackTime));
        yield return StartCoroutine(waitAttack(attackTime * 2));
    }

    private void runKnightAttackLogic()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > lastClickedTime + nextAttackTime && !isAttacking && !shootingSwords && !isBlocking)
        {
            if (playerInput.actions["attack"].triggered)
            {
                lastClickedTime = Time.time;
                noOfClicks++;

                // Start tracking attack hold time
                StartCoroutine(DetectHeavyAttack(noOfClicks));
            }
        }
    }

    private IEnumerator DetectHeavyAttack(int attackStage)
    {
        float pressStartTime = Time.time;
        bool isHeavy = false;

        // Wait while button is still being held, up to a threshold
        while (playerInput.actions["attack"].IsPressed())
        {
            if (Time.time - pressStartTime > 0.3f) // Adjust this value for heavy attack threshold
            {
                isHeavy = true;
                break;
            }
            yield return null; // Wait for next frame
        }

        // Execute attack after determining light or heavy
        StopCoroutine(wait(attackStage));
        StartCoroutine(HandleComboAttack(isHeavy ? animTime : animTimeTwo, attackStage, isHeavy));
    }


    bool cooling = false;
    IEnumerator lowerHeat()
    {
        if (shooting || cooling || character.equippedWeapon.weaponType == null || character.equippedWeapon.weaponType.isReloading)
            yield break;

        cooling = true;
        if (character.equippedWeapon.weaponType != null)
        {
            print("cooldownHeat in routine");
            yield return new WaitForSeconds(character.equippedWeapon.weaponType.cooldownRate);
            if (character.equippedWeapon.weaponType.currentHeat - character.equippedWeapon.weaponType.cooldownVal < 0)
            {
                character.equippedWeapon.weaponType.currentHeat = 0;
                cooling = false;
                yield break;
            }
            else
                character.equippedWeapon.weaponType.currentHeat -= character.equippedWeapon.weaponType.cooldownVal;

            cooling = false;
        }
    }

    private void runEngineerAttackLogic()
    {
        /*
        if ((playerInput.actions["RightClick"].WasPressedThisFrame() || playerInput.actions["RightClick"].IsPressed()) && !isAttacking)
        {
            isAttacking = true;

            int attackStage = noOfClicks + 1;

            if (attackStage <= 3)
            {
                // Handle Heavy Attack
                StartCoroutine(HandleComboAttack(animTime, attackStage, false));
            }
            // Reset after a delay
            //StartCoroutine(wait(attackTime));
        }*/

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > lastClickedTime + nextAttackTime && !isAttacking && !shooting)
        {
            if (playerInput.actions["RightClick"].triggered)
            {
                lastClickedTime = Time.time;
                noOfClicks++;

                // Start tracking attack hold time
                StartCoroutine(HandleComboAttack(0, noOfClicks, false));
            }
        }
    }

    // Combo Input Logic to Track Timing
    private IEnumerator wait(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0; // Reset Combo if no input in time
        }

        if (currentClass == WeaponBase.weaponClassTypes.Knight)
            animationControl.resetKnight();

        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
            animationControl.resetEngineer();
        if(noOfClicks >= 3 && (animationControl.getAnimationInfo().IsName("attackThree") || animationControl.getAnimationInfo().IsName("heavyThree")))
        {
            yield return new WaitForSeconds(comboCooldown);
            noOfClicks = 0;
            //isAttacking = false;
        }
        if(noOfClicks >= 3 && currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            yield return new WaitForSeconds(comboCooldown);
            noOfClicks = 0;
            //isAttacking = false;
        }
    }

    private IEnumerator waitAttack(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        // Reset attacking state if no new input occurred
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            isAttacking = false;
        }
    }





    private void runLogic()
    {
        if (bulletSpawn != null)
            renderLine();
        //KNIGHT LOGIC
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            runKnightAttackLogic();
            

            if (playerInput.actions["RightClick"].triggered && !isAttacking)// && animationControl.getAnimationInfo().IsName("Locomotion"))
            {
                isBlocking = true;
                isAttacking = false;
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
            if (playerInput.actions["RightClick"].WasReleasedThisFrame() && isBlocking)
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
            //if ((playerInput.actions["attack"].IsPressed() && equippedWeapon.bulletCount <= 0 && equippedWeapon.isReloading == false) || (playerInput.actions["Reload"].triggered && equippedWeapon.bulletCount < equippedWeapon.magSize && equippedWeapon.isReloading == false))//playerInput.actions["attack"].IsPressed() && pistolBulletCount <= 0 && !pistolReloading && pistolBulletCount < pistolMagSize && isAttacking == false && !repairing)


            //if (playerInput.actions["attack"].IsPressed() && equippedWeapon.isReloading == false)//playerInput.actions["attack"].IsPressed() && pistolBulletCount <= 0 && !pistolReloading && pistolBulletCount < pistolMagSize && isAttacking == false && !repairing)
            //{
                //pistolBulletCount = 0;
                //canPistolShoot = false;
                //StartCoroutine(equippedWeapon.Reload());
                //StartCoroutine(animationControl.gunnerReload(equippedWeapon.reloadTime));
            //}


            // Check mouse input for shooting
            if (playerInput.actions["attack"].WasPressedThisFrame())
            {
                
                shooting = true;
            }
            else if (playerInput.actions["attack"].WasReleasedThisFrame())
            {
                shooting = false;
            }

            float triggerValue = playerInput.actions["attack"].ReadValue<float>();
            //Debug.Log("Trigger Value: " + triggerValue);

            if (triggerValue > 0.5f && !isAttacking && (animationControl.getAnimationInfo().IsName("Locomotion"))) 
            {
                shooting = true;
                print("shhoting true");
            }

            if (shooting && !isReloading)
            {
                if(character.equippedWeapon.weaponType == null)
                    print("weapon is null in MI");
                    //return;
                if(character.equippedWeapon.weaponType.currentHeat >= character.equippedWeapon.weaponType.overHeatMax)
                {
                    shooting = false;
                    StartCoroutine(equippedWeapon.Reload());
                    StartCoroutine(animationControl.gunnerReload(equippedWeapon.reloadTime));
                    return;
                }
                print("Calling shoot in MI");
                StartCoroutine(character.equippedWeapon.weaponMesh.GetComponent<weaponType>().Shoot());
            }
            else
            {
                shooting = false;
                StartCoroutine(lowerHeat());
            }
        }

        //Engineer Logic
        if (currentClass == WeaponBase.weaponClassTypes.Engineer && placing == false)
        {
            if (playerInput.actions["attack"].IsPressed() && playerInput.actions["RightClick"].IsPressed())
            {
                //isAttacking = false;
                //StartCoroutine(waitAttack(.03f));
            }

            if (((playerInput.actions["attack"].IsPressed() && equippedWeapon.bulletCount <= 0) || (playerInput.actions["Reload"].triggered && equippedWeapon.bulletCount < equippedWeapon.magSize)) && equippedWeapon.canShoot && equippedWeapon.isReloading == false)//playerInput.actions["attack"].IsPressed() && pistolBulletCount <= 0 && !pistolReloading && pistolBulletCount < pistolMagSize && isAttacking == false && !repairing)
            {
                //StartCoroutine(equippedWeapon.Reload());
                //StartCoroutine(animationControl.engineerReload(equippedWeapon.reloadTime));

            }

            runEngineerAttackLogic();




            if (playerInput.actions["Attack"].WasPressedThisFrame() && !isAttacking)
            {
                shooting = true;
                //StartCoroutine(lowerHeat());
            }
            else if (playerInput.actions["Attack"].WasReleasedThisFrame())
            {
                shooting = false;
                //StartCoroutine(buildHeat());
            }

            float triggerValue = playerInput.actions["Attack"].ReadValue<float>();
            //Debug.Log("Trigger Value: " + triggerValue); 

            if (triggerValue > 0.5f && !isAttacking && (animationControl.getAnimationInfo().IsName("Locomotion")))
            {
                shooting = true;
            }

            if (shooting && !equippedWeapon.isReloading && isAttacking == false && !repairing && equippedWeapon.canShoot)
            {
                if (isAttacking)
                    return;
                if (character.equippedWeapon.weaponType.currentHeat >= character.equippedWeapon.weaponType.overHeatMax)
                {
                    shooting = false;
                    StartCoroutine(equippedWeapon.Reload());
                    StartCoroutine(animationControl.engineerReload(equippedWeapon.reloadTime));
                    return;
                }

                //StartCoroutine(pistolShoot());
                StartCoroutine(character.equippedWeapon.weaponType.Shoot());
            }
            else
            {
                StartCoroutine(lowerHeat());
            }


            if (canRepair)
            {
                if (playerInput.actions["Repair"].IsPressed() && !isAttacking && !pistolReloading && !shooting)
                {
                    Debug.Log("Starting repair");
                    repairing = true;
                }
                if (playerInput.actions["Repair"].WasReleasedThisFrame())
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

        }

        //Class ability Logic
        //if(!usingSpellRunes && !spellsTriggered)
        if (!spellsTriggered)
        {
            if (placing || shootingLaser || shootingRocket || throwingGrenade)
            {
                print("abiity in use cant use again");
                return;
            }
            //if(currentClass == WeaponBase.weaponClassTypes.Knight)
            if (playerInput.actions["AbilityOne"].triggered && !abilityInUse)
            {
                print("Using ability One");
                abilityInUse = true;

                gameObject.GetComponent<classAbilties>().activateAbilityOne(currentClass);

                if (currentClass == WeaponBase.weaponClassTypes.Knight)
                    StartCoroutine(abilityWait(classAbilties.instance.ka1Time - .05f));
                else if(currentClass == WeaponBase.weaponClassTypes.Gunner)
                    StartCoroutine(abilityWait(classAbilties.instance.ga1Time - .05f));
                else
                    StartCoroutine(abilityWait(classAbilties.instance.ea1Time - .05f));
                //StartCoroutine(abilityWait());
            }
            else if (playerInput.actions["AbilityTwo"].triggered && !abilityInUse)
            {
                print("Using ability Two");
                abilityInUse = true;
                gameObject.GetComponent<classAbilties>().activateAbilityTwo(currentClass);
                //StartCoroutine(abilityWait());
                if (currentClass == WeaponBase.weaponClassTypes.Knight)
                    StartCoroutine(abilityWait(classAbilties.instance.ka2Time - .05f));
                else if (currentClass == WeaponBase.weaponClassTypes.Gunner)
                    StartCoroutine(abilityWait(classAbilties.instance.ga2Time - .05f));
                else
                    StartCoroutine(abilityWait(classAbilties.instance.ea2Time - .05f));
            }
            else if (playerInput.actions["AbilityThree"].triggered && !abilityInUse)
            {
                print("Using ability Three");
                abilityInUse = true;
                if (currentClass == WeaponBase.weaponClassTypes.Knight && !classAbilties.instance.a3cooldown)
                {
                    //audioManager.PlaySFX("SwordShot");
                    animationControl.knightShootSwords();
                    shootingSwords = true;
                }
                gameObject.GetComponent<classAbilties>().activateAbilityThree(currentClass);
                //StartCoroutine(abilityWait());
                if (currentClass == WeaponBase.weaponClassTypes.Knight)
                    StartCoroutine(abilityWait(classAbilties.instance.ka3Time - .05f));
                else if (currentClass == WeaponBase.weaponClassTypes.Gunner)
                    StartCoroutine(abilityWait(classAbilties.instance.ga3Time - .05f));
                else
                    StartCoroutine(abilityWait(classAbilties.instance.ea3Time - .05f));
            }
        }
        else if(spellsTriggered)
        {
            if (placing || shootingLaser || shootingRocket || throwingGrenade || shootingSwords)
            {
                print("abiity in use cant use spell cast");
                return;
            }
            if (character.equippedRunes[0] != null && playerInput.actions["AbilityOne"].triggered && !abilityInUse && character.equippedRunes[0].runeType == Rune.RuneType.Spell)
            {
                print("Using spellCast One");
                abilityInUse = true;
                gameObject.GetComponent<spellCastManager>().activateSpellCast(character.equippedRunes[0], 1);
                uiManager.ActivateCooldownOnAbility(1, true);
                //StartCoroutine(abilityCooldown(1f, 1));
                //StartCoroutine(abilityWait());
                if (currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
                {
                    StartCoroutine(abilityWait(1));
                    
                }
            }
            else if (character.equippedRunes[1] != null && playerInput.actions["AbilityTwo"].triggered && !abilityInUse && character.equippedRunes[1].runeType == Rune.RuneType.Spell)
            {
                print("Using spellCast Two");
                abilityInUse = true;
                gameObject.GetComponent<spellCastManager>().activateSpellCast(character.equippedRunes[1], 2);
                uiManager.ActivateCooldownOnAbility(2, true);
                //StartCoroutine(abilityCooldown(1f, 2));
                //StartCoroutine(abilityWait());
                if (currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
                {
                    StartCoroutine(abilityWait(1));
                    
                }
            }
            else if (character.equippedRunes[2] != null && playerInput.actions["AbilityThree"].triggered && !abilityInUse && character.equippedRunes[2].runeType == Rune.RuneType.Spell)
            {
                print("Using spellCast Three");
                abilityInUse = true;
                if (currentClass == WeaponBase.weaponClassTypes.Knight)
                {
                    //animationControl.knightShootSwords();
                    //shootingSwords = true;
                }
                gameObject.GetComponent<spellCastManager>().activateSpellCast(character.equippedRunes[2], 3);
                uiManager.ActivateCooldownOnAbility(3, true);
                //StartCoroutine(abilityCooldown(1f, 3));
                //StartCoroutine(abilityWait());
                if (currentClass == WeaponBase.weaponClassTypes.Knight || currentClass == WeaponBase.weaponClassTypes.Gunner)
                {
                    StartCoroutine(abilityWait(1));
                    
                }
            }
        }

        
    }

    IEnumerator abilityWait(float time)
    {
        yield return new WaitForSeconds(time);
        abilityInUse = false;
        yield break;
    }

    IEnumerator abilityCooldown(float time, int abilityIndex)
    {
        yield return StartCoroutine(uiManager.StartCooldownSlider(abilityIndex, (0.98f / time), true));
        yield return new WaitForSeconds(0.2f);
        uiManager.DeactivateCooldownOnAbility(abilityIndex, true);
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

    private IEnumerator RechargeDashBar()
    {
        Slider slider = dashBar.GetComponent<Slider>();
        Debug.Log("starting recharge dash bar");
        canDash = false;
        slider.value = 0;
        while (slider.value < maxDashValue && slider != null)
        {
            Debug.Log("recharging dash bar");
            slider.value += 2.0f * Time.deltaTime;
            /*if (maxDashValue - slider.value < 10)
            {
                //slider.value = maxDashValue;
            }
            else
            {
                
            }*/
            yield return null;

        }
        canDash = true;
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

    







    private void activateSwordSlashes()
    {
        SS1 = Instantiate(swordSlashPrefab);
        SS2 = Instantiate(swordSlash2);
        SS3 = Instantiate(swordSlash3);
        HS1 = Instantiate(HSP1);
        HS2 = Instantiate(HSP2);
        HS3 = Instantiate(HSP3);
        ES1 = Instantiate(ESP1);
        ES2 = Instantiate(ESP2);
        ES3 = Instantiate(ESP3);
        SS1.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        SS2.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        SS3.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        ES1.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        ES2.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        ES3.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        HS1.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        HS2.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        HS3.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        SS1.transform.SetParent(player.transform, false);
        SS2.transform.SetParent(player.transform, false);
        SS3.transform.SetParent(player.transform, false);
        ES1.transform.SetParent(player.transform, false);
        ES2.transform.SetParent(player.transform, false);
        ES3.transform.SetParent(player.transform, false);
        HS1.transform.SetParent(player.transform, false);
        HS2.transform.SetParent(player.transform, false);
        HS3.transform.SetParent(player.transform, false);
        SS1.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        SS2.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        SS3.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        ES1.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        ES2.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        ES3.transform.position = new Vector3(player.transform.position.x, .75f, player.transform.position.z);
        HS1.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        HS2.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
        HS3.transform.position = new Vector3(player.transform.position.x, .85f, player.transform.position.z);
    }
}








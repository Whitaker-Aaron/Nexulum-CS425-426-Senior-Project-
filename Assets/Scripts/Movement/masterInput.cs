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
    Vector2 move;
    Vector3 lookPos;
    public float speed = 3f;

    //animation variables
    private PlayerAnimation animationControl;
    Vector3 movement, camForward;
    new Transform camera;


    //Knight Combat Variables
    bool isAttacking = false;
    bool inputPaused = false;
    bool returningFromMenu = true;
    public float cooldown = 1f;
    public float nextAttackTime = .3f;
    private static int noOfClicks = 0;
    private float lastClickedTime = 0;
    public float maxComboDelay = .7f;
    public float animTime = 0.5f;
    public float animTimeTwo = 0.5f;
    public float animTimeThree = 0.99f;
    public float differenceTime = .02f;
    public float animDiff = 1.2f;


    //Gunner Variables

    //bullet
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    //rifle
    bool isReloading = false;
    public float reloadTime = 2f;
    public float fireRateTime = .1f;
    bool canShoot = true;
    public int bulletCount;
    public int magSize = 10;



    //--------------FUNCTIONS--------------


    //onMove is implemented through InputSystem in unity, context is the input
    public void onMove(InputAction.CallbackContext context)
    {
        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused)
            return;
        move = context.ReadValue<Vector2>();
    }

    

    //actual player translation for FixedUpdate
    public void movePlayer()
    {
        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight) || inputPaused)
            return;
        Vector3 movement = new Vector3(move.x, 0, move.y);

        player.transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void pauseInput(InputAction.CallbackContext context)
    {
        inputPaused = !inputPaused;
        returningFromMenu = !returningFromMenu;
        animationControl.stop();
        Input.ResetInputAxes();
        noOfClicks = 0;

    }

    //Knight Functions
    IEnumerator wait(float animationTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        isAttacking = false;
        yield break;
    }

    IEnumerator waitThree(float animationTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(animationTime);
        animationControl.resetKnight();
        isAttacking = false;
        yield break;
    }


    //Gunner functions

    IEnumerator shoot()
    {
        canShoot = false;
        print("Shooting");
        while (Input.GetButton("Fire1") && bulletCount > 0 && isReloading == false)
        {
            bulletCount--;
            //int temp = bulletCount;
            //bulletUI.text = temp.ToString();
            //muzzleFlash.Play();
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //bullet.GetComponent<bullet>().setPower(true);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRateTime);
        }
        canShoot = true;
        yield break;
    }

    IEnumerator reload()
    {
        print("reloading");
        if (bulletCount == magSize)
            yield break;

        isReloading = true;
        //FindObjectOfType<AudioManager>().Pause("AutoShot");
        yield return new WaitForSeconds(reloadTime);
        bulletCount = magSize;
        //int temp = bulletCount;
        //bulletUI.text = temp.ToString();
        isReloading = false;
        canShoot = true;
        yield break;
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
        if ((isAttacking && currentClass == WeaponBase.weaponClassTypes.Knight))
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



        if (inputPaused) return;

        //KNIGHT LOGIC
        if(currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClicks = 0;
            }
            if (Time.time > lastClickedTime + nextAttackTime && isAttacking == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //print("click: " + noOfClicks);
                    
                    lastClickedTime = Time.time;
                    noOfClicks++;
                    if (noOfClicks >= 1)
                    {
                        if (animationControl.getAnimationInfo().IsName("attackTwo") && animationControl.getAnimationInfo().normalizedTime > .99f)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        if (animationControl.getAnimationInfo().IsName("attackThree") && animationControl.getAnimationInfo().normalizedTime < animTimeThree)
                        {
                            noOfClicks = 0;
                            return;
                        }
                        //anim.SetTrigger("Attack");
                        //anim.SetBool("attack1", true);
                        animationControl.knightAttackOne();
                        StartCoroutine(wait(animTime));
                        //anim.Play("attackOne");
                        //nextAttackTime = anim.GetCurrentAnimatorStateInfo(0).length - differenceTime;
                        //print("Anim: " + anim.GetBool("attack1"));
                    }
                    noOfClicks = Mathf.Clamp(noOfClicks, 1, 3);

                    if (noOfClicks >= 2 && animationControl.getAnimationInfo().normalizedTime > animTime && animationControl.getAnimationInfo().IsName("attackOne"))
                    {
                        //anim.SetBool("attack2", true);
                        //anim.SetBool("attack1", false);
                        //anim.Play("attackTwo");
                        animationControl.knightAttackTwo();
                        StartCoroutine(wait(animTimeTwo));
                    }

                    if (noOfClicks >= 3 && animationControl.getAnimationInfo().normalizedTime > animTimeTwo && animationControl.getAnimationInfo().IsName("attackTwo"))
                    {
                        nextAttackTime += differenceTime;
                        noOfClicks = 0;
                        animationControl.knightAttackThree();
                        StartCoroutine(waitThree(animTimeThree + animDiff));
                        animationControl.resetKnight();
                        nextAttackTime -= differenceTime;

                    }
                    else
                    {

                        animationControl.resetKnight();
                    }
                }
            }
        }

        if(currentClass == WeaponBase.weaponClassTypes.Gunner)
        {
            if (bulletCount <= 0)
            {
                bulletCount = 0;
                canShoot = false;
                StartCoroutine(reload());
            }


            //muzzleFlash.transform.position = flashSpawn.transform.position;

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(reload());
            }


            if (Input.GetButtonDown("Fire1") && isReloading == false && bulletCount > 0 && canShoot)
            {
                StartCoroutine(shoot());
            }
        }

        returningFromMenu = false;
    }
    private void FixedUpdate()
    {
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

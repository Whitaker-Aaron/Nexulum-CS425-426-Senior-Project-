using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class playerAnimationController : MonoBehaviour, PlayerAnimation
{
    new Transform camera;
    private Animator animator;
    Vector3 moveInput, camForward, movement;
    float forwardAmount, turnAmount;
    GameObject player;



    //General movement functions---------------


    public void updatePlayerAnimation(Vector3 movementDirection)
    {
        Move(movementDirection);
    }

    private void Move(Vector3 moving)
    {
        if (moving.magnitude > 1)
        {
            moving.Normalize();
        }

        this.moveInput = moving;

        // Convert world movement into local movement
        convertMoveInput();
        // Update the animator with the converted local movement
        updateAnimator();
    }

    void convertMoveInput()
    {
        // Convert the camera-relative movement (moveInput) to the player's local space
        Vector3 localMove = player.transform.InverseTransformDirection(moveInput);

        // Local Z-axis for forward-backward movement (positive Z = forward, negative Z = backward)
        forwardAmount = localMove.x;

        // Local X-axis for left-right movement (positive X = right, negative X = left)
        turnAmount = localMove.z;
    }

    void updateAnimator()
    {
        // Update the animation parameters based on the converted local movement
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
    /*
    public void updatePlayerAnimation(Vector3 movementDirection)
    {
        Move(movementDirection);
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
    */
    /*
    void convertMoveInput()
    {
        Vector3 localMove = player.transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.z;

        forwardAmount = localMove.x;
    }
    void updateAnimator()
    {
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
    */
    //-------------------------------------------------------

    //Knight Functions---------------------------------------

    public AnimatorStateInfo getAnimationInfo()
    {
        if (gameObject.GetComponent<masterInput>().currentClass == WeaponBase.weaponClassTypes.Knight)
            return animator.GetCurrentAnimatorStateInfo(0);
        else if (gameObject.GetComponent<masterInput>().currentClass == WeaponBase.weaponClassTypes.Gunner)
            return animator.GetCurrentAnimatorStateInfo(1);
        else if (gameObject.GetComponent<masterInput>().currentClass == WeaponBase.weaponClassTypes.Engineer)
            return animator.GetCurrentAnimatorStateInfo(2);
        else
            return animator.GetCurrentAnimatorStateInfo(0);
    }

    public IEnumerator startKnightBlock(float time)
    {
        animator.SetBool("blocking", true);
        animator.Play("startBlock");
        yield return new WaitForSeconds(time);
        animator.Play("blocking");
        yield break;
    }

    public IEnumerator stopKnightBlock(float time)
    {
        animator.SetBool("blocking", false);
        animator.SetBool("isWalking", false);
        animator.Play("stopBlock");
        yield return new WaitForSeconds(time);
        animator.Play("Locomotion");
        yield break;
    }

    public void blocking()
    {
        animator.SetBool("blocking", true);
        animator.SetBool("isWalking", true);
        animator.Play("blocking");
    }

    IEnumerator attackWait(float time, string animName, int index)
    {
        yield return new WaitForSeconds(time);
        //if (getAnimationInfo().IsName("attack2") || getAnimationInfo().IsName("Attack3") || getAnimationInfo().IsName("engAttack3") || getAnimationInfo().IsName("engAttack2"))
            //yield break;
        //print("animation: " + animName);
        
        animator.Play(animName, index);
        yield break;
    }


    public void knightAttackOne(float time)
    {
        
        //if (getAnimationInfo().IsName("Attack2") || getAnimationInfo().IsName("Attack3"))
            //return;
        animator.SetBool("attack1", true);
        animator.Play("attackOne");
        StartCoroutine(attackWait(time, "waitOne", 0));
    }
    public void knightAttackTwo(float time)
    {
        animator.SetBool("attack1", false);
        animator.SetBool("attack2", true);
        animator.Play("attackTwo");
        StartCoroutine(attackWait(time, "waitTwo", 0));
    }

    public void knightAttackThree()
    {

        animator.SetBool("attack1", false);
        animator.SetBool("attack2", false);
        animator.SetBool("attack3", true);
        animator.Play("attackThree");

    }

    public void stop()
    {
        animator.StopPlayback();
        //resetKnight();
        //animator.enabled = !animator.enabled;
    }

    public void knightShootSwords()
    {
        animator.SetBool("shootSwords", true);
        animator.Play("knightSwordShoot");
    }

    public void stopShootSword()
    {
        animator.SetBool("shootSwords", false);
    }

    public void resetKnight()
    {
        if(animator.GetBool("attack3") == true)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
            animator.SetBool("attack3", false);
        }
        else if (animator.GetBool("attack2") == true)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
        }
        else
        {
            animator.SetBool("attack1", false);
        }
    }
    //-------------------------------------------------------

    //GUNNER ANIMATIONS--------------------------------------

    public void gunnerReload()
    {
        animator.SetBool("reload", true);
        animator.Play("Reload Blend Tree");
        animator.SetBool("reload", false);
    }


    //-------------------------------------------------------


    //Engineer Animations------------------------------------

    public void engineerReload()
    {
        animator.SetBool("reload", true);
        animator.Play("engReloadBlendTree");
        animator.SetBool("reload", false);
    }

    public void engAttackOne(float time)
    {
        if (getAnimationInfo().IsName("engAttackTwo") || getAnimationInfo().IsName("engAttackThree"))
            return;
        animator.SetBool("engAttack1", true);
        animator.Play("engAttackOne", 2);
        StartCoroutine(attackWait(time, "engWaitOne", 2));
    }

    public void engAttackTwo(float time)
    {
        animator.SetBool("engAttack2", true);
        animator.Play("engAttackTwo", 2);
        animator.SetBool("engAttack1", false);


        StartCoroutine(attackWait(time * 1.5f, "engWaitTwo", 2));
    }

    public void engAttackThree()
    {
        animator.SetBool("engAttack3", true);
        animator.Play("engAttackThree", 2);
        animator.SetBool("engAttack1", false);
        animator.SetBool("engAttack2", false);
    }
    public void resetEngineer()
    {
        if (animator.GetBool("engAttack3") == true)
        {
            animator.SetBool("engAttack1", false);
            animator.SetBool("engAttack2", false);
            animator.SetBool("engAttack3", false);
        }
        else if (animator.GetBool("engAttack2") == true)
        {
            animator.SetBool("engAttack1", false);
            animator.SetBool("engAttack2", false);
        }
        else
        {
            animator.SetBool("engAttack1", false);
        }
    }

    //-------------------------------------------------------

    //Change animation layer---------------------------------

    public void changeClassLayer(int layerOne, int layerTwo)
    {

        if (animator == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            animator = player.GetComponent<Animator>();
        }
        Debug.Log(animator);

        animator.SetLayerWeight(layerOne, 0);
        animator.SetLayerWeight(layerTwo, 1);
    }


    //-------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();//.getAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}

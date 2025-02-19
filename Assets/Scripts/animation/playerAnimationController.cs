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

    public void knightHeavyOne(float time)
    {
        animator.SetBool("H1", true);
        animator.Play("heavyOne");
        StartCoroutine(attackWait(time, "heavyWaitOne", 0));
    }

    public void knightHeavyTwo(float time)
    {
        animator.SetBool("H1", false);
        animator.SetBool("H2", true);
        animator.Play("heavyTwo");
    }

    public void knightHeavyThree()
    {
        animator.SetBool("H3", true);
        animator.SetBool("H1", false);
        animator.SetBool("H2", false);
        animator.Play("heavyThree");
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
        if(animator.GetBool("attack3") == true || animator.GetBool("H3") == true)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
            animator.SetBool("attack3", false);
            animator.SetBool("H1", false);
            animator.SetBool("H2", false);
            animator.SetBool("H3", false);
        }
        else if (animator.GetBool("attack2") == true || animator.GetBool("H2") == true)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
            animator.SetBool("H1", false);
            animator.SetBool("H2", false);
        }
        else
        {
            animator.SetBool("attack1", false);
            animator.SetBool("H1", false);
        }
    }
    //-------------------------------------------------------

    //GUNNER ANIMATIONS--------------------------------------

    public IEnumerator gunnerReload(float time)
    {
        var temp = animator.speed;
        animator.SetBool("reload", true);
        animator.Play("Reload Blend Tree");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName("Reload Blend Tree"));
        //if(animator.GetCurrentAnimatorStateInfo(1).IsName("Reload Blend Tree"))
        animator.speed = animator.GetCurrentAnimatorStateInfo(1).length / time;
        print("Animator speed is: " + animator.speed);
        yield return StartCoroutine(reloadWait(1, time, temp));

        //float reloadDuration = animator.GetCurrentAnimatorStateInfo(1).length; 
        //animator.speed = reloadDuration / time; // Adjust animation speed dynamically

        //StartCoroutine(ResetAnimatorSpeed(reloadDuration / time)); // Reset after animation
    }

    IEnumerator reloadWait(int layer, float time, float ogSpeed)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("reload", false);
        animator.Play("Locomotion", layer);
        animator.speed = ogSpeed;
        yield break;
    }


    //-------------------------------------------------------


    //Engineer Animations------------------------------------

    /*
    public void engineerReload(float time)
    {
        
        animator.SetBool("reload", true);
        animator.Play("engReloadBlendTree");
        animator.SetFloat("blendTreeSpeed", (animator.GetCurrentAnimatorStateInfo(2).length / time) * 2f);
        animator.SetBool("reload", false);
        //StartCoroutine(engrReloadWait((animator.GetCurrentAnimatorStateInfo(2).length / time) + 1));
    }
    /*
    public void engineerReload(float time)
    {
        animator.SetBool("reload", true);
        animator.Play("engReloadBlendTree");

        float reloadDuration = animator.GetCurrentAnimatorStateInfo(2).length; // Check layer 0 instead of 2
        animator.speed = reloadDuration / time; // Adjust animation speed dynamically

        StartCoroutine(ResetAnimatorSpeed(reloadDuration / time)); // Reset after animation
    }
    */

    public IEnumerator engineerReload(float reloadTime)
    {
        var temp = animator.speed;
        animator.SetBool("reload", true);
        animator.Play("engReloadBlendTree");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(2).IsName("engReloadBlendTree"));
        animator.speed = animator.GetCurrentAnimatorStateInfo(2).length / reloadTime;
        StartCoroutine(reloadWait(2, reloadTime, temp));
        // Get the default reload animation duration from the blend tree
        //float baseReloadDuration = GetReloadAnimationLength();

        // Calculate the new speed multiplier to fit the desired reload time
        //float speedMultiplier = baseReloadDuration / reloadTime;

        // Ensure the speed multiplier is at least 1 to avoid too fast animation
        //speedMultiplier = Mathf.Max(speedMultiplier, 1f);

        // Apply the new speed to the blend tree parameter
        //animator.SetFloat("reloadSpeed", speedMultiplier);

        // Start a coroutine to reset values after reload finishes
        //StartCoroutine(ResetReload(reloadTime));
    }

    private float GetReloadAnimationLength()
    {
        int layerIndex = 0; // Change if reload animation is on a different layer
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // Account for the animation length, avoiding division by zero
        return Mathf.Max(stateInfo.length, 0.1f); // Use a small minimum value
    }

    private IEnumerator ResetReload(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animator.SetBool("reload", false);
        animator.SetFloat("reloadSpeed", 1f); // Reset speed to normal
    }

    IEnumerator ResetAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.speed = 1f; // Reset to default speed
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


        StartCoroutine(attackWait(time * 2f, "engWaitTwo", 2));
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

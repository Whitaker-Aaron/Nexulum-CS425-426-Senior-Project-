using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

        convertMoveInput();
        updateAnimator();
    }

    void convertMoveInput()
    {
        Vector3 localMove = player.transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.z;

        forwardAmount = localMove.x;
    }
    void updateAnimator()
    {
        if (forwardAmount != 0 && (getAnimationInfo().IsName("waitOne") || getAnimationInfo().IsName("waitTwo")) )
        {
            //animator.SetTrigger("return");
        }
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
    //-------------------------------------------------------

    //Knight Functions---------------------------------------

    public AnimatorStateInfo getAnimationInfo()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }

    IEnumerator attackWait(float time, string animName)
    {
        yield return new WaitForSeconds(time);
        if (getAnimationInfo().IsName("attack2") || getAnimationInfo().IsName("Attack3"))
            yield break;
        //print("animation: " + animName);
        animator.Play(animName);
        yield break;
    }


    public void knightAttackOne(float time)
    {
        
        if (getAnimationInfo().IsName("Attack2") || getAnimationInfo().IsName("Attack3"))
            return;
        animator.SetBool("attack1", true);
        animator.Play("attackOne");
        StartCoroutine(attackWait(time, "waitOne"));
    }
    public void knightAttackTwo(float time)
    {
        animator.SetBool("attack1", false);
        animator.SetBool("attack2", true);
        animator.Play("attackTwo");
        StartCoroutine(attackWait(time, "waitTwo"));
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}

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
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
    //-------------------------------------------------------

    //Knight Functions---------------------------------------

    public AnimatorStateInfo getAnimationInfo()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }

    public void knightAttackOne()
    {
        animator.SetBool("attack1", true);
        animator.Play("attackOne");
    }
    public void knightAttackTwo()
    {
        animator.SetBool("attack2", true);
        animator.SetBool("attack1", false);
        animator.Play("attackTwo");
    }

    public void knightAttackThree()
    {
        animator.SetBool("attack3", true);
        animator.SetBool("attack2", false);
        animator.SetBool("attack1", false);
        animator.Play("attackThree");

    }

    public void resetKnight()
    {
        if(animator.GetBool("attack3") == true)
        {
            animator.SetBool("attack3", false);
            animator.SetBool("attack2", false);
            animator.SetBool("attack1", false);
        }
        else if (animator.GetBool("attack2") == true)
        {
            animator.SetBool("attack2", false);
            animator.SetBool("attack1", false);
        }
        else
        {
            animator.SetBool("attack1", false);
        }
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

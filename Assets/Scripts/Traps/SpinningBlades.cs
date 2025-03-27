using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlades : MonoBehaviour , i_Trap
{
    private Animator animator;
    private Vector3 initialPosition;
    private Vector3 previousPosition;
    public int damageAmount;
    public enum SpinDirection { Left, Right }
    public SpinDirection Direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        previousPosition = transform.position;
    }

    void Update()
    {
        DetectMovementDirection();
        SpinningAnimation();
    }

    private void DetectMovementDirection()
    {
        if (transform.position.x < previousPosition.x)
        {
            Direction = SpinDirection.Left;
        }
        else if (transform.position.x > previousPosition.x)
        {
            Direction = SpinDirection.Right;
        }
        previousPosition = transform.position;
    }

    private void SpinningAnimation()
    {
        if (Direction == SpinDirection.Left)
        {
            animator.SetBool("isLeft", true);
        }
        else if (Direction == SpinDirection.Right)
        {
            animator.SetBool("isLeft", false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        CharacterBase playerHealth = other.GetComponent<CharacterBase>();

        if (playerHealth != null)
        {
            var knockbackDir = other.transform.position - transform.position;
            playerHealth.takeDamage(damageAmount, knockbackDir);
            Debug.Log("Player hit by spikes and took " + damageAmount + " damage.");
        }
    }

}

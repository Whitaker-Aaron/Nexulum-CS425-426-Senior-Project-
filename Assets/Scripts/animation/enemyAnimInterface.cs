using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface EnemyAnimation
{
    void updateAnimation(Vector3 movementDirection);
    void minionAttack();
    void mageAttack();
    float getAnimationTime();
    void takeHit();
    AnimatorStateInfo getAnimationInfo();
    //void setMovement(float forward, float turn);

}

public class enemyAnimInterface : MonoBehaviour
{
    //private Animator animator;
    //private readonly int forwardHash = Animator.StringToHash("Forward");
    //private readonly int turnHash = Animator.StringToHash("Turn");

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    //public void SetMovement(float forward, float turn)
    //{
        //animator.SetFloat(forwardHash, forward);
       // animator.SetFloat(turnHash, turn);
    //}
}

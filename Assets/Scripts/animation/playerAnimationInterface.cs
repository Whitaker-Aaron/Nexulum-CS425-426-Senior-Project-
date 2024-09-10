using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for player animation
public interface PlayerAnimation
{
    void updatePlayerAnimation(Vector3 movementDirection);
    AnimatorStateInfo getAnimationInfo();
    void knightAttackOne();
    void knightAttackTwo();
    void knightAttackThree();
    void resetKnight();
}
public class playerAnimationInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
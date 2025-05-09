using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for player animation
public interface PlayerAnimation
{
    void updatePlayerAnimation(Vector3 movementDirection);
    AnimatorStateInfo getAnimationInfo();
    void knightAttackOne(float time);
    void knightAttackTwo(float time);
    void knightAttackThree();

    public void knightHeavyOne(float time);

    public void knightHeavyTwo(float time);

    public void knightHeavyThree();

    void knightShootSwords();
    void resetKnight();

    IEnumerator startKnightBlock(float time);

    IEnumerator stopKnightBlock(float time);

    void blocking();

    IEnumerator engineerReload(float time);
    void falling(string curClass);

    void stopFall(string curClass);


    void engAttackOne(float time);

    void engAttackTwo(float time);

    void engAttackThree();
    void resetEngineer();

    IEnumerator gunnerReload(float time);
    void gunnerRocketPod(bool choice);
    void stop();

    void changeClassLayer(int layerOne, int layerTwo);
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

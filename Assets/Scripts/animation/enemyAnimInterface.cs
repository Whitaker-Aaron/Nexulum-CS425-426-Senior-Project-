using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface EnemyAnimation
{
    void setMovement(float forward, float turn);

}

public class enemyAnimInterface : MonoBehaviour
{

    private Animator anim;
    private readonly int forwardHash = Animator.StringToHash("Forward");
    private readonly int turnHash = Animator.StringToHash("Turn");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetMovement(float forward, float turn)
    {
        anim.SetFloat(forwardHash, forward);
        anim.SetFloat(turnHash, turn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

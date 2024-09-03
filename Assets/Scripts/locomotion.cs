using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class playerLocomotion : MonoBehaviour
{
    lookAtCursor player;
    Animator animator;
    Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<lookAtCursor>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

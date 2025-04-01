using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{
    EnemyStateManager enemyStateMananger;
    EnemyFrame enemyFrame;
    public bool toggle = false;

    void Awake()
    {
        //
    }
    void Start()
    {
        Debug.Log("Enemy test behavior script start");

        enemyStateMananger = gameObject.GetComponent<EnemyStateManager>();
        enemyFrame = gameObject.GetComponent<EnemyFrame>();
        // enemyStateMananger.PauseMovementFor(5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle) // This represents whatever your condition is
        {
            enemyFrame.takeDamage(0, Vector3.zero, EnemyFrame.DamageSource.Enemy, EnemyFrame.DamageType.Ice);
        }
    }
}

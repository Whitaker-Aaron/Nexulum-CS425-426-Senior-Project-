using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{
    EnemyStateManager enemyStateMananger;
    public bool toggle = false;

    void Awake()
    {
        //
    }
    void Start()
    {
        Debug.Log("Enemy test behavior script start");

        EnemyStateManager enemyStateMananger = gameObject.GetComponent<EnemyStateManager>();
        // enemyStateMananger.PauseMovementFor(5f);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateManager enemyStateMananger = gameObject.GetComponent<EnemyStateManager>();
        if (toggle) // This represents whatever your condition is
        {
            enemyStateMananger.PauseMovementFor(5f);
        }
    }
}

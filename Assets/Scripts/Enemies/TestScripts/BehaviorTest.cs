using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{

    EnemyStateManager enemyStateMananger;
    EnemyLOS enemyLOS;
    public float distance;

    void Start()
    {
        Debug.Log("Enemy test behavior script start");

        EnemyStateManager enemyStateMananger = gameObject.GetComponent<EnemyStateManager>();
        EnemyLOS enemyLOS = gameObject.GetComponent<EnemyLOS>();

        StartCoroutine(enemyStateMananger.PauseMovement(5f));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy test behavior script update");
        distance = enemyLOS.distancetotarget;
    }
}

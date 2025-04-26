using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{
    EnemyStateManager enemyStateMananger;
    EnemyFrame enemyFrame;
    public bool toggle = false;
    public Color color;
    Renderer renderer;

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

        renderer = gameObject.AddComponent(typeof(Renderer)) as Renderer;

        if (renderer != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("Renderer component null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        if (toggle)
        {
            enemyFrame.takeDamage(0, Vector3.zero, EnemyFrame.DamageSource.Enemy, EnemyFrame.DamageType.Ice);
        }
    }
}

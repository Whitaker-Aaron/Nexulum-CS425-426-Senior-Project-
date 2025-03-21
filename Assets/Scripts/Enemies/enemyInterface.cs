using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyFrame;

public interface enemyInt
{
    bool isAttacking { get; set; }
    bool isActive { get; set; }
    enemyInt getType();

    void onDeath();
}

public class enemyInterface
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArcher : MonoBehaviour, enemyInt, archerInterface
{
    private bool _isAttacking;
    public bool isAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)  // Only set if the value is different
            {
                _isAttacking = value;
                // Do the other necessary actions
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enemyInt getType()
    {
        return this;
    }

    public void onDeath()
    {

    }


}

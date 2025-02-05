using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArcher : MonoBehaviour, enemyInt, archerInterface
{
    private bool _isAttacking;
    private EnemyState enemyState;
    private EnemyStateManager enemyStateManager;

    public int damage;

    public GameObject bowPrefab;
    private bow bow;
    public Transform arrowSpawn;

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
        enemyStateManager = gameObject.GetComponent<EnemyStateManager>();
        enemyState = enemyStateManager.GetCurrentState();
        bow = bowPrefab.GetComponent<bow>();
        bow.setArcher(this);
    }


    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        enemyState = enemyStateManager.GetCurrentState();
        print("Enemy state is: " + enemyState.stateName);
        if(enemyState != null && enemyState.stateName == "Chase")
        {
            print("Enemy can shoot bow");
            if (bow.canShoot)
            {
                if(bow.bulletCount <= 0)
                    StartCoroutine(bow.Reload());
                if(bow.bulletCount >= 1)
                    StartCoroutine(bow.Shoot());
            }
        }
    }


    public enemyInt getType()
    {
        return this;
    }

    public void onDeath()
    {

    }


}

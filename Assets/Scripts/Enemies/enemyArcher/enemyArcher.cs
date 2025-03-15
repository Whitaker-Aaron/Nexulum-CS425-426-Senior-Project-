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

    public float detectionRange;
    bool inRange;
    public LayerMask Player;

    public GameObject playerObj;

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

    private bool _isActive;
    public bool isActive
    {
        get { return _isActive; }
        set
        {
            if (_isActive != value)  // Only set if the value is different
            {
                _isActive = value;
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
        bow.setArcher(gameObject.GetComponent<enemyArcher>());
    }


    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        checkDistance();

        enemyState = enemyStateManager.GetCurrentState();
        print("Enemy state is: " + enemyState.GetName());
        if((inRange && playerObj != null) && enemyState != null && (enemyState.GetName() == "Chase" || enemyState.GetName() == "Search"))
        {
            gameObject.transform.LookAt(playerObj.transform.position, Vector3.up);
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

    void checkDistance()
    {
        Collider[] objs = Physics.OverlapSphere(gameObject.transform.position + Vector3.up, detectionRange, Player);

        foreach(Collider obj in objs)
        {
            if(obj.gameObject.tag == "Player")
            {
                if(Vector3.Distance(obj.gameObject.transform.position, gameObject.transform.position) > detectionRange)
                {
                    inRange = false;
                    return;
                }
                else
                {
                    inRange = true;
                    assignPlayer(obj);
                }
                
                
            }

        }
    }

    void assignPlayer(Collider obj)
    {
        playerObj = obj.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position + Vector3.up, detectionRange);
    }

}

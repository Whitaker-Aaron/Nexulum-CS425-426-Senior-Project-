using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        if(inRange && playerObj != null)// && enemyState != null && (enemyState.GetName() == "Chase" || enemyState.GetName() == "Search"))
        {
            gameObject.transform.LookAt(playerObj.transform.position, Vector3.up);
            print("Enemy can shoot bow");
            if (bow.canShoot)
            {
                print("SHooting in archer");
                //if(bow.bulletCount <= 0)
                    //StartCoroutine(bow.Reload());
                //if(bow.bulletCount >= 1)
                StartCoroutine(shootBow());

            }
        }
    }

    public float aimTime = .4f;
    private bool shooting = false;

    private IEnumerator shootBow()
    {
        if (shooting)
            yield break;

        shooting = true;

        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("aiming", true);
        gameObject.GetComponent<NavMeshAgent>().speed = 2;
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("idleAim"))
            animator.Play("drawArrow");
        yield return new WaitForSeconds(aimTime);
        animator.SetBool("shoot", true);
        StartCoroutine(bow.Shoot());
        animator.Play("shoot");
        yield return new WaitForSeconds(.3f);
        gameObject.GetComponent<NavMeshAgent>().speed = 0;


        Collider[] objs = Physics.OverlapSphere(gameObject.transform.position + Vector3.up, detectionRange, Player);

        if (objs.Length == 0)
        {
            assignPlayer(null);
            animator.SetBool("aiming", false);
            animator.SetBool("shoot", false);
            animator.Play("Locomotion");
            yield break;
        }
        else
        {
            foreach (Collider obj in objs)
            {
                if (obj.gameObject.tag == "Player")
                {
                    if (Vector3.Distance(obj.gameObject.transform.position, gameObject.transform.position) > detectionRange)
                    {
                        inRange = false;
                        assignPlayer(null);
                        animator.SetBool("aiming", false);
                        animator.SetBool("shoot", false);
                        animator.Play("Locomotion");
                        yield break;
                    }
                    else
                    {
                        inRange = true;
                        assignPlayer(obj);
                        animator.SetBool("aiming", true);
                        //print("SHooting at player");
                    }


                }

            }
        }

        yield return new WaitUntil(() => bowPrefab.GetComponent<bow>().getCanShoot());
        shooting = false;

        yield break;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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

        if(objs.Length == 0) 
        {
            assignPlayer(null);
            return;
        }
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
                    //print("SHooting at player");
                }
                
                
            }

        }
    }

    void assignPlayer(Collider obj)
    {
        if(obj != null) 
            playerObj = obj.gameObject;
        else
            playerObj = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position + Vector3.up, detectionRange);
    }

}

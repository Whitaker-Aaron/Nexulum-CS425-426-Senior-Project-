using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyArcher : MonoBehaviour, enemyInt, archerInterface
{
    private bool _isAttacking;
    private EnemyState enemyState;
    private EnemyStateManager enemyStateManager;
    private EnemyLOS enemyLOS;

    public int damage;

    public GameObject bowPrefab;
    private bow bow;
    public Transform arrowSpawn;

    public float detectionRange;
    bool inRange;
    public LayerMask Player;

    public GameObject playerObj;

    Animator animator;

    public GameObject warning;
    public float promptTime = .6f;

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
        enemyLOS = gameObject.GetComponent<EnemyLOS>();
        enemyState = enemyStateManager.GetCurrentState();
        bow = bowPrefab.GetComponent<bow>();
        bow.setArcher(gameObject.GetComponent<enemyArcher>());
        animator = gameObject.GetComponent<Animator>();

        if (this.detectionRange > enemyLOS.detectionRange) // Catch if attack radius is larger than the vision radius - Aisling
        {
            Debug.LogWarning("enemyArcher.cs - Local detection range for attacks exceeds detection range for sight. Setting sight range equal to attack range.");
            enemyLOS.detectionRange = this.detectionRange;
        }
    }


    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        checkDistance();

        enemyState = enemyStateManager.GetCurrentState();
        print("Enemy state is: " + enemyState.GetName());
        bool aggressiveState = enemyState.GetName() == "Chase" || enemyState.GetName() == "Search";
        if(inRange && playerObj != null && enemyState.GetName() == "Chase")// && enemyState != null && (enemyState.GetName() == "Chase" || enemyState.GetName() == "Search"))
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
        else
        {
            StopCoroutine(shootBow() );
            animator.SetBool("aiming", false);
            animator.SetBool("shoot", false);
            animator.Play("Locomotion");
        }
    }

    public float aimTime = .4f;
    private bool shooting = false;
    bool first = true;

    private IEnumerator shootBow()
    {
        if (shooting)
            yield break;
        if (first)
        {
            first = false;
            yield return new WaitForSeconds(.6f);//buffer time for player entering area
        }
            

        shooting = true;

        //animator = gameObject.GetComponent<Animator>();

        animator.SetBool("aiming", true);
        //gameObject.GetComponent<NavMeshAgent>().speed = 2;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") || animator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            animator.Play("drawArrow");
        gameObject.GetComponent<EnemyStateManager>().StopMovement();
        

        yield return new WaitUntil(() => bowPrefab.GetComponent<bow>().getCanShoot());
        warning.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(promptTime);
        
        animator.SetBool("shoot", true);
        animator.Play("shoot");
        StartCoroutine(bow.Shoot());
        
        yield return new WaitForSeconds(.3f);

        gameObject.GetComponent<EnemyStateManager>().StopMovement();
        if (playerObj == null)
        {
            animator.SetBool("aiming", false);
            animator.SetBool("shoot", false);
            animator.Play("Locomotion");
        }

        /*
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
        */
        //yield return new WaitUntil(() => bowPrefab.GetComponent<bow>().getCanShoot());
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
                    assignPlayer(null);
                    first = true;
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

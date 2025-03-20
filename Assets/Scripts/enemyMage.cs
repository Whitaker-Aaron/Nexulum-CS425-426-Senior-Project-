using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMage : MonoBehaviour, mageInterface, enemyInt
{

    public float detectionRadius;

    private List<GameObject> enemies;

    private GameObject player;

    public LayerMask playerLayer;

    private bool canCastSpell = true;

    public float castTime = 3f;

    [SerializeField]
    private Transform spellProjSpawn;

    private Animator animator;

    [SerializeField] GameObject spellEffect;



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

    public enemyInt getType()
    {
        return this;
    }

    private void Awake()
    {
        //gameObject.GetComponent<SphereCollider>().radius = detectionRadius;
        enemies = new List<GameObject>();
        player = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();

        if(player != null)
        {
            gameObject.transform.LookAt(player.transform.position, Vector3.up);
        }
    }

    public void onDeath()
    {

    }



    public IEnumerator spellCast()
    {
        if (!canCastSpell)
            yield break;
        
        
        
        canCastSpell = false;
        isAttacking = true;
        if (!animator.GetBool("Attack"))
        {
        animator.SetBool("Attack", true);
        //animator.Play("Attack");
        }

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("walking blend tree"))
        //animator.Play("Attack");
        gameObject.GetComponent<EnemyAnimation>().mageAttack();
        yield return new WaitForSeconds(.5f);
        spellEffect.GetComponent<ParticleSystem>().Play();
        projectileManager.Instance.getProjectile("enemyMagePoolOne", spellProjSpawn.position, spellProjSpawn.rotation);
        yield return new WaitForSeconds(castTime);
        //animator.SetBool("Attack", false);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            animator.Play("walking blend tree");
            canCastSpell = true;
        isAttacking = false;
        yield break;
    }

    public void detectPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position + new Vector3(0,1,0), detectionRadius);

        foreach(Collider col in cols)
        {
            if (col.gameObject.tag == "Player")
            {
                player = col.gameObject;
            }
            else if (col.gameObject.tag == "Enemy" && col.gameObject != gameObject)
            {
                enemies.Add(col.gameObject);
            }
        }

        if(player != null)
        {
            Vector3 temp = (player.transform.position - gameObject.transform.position).normalized;
            Ray ray = new Ray(transform.position + new Vector3(0, 0.75f, 0), temp);
            RaycastHit[] hits = Physics.RaycastAll(ray, detectionRadius);

            foreach (RaycastHit hit in hits)
            {
                // Check if the hit object is the player
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
                {
                    if (canCastSpell)
                    {
                        StartCoroutine(spellCast());
                    }
                }
            }

        }
    }


    private void OnDrawGizmos()
    {
        // Visualize detection range in the editor
        Gizmos.color = Color.red;
        if (player != null)
        {
            Vector3 temp = (player.transform.position - gameObject.transform.position).normalized;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.75f, 0), temp * detectionRadius);
        }
        Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(0, 1, 0), detectionRadius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMage : MonoBehaviour, mageInterface
{

    public float detectionRadius;

    private List<GameObject> enemies;

    private GameObject player;

    public LayerMask playerLayer;

    private bool canCastSpell = true;

    public float castTime = 3f;

    [SerializeField]
    private Transform spellProjSpawn;

    private void Awake()
    {
        //gameObject.GetComponent<SphereCollider>().radius = detectionRadius;
        enemies = new List<GameObject>();
        player = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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



    public IEnumerator spellCast()
    {
        canCastSpell = false;
        projectileManager.Instance.getProjectile("enemyMagePoolOne", spellProjSpawn.position, spellProjSpawn.rotation);
        yield return new WaitForSeconds(castTime);
        canCastSpell = true;
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

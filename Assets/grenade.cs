using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{

    public float fuseTime = 3f;
    public float blastRadius = 3f;
    public GameObject explosion;
    public LayerMask enemy;
    public int damage = 25;


    public IEnumerator explode()
    {
        yield return new WaitForSeconds(fuseTime);
        var exp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        exp.GetComponent<ParticleSystem>().Play();

        Collider[] enemies = Physics.OverlapSphere(gameObject.transform.position, blastRadius, enemy);

        foreach(Collider c in enemies)
        {
            if (c.gameObject.tag == "Enemy")
            {
                c.GetComponent<EnemyFrame>().takeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, blastRadius);
    }

    private void Awake()
    {
        //StartCoroutine(explode());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

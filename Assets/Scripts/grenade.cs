using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{

    public float fuseTime = 3f;
    public float blastRadius = 3f;
    public float earthBlastRadius = 3f;
    public GameObject explosion;
    public LayerMask enemy;
    UIManager uiManager;
    public int damage = 25;
    public int earthDamage = 25;

    public bool isEarth = false;


    public void increaseBlastRadius(float amount)
    {
        print("changing grenade rad from " + blastRadius + " to " + (blastRadius + amount));
        blastRadius += amount;
        earthBlastRadius += amount;
    }

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
                c.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Explosion);
                uiManager.DisplayDamageNum(c.gameObject.transform, damage);
            }
        }

        if (isEarth)
        {

            EffectsManager.instance.getFromPool("earthGrenade", gameObject.transform.position);
            enemies = Physics.OverlapSphere(gameObject.transform.position, earthBlastRadius, enemy);
            foreach (Collider c in enemies)
            {
                if (c.gameObject.tag == "Enemy")
                {
                    c.GetComponent<EnemyFrame>().takeDamage(earthDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Earth);
                    uiManager.DisplayDamageNum(c.gameObject.transform, damage);
                }
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
        if(uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

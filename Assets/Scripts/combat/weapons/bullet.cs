using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject target;
    public float lifeTime = 4f;
    public int damage = 0;
    public int bossdamageMult;
    public LayerMask enemy, boss;
    public float explosiveRange;


    //effects
    //public ParticleSystem explosion, bulletHit, fireExpld;
    //private ParticleSystem expld, bullhit, fireexpld;

    //public GameObject fireTrail;
    //private GameObject firetrail;

    private void Awake()
    {

        //target = GameObject.FindWithTag("Player");
        //index = index.GetComponent<enemyIndex>();
        //enemyName = index.getEnemyName();
        //Destroy(gameObject, lifeTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "material")
            return;
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("BulletImpact");
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag == "Boss")
        {

            //collision.gameObject.GetComponent<BossBehavior>().takeDamage(damage);
            //collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("BulletImpact");
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.tag == "enemyProjectile")
        {

            //target.GetComponent<PlayerBehavior>().increaseStreak();
            //target.GetComponent<PlayerBehavior>().increaseStreak();
            //Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);

            return;
        }

    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (firetrail != null)
        //firetrail.transform.position = transform.position;
    }
}
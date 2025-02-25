using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceSwordProj : projectile
{
    private int count = 0;
    [SerializeField] private int maxHitCount;

    private Collider[] hitObjs; 
    private void FixedUpdate()
    {
        //if (hitPoint != Vector3.zero)// || hitEnemy)

        if (!stop)
        {
            moveProj();
        }
    }

    private void Awake()
    {
        bulletHitEffect = "iceSwordProjHit";
        GetDamage("Player");
    }

    private void OnEnable()
    {
        stop = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        //RaycastHit hit;
        GetDamage("Player");

        hitObjs = new Collider[maxHitCount];


    }

    private void OnDisable()
    {
        count = 0;
        
        hitObjs = null;
    }

    protected override void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || (other.gameObject.GetComponent<projectile>() != null && other.gameObject.GetComponent<projectile>().poolName == "iceSwordProjPool"))
            return;

        if(count == maxHitCount - 1)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                playEffect(gameObject.transform.position);
            }
            else
            {
                playEffect(gameObject.transform.position);
            }
        }

        if(other.gameObject.tag == "Enemy")
        {
            count++;
            other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, gameObject.transform.forward, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            playEffect(gameObject.transform.position);
        }
        else
        {
            playEffect(gameObject.transform.position);
        }
    }
}

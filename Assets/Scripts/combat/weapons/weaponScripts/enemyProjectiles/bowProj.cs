using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowProj : projectile
{
    private GameObject archerParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //GetDamage(false);
        hitPoint = gameObject.transform.position;
        if (other.gameObject.tag == "Player")
        {
            
            other.gameObject.GetComponent<CharacterBase>().takeDamage(damage, gameObject.transform.forward);
            playEffect(hitPoint);
        }
        else if (other.gameObject.tag == "Enemy" && other.gameObject != archerParent)
        {
            other.gameObject.GetComponent<EnemyFrame>().takeDamage(damage, gameObject.transform.forward, EnemyFrame.DamageSource.Enemy, EnemyFrame.DamageType.Projectile);
            playEffect(hitPoint);
        }
        else
        {
            playEffect(hitPoint);
        }
    }

    private void Awake()
    {
        bulletHitEffect = "archerHitPool";
        GetDamage("Archer");
    }

    private void FixedUpdate()
    {
        if (!stop)
        {
            moveProj();
        }
    }

    private void OnEnable()
    {
        stop = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        if (uiManager == null) uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        bulletHitEffect = "archerHitPool";
        GetDamage("archer");
    }

    private void OnDisable()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected override void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void setArcher(enemyArcher archer)
    {
        archerParent = archer.gameObject;
        setParent(archerParent);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifleProj : projectile
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //if (hitPoint != Vector3.zero)// || hitEnemy)

        checkDistance();

        if (!stop)
        {
            moveProj();
        }
    }

    private void Awake()
    {
        bulletHitEffect = "bulletHitPool";
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
        ignore = LayerMask.GetMask("Material", "Ignore Raycast", "Shield");

    }

    protected override void moveProj()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void checkDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~ignore))// && poolName != "enemyMagePoolOne")
        {
            hitPoint = hit.point;
        }

        float step = speed * Time.fixedDeltaTime;
        float distanceToHit = Vector3.Distance(transform.position, hitPoint);

        if ((distanceToHit <= step || distanceToHit <= bufferDistance || hitEnemy) && hitPoint != null)
        {
            if (hit.collider == null)
                return;

            if (hit.collider.gameObject.tag == "bossPart")
            {
                hitEnemy = true;
                int updatedDamage = damage;
                if (Vector3.Distance(playerBase.gameObject.transform.position, hitPoint) > masterInput.instance.shootingRange)// && playerBase.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
                {
                    updatedDamage = damage / masterInput.instance.engrDmgMod;

                }
                if (hit.collider.gameObject.GetComponent<bossPart>() != null)
                {
                    hit.collider.gameObject.GetComponent<bossPart>().takeDamage(damage);
                    uiManager.DisplayDamageNum(hit.collider.gameObject.transform, updatedDamage);
                }

            }
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hitEnemy = true;
                int updatedDamage = damage;
                //if (playerBase.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Gunner && Vector3.Distance(playerBase.gameObject.transform.position, hitPoint) > masterInput.instance.shootingRange)
                //{
                //    updatedDamage = damage / masterInput.instance.gunnerDmgMod;

                //}
                if (playerBase == null)
                    playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
                if (Vector3.Distance(playerBase.gameObject.transform.position, hitPoint) > masterInput.instance.shootingRange)// && playerBase.equippedWeapon.weaponClassType == WeaponBase.weaponClassTypes.Engineer)
                {
                    updatedDamage = damage / masterInput.instance.gunnerDmgMod;

                }

                hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(updatedDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("BulletImpact");
                uiManager.DisplayDamageNum(hit.collider.gameObject.transform, updatedDamage);

            }

            playEffect(hitPoint);
            // We've reached the hit point, stop the projectile
            stop = true;

            /*
            if(gunnerFire)
            {
                gunnerFire = false;

                Collider[] enemies = Physics.OverlapSphere(hitPoint, fireRad, enemy);
                foreach (Collider enemy in enemies)
                {
                    enemy.gameObject.GetComponent<EnemyFrame>().takeDamage(fireDamage);
                }
            }
            */

        }

    }

    void handleTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            returnToPool();
        }
    }
}

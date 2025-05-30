using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class pistolBasic : weaponType
{

    // Start is called before the first frame update
    void Start()
    {
        bulletSpawn = masterInput.instance.pistolBulletSpawn;
        playerInput = masterInput.instance.gameObject.GetComponent<PlayerInput>();
        isReloading = false;
        canShoot = true;
    }

    void Awake()
    {
        isReloading = false;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Shoot()
    {
        print("canShoot: " + canShoot);
        print("isReloading: " + isReloading);

        if (!canShoot || isReloading) 
            yield break;

        currentHeat += overHeatRate;
        if(bulletSpawn == null || bulletSpawn != masterInput.instance.pistolBulletSpawn)
            bulletSpawn = masterInput.instance.pistolBulletSpawn;

        print("Starting shoot coroutine in pistolBasic");

        canShoot = false;

        GameObject bullet = projectileManager.Instance.getProjectile("pistolPool", bulletSpawn.position, bulletSpawn.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50f; // Standard speed
        EffectsManager.instance.getFromPool("pistolFlash", bulletSpawn.position, bulletSpawn.rotation, true, true);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("Laser");
        yield return new WaitForSeconds(fireRateTime);
        
        canShoot = true;
        yield break;
    }
}

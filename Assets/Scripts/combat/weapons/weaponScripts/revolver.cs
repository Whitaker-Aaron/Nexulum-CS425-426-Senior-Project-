using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class revolver : weaponType
{
    // Start is called before the first frame update
    void Start()
    {
        bulletSpawn = masterInput.instance.pistolBulletSpawn;
        playerInput = masterInput.instance.gameObject.GetComponent<PlayerInput>();
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

        if (bulletSpawn == null || bulletSpawn != masterInput.instance.pistolBulletSpawn)
            bulletSpawn = masterInput.instance.pistolBulletSpawn;

        print("Starting shoot coroutine in pistolBasic");

        canShoot = false;

        bulletCount--;
        GameObject bullet = projectileManager.Instance.getProjectile("pistolPool", bulletSpawn.position, bulletSpawn.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50f; // Standard speed
        EffectsManager.instance.getFromPool("revolverFlash", bulletSpawn.position, bulletSpawn.rotation);
        yield return new WaitForSeconds(fireRateTime);

        canShoot = true;
        yield break;
    }
}

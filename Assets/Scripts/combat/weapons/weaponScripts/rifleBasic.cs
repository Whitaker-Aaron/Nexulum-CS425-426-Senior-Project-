using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class rifleBasic : weaponType
{
    // Start is called before the first frame update
    void Start()
    {
        bulletSpawn = masterInput.instance.bulletSpawn;
        playerInput = masterInput.instance.gameObject.GetComponent<PlayerInput>();
        canShoot = true;
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        //int("canShoot: " + canShoot);
        //int("isReloading: " + isReloading);
    }

    public override IEnumerator Shoot()
    {
        //nt("canShoot: " + canShoot);
        //int("isReloading: " + isReloading);
        if (!canShoot || isReloading)
            yield break;

        print("Shooting in rifleBasic");
        if (bulletSpawn == null || bulletSpawn != masterInput.instance.bulletSpawn)
            bulletSpawn = masterInput.instance.bulletSpawn;


        canShoot = false;

        bulletCount--;
        GameObject bullet = projectileManager.Instance.getProjectile("riflePool", bulletSpawn.position, bulletSpawn.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50f; // Standard speed
        EffectsManager.instance.getFromPool("rifleFlash", bulletSpawn.position, bulletSpawn.rotation);
        yield return new WaitForSeconds(fireRateTime);

        canShoot = true;
        yield break;
    }
}

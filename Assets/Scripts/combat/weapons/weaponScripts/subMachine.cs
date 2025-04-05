using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class subMachine : weaponType
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
        
    }

    public override IEnumerator Shoot()
    {
        //nt("canShoot: " + canShoot);
        //int("isReloading: " + isReloading);
        if (!canShoot || isReloading)
            yield break;

        print("Shooting in subMachine");
        currentHeat += overHeatRate;
        if (bulletSpawn == null || bulletSpawn != masterInput.instance.bulletSpawn)
            bulletSpawn = masterInput.instance.bulletSpawn;


        canShoot = false;

        GameObject bullet = projectileManager.Instance.getProjectile("subMachinePool", bulletSpawn.position, bulletSpawn.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50f; // Standard speed
        EffectsManager.instance.getFromPool("subMachineFlash", bulletSpawn.position, bulletSpawn.rotation, true, true);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("Laser");
        yield return new WaitForSeconds(fireRateTime);

        canShoot = true;
        yield break;
    }
}

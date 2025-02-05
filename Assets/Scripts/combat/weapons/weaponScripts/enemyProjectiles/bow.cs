using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow : weaponType
{
    private enemyArcher archer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }

    public override IEnumerator Shoot()
    {
        if (!canShoot || isReloading)
            yield break;

        if (bulletSpawn == null || bulletSpawn != archer.arrowSpawn)
            bulletSpawn = archer.arrowSpawn;

        print("Starting shoot coroutine in bow");

        canShoot = false;

        bulletCount--;
        GameObject arrow = projectileManager.Instance.getProjectile("archerPool", bulletSpawn.position, bulletSpawn.rotation);
        arrow.GetComponent<bowProj>().setArcher(archer);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * 50f; // Standard speed
        EffectsManager.instance.getFromPool("bowShot", bulletSpawn.position, bulletSpawn.rotation);
        yield return new WaitForSeconds(fireRateTime);

        canShoot = true;
        yield break;
    }

    public void setArcher(enemyArcher arch)
    {
        archer = arch;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class weaponType : MonoBehaviour
{

    public Transform bulletSpawn;
    public float fireRateTime;
    public int magSize;
    public int bulletCount;
    public float reloadTime;
    public float rangeModifier;
    public bool canShoot = true;
    public bool isReloading = false;
    protected PlayerInput playerInput;
    protected projectileManager projectileManager;
    protected CharacterBase character;

    public abstract IEnumerator Shoot();

    public virtual IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        print("Reloading in weaponType");
        print(canShoot);
        //if (bulletCount == magSize)
            //yield break;
        //print("Reloading in pistolBasic");
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        bulletCount = magSize;
        isReloading = false;
        canShoot = true;
        print("canShoot in weaponType: " + canShoot);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = masterInput.instance.gameObject.GetComponent<PlayerInput>();
        bulletSpawn = masterInput.instance.bulletSpawn;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

}

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

    public float cooldownRate, cooldownVal, overHeatRate, overHeatMax, currentHeat;

    public abstract IEnumerator Shoot();

    public virtual IEnumerator Reload()
    {
        //playerAnimationController animator = masterInput.instance.gameObject.GetComponent<playerAnimationController>();
        //if (animator.getAnimationInfo().IsName("Reload Blend Tree") || animator.getAnimationInfo().IsName("engReloadBlendTree"))
            //yield break;
            if(isReloading)
                yield break;

        print("Reloading in weaponType");
        print(canShoot);
        //if (bulletCount == magSize)
            //yield break;
        //print("Reloading in pistolBasic");
        isReloading = true;
        canShoot = false;

        
        //if (animator.getAnimationInfo().IsName("Locomotion") && GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetLayerWeight(1) == 1)
        //{
            //print("activating reload anim");
            //animator.gunnerReload(reloadTime);
        //}
            
        yield return new WaitForSeconds(reloadTime);
        currentHeat = 0;
        //bulletCount = magSize;
        isReloading = false;
        canShoot = true;
        print("canShoot in weaponType: " + canShoot);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = masterInput.instance.gameObject.GetComponent<PlayerInput>();
        bulletSpawn = masterInput.instance.bulletSpawn;
        //Reload();
        currentHeat = 0;
        isReloading = false;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        isReloading = false;
        canShoot = true; 
    }

    public void setCanShoot(bool choice)
    {
        Debug.Log("setCanShoot: " + choice);
        canShoot = choice;
    }

    public void setIsReloading(bool choice)
    {
        Debug.Log("setIsReloading: " + choice);
        isReloading = choice;
    }

}

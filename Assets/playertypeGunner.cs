using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playertypeGunner : MonoBehaviour
{
    bool run = false;

    //bullet
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    //rifle
    bool isReloading = false;
    public float reloadTime = 2f;
    public float fireRateTime = .1f;
    bool canShoot = true;
    public ParticleSystem muzzleFlash;
    private ParticleSystem muzzleflash;
    public Transform flashSpawn;
    public int bulletCount;
    public int magSize = 10;
    public TMP_Text bulletUI;



    // Start is called before the first frame update
    void Start()
    {
        bulletCount = magSize;
        int temp = bulletCount;
        canShoot = true;
        //bulletUI.text = temp.ToString();
        //Instantiate(muzzleFlash, flashSpawn.position, flashSpawn.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletCount <= 0)
        {
            bulletCount = 0;
            canShoot = false;
            StartCoroutine(reload());
        }


        //muzzleFlash.transform.position = flashSpawn.transform.position;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(reload());
        }


        if (Input.GetButtonDown("Fire1") && isReloading == false && bulletCount > 0 && canShoot)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        canShoot = false;
        print("Shooting");
        while (Input.GetButton("Fire1") && bulletCount > 0 && isReloading == false)
        {
            bulletCount--;
            //int temp = bulletCount;
            //bulletUI.text = temp.ToString();
            //muzzleFlash.Play();
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //bullet.GetComponent<bullet>().setPower(true);
            bullet.GetComponent<Rigidbody>().velocity = -bulletSpawn.right * bulletSpeed;
            yield return new WaitForSeconds(fireRateTime);
        }
        canShoot = true;
        yield break;
    }

    IEnumerator reload()
    {
        print("reloading");
        if (bulletCount == magSize)
            yield break;

        isReloading = true;
        //FindObjectOfType<AudioManager>().Pause("AutoShot");
        yield return new WaitForSeconds(reloadTime);
        bulletCount = magSize;
        //int temp = bulletCount;
        //bulletUI.text = temp.ToString();
        isReloading = false;
        canShoot = true;
        yield break;
    }

    public void setRun(bool choice)
    {
        run = choice;
    }
}
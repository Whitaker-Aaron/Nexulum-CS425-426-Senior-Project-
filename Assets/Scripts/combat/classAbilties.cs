/*-----------------------------------------------------
 * classAbilities Script
 * Author: Spencer Garcia
 * Start Date: 9/26/2024
 * 
 * Description:
 * 
 * class ability manager, functions activated from masterInput.
 * base stats contained in this file but could be changed
 * when we implement the skill trees.
 * 
 * --------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class classAbilties : MonoBehaviour
{

    //----------------Variables------------------

    GameObject player;
    public LayerMask enemy;

    //private WeaponBase.weaponClassTypes currentClass;

    //Knight
    bool bubble = false;
    public float bubbleTime = 5f;
    public GameObject knightBubblePrefab;
    public float bubbleRadius;
    public GameObject knightBubbleEffect;

    public GameObject swordShot;
    public Transform swordSpawn;
    public float swordSpeed;
    public float swordTime, swordAbilityTime;
    bool shootingSwords, shotSword = false;
    public GameObject swordShotEffect;


    //Gunner
    public GameObject rocketPrefab;
    public float rocketSpeed = 5f;
    public float rocketTime;
    bool shootingRocket, shotRocket = false;

    bool shootingLaser, shotLaser, checkHit = false;
    public GameObject laserPrefab;
    public float laserCooldown = 4f;
    GameObject currentLaserEffect = null;
    public float maxLaserDistance = 10f;
    public int laserDamage = 2;
    public float laserHitRate = .4f;

    bool throwingGrenade, threwGrenade = false;
    public GameObject grenadePrefab;
    public float grenadeSpeed;
    public Transform grenadeSpawn;
    public float grenadeCooldown;


    //Engineer
    public GameObject turretPrefab;
    public GameObject turretTransparentPrefab;
    GameObject currentTurret;
    public float turretPlacementRadius = 3f;
    public float playerRad = .3f;
    bool placing = false;
    Vector3 mousePos = Vector3.zero;
    public float turretSpawnHeight;
    bool instant = false;
    public LayerMask ground;

    bool placingTesla, placingOne, placingTwo = false;
    public GameObject teslaTransparent;
    public GameObject teslaPrefab;
    GameObject currentTesla, nextCurrentTesla = null;
    int teslaCount = 0;
    float teslaDistance;
    Vector3 teslaDirection;
    public float teslaSpawnHeight;
    public float teslaPlacementRadius = 4f;
    public GameObject teslaWall;

    //-------------------------------------------

    //----------------Functions------------------

    public void activateAbilityOne(WeaponBase.weaponClassTypes currentClass)
    {
        if(currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            StartCoroutine(bubbleShield());
        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {
            gameObject.GetComponent<masterInput>().shootingRocket = true;
            shootingRocket = true;
        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            placing = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
            //currentTurret = turretTransparentPrefab;
            
        }
    }

    public void activateAbilityTwo(WeaponBase.weaponClassTypes currentClass)
    {
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner && !throwingGrenade)
        {
            throwingGrenade = true;
            gameObject.GetComponent<masterInput>().throwingGrenade = true;
        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            placingTesla = true;
            placingOne = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
        }
    }

    public void activateAbilityThree(WeaponBase.weaponClassTypes currentClass)
    {
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            shootingSwords = true;
            
            GameObject currentEffect = Instantiate(swordShotEffect, player.transform.position, Quaternion.identity);
            currentEffect.transform.SetParent(player.transform);
            currentEffect.transform.position = player.transform.position;
            currentEffect.GetComponent<ParticleSystem>().Play();

            StartCoroutine(stopSword(currentEffect));
        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingRocket)
        {
            shootingLaser = true;
            gameObject.GetComponent<masterInput>().shootingLaser = true;
            Transform pos = gameObject.GetComponent<masterInput>().bulletSpawn;
            currentLaserEffect = Instantiate(laserPrefab, pos.position, Quaternion.identity);
            currentLaserEffect.GetComponent<ParticleSystem>().Stop();
            currentLaserEffect.transform.SetParent(player.transform, false);
            pos = gameObject.GetComponent<masterInput>().bulletSpawn;
            currentLaserEffect.transform.position = pos.position;
            StartCoroutine(laserStop());
        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {

        }
    }

    //Knight

    IEnumerator bubbleShield()
    {
        bubble = true;
        player.GetComponent<CharacterBase>().bubbleShield = true;
        print("Activating shield");

        GameObject currentShield = Instantiate(knightBubblePrefab, player.transform.position, Quaternion.identity);
        currentShield.transform.SetParent(player.transform, false);
        currentShield.transform.position = player.transform.position;
        currentShield.GetComponent<CapsuleCollider>().radius = bubbleRadius;
        currentShield.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);

        GameObject currentEffect = Instantiate(knightBubbleEffect, player.transform.position, Quaternion.identity);
        currentEffect.transform.SetParent(player.transform);
        currentEffect.transform.position = player.transform.position;
        currentEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(bubbleTime);
        Destroy(currentShield);
        bubble = false;
        player.GetComponent<CharacterBase>().bubbleShield = false;
        print("Deactivating shield");
        if(currentEffect != null)
        {
            currentEffect.GetComponent<ParticleSystem>().Stop();
            Destroy(currentEffect);
        }
        yield break;
    }

    IEnumerator swordShooting()
    {
        shotSword = true;
        var sword = Instantiate(swordShot, swordSpawn.position, swordSpawn.rotation);
        sword.GetComponent<Rigidbody>().velocity = swordSpawn.transform.forward * swordSpeed;
        yield return new WaitForSeconds(swordTime);
        shotSword = false;
        yield break;
    }

    IEnumerator stopSword(GameObject currentEffect)
    {
        yield return new WaitForSeconds(swordAbilityTime);
        gameObject.GetComponent<playerAnimationController>().stopShootSword();
        gameObject.GetComponent<masterInput>().shootingSwords = false;
        shootingSwords = false;
        currentEffect.GetComponent<ParticleSystem>().Stop();
        Destroy (currentEffect);
        yield break;
    }

    //Gunner

    IEnumerator shootRocket()
    {
        shotRocket = true;
        var rocket = Instantiate(rocketPrefab, swordSpawn.position, swordSpawn.rotation);
        rocket.GetComponent<Rigidbody>().velocity = swordSpawn.transform.forward * rocketSpeed;
        gameObject.GetComponent<masterInput>().shootingRocket = true;
        yield return new WaitForSeconds(rocketTime);
        shotRocket = false;
        shootingRocket = false;
        gameObject.GetComponent<masterInput>().shootingRocket = false;
        yield break;
    }

    IEnumerator laserStop()
    {
        yield return new WaitForSeconds(laserCooldown);
        shootingLaser = false;
        gameObject.GetComponent<masterInput>().shootingLaser = false;
        currentLaserEffect.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Destroy(currentLaserEffect);
        yield break;
    }

    IEnumerator checkLaserHit()
    {
        if (!shootingLaser)
            yield break;
        checkHit = true;
        RaycastHit hit;
        //Transform pos = gameObject.GetComponent<masterInput>().bulletSpawn;
        if (Physics.Raycast(currentLaserEffect.transform.position, currentLaserEffect.transform.forward, out hit, maxLaserDistance, enemy))
        {
            print("Hitting enemy");
            hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(laserDamage);
        }
        yield return new WaitForSeconds(laserHitRate);
        checkHit = false;
        yield break;
    }

    IEnumerator grenadeWait(float time)
    {
        yield return new WaitForSeconds(time);
        throwingGrenade = false;
        threwGrenade = false;
        yield break;
    }

    //Engineer

   

    void activateTesla()
    {
        
        //print("activating turret");
        //player look at cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, 100, ground))
        {
            mousePos = hit.point;
            
        }

        if (instant)
        {
            instant = false;
            if(teslaCount == 0)
            {
                currentTesla = GameObject.Instantiate(teslaTransparent, player.transform.position, Quaternion.LookRotation(player.transform.forward));
            }
            if(teslaCount == 1)
            {
                nextCurrentTesla = GameObject.Instantiate(teslaTransparent, player.transform.position, Quaternion.LookRotation(player.transform.forward));
            }
            
        }

        if(teslaCount == 0)
        {
            print("updating 0");
            teslaDistance = Vector3.Distance(player.transform.position, mousePos);
            teslaDirection = (mousePos - player.transform.position).normalized;

            if (teslaDistance <= turretPlacementRadius && teslaDistance > playerRad)
            {
                currentTesla.gameObject.transform.position = mousePos;
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
            else if (teslaDistance <= playerRad)
            {
                currentTesla.transform.position = player.transform.position + teslaDirection * (playerRad + 0.1f);  // Small buffer to avoid overlap

                // Ensure the turret stays above the ground to avoid going under the player
                currentTesla.transform.position = new Vector3(
                    currentTesla.transform.position.x,
                    Mathf.Max(currentTesla.transform.position.y, player.transform.position.y + 0.03f),  // Ensure turret stays slightly above player's Y position
                    currentTesla.transform.position.z
                );
                currentTesla.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                //Vector3 Direction = (mousePos - player.transform.position).normalized;
                //currentTurret.gameObject.transform.position = player.transform.position + Direction * playerRad;
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
            else
            {
                //Vector3 direction = (mousePos - player.transform.position).normalized;
                currentTesla.gameObject.transform.position = player.transform.position + teslaDirection * turretPlacementRadius;
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
        }
        if(teslaCount == 1)
        {
            print("updating 1");
            teslaDistance = Vector3.Distance(currentTesla.transform.position, mousePos);
            teslaDirection = (mousePos - currentTesla.transform.position).normalized;

            if (teslaDistance <= teslaPlacementRadius && teslaDistance > playerRad)
            {
                nextCurrentTesla.gameObject.transform.position = mousePos;
                //nextCurrentTesla.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
            else if (teslaDistance <= playerRad)
            {
                nextCurrentTesla.transform.position = currentTesla.transform.position + teslaDirection * (playerRad + 0.1f);  // Small buffer to avoid overlap

                // Ensure the turret stays above the ground to avoid going under the player
                nextCurrentTesla.transform.position = new Vector3(
                    nextCurrentTesla.transform.position.x,
                    Mathf.Max(nextCurrentTesla.transform.position.y, currentTesla.transform.position.y + 0.03f),  // Ensure turret stays slightly above player's Y position
                    nextCurrentTesla.transform.position.z
                );
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                //Vector3 Direction = (mousePos - player.transform.position).normalized;
                //currentTurret.gameObject.transform.position = player.transform.position + Direction * playerRad;
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
            else
            {
                //Vector3 direction = (mousePos - player.transform.position).normalized;
                nextCurrentTesla.gameObject.transform.position = currentTesla.transform.position + teslaDirection * teslaPlacementRadius;
                //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
        }
        


        if(Input.GetMouseButtonDown(0) && teslaCount == 0 && placingOne && !placingTwo) 
        {
            print("activate 0");
            teslaCount = 1;
            instant = true;
            StartCoroutine(teslaWait());
            if (currentTesla != null)// && teslaDistance <= turretPlacementRadius)
            {
                //placing = false;
                //gameObject.GetComponent<masterInput>().placing = false;
                //Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTesla.transform.position;
                Destroy(currentTesla);
                currentTesla = Instantiate(teslaPrefab, pos + new Vector3 (0,teslaSpawnHeight,0), Quaternion.identity);
            }
            /*
            if(currentTurret != null && (teslaDistance > turretPlacementRadius || teslaDistance < playerRad))
            {
                //placing = false;
                //gameObject.GetComponent<masterInput>().placing = false;
                //Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTesla.transform.position;
                Destroy(currentTesla);
                currentTesla = Instantiate(teslaPrefab, pos + new Vector3(0, teslaSpawnHeight, 0), Quaternion.identity);
            }
            */
            

        }
        if(Input.GetMouseButtonDown(0) && teslaCount == 1 && !placingOne && placingTwo)
        {
            print("activate 1");
            teslaCount = 0;
            if (nextCurrentTesla != null)// && teslaDistance <= turretPlacementRadius)
            {
                placingTesla = false;
                gameObject.GetComponent<masterInput>().placing = false;
                //Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = nextCurrentTesla.transform.position;
                Destroy(nextCurrentTesla);
                nextCurrentTesla = Instantiate(teslaPrefab, pos + new Vector3(0, teslaSpawnHeight, 0), Quaternion.identity);
            }
            placingOne = false;
            placingTwo = false;

            Vector3 difference = (nextCurrentTesla.transform.position - currentTesla.transform.position) / 2;

            var tesWall = GameObject.Instantiate(teslaWall, nextCurrentTesla.transform.position - difference + new Vector3(0, 1, 0), Quaternion.identity);// Quaternion.Euler(difference + new Vector3(0, 90, 0)));
            tesWall.transform.LookAt(new Vector3(nextCurrentTesla.transform.position.x, 1, nextCurrentTesla.transform.position.z));
            
            tesWall.transform.SetParent(currentTesla.transform);
            /*
            if (currentTurret != null && (teslaDistance > turretPlacementRadius || teslaDistance < playerRad))
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                //Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = nextCurrentTesla.transform.position;
                Destroy(nextCurrentTesla);
                nextCurrentTesla = Instantiate(teslaPrefab, pos + new Vector3(0, teslaSpawnHeight, 0), Quaternion.identity);
            }
            */
        }
    }

    IEnumerator teslaWait()
    {
        yield return new WaitForSeconds(.5f);
        placingOne = false;
        placingTwo = true;
        yield break;
    }


    void activateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100, ground))
        {
            mousePos = hit.point;

        }

        if (instant)
        {
            instant = false;
            currentTurret = GameObject.Instantiate(turretTransparentPrefab, player.transform.position, Quaternion.LookRotation(player.transform.forward));
        }

        float distance = Vector3.Distance(player.transform.position, mousePos);
        Vector3 direction = (mousePos - player.transform.position).normalized;

        if (distance <= turretPlacementRadius && distance > playerRad)
        {
            currentTurret.gameObject.transform.position = mousePos;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else if (distance <= playerRad)
        {
            currentTurret.transform.position = player.transform.position + direction * (playerRad + 0.1f);  // Small buffer to avoid overlap

            // Ensure the turret stays above the ground to avoid going under the player
            currentTurret.transform.position = new Vector3(
                currentTurret.transform.position.x,
                Mathf.Max(currentTurret.transform.position.y, player.transform.position.y + 0.03f),  // Ensure turret stays slightly above player's Y position
                currentTurret.transform.position.z
            );
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            //Vector3 Direction = (mousePos - player.transform.position).normalized;
            //currentTurret.gameObject.transform.position = player.transform.position + Direction * playerRad;
            //currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else
        {
            //Vector3 direction = (mousePos - player.transform.position).normalized;
            currentTurret.gameObject.transform.position = player.transform.position + direction * turretPlacementRadius;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentTurret != null && distance <= turretPlacementRadius)
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTurret.transform.position;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, pos + new Vector3(0, turretSpawnHeight, 0), rot);
            }
            /*
            if (currentTurret != null && (distance > turretPlacementRadius || distance < playerRad))
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTurret.transform.position;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, pos + new Vector3(0, turretSpawnHeight, 0), rot);
            }
            */

        }
    }

    //-------------------------------------------


    //--------------Main Functions---------------

    private void Awake()
    {
        //currentClass = gameObject.GetComponent<masterInput>().currentClass;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(placing)
        {
            activateTurret();
        }
        if(placingTesla)
        {
            activateTesla();
        }

        if(shootingSwords)
        {
            if(Input.GetMouseButtonDown(0) && !shotSword)
            {
                StartCoroutine(swordShooting());
            }
        }

        if(shootingRocket)
        {
            if (Input.GetMouseButtonDown(0) && !shotRocket)
                StartCoroutine(shootRocket());
        }

        if(shootingLaser)
        {
            if(Input.GetMouseButtonDown(0) && !shotLaser)
            {
                shotLaser = true;

                //if (currentLaserEffect != null)
                currentLaserEffect.GetComponent<ParticleSystem>().Clear(true);
                currentLaserEffect.GetComponent<ParticleSystem>().Play();
            }
            if(Input.GetMouseButtonUp(0) && shotLaser)
            {
                shotLaser = false;
                //if (currentLaserEffect != null)
                currentLaserEffect.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                currentLaserEffect.GetComponent<ParticleSystem>().Clear(true);
            }
        }
        else
        {
            shotLaser = false;
            shootingLaser = false;
            if (currentLaserEffect != null)
            {
                currentLaserEffect.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                currentLaserEffect.GetComponent<ParticleSystem>().Clear(true);
            }
        }

        if (shotLaser && !checkHit)
        {
            StartCoroutine(checkLaserHit());
        }

        if(throwingGrenade && !threwGrenade)
        {
            if(Input.GetMouseButtonDown(0))
            {
                threwGrenade = true;
                gameObject.GetComponent<masterInput>().throwingGrenade = false;
                GameObject grenade = Instantiate(grenadePrefab, grenadeSpawn.transform.position, grenadeSpawn.transform.rotation);
                grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * grenadeSpeed;
                StartCoroutine(grenade.GetComponent<grenade>().explode());
                StartCoroutine(grenadeWait(grenadeCooldown));
            }
            
        }
    }
}

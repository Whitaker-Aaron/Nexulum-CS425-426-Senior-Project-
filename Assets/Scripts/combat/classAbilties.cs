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
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {

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
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

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
        yield break;
    }

    //Engineer

   

    void activateTurret()
    {
        print("activating turret");
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
            currentTurret = GameObject.Instantiate(turretTransparentPrefab, player.transform.position, Quaternion.LookRotation(player.transform.forward));
        }

        float distance = Vector3.Distance(player.transform.position, mousePos);
        Vector3 direction = (mousePos - player.transform.position).normalized;

        if (distance <= turretPlacementRadius && distance > playerRad)
        {
            currentTurret.gameObject.transform.position = mousePos;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else if(distance <= playerRad)
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

        if(Input.GetMouseButtonDown(0)) 
        {
            if (currentTurret != null && distance <= turretPlacementRadius)
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTurret.transform.position;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, pos + new Vector3 (0,turretSpawnHeight,0), rot);
            }
            if(currentTurret != null && (distance > turretPlacementRadius || distance < playerRad))
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTurret.transform.position;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, pos + new Vector3(0, turretSpawnHeight, 0), rot);
            }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(placing)
        {
            activateTurret();
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
    }
}

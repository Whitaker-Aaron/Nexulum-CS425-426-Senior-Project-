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
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class classAbilties : MonoBehaviour
{

    //----------------Variables------------------

    public static classAbilties instance;

    GameObject player;
    public LayerMask enemy;
    public LayerMask playerLayer;
    UIManager uiManager;

    private PlayerInput playerInput;
    private bool usingMouseInput = false;

    bool isGamepadLooking = false;
    bool isMouseLooking = false;
    bool casting = false;
    public float minPlacementDistance = .5f;
    public float maxPlacementDistance = 2.5f;
    public Vector3 spawnOffset = new Vector3(0, .2f, 0);

    Vector3 lookPos = Vector3.zero;
    Vector3 lookDir = Vector3.zero;

    //Knight
    [HideInInspector] public bool bubble = false;
    public float bubbleTime = 5f;
    [SerializeField] private GameObject knightBubblePrefab;
    public float bubbleRadius;
    [SerializeField] private GameObject knightBubbleEffect;

    [SerializeField] private GameObject swordShot;
    [SerializeField] private Transform swordSpawn;
    public float swordSpeed;
    public float swordTime, swordAbilityTime;
    bool shootingSwords, shotSword = false;
    public int swordShotDamage = 15;
    [SerializeField] private GameObject swordShotEffect;
    [SerializeField] private GameObject swordShotIceEffect;
    [SerializeField] private GameObject combatAuraEffectStart, combatAuraEffectEnd, combatAuraEffectLoop;
    public float comatAuraRadius = 4f;
    private bool activatedAura, checkingAura = false;
    public float auraTime = 5f;
    private Vector3 currentAura;
    bool buffingPlayer = false;
    public float buffRate = .5f;
    public int auraAttackBuff = 25;
    


    //Gunner
    [SerializeField] private GameObject rocketPrefab;
    public float rocketSpeed = 5f;
    public float rocketTime;
    bool shootingRocket, shotRocket = false;

    bool shootingLaser, shotLaser, checkHit = false;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject laserIcePrefab;
    public float laserCooldown = 4f;
    GameObject currentLaserEffect = null;
    public float maxLaserDistance = 10f;
    public int laserDamage = 2;
    public int laserIceDamage = 0;
    public float laserHitRate = .4f;

    bool throwingGrenade, threwGrenade = false;
    public GameObject grenadePrefab;
    public float grenadeSpeed;
    public Transform grenadeSpawn;
    public float grenadeCooldown;

    //skill tree 
    bool SSExplode = false;


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
    public GameObject teslaPrefab, teslaParentPrefab;
    GameObject currentTesla, nextCurrentTesla = null;
    int teslaCount = 0;
    float teslaDistance;
    Vector3 teslaDirection;
    public float teslaSpawnHeight;
    public float teslaPlacementRadius = 4f;
    public float teslaMinPlacementRad = 2.5f;
    public GameObject teslaWall;//, teslaParent;
    GameObject newTesla = null, newTesla2 = null;

    int teslaNumCount, turretNumCount = 0;
    public int teslaMaxQuantity, turretMaxQuantity;
    private int towerMaxQuantity;

    private GameObject[] placedTowers;
    private int totalTowerCount;

    [SerializeField] private GameObject clonePrefab;
    public GameObject currentClone;


    //cooldown rates
    public float ka1Time, ka2Time, ka3Time;
    public float ga1Time, ga2Time, ga3Time;
    public float ea1Time, ea2Time, ea3Time;
    public bool a1cooldown, a2cooldown, a3cooldown = false;
    private Coroutine acc1, acc2, acc3;


    //skill tree start
    SkillTreeManager skillTreeManagerObj;


    //------------------Runes--------------------
    [HideInInspector] public bool fireBool, iceBool, earthBool, windBool, electricBool, waterBool = false;

    //knight


    [SerializeField] private float krFireTime, krFireRate;
    [SerializeField] private int krFireDmg;

    //-------------------------------------------

    //----------------Functions------------------

    public void activateAbilityOne(WeaponBase.weaponClassTypes currentClass)
    {
        if (a1cooldown || acc1 != null || turretNumCount == turretMaxQuantity)
        {
            Debug.Log("Ability 1 is on cooldown.");
            return;
        }
        Debug.Log("Activating Ability 1.");

        
        a1cooldown = true;

        if (currentClass == WeaponBase.weaponClassTypes.Knight && !bubble)
        {
            uiManager.ActivateCooldownOnAbility(1);
            StartCoroutine(bubbleShield());
            acc1 = StartCoroutine(abilitiesCooldown(1, ka1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
            gameObject.GetComponent<masterInput>().bubble = true;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingRocket)
        {
            uiManager.ActivateCooldownOnAbility(1);
            gameObject.GetComponent<masterInput>().shootingRocket = true;
            shootingRocket = true;
            StartCoroutine(abilitiesCooldown(1, ga1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && turretNumCount < turretMaxQuantity  && teslaNumCount < teslaMaxQuantity && !placing)
        {
            if(turretNumCount == turretMaxQuantity)
            {
                return;
            }
            uiManager.ActivateCooldownOnAbility(1);
            print("turretCount = " + turretNumCount);
            placing = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
            StartCoroutine(abilitiesCooldown(1, ea1Time));
            //currentTurret = turretTransparentPrefab;

        }
        else
            return;
    }

    public void activateAbilityTwo(WeaponBase.weaponClassTypes currentClass)
    {
        if (a2cooldown)
        {
            Debug.Log("Ability 2 is on cooldown.");
            return;
        }
        Debug.Log("Activating Ability 2.");
        uiManager.ActivateCooldownOnAbility(2);
        a2cooldown = true;

        if (currentClass == WeaponBase.weaponClassTypes.Knight && !activatedAura)
        {
            activatedAura = true;
            StartCoroutine(auraWait());
            acc2 = StartCoroutine(abilitiesCooldown(2, ka2Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !throwingGrenade)
        {
            throwingGrenade = true;
            gameObject.GetComponent<masterInput>().throwingGrenade = true;
            StartCoroutine(abilitiesCooldown(2, ga2Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && teslaNumCount < teslaMaxQuantity && !placing)
        {
            teslaNumCount += 1;
            totalTowerCount += 1;
            placingTesla = true;
            placingOne = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
            StartCoroutine(abilitiesCooldown(2, ea2Time));
        }
    }

    public void activateAbilityThree(WeaponBase.weaponClassTypes currentClass)
    {
        if (a3cooldown)
        {
            Debug.Log("Ability 3 is on cooldown.");
            return;
        }
        Debug.Log("Activating Ability 3.");
        uiManager.ActivateCooldownOnAbility(3);
        a3cooldown = true;

        if (currentClass == WeaponBase.weaponClassTypes.Knight && !shootingSwords)
        {
            shootingSwords = true;
            gameObject.GetComponent<masterInput>().shootingSwords = true;

            if (iceBool)
            {
                GameObject currentEffect = Instantiate(swordShotIceEffect, player.transform.position, Quaternion.identity);
                currentEffect.transform.SetParent(player.transform);
                currentEffect.transform.position = player.transform.position;
                currentEffect.GetComponent<ParticleSystem>().Play();
                StartCoroutine(stopSword(currentEffect));
            }
            else
            {
                GameObject currentEffect = Instantiate(swordShotEffect, player.transform.position, Quaternion.identity);
                currentEffect.transform.SetParent(player.transform);
                currentEffect.transform.position = player.transform.position;
                currentEffect.GetComponent<ParticleSystem>().Play();
                StartCoroutine(stopSword(currentEffect));
            }

            
            acc3 = StartCoroutine(abilitiesCooldown(3, ka3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingLaser)
        {
            shootingLaser = true;
            gameObject.GetComponent<masterInput>().shootingLaser = true;
            Transform pos = gameObject.GetComponent<masterInput>().bulletSpawn;
            if(iceBool)
            {
                currentLaserEffect = Instantiate(laserIcePrefab, pos.position, Quaternion.identity);
                currentLaserEffect.GetComponent<ParticleSystem>().Stop();
                currentLaserEffect.transform.SetParent(player.transform, false);
                pos = gameObject.GetComponent<masterInput>().bulletSpawn;
                currentLaserEffect.transform.position = pos.position;
                currentLaserEffect.transform.forward = masterInput.instance.bulletSpawn.forward;
                StartCoroutine(laserStop());
            }
            else
            {
                currentLaserEffect = Instantiate(laserPrefab, pos.position, Quaternion.identity);
                currentLaserEffect.GetComponent<ParticleSystem>().Stop();
                currentLaserEffect.transform.SetParent(player.transform, false);
                pos = gameObject.GetComponent<masterInput>().bulletSpawn;
                currentLaserEffect.transform.position = pos.position;
                currentLaserEffect.transform.forward = masterInput.instance.bulletSpawn.forward;
                StartCoroutine(laserStop());
            }
            
            acc3 = StartCoroutine(abilitiesCooldown(3, ga3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            currentClone = Instantiate(clonePrefab, player.transform.position, player.transform.rotation);
            acc3 = StartCoroutine(abilitiesCooldown(3, ea3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
    }

    IEnumerator abilitiesCooldown(int ability, float time)
    {
        if (turretNumCount == turretMaxQuantity || teslaNumCount == teslaMaxQuantity)
            yield break;
        yield return StartCoroutine(uiManager.StartCooldownSlider(ability, (0.98f/time)));
        //yield return new WaitForSeconds(time);

        switch (ability)
        {
            case 1:
                yield return new WaitForSeconds(0.2f);
                a1cooldown = false;
                acc1 = null;
                print("ability 1 done");
          
                break;
            case 2:
                yield return new WaitForSeconds(0.2f);
                a2cooldown = false;
                acc2 = null;
                print("ability 2 done");
                break;
            case 3:
                yield return new WaitForSeconds(0.2f);
                a3cooldown = false;
                acc3 = null;
                print("ability 3 done");
                break;
        }
        uiManager.DeactivateCooldownOnAbility(ability);
        switch(masterInput.instance.currentClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                masterInput.instance.shootingSwords = false;
                break;

            case WeaponBase.weaponClassTypes.Gunner:
                masterInput.instance.shootingRocket = false;
                masterInput.instance.shootingLaser = false;
                masterInput.instance.throwingGrenade = false;
                break;

            case WeaponBase.weaponClassTypes.Engineer:
                //masterInput.instance.placing = false;
                break;
        }


        yield break;
    }


    

    //Knight

    IEnumerator bubbleShield()
    {
        bubble = true;
        player.GetComponent<CharacterBase>().bubbleShield = true;
        print("Activating shield");

        if(earthBool)
        {
            EffectsManager.instance.getFromPool("earthShield", player.transform.position, Quaternion.identity, true, false);
            yield return new WaitForSeconds(.5f);
            EffectsManager.instance.getFromPool("earthShield", player.transform.position, Quaternion.identity, true, false);
            yield return new WaitForSeconds(bubbleTime);
            EffectsManager.instance.getFromPool("earthShield", player.transform.position, Quaternion.identity, true, false);
        }
        else
        {
            EffectsManager.instance.getFromPool("bubbleShield", player.transform.position, Quaternion.identity, true, false);
            yield return new WaitForSeconds(.5f);
            EffectsManager.instance.getFromPool("bubbleShield", player.transform.position, Quaternion.identity, true, false);
            yield return new WaitForSeconds(bubbleTime);
            EffectsManager.instance.getFromPool("bubbleShield", player.transform.position, Quaternion.identity, true, false);
        }
        //Instantiate(knightBubblePrefab, player.transform.position, Quaternion.identity);
        //currentShield.transform.SetParent(player.transform, false);
        //currentShield.transform.position = player.transform.position;
        //currentShield.GetComponent<CapsuleCollider>().radius = bubbleRadius;
        //currentShield.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);

        //GameObject currentEffect = Instantiate(knightBubbleEffect, player.transform.position, Quaternion.identity);
        //currentEffect.transform.SetParent(player.transform);
        //currentEffect.transform.position = player.transform.position;
        //currentEffect.GetComponent<ParticleSystem>().Play();
        //yield return new WaitForSeconds(bubbleTime);
        //Destroy(currentShield);
        bubble = false;
        player.GetComponent<CharacterBase>().bubbleShield = false;
        print("Deactivating shield");
        //if(currentEffect != null)
        //{
            //currentEffect.GetComponent<ParticleSystem>().Stop();
            //Destroy(currentEffect);
        //}
        yield break;
    }

    IEnumerator auraWait()
    {
        checkingAura = true;
        currentAura = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        //GameObject tempEffect = Instantiate(combatAuraEffectStart, currentAura, Quaternion.identity);
        if(!fireBool)
        {
            EffectsManager.instance.getFromPool("caPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            yield return new WaitForSeconds(.3f);
            EffectsManager.instance.getFromPool("caPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            yield return new WaitForSeconds(auraTime);
            EffectsManager.instance.getFromPool("caPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            checkingAura = false;
            activatedAura = false;
            currentAura = Vector3.zero;
        }
        else
        {
            EffectsManager.instance.getFromPool("faPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            yield return new WaitForSeconds(.3f);
            EffectsManager.instance.getFromPool("faPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            yield return new WaitForSeconds(auraTime);
            EffectsManager.instance.getFromPool("faPool", currentAura + new Vector3(0, .1f, 0), Quaternion.identity, false, false);
            checkingAura = false;
            activatedAura = false;
            currentAura = Vector3.zero;
        }
        
        /*
        tempEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.3f);
        tempEffect.GetComponent<ParticleSystem>().Stop();
        GameObject tempEffect2 = Instantiate(combatAuraEffectLoop, currentAura, Quaternion.identity);
        tempEffect2.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(auraTime);
        checkingAura = false;
        activatedAura = false;
        tempEffect2.GetComponent<ParticleSystem>().Stop();
        Destroy(tempEffect);
        tempEffect = Instantiate(combatAuraEffectEnd, currentAura, Quaternion.identity);
        tempEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.5f);
        tempEffect.GetComponent<ParticleSystem>().Stop();
        Destroy(tempEffect);
        Destroy(tempEffect2);
        currentAura = Vector3.zero;
        */
        yield break;
    }

    void buff(bool choice, string name, GameObject c)
    {
        
        if (name == "attack")
        {
            
            if (choice)
            {
                switch (gameObject.GetComponent<masterInput>().currentClass)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        //int temp = c.gameObject.GetComponent<CharacterBase>().knightObject.baseAttack;
                        c.GetComponent<CharacterBase>().buffPlayer(choice, "attack", auraAttackBuff);
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:

                        break;
                    case WeaponBase.weaponClassTypes.Engineer:

                        break;
                }
                //buffingPlayer = false;
            }
            else
            {
                switch (gameObject.GetComponent<masterInput>().currentClass)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        //int temp = c.gameObject.GetComponent<CharacterBase>().knightObject.baseAttack;
                        c.GetComponent<CharacterBase>().buffPlayer(choice, "attack", auraAttackBuff);
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:

                        break;
                    case WeaponBase.weaponClassTypes.Engineer:

                        break;
                }
            }
            
        }
        
    }

    IEnumerator swordShooting()
    {
        shotSword = true;

        if(iceBool)
        {
            //print(projectileManager.Instance);
            GameObject sword = projectileManager.Instance.getProjectile("swordShotIcePool", swordSpawn.position, swordSpawn.rotation);
            sword.GetComponent<swordShot>().isIce = true;
            //sword.GetComponent<Rigidbody>().velocity = swordSpawn.transform.forward * swordSpeed;
            //sword.GetComponent<swordShot>().damage = swordShotDamage;
        }
        else if(SSExplode)
        {
            GameObject sword = projectileManager.Instance.getProjectile("swordShotExplodePool", swordSpawn.position, swordSpawn.rotation);
            sword.GetComponent<swordShot>().activateExplosion();
            //sword.GetComponent<Rigidbody>().velocity = swordSpawn.forward * swordSpeed;
            //sword.GetComponent<swordShot>().damage = swordShotDamage;
        }
        else
        {
            //print(projectileManager.Instance);
            GameObject sword = projectileManager.Instance.getProjectile("swordShotPool", swordSpawn.position, swordSpawn.rotation);
            //sword.GetComponent<Rigidbody>().velocity = swordSpawn.forward * swordSpeed;
            //sword.GetComponent<swordShot>().damage = swordShotDamage;
        }
            

        
        yield return new WaitForSeconds(swordTime);
        shotSword = false;
        yield break;
    }

    IEnumerator stopSword(GameObject currentEffect)
    {
        yield return new WaitForSeconds(swordAbilityTime);
        gameObject.GetComponent<playerAnimationController>().stopShootSword();
        yield return new WaitUntil(() => gameObject.GetComponent<playerAnimationController>().getAnimationInfo().IsName("Locomotion"));
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

        if(fireBool)
            rocket.GetComponent<rocket>().fireB = true;

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

        if (Physics.Raycast(masterInput.instance.bulletSpawn.transform.position, masterInput.instance.bulletSpawn.transform.forward, out hit, maxLaserDistance, enemy))
        {
            print("Hitting enemy");
            hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(laserDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            if(iceBool)
            {
                hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(laserIceDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
            }
        }
        yield return new WaitForSeconds(laserHitRate);
        checkHit = false;
        yield break;
    }

    IEnumerator grenadeWait(float time)
    {
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<masterInput>().throwingGrenade = false;
        yield return new WaitForSeconds(time);
        throwingGrenade = false;
        threwGrenade = false;
        yield break;
    }

    //Engineer

    public void mouseTurretPlace(InputAction.CallbackContext context)
    {


        if (casting && currentTurret != null)
        {

            if (masterInput.instance.inputPaused || isGamepadLooking)
                return;

            isMouseLooking = true;
            isGamepadLooking = false;

            Vector2 mousePosition = context.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                lookPos = hit.point;
            }


            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                currentTurret.transform.position = lookPos;// + spawnOffset;
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                currentTurret.transform.position = player.transform.position + direction * (minPlacementDistance + 0.1f); // Small buffer to avoid overlap
                currentTurret.transform.position = new Vector3(
                    currentTurret.transform.position.x,
                    0,//spawnOffset.y,
                    currentTurret.transform.position.z
                );
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else
            {
                currentTurret.transform.position = player.transform.position + direction * maxPlacementDistance;// + spawnOffset;
                currentTurret.transform.rotation = player.transform.rotation;
            }
        }




    }

    public void mouseTeslaPlace(InputAction.CallbackContext context)
    {
        if (!casting)
            return;
        if (teslaCount == 0 && currentTesla != null)
        {

            if (masterInput.instance.inputPaused || isGamepadLooking)
                return;

            isMouseLooking = true;
            isGamepadLooking = false;

            Vector2 mousePosition = context.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                lookPos = hit.point;
            }


            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                currentTesla.transform.position = lookPos + new Vector3(0, 0, 0);// + spawnOffset;
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                currentTesla.transform.position = player.transform.position + direction * (minPlacementDistance + 0.1f); // Small buffer to avoid overlap
                currentTesla.transform.position = new Vector3(
                    currentTesla.transform.position.x,
                    0,//spawnOffset.y,
                    currentTesla.transform.position.z
                );
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else
            {
                currentTesla.transform.position = player.transform.position + direction * maxPlacementDistance + new Vector3(0, 0, 0);// + spawnOffset;
                currentTesla.transform.rotation = player.transform.rotation;
            }
        }
        if (teslaCount == 1 && nextCurrentTesla != null)
        {
            if (masterInput.instance.inputPaused || isGamepadLooking)
                return;

            isMouseLooking = true;
            isGamepadLooking = false;

            Vector2 mousePosition = context.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                lookPos = hit.point;
            }


            float distanceFromTurret = Vector3.Distance(newTesla.transform.position, lookPos);
            Vector3 direction = (lookPos - newTesla.transform.position).normalized;

            if (distanceFromTurret <= teslaMinPlacementRad && distanceFromTurret > minPlacementDistance)
            {
                nextCurrentTesla.transform.position = new Vector3(lookPos.x, .2f, lookPos.z);// + spawnOffset;
                //nextCurrentTesla.transform.rotation = currentTesla.transform.rotation;
            }
            else if (distanceFromTurret <= teslaMinPlacementRad)
            {
                nextCurrentTesla.transform.position = newTesla.transform.position + direction * (teslaMinPlacementRad + 0.1f); // Small buffer to avoid overlap
                nextCurrentTesla.transform.position = new Vector3(
                    nextCurrentTesla.transform.position.x,
                    .2f,
                    nextCurrentTesla.transform.position.z
                );
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
            else
            {
                nextCurrentTesla.transform.position = newTesla.transform.position + direction * teslaMinPlacementRad;// + spawnOffset;
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
        }
    }

    public void gamepadTurretPlace(InputAction.CallbackContext context)
    {

        //Vector3 mousePos = Vector3.zero;
        //RaycastHit hit;

        // Determine input type (mouse vs. gamepad)
        if (playerInput.actions["MouseLook"].triggered)
        {
            isMouseLooking = true; // Set flag for mouse input
            isGamepadLooking = false;
        }
        else if (Mathf.Abs(playerInput.actions["GamePadLook"].ReadValue<Vector2>().magnitude) > 0.1f) // Check for joystick movement
        {
            isMouseLooking = false; // Set flag for gamepad input
            isGamepadLooking = true;
        }

        if (masterInput.instance.inputPaused || isMouseLooking)
            return;
        else if (casting && currentTurret != null)
        {
            Vector2 rightStickInput = context.ReadValue<Vector2>();

            if (rightStickInput != Vector2.zero)
            {
                Vector3 playerForward = Camera.main.transform.forward;
                Vector3 playerRight = Camera.main.transform.right;

                // Ignore Y axis to keep player level
                playerForward.y = 0;
                playerRight.y = 0;

                // Normalize camera directions
                playerForward.Normalize();
                playerRight.Normalize();

                Vector3 lookDirection = (playerRight * rightStickInput.x) + (playerForward * rightStickInput.y);

                lookPos = player.transform.position + lookDirection * 2.5f; // Adjust distance as needed
            }

            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            // Position the turret based on distance from player
            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                currentTurret.transform.position = lookPos + spawnOffset;
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                currentTurret.transform.position = player.transform.position + direction * (minPlacementDistance + 0.1f); // Small buffer to avoid overlap
                currentTurret.transform.position = new Vector3(
                    currentTurret.transform.position.x,
                    spawnOffset.y,
                    currentTurret.transform.position.z
                );
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else
            {
                currentTurret.transform.position = player.transform.position + direction * maxPlacementDistance + spawnOffset;
                currentTurret.transform.rotation = player.transform.rotation;
            }
        }
        else
            return;

    }

    public void gamepadTeslaPlace(InputAction.CallbackContext context)
    {

    }

    void removeAllTowers()
    {
        for (int i = placedTowers.Length; i >= 0; i--) // Start from the end of the list
        {
            GameObject tower = placedTowers[i];
            if (tower != null)
            {
                 // Destroy the GameObject
            }
        }
    }

    public void removeTower(int count)
    {
        GameObject temp = placedTowers[count];
        placedTowers[count] = null; // Remove the specific tower
        Destroy(temp);

        turretNumCount -= 1;
        totalTowerCount -= 1;

        for (int j = count + 1; j <= placedTowers.Length; j++)
        {
            temp = placedTowers[j];
            if (temp == null)
                return;
            temp.GetComponent<turretCombat>().assignKey(count);
            placedTowers[j-1] = temp;
            if (j == placedTowers.Length - 1)
            {
                placedTowers[placedTowers.Length] = null;
                return;
            }
        }
        return;
    }

    void activateTesla()
    {
        if (instant)
        {
            instant = false;
            if(teslaCount == 0)
            {
                currentTesla = GameObject.Instantiate(teslaTransparent, player.transform.position + new Vector3(0,0,1.5f), Quaternion.LookRotation(player.transform.forward));
                currentTesla.transform.parent = player.transform;
                currentTesla.transform.position = new Vector3(currentTesla.transform.position.x, 0, currentTesla.transform.position.z);
                casting = true;
            }
            if(teslaCount == 1)
            {
                casting = true;
                nextCurrentTesla = GameObject.Instantiate(teslaTransparent, player.transform.position, Quaternion.LookRotation(player.transform.forward));
                nextCurrentTesla.transform.position = new Vector3(newTesla.transform.position.x, 0, newTesla.transform.position.z + 2f);

            }

        }


        
        if(playerInput.actions["Attack"].triggered && teslaCount == 0 && placingOne && !placingTwo) 
        {
            //print("activate 0");
            teslaCount = 1;
            instant = true;
            StartCoroutine(teslaWait());
            if (currentTesla != null)// && teslaDistance <= turretPlacementRadius)
            {
                Vector3 pos = currentTesla.transform.position;
                Destroy(currentTesla);
                newTesla = Instantiate(teslaPrefab, pos + new Vector3 (0,teslaSpawnHeight,0), Quaternion.identity);
            }
            

        }
        if (playerInput.actions["Attack"].triggered && teslaCount == 1 && !placingOne && placingTwo)
        {
            //print("activate 1");
            casting = false;
            teslaCount = 0;
            if (nextCurrentTesla != null)// && teslaDistance <= turretPlacementRadius)
            {
                placingTesla = false;
                Vector3 pos = nextCurrentTesla.transform.position;
                Destroy(nextCurrentTesla);
         
                newTesla2 = Instantiate(teslaPrefab, pos + new Vector3(0, teslaSpawnHeight, 0), Quaternion.identity);

                placingOne = false;
                placingTwo = false;

                Vector3 difference = (newTesla2.transform.position - newTesla.transform.position) / 2;

                GameObject tesWall = GameObject.Instantiate(teslaWall, newTesla2.transform.position - difference + new Vector3(0, 1, 0), Quaternion.identity);
                var teslaParent = GameObject.Instantiate(teslaParentPrefab, tesWall.transform.position, Quaternion.identity);
                tesWall.transform.LookAt(new Vector3(newTesla2.transform.position.x, 1, newTesla2.transform.position.z));

                tesWall.transform.SetParent(teslaParent.transform);
                newTesla.transform.SetParent(teslaParent.transform);
                newTesla2.transform.SetParent(teslaParent.transform);
                teslaParent.GetComponent<teslaTower>().assignVars(newTesla, newTesla2, tesWall);
                teslaParent.GetComponent<teslaTower>().setParents();

                StartCoroutine(playerInputWait());
                gameObject.GetComponent<masterInput>().abilityInUse = false;
                placedTowers.Append(teslaParent);
                Destroy(currentTesla);
                Destroy(nextCurrentTesla);
            }
            
        }
    }

    IEnumerator playerInputWait()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<masterInput>().placing = false;
        yield break;
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
        if (instant)
        {
            instant = false;
            if (turretTransparentPrefab != null) 
            {
                currentTurret = GameObject.Instantiate(turretTransparentPrefab, player.transform.position + new Vector3(2.5f,0,0), player.transform.rotation);
                
                currentTurret.transform.position = player.transform.position + new Vector3(0, 0, 2.5f);
                currentTurret.transform.parent = player.transform;
                casting = true;
            }
            else
            {
                Debug.LogError("turretTransparentPrefab is null");
                return; 
            }
        }

        if (currentTurret == null)
        {
            Debug.LogError("currentTurret is not initialized");
            return; // Exit if currentTurret is null
        }

        if (playerInput.actions["Attack"].triggered)
        {
            if (currentTurret != null) 
            {
                casting = false;
                placing = false; 
                instant = true;
                Quaternion rot = currentTurret.transform.rotation; 
                Vector3 pos = currentTurret.transform.position; 

                Destroy(currentTurret); // Destroy the temporary turret

                // Instantiate the final turret
                if (turretPrefab != null) // Check if turretPrefab is assigned
                {
                    var newTurret = Instantiate(turretPrefab, pos + spawnOffset, rot);
                    newTurret.GetComponent<turretCombat>().assignKey(totalTowerCount);
                    placedTowers[totalTowerCount] = newTurret; // Add to placed towers
                    turretNumCount += 1;
                    totalTowerCount += 1;
                    StartCoroutine(playerInputWait()); // Wait for input
                }
                else
                {
                    Debug.LogError("turretPrefab is null");
                }
            }
            gameObject.GetComponent<masterInput>().abilityInUse = false; // Reset ability in use
            
            
            
        }
    }

    //-------------------------------------------


    //--------------Main Functions---------------

    private void OnDrawGizmos()
    {
        if (currentAura != null)
            Gizmos.DrawWireSphere(currentAura, comatAuraRadius);
    }

    private void Awake()
    {
        instance = this;
        //currentClass = gameObject.GetComponent<masterInput>().currentClass;
        player = GameObject.FindGameObjectWithTag("Player");
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        teslaNumCount = 0;
        turretNumCount = 0;

        skillTreeManagerObj = GameObject.FindGameObjectWithTag("skillTreeManager").GetComponent<SkillTreeManager>();

        towerMaxQuantity = turretMaxQuantity + teslaMaxQuantity;
        placedTowers = new GameObject[towerMaxQuantity];

        playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        teslaNumCount = 0;
        turretNumCount = 0;

        

        //skillTreeManagerObj = GameObject.FindGameObjectWithTag("skillTreeManager").GetComponent<skillTreeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bubble)
        {
            
        }

        if(checkingAura && currentAura != null)
        {
            Collider[] colliders = Physics.OverlapSphere(currentAura, comatAuraRadius, playerLayer);

            if(colliders.Length > 0 && colliders.Length < 2) 
            {
                if (colliders[0].gameObject.tag == "Player" && buffingPlayer == false)
                {
                    buffingPlayer = true;
                    //player.GetComponent<CharacterBase>().buffPlayer();
                    buff(true, "attack", colliders[0].gameObject);
                }
                
            }
            if(colliders.Length == 0 && buffingPlayer == true)
            {
                buffingPlayer = false;
                buff(false, "attack", player.gameObject);
            }

            Collider[] collidersE = Physics.OverlapSphere(currentAura, comatAuraRadius, enemy);

            foreach (Collider c in collidersE)
            {
                if (c.gameObject.tag == "Enemy" && fireBool)
                {
                    if (!c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated)
                    {
                        c.gameObject.GetComponent<EnemyFrame>().dmgOverTimeActivated = true;
                        StartCoroutine(c.gameObject.GetComponent<EnemyFrame>().dmgOverTime(krFireDmg, krFireTime, krFireRate, EnemyFrame.DamageType.Fire));
                    }

                }
            }

        }
        if(!checkingAura && buffingPlayer == true)
        {
            buffingPlayer = false;
            buff(false, "attack", player.gameObject);
        }


        if (placing)
        {
            activateTurret();
        }
        if(placingTesla)
        {
            activateTesla();
        }

        if(shootingSwords)
        {
            if(playerInput.actions["Attack"].IsPressed() && !shotSword)
            {
                StartCoroutine(swordShooting());
            }
        }

        if(shootingRocket)
        {
            if (playerInput.actions["Attack"].triggered && !shotRocket)
                StartCoroutine(shootRocket());
        }

        if(shootingLaser)
        {
            if(playerInput.actions["Attack"].IsPressed() && !shotLaser)
            {
                shotLaser = true;

                //if (currentLaserEffect != null)
                currentLaserEffect.GetComponent<ParticleSystem>().Clear(true);
                currentLaserEffect.GetComponent<ParticleSystem>().Play();
            }
            if(playerInput.actions["Attack"].WasReleasedThisFrame() && shotLaser)
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
            gameObject.GetComponent<masterInput>().throwingGrenade = true;
            if (playerInput.actions["Attack"].triggered)
            {
                threwGrenade = true;
                //gameObject.GetComponent<masterInput>().throwingGrenade = false;
                GameObject grenade = Instantiate(grenadePrefab, grenadeSpawn.transform.position, grenadeSpawn.transform.rotation);
                if(earthBool)
                    grenade.GetComponent<grenade>().isEarth = true;
                grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * grenadeSpeed;
                StartCoroutine(grenade.GetComponent<grenade>().explode());
                StartCoroutine(grenadeWait(grenadeCooldown));
            }
            
        }

        //skill tree upgrade testing
        //if(Input.GetKeyDown(KeyCode.T))
        //{
            //Debug.Log("Activating skill upgrade");
            //var temp = bubbleRadius;
            //skillTreeManagerObj.unlockSkill("IncBubRad");
            //print("Bubble rad has been changed from: " + temp + " to: " + bubbleRadius);
        //}
    }


    //-----------------modify functions---------------------


    //-----------------Rune modifiers-----------------------


    public void activateFireRune(bool choice)
    {
        if (choice)
        {
            /*
            switch(gameObject.GetComponent<masterInput>().currentClass)
            {
                case WeaponBase.weaponClassTypes.Knight:
                    fireBool = true;
                    break;
                case WeaponBase.weaponClassTypes.Gunner:

                    break;
                case WeaponBase.weaponClassTypes.Engineer:

                    break;
            }
            */
            fireBool = true;
        }
        else
        {
            /*
            switch (gameObject.GetComponent<masterInput>().currentClass)
            {
                case WeaponBase.weaponClassTypes.Knight:
                    fireBool = false;
                    break;
                case WeaponBase.weaponClassTypes.Gunner:

                    break;
                case WeaponBase.weaponClassTypes.Engineer:

                    break;
            }
            */
            fireBool = false;
        }
    }

    public void activateIceRune(bool choice)
    {
        if(choice)
        {
            iceBool = true;
        }
        else
        {
            iceBool = false;
        }
    }

    public void activateEarthRune(bool choice)
    {
        if (choice)
        {
            earthBool = true;
        }
        else
        {
            earthBool = false;
        }
    }


    //-------------------skill tree-------------------------


    //knight
    public void modifyBubbleRad(float amount)
    {
        Debug.Log("Modified bubble radius by: " + amount);
        bubbleRadius += amount;
    }

    public void modifyCombatAuraRad(float amount)
    {
        Debug.Log("Modified CombatAura radius by: " + amount);
        comatAuraRadius += amount;
    }

    public void triggerSSExpolode()
    {
        SSExplode = true;
    }


    //Gunner

    public void modifyGrenadeRad(float amount)
    {
        //Debug.Log("Modified grenade explosion radius by: " + amount);
        grenadePrefab.GetComponent<grenade>().increaseBlastRadius(amount);
    }

    public void modifyRocketRad(float amount)
    {
        //Debug.Log("Modified grenade explosion radius by: " + amount);
        rocketPrefab.GetComponent<rocket>().increaseBlastRadius(amount);
    }

    
}

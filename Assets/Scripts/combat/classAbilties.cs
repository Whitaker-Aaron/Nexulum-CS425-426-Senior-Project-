﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿/*-----------------------------------------------------
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
    AudioManager audioManager;

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
    public float turretInitialHeight = 0.2f; // Public variable for initial turret height
    bool instant = false;
    public LayerMask ground;
    public float cloneTime;

    bool placingTesla, placingOne, placingTwo = false;
    public GameObject teslaTransparent;
    public GameObject teslaPrefab, teslaParentPrefab;
    GameObject currentTesla, nextCurrentTesla = null;
    int teslaCount = 0;
    float teslaDistance;
    Vector3 teslaDirection;
    public float teslaSpawnHeight;
    public float teslaInitialHeight = 0.2f; // Public variable for initial Tesla height
    public float teslaPlacementRadius = 4f;
    public float teslaMinPlacementRad = 2.5f;
    public GameObject teslaWall;//, teslaParent;
    GameObject newTesla = null, newTesla2 = null;

    int teslaNumCount, turretNumCount = 0;
    public int teslaMaxQuantity, turretMaxQuantity;
    private int towerMaxQuantity;

    private List<GameObject> placedTurrets = new List<GameObject>();
    private List<GameObject> placedTeslas = new List<GameObject>();


    private int totalTowerCount;

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private GameObject cloneWall;
    public GameObject currentClone;
    bool cloning = false;
    public float cloneRadius = 1f;
    Collider[] enemiesInClone;


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

    [SerializeField] private GameObject fireRuneEffect, iceRuneEffect, earthRuneEffect;
    private GameObject CFRE, CIRE, CERE; //current fire/ice/earth rune effect

    //knight


    [SerializeField] private float krFireTime, krFireRate;
    [SerializeField] private int krFireDmg;

    //-------------------------------------------

    //----------------Functions------------------

    public void activateAbilityOne(WeaponBase.weaponClassTypes currentClass)
    {
        if (a1cooldown || acc1 != null)// || turretNumCount == turretMaxQuantity)
        {
            Debug.Log("Ability 1 is on cooldown.");
            return;
        }
        Debug.Log("Activating Ability 1.");

        
        //EffectsManager.instance.getFromPool("fireRuneEffect", player.transform.position, Quaternion.identity, true, false);
        a1cooldown = true;


        if (currentClass == WeaponBase.weaponClassTypes.Knight && !bubble)
        {
            uiManager.ActivateCooldownOnAbility(1);
            audioManager.PlaySFX("BubbleShield");
            StartCoroutine(bubbleShield());
            if(earthBool)
                playRuneEffect("Earth");
            acc1 = StartCoroutine(abilitiesCooldown(1, ka1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
            gameObject.GetComponent<masterInput>().bubble = true;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingRocket)
        {
            if (fireBool)
                playRuneEffect("Fire");
            gameObject.GetComponent<playerAnimationController>().gunnerRocketPod(true);
            uiManager.ActivateCooldownOnAbility(1);
            gameObject.GetComponent<masterInput>().shootingRocket = true;
            shootingRocket = true;
            StartCoroutine(abilitiesCooldown(1, ga1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && !placing)
        {
            if(fireBool)
                playRuneEffect("Fire");
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
            if(fireBool)
                playRuneEffect("Fire");
            activatedAura = true;
            StartCoroutine(auraWait());
            acc2 = StartCoroutine(abilitiesCooldown(2, ka2Time));
            audioManager.PlaySFX("CombatAura");
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !throwingGrenade)
        {
            if (earthBool)
                playRuneEffect("Earth");
            gameObject.GetComponent<playerAnimationController>().gunnerRocketPod(true);
            throwingGrenade = true;
            gameObject.GetComponent<masterInput>().throwingGrenade = true;
            StartCoroutine(abilitiesCooldown(2, ga2Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && !placing)
        {
            if (iceBool)
            {
                //playRuneEffect("Ice");
                CIRE.SetActive(true);
                CIRE.GetComponent<ParticleSystem>().Play();
                if (CIRE.GetComponent<ParticleSystem>().isPlaying)
                    print("Playing ice effect");
            }
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
                playRuneEffect("Ice");
                CIRE.SetActive(true);
                CIRE.GetComponent<ParticleSystem>().Play();
                if (CIRE.GetComponent<ParticleSystem>().isPlaying)
                    print("Playing ice effect");
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
                audioManager.PlaySFX("CombatAura");
                currentEffect.GetComponent<ParticleSystem>().Play();
                StartCoroutine(stopSword(currentEffect));
            }

            if (iceBool)
            {
                playRuneEffect("Ice");
            }
            acc3 = StartCoroutine(abilitiesCooldown(3, ka3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingLaser)
        {
            shootingLaser = true;
            gameObject.GetComponent<masterInput>().shootingLaser = true;
            Transform pos = gameObject.GetComponent<masterInput>().bulletSpawn;
            if (iceBool)
            {
                playRuneEffect("Ice");
                
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
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && currentClone == null)
        {
            if (earthBool)
            {
                playRuneEffect("Earth");
                currentClone = Instantiate(cloneWall, player.transform.position + (player.transform.forward * 1.5f), player.transform.rotation);
            }
            else
                currentClone = Instantiate(clonePrefab, player.transform.position, player.transform.rotation);
            acc3 = StartCoroutine(abilitiesCooldown(3, ea3Time));
            StartCoroutine(cloneStart());
            audioManager.PlaySFX("CombatAura");
            gameObject.GetComponent<masterInput>().abilityInUse = false;
            cloning = true;
        }
        
        
    }

    IEnumerator abilitiesCooldown(int ability, float time)
    {
        //if (turretNumCount == turretMaxQuantity || teslaNumCount == teslaMaxQuantity)
        //  yield break;
        yield return StartCoroutine(uiManager.StartCooldownSlider(ability, (0.98f / time)));
        //yield return new WaitForSeconds(time);

        switch (ability)
        {
            case 1:
                yield return new WaitForSeconds(0.2f);
                //if (CFRE.GetComponent<ParticleSystem>().isPlaying)
                
                a1cooldown = false;
                acc1 = null;
                print("ability 1 done");

                break;
            case 2:
                yield return new WaitForSeconds(0.2f);
                //if (CIRE.GetComponent<ParticleSystem>().isPlaying)
                //CIRE.GetComponent<ParticleSystem>().Stop();
                a2cooldown = false;
                acc2 = null;
                print("ability 2 done");
                break;
            case 3:
                yield return new WaitForSeconds(0.2f);
                //if (CERE.GetComponent<ParticleSystem>().isPlaying)
                //CERE.GetComponent<ParticleSystem>().Stop();
                a3cooldown = false;
                acc3 = null;
                print("ability 3 done");
                break;
        }
        uiManager.DeactivateCooldownOnAbility(ability);
        switch (masterInput.instance.currentClass)
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
                cloning = false;
                Destroy(currentClone);
                break;
        }


        yield break;
    }

    void playRuneEffect(string name)
    {
        switch(name)
        {
            case "Fire":
                CFRE.SetActive(true);
                CFRE.GetComponent<ParticleSystem>().Play();
                break;
            case "Ice":
                CIRE.SetActive(true);
                CIRE.GetComponent<ParticleSystem>().Play();
                break;
            case "Earth":
                CERE.SetActive(true);
                CERE.GetComponent<ParticleSystem>().Play();
                break;
        }
    }

    void stopRuneEffect(string name)
    {
        switch(name)
        {
            case "Fire":
                CFRE.SetActive(false);
                CFRE.GetComponent<ParticleSystem>().Stop();
                break;
            case "Ice":
                CIRE.SetActive(false);
                CIRE.GetComponent<ParticleSystem>().Stop();
                break;
            case "Earth":
                CERE.SetActive(false);
                CERE.GetComponent<ParticleSystem>().Stop();
                break;
        }
    }



    //Knight

    IEnumerator bubbleShield()
    {
        bubble = true;
        player.GetComponent<CharacterBase>().bubbleShield = true;
        print("Activating shield");

        if (earthBool)
        {
            EffectsManager.instance.getFromPool("earthShield", player.transform.position + Vector3.up, Quaternion.identity, true, false);
            yield return new WaitForSeconds(.5f);
            EffectsManager.instance.getFromPool("earthShield", player.transform.position + Vector3.up, Quaternion.identity, true, false);
            yield return new WaitForSeconds(bubbleTime);
            EffectsManager.instance.getFromPool("earthShield", player.transform.position + Vector3.up, Quaternion.identity, true, false);
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
        stopRuneEffect("Earth");
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
        if (!fireBool)
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
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX("SwordShot");
        if (iceBool)
        {
            //print(projectileManager.Instance);
            GameObject sword = projectileManager.Instance.getProjectile("swordShotIcePool", swordSpawn.position, swordSpawn.rotation);
            sword.GetComponent<swordShot>().isIce = true;
            //sword.GetComponent<Rigidbody>().velocity = swordSpawn.transform.forward * swordSpeed;
            //sword.GetComponent<swordShot>().damage = swordShotDamage;
        }
        else if (SSExplode)
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
        stopRuneEffect("Ice");
        shootingSwords = false;
        currentEffect.GetComponent<ParticleSystem>().Stop();
        Destroy(currentEffect);
        yield break;
    }

    //Gunner

    IEnumerator shootRocket()
    {
        shotRocket = true;
        var rocket = Instantiate(rocketPrefab, swordSpawn.position, swordSpawn.rotation);

        if (fireBool)
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
        stopRuneEffect("Ice");
        Destroy(currentLaserEffect);
        yield break;
    }

    IEnumerator checkLaserHit()
    {
        if (!shootingLaser)
            yield break;
        checkHit = true;
        RaycastHit hit;

        if (masterInput.instance != null && masterInput.instance.bulletSpawn != null && 
            Physics.Raycast(masterInput.instance.bulletSpawn.transform.position, masterInput.instance.bulletSpawn.transform.forward, out hit, maxLaserDistance, enemy))
        {
            if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.tag == "Enemy")
            {
                print("Hitting enemy");
                EnemyFrame enemyFrame = hit.collider.gameObject.GetComponent<EnemyFrame>();
                if (enemyFrame != null)
                {
                    enemyFrame.takeDamage(laserDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
                    UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserDamage);
                    if (iceBool)
                    {
                        enemyFrame.takeDamage(laserIceDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
                        UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserIceDamage);
                    }
                }
                else
                {
                    Debug.LogWarning("Object with tag 'Enemy' doesn't have EnemyFrame component");
                }
            }
            if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.tag == "bossPart")
            {
                print("Hitting enemy");
                bossPart bPart = hit.collider.gameObject.GetComponent<bossPart>();
                if (bPart != null)
                {
                    bPart.takeDamage(laserDamage);
                    UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserDamage);
                    if (iceBool)
                    {
                        bPart.takeDamage(laserIceDamage);
                        UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserIceDamage);
                    }
                }
                else
                {
                    Debug.LogWarning("Object with tag 'bossPart' doesn't have bossPart component");
                }
            }
            else if(hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.tag == "Boss")
            {
                print("Hitting enemy");
                golemBoss boss = hit.collider.gameObject.GetComponent<golemBoss>();
                if (boss != null)
                {
                    boss.takeDamage(laserDamage);
                    UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserDamage);
                    if (iceBool)
                    {
                        boss.takeDamage(laserIceDamage);
                        UIManager.instance.DisplayDamageNum(hit.collider.gameObject.transform, laserIceDamage);
                    }
                }
                else
                {
                    Debug.LogWarning("Object with tag 'Boss' doesn't have golemBoss component");
                }
            }
            //print("Hitting enemy");
            //hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(laserDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Projectile);
            //if (iceBool)
            //{
            //    hit.collider.gameObject.GetComponent<EnemyFrame>().takeDamage(laserIceDamage, Vector3.zero, EnemyFrame.DamageSource.Player, EnemyFrame.DamageType.Ice);
            //}
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

    IEnumerator cloneStart()
    {
        yield return new WaitForSeconds(cloneTime);

        cloning = false;
        Destroy(currentClone);
        stopRuneEffect("Earth");
        currentClone = null;
        yield break;
    }

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
                // Don't override the y-coordinate from the raycast hit
            }

            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            // Temporarily detach from player to set position
            currentTurret.transform.parent = null;

            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                // Use the lookPos directly to preserve elevation
                currentTurret.transform.position = lookPos + spawnOffset;
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                Vector3 pos = player.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTurret.transform.position = hit.point + spawnOffset;
                }
                else
                {
                    currentTurret.transform.position = pos + spawnOffset;
                }
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else
            {
                Vector3 pos = player.transform.position + direction * maxPlacementDistance;
                // Perform a raycast to find the ground height at this position
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTurret.transform.position = hit.point + spawnOffset;
                }
                else
                {
                    currentTurret.transform.position = pos + spawnOffset;
                }
                currentTurret.transform.rotation = player.transform.rotation;
            }
            
            // Re-parent to player after positioning
            currentTurret.transform.parent = player.transform;
        }
    }

    public void mouseTeslaPlace(InputAction.CallbackContext context)
    {
        if (!casting)
            return;
            
        // First Tesla node placement
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

            // Temporarily detach from player to set position
            currentTesla.transform.parent = null;

            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(lookPos.x, lookPos.y + 10, lookPos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = player.transform.position + direction * distanceFromPlayer + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                Vector3 pos = player.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else
            {
                Vector3 pos = player.transform.position + direction * maxPlacementDistance;
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            
            // Re-parent to player after positioning
            currentTesla.transform.parent = player.transform;
        }
        
        // Second Tesla node placement
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

            float distanceFromTesla = Vector3.Distance(newTesla.transform.position, lookPos);
            Vector3 direction = (lookPos - newTesla.transform.position).normalized;

            if (distanceFromTesla <= teslaMinPlacementRad && distanceFromTesla > minPlacementDistance)
            {
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(lookPos.x, lookPos.y + 10, lookPos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = new Vector3(lookPos.x, teslaInitialHeight, lookPos.z);
                }
            }
            else if (distanceFromTesla <= minPlacementDistance)
            {
                Vector3 pos = newTesla.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
            else
            {
                Vector3 pos = newTesla.transform.position + direction * teslaMinPlacementRad;
                // Perform a raycast to find the ground height at this position
                //RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
        }
    }

    public void gamepadTurretPlace(InputAction.CallbackContext context)
    {
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

            // Temporarily detach from player to set position
            currentTurret.transform.parent = null;

            // Position the turret based on distance from player
            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(lookPos.x, lookPos.y + 10, lookPos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTurret.transform.position = hit.point + spawnOffset;
                }
                else
                {
                    currentTurret.transform.position = lookPos + spawnOffset;
                }
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                Vector3 pos = player.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTurret.transform.position = hit.point + spawnOffset;
                }
                else
                {
                    currentTurret.transform.position = pos + spawnOffset;
                }
                currentTurret.transform.rotation = player.transform.rotation;
            }
            else
            {
                Vector3 pos = player.transform.position + direction * maxPlacementDistance;
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTurret.transform.position = hit.point + spawnOffset;
                }
                else
                {
                    currentTurret.transform.position = pos + spawnOffset;
                }
                currentTurret.transform.rotation = player.transform.rotation;
            }
            
            // Re-parent to player after positioning
            currentTurret.transform.parent = player.transform;
        }
        else
            return;
    }

    public void gamepadTeslaPlace(InputAction.CallbackContext context)
    {
        if (!casting)
            return;
            
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
            
        isGamepadLooking = true;
        isMouseLooking = false;

        // First Tesla placement
        if (teslaCount == 0 && currentTesla != null)
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

            // Temporarily detach from player to set position
            currentTesla.transform.parent = null;

            // Position the tesla based on distance from player
            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(lookPos.x, lookPos.y + 10, lookPos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = lookPos + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                Vector3 pos = player.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            else
            {
                Vector3 pos = player.transform.position + direction * maxPlacementDistance;
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    currentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    currentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                currentTesla.transform.rotation = player.transform.rotation;
            }
            
            // Re-parent to player after positioning
            currentTesla.transform.parent = player.transform;
        }
        // Second Tesla placement
        else if (teslaCount == 1 && nextCurrentTesla != null && newTesla != null)
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

                lookPos = newTesla.transform.position + lookDirection * teslaMinPlacementRad; // Use teslaMinPlacementRad for consistency
            }

            float distanceFromTesla = Vector3.Distance(newTesla.transform.position, lookPos);
            Vector3 direction = (lookPos - newTesla.transform.position).normalized;

            // Position the second tesla based on distance from first tesla
            if (distanceFromTesla <= teslaMinPlacementRad && distanceFromTesla > minPlacementDistance)
            {
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(lookPos.x, lookPos.y + 10, lookPos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = lookPos + new Vector3(0, teslaInitialHeight, 0);
                }
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
            else if (distanceFromTesla <= minPlacementDistance)
            {
                Vector3 pos = newTesla.transform.position + direction * (minPlacementDistance + 0.1f);
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
            else
            {
                Vector3 pos = newTesla.transform.position + direction * teslaMinPlacementRad;
                // Perform a raycast to find the ground height at this position
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), Vector3.down, out hit, 20, ground))
                {
                    nextCurrentTesla.transform.position = hit.point + new Vector3(0, teslaInitialHeight, 0);
                }
                else
                {
                    nextCurrentTesla.transform.position = pos + new Vector3(0, teslaInitialHeight, 0);
                }
                nextCurrentTesla.transform.rotation = newTesla.transform.rotation;
            }
        }
    }

    void removeAllTowers()
    {
        /*
        foreach (GameObject tower in placedTowers)
        {
            if (tower != null)
            {
                Destroy(tower);
            }
        }

        placedTowers.Clear(); // ✅ Resets the list properly
        turretNumCount = 0;
        teslaNumCount = 0;
        totalTowerCount = 0;
        */
    }


    public void removeTower(int count)
    {
        /*
        if (count < 0 || count >= placedTowers.Count)
        {
            Debug.LogWarning("Invalid tower index: " + count);
            return;
        }

        GameObject temp = placedTowers[count];

        if (temp != null)
        {
            Destroy(temp);
        }

        placedTowers.RemoveAt(count); // ✅ Removes the item from the list
        turretNumCount--;
        totalTowerCount--;

        // Reassign keys for remaining turrets
        for (int i = count; i < placedTowers.Count; i++)
        {
            placedTowers[i].GetComponent<turretCombat>().assignKey(i);
        }
        */
    }


    void activateTesla()
    {
        // Initialize the transparent Tesla objects for placement
        if (instant)
        {
            instant = false;
            if (teslaCount == 0)
            {
                // First Tesla node initialization
                Vector3 spawnPosition = player.transform.position + player.transform.forward * 1.5f + new Vector3(0, teslaInitialHeight, 0);
                currentTesla = Instantiate(teslaTransparent, spawnPosition, Quaternion.LookRotation(player.transform.forward));
                currentTesla.transform.parent = player.transform; // Keep parented to player during placement
                casting = true;
            }
            else if (teslaCount == 1)
            {
                // Second Tesla node initialization
                casting = true;
                Vector3 spawnPosition = newTesla.transform.position + newTesla.transform.forward * teslaMinPlacementRad + new Vector3(0, teslaInitialHeight, 0);
                nextCurrentTesla = Instantiate(teslaTransparent, spawnPosition, Quaternion.LookRotation(newTesla.transform.forward));
            }
        }

        // Place first Tesla node
        if (playerInput.actions["Attack"].triggered && teslaCount == 0 && placingOne && !placingTwo)
        {
            // Handle max quantity limit
            if (teslaNumCount >= teslaMaxQuantity)
            {
                teslaNumCount--;
                GameObject oldestTesla = placedTeslas[0]; // Oldest Tesla
                placedTeslas.RemoveAt(0);
                Destroy(oldestTesla);
            }

            teslaCount++;
            instant = true;
            StartCoroutine(teslaWait());

            if (currentTesla != null)
            {
                Vector3 pos = currentTesla.transform.position;
                Destroy(currentTesla);
                currentTesla = null; // Ensure reference is cleared
                // Use teslaSpawnHeight for the final tower height
                newTesla = Instantiate(teslaPrefab, new Vector3(pos.x, pos.y - teslaInitialHeight + teslaSpawnHeight, pos.z), Quaternion.identity);
            }
            return;
        }

        // Place second Tesla node and create Tesla wall
        if (playerInput.actions["Attack"].triggered && teslaCount == 1 && !placingOne && placingTwo && newTesla != null)
        {
            teslaCount = 0;

            if (nextCurrentTesla != null)
            {
                placingTesla = false;
                Vector3 pos = nextCurrentTesla.transform.position;
                Destroy(nextCurrentTesla);

                // Use teslaSpawnHeight for the final tower height
                newTesla2 = Instantiate(teslaPrefab, new Vector3(pos.x, pos.y - teslaInitialHeight + teslaSpawnHeight, pos.z), Quaternion.identity);
                placingOne = false;
                placingTwo = false;

                // Create Tesla wall between the two nodes
                Vector3 difference = (newTesla2.transform.position - newTesla.transform.position) / 2;
                GameObject tesWall = Instantiate(teslaWall, newTesla2.transform.position - difference + new Vector3(0, 1, 0), Quaternion.identity);
                var teslaParent = Instantiate(teslaParentPrefab, tesWall.transform.position, Quaternion.identity);

                // Set up the Tesla wall and parent relationships
                tesWall.transform.LookAt(new Vector3(newTesla2.transform.position.x, 1, newTesla2.transform.position.z));
                tesWall.transform.SetParent(teslaParent.transform);
                newTesla.transform.SetParent(teslaParent.transform);
                newTesla2.transform.SetParent(teslaParent.transform);
                teslaParent.GetComponent<teslaTower>().assignVars(newTesla, newTesla2, tesWall);
                teslaParent.GetComponent<teslaTower>().setParents();

                // Apply ice effect if ice rune is active
                if(iceBool)
                {
                    newTesla.gameObject.GetComponent<teslaBase>().iceEffect.GetComponent<ParticleSystem>().Play();
                    newTesla2.gameObject.GetComponent<teslaBase>().iceEffect.GetComponent<ParticleSystem>().Play();
                    teslaParent.GetComponent<teslaTower>().setIce(true);
                }

                // Clean up and reset states
                StartCoroutine(playerInputWait());
                gameObject.GetComponent<masterInput>().abilityInUse = false;
                gameObject.GetComponent<masterInput>().placing = false;

                placedTeslas.Add(teslaParent);
                teslaNumCount++;
                totalTowerCount++;

                Destroy(currentTesla);
                Destroy(nextCurrentTesla);
            }
            stopRuneEffect("Ice");
            casting = false;
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
        // Initialize the transparent turret for placement
        if (instant)
        {
            instant = false;
            if (turretTransparentPrefab != null)
            {
                // Spawn the turret in front of the player like the Tesla, with customizable height
                Vector3 spawnPosition = player.transform.position + player.transform.forward * 1.5f + new Vector3(0, turretInitialHeight, 0);
                currentTurret = Instantiate(turretTransparentPrefab, spawnPosition, player.transform.rotation);
                currentTurret.transform.parent = player.transform; // Keep parented to player during placement
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
            return;
        }

        // Place the turret when Attack is triggered
        if (playerInput.actions["Attack"].triggered)
        {
            stopRuneEffect("Fire");
            casting = false;
            placing = false;
            instant = true;

            Quaternion rot = currentTurret.transform.rotation;
            Vector3 pos = currentTurret.transform.position;

            Destroy(currentTurret); // Destroy the temporary turret

            // Remove oldest turret if at max limit
            if (turretNumCount >= turretMaxQuantity)
            {
                if (placedTurrets.Count > 0)
                {
                    GameObject oldestTurret = placedTurrets[0]; // Get first turret (oldest)
                    placedTurrets.RemoveAt(0); // Remove from list
                    Destroy(oldestTurret); // Destroy the object
                }
            }

            if (turretPrefab != null)
            {
                // Create the actual turret
                GameObject newTurret = Instantiate(turretPrefab, pos + spawnOffset, rot);
                
                // Apply fire effect if fire rune is active
                if (fireBool)
                {
                    print("Firebool true in turretplace");
                    newTurret.GetComponent<turretCombat>().activateFire(true);
                }
                else
                    newTurret.GetComponent<turretCombat>().activateFire(false);

                placedTurrets.Add(newTurret);

                // Update counters
                if (turretNumCount < turretMaxQuantity)
                {
                    turretNumCount++;
                    totalTowerCount++;
                }

                StartCoroutine(playerInputWait());
            }
            else
            {
                Debug.LogError("turretPrefab is null");
            }
            
            stopRuneEffect("Fire");
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
    }




//-------------------------------------------


//--------------Main Functions---------------

    private void OnDrawGizmos()
    {
        if (currentAura != null)
            Gizmos.DrawWireSphere(currentAura, comatAuraRadius);

        if (currentClone != null)
            Gizmos.DrawWireSphere(currentClone.transform.position + Vector3.up, cloneRadius);
    }

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        teslaNumCount = 0;
        turretNumCount = 0;
        totalTowerCount = 0;

        skillTreeManagerObj = GameObject.FindGameObjectWithTag("skillTreeManager").GetComponent<SkillTreeManager>();

        placedTurrets = new List<GameObject>(); // ✅ Use List instead of array
        placedTeslas = new List<GameObject>();

        playerInput = GetComponent<PlayerInput>();
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //rune effect initilization
        CFRE = Instantiate(fireRuneEffect, player.transform);
        CIRE = Instantiate(iceRuneEffect, player.transform);
        CERE = Instantiate(earthRuneEffect, player.transform);
        CFRE.GetComponent<ParticleSystem>().Stop();
        CIRE.GetComponent<ParticleSystem>().Stop();
        CERE.GetComponent<ParticleSystem>().Stop();
        CFRE.transform.localPosition = Vector3.zero;
        CIRE.transform.localPosition = Vector3.zero;
        CERE.transform.localPosition = Vector3.zero;
        CFRE.SetActive(false);
        CIRE.SetActive(false);
        CERE.SetActive(false);
        //fireRuneEffect.transform.parent = player.transform;
        //iceRuneEffect.transform.parent = player.transform;
        //earthRuneEffect.transform.parent = player.transform;


        teslaNumCount = 0;
        turretNumCount = 0;
        enemiesInClone = new Collider[0];


        //skillTreeManagerObj = GameObject.FindGameObjectWithTag("skillTreeManager").GetComponent<skillTreeManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (checkingAura && currentAura != null)
        {
            Collider[] colliders = Physics.OverlapSphere(currentAura, comatAuraRadius, playerLayer);

            if (colliders.Length > 0 && colliders.Length < 2)
            {
                if (colliders[0].gameObject.tag == "Player" && buffingPlayer == false)
                {
                    buffingPlayer = true;
                    //player.GetComponent<CharacterBase>().buffPlayer();
                    buff(true, "attack", colliders[0].gameObject);
                }

            }
            if (colliders.Length == 0 && buffingPlayer == true)
            {
                buffingPlayer = false;
                buff(false, "attack", player.gameObject);
            }

            Collider[] collidersE = Physics.OverlapSphere(currentAura, comatAuraRadius, enemy);

            foreach (Collider c in collidersE)
            {
                if (c != null && c.gameObject != null)
                {
                    // Check for Enemy tag
                    if (c.gameObject.tag == "Enemy" && fireBool)
                    {
                        EnemyFrame enemyFrame = c.gameObject.GetComponent<EnemyFrame>();
                        if (enemyFrame != null && !enemyFrame.dmgOverTimeActivated)
                        {
                            enemyFrame.dmgOverTimeActivated = true;
                            StartCoroutine(enemyFrame.dmgOverTime(krFireDmg, krFireTime, krFireRate, EnemyFrame.DamageType.Fire));
                        }
                    }
                    // Check for Boss tag
                    else if (c.gameObject.tag == "Boss" && fireBool)
                    {
                        // Use golemBoss component for Boss tag
                        golemBoss boss = c.gameObject.GetComponent<golemBoss>();
                        if (boss != null && !boss.dmgOverTimeActivated)
                        {
                            boss.dmgOverTimeActivated = true;
                            StartCoroutine(boss.dmgOverTime(krFireDmg, krFireTime, krFireRate, EnemyFrame.DamageType.Fire));
                        }
                    }
                    // Check for bossPart tag
                    else if (c.gameObject.tag == "bossPart" && fireBool)
                    {
                        // Use bossPart component for bossPart tag
                        bossPart bPart = c.gameObject.GetComponent<bossPart>();
                        if (bPart != null && bPart.parent != null)
                        {
                            golemBoss parentBoss = bPart.parent.GetComponent<golemBoss>();
                            if (parentBoss != null && !parentBoss.dmgOverTimeActivated)
                            {
                                parentBoss.dmgOverTimeActivated = true;
                                StartCoroutine(parentBoss.dmgOverTime(krFireDmg, krFireTime, krFireRate, EnemyFrame.DamageType.Fire));
                            }
                        }
                    }
                }
            }

        }

        if (!checkingAura && buffingPlayer == true)
        {
            buffingPlayer = false;
            buff(false, "attack", player.gameObject);
            stopRuneEffect("Fire");
        }


        if (placing)
        {
            activateTurret();
        }
        if (placingTesla)
        {
            activateTesla();
        }

        if(cloning && currentClone != null)
        {
            enemiesInClone = Physics.OverlapSphere(currentClone.transform.position + Vector3.up, cloneRadius, enemy);

            foreach(Collider c in enemiesInClone)
            {
                if(c == null) continue;
                
                print("Setting new target in class for an enemy");
                c.gameObject.transform.LookAt(currentClone.transform.position);
                
                // Check if it's a boss
                if(c.gameObject.CompareTag("Boss"))
                {
                    print("Setting new target for Boss");
                    // Make the boss look at the clone
                    c.gameObject.transform.LookAt(currentClone.transform.position);
                    
                    // Try to get EnemyLOS component if it exists on the boss
                    EnemyLOS enemyLOS = c.gameObject.GetComponent<EnemyLOS>();
                    if(enemyLOS != null)
                    {
                        enemyLOS.ChangeTarget(c.gameObject);
                    }
                }
                else
                {
                    // Handle regular enemies
                    EnemyLOS enemyLOS = c.gameObject.GetComponent<EnemyLOS>();
                    if(enemyLOS != null)
                    {
                        enemyLOS.ChangeTarget(c.gameObject);
                    }
                }
            }
        }
        if(!cloning && enemiesInClone.Length > 0)
        {
            //stopRuneEffect("Earth");
            foreach (Collider c in enemiesInClone)
            {
                if(c == null) continue;

                // Check if it's a boss
                if(c.gameObject.CompareTag("Boss"))
                {
                    print("Resetting Boss target to player");
                    // Make the boss look at the player
                    c.gameObject.transform.LookAt(player.transform.position);
                    
                    // Try to get EnemyLOS component if it exists on the boss
                    EnemyLOS enemyLOS = c.gameObject.GetComponent<EnemyLOS>();
                    if(enemyLOS != null)
                    {
                        enemyLOS.ChangeTarget(player.gameObject);
                    }
                }
                else
                {
                    // Reset regular enemies targeting to player
                    EnemyLOS enemyLOS = c.gameObject.GetComponent<EnemyLOS>();
                    if(enemyLOS != null)
                    {
                        enemyLOS.ChangeTarget(player.gameObject);
                    }
                }
            }
            enemiesInClone = new Collider[0];
        }

        if (shootingSwords)
        {
            if (playerInput.actions["Attack"].IsPressed() && !shotSword)
            {
                StartCoroutine(swordShooting());
            }
        }

        if (shootingRocket)
        {
            if (playerInput.actions["Attack"].triggered && !shotRocket)
            {
                stopRuneEffect("Fire");
                StartCoroutine(shootRocket());
                gameObject.GetComponent<playerAnimationController>().gunnerRocketPod(false);
            }
        }

        if (shootingLaser)
        {
            if (playerInput.actions["Attack"].IsPressed() && !shotLaser)
            {
                shotLaser = true;

                //if (currentLaserEffect != null)
                currentLaserEffect.GetComponent<ParticleSystem>().Clear(true);
                currentLaserEffect.GetComponent<ParticleSystem>().Play();
            }
            if (playerInput.actions["Attack"].WasReleasedThisFrame() && shotLaser)
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

        if (throwingGrenade && !threwGrenade)
        {
            gameObject.GetComponent<masterInput>().throwingGrenade = true;
            if (playerInput.actions["Attack"].triggered)
            {
                threwGrenade = true;
                //gameObject.GetComponent<masterInput>().throwingGrenade = false;
                GameObject grenade = Instantiate(grenadePrefab, grenadeSpawn.transform.position, grenadeSpawn.transform.rotation);

                if (earthBool)
                    grenade.GetComponent<grenade>().isEarth = true;
                grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * grenadeSpeed;
                gameObject.GetComponent<playerAnimationController>().gunnerRocketPod(false);
                StartCoroutine(grenade.GetComponent<grenade>().explode());
                StartCoroutine(grenadeWait(grenadeCooldown));

                stopRuneEffect("Earth");
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
        if (choice)
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

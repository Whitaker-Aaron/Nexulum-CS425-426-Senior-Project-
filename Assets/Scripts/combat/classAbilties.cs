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
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class classAbilties : MonoBehaviour
{

    //----------------Variables------------------

    GameObject player;
    public LayerMask enemy;
    UIManager uiManager;

    private PlayerInput playerInput;
    private bool usingMouseInput = false;


    //Knight
    bool bubble = false;
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
    [SerializeField] private GameObject combatAuraEffectStart, combatAuraEffectEnd, combatAuraEffectLoop;
    public float comatAuraRadius = 4f;
    private bool activatedAura, checkingAura = false;
    public float auraTime = 5f;
    private Vector3 currentAura;
    bool buffingPlayer = false;
    public float buffRate = .5f;
    public int auraHealthBuff = 25;
    


    //Gunner
    [SerializeField] private GameObject rocketPrefab;
    public float rocketSpeed = 5f;
    public float rocketTime;
    bool shootingRocket, shotRocket = false;

    bool shootingLaser, shotLaser, checkHit = false;
    [SerializeField] private GameObject laserPrefab;
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
    public GameObject teslaPrefab, teslaParentPrefab;
    GameObject currentTesla, nextCurrentTesla = null;
    int teslaCount = 0;
    float teslaDistance;
    Vector3 teslaDirection;
    public float teslaSpawnHeight;
    public float teslaPlacementRadius = 4f;
    public float teslaMinPlacementRad = 2.5f;
    public GameObject teslaWall, teslaParent;

    int teslaNumCount, turretNumCount = 0;
    [SerializeField] private int teslaMaxQuantity, turretMaxQuantity;

    private Dictionary<int, GameObject> placedTowers;
    private int totalTowerCount;


    //cooldown rates
    [SerializeField] private float ka1Time, ka2Time, ka3Time;
    [SerializeField] private float ga1Time, ga2Time, ga3Time;
    [SerializeField] private float ea1Time, ea2Time, ea3Time;
    private bool a1cooldown, a2cooldown, a3cooldown = false;
    private Coroutine acc1, acc2, acc3;


    //skill tree start
    SkillTreeManager skillTreeManagerObj;

    //-------------------------------------------

    //----------------Functions------------------

    public void activateAbilityOne(WeaponBase.weaponClassTypes currentClass)
    {
        if (a1cooldown || acc1 != null)
        {
            Debug.Log("Ability 1 is on cooldown.");
            return;
        }
        Debug.Log("Activating Ability 1.");

        uiManager.ActivateCooldownOnAbility(1);
        a1cooldown = true;

        if (currentClass == WeaponBase.weaponClassTypes.Knight && !bubble)
        {
            StartCoroutine(bubbleShield());
            acc1 = StartCoroutine(abilitiesCooldown(1, ka1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingRocket)
        {
            gameObject.GetComponent<masterInput>().shootingRocket = true;
            shootingRocket = true;
            StartCoroutine(abilitiesCooldown(1, ga1Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && turretNumCount < turretMaxQuantity)
        {
            turretNumCount += 1;
            totalTowerCount += 1;
            placing = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
            StartCoroutine(abilitiesCooldown(1, ea1Time));
            //currentTurret = turretTransparentPrefab;

        }
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
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer && teslaNumCount < teslaMaxQuantity)
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
            
            GameObject currentEffect = Instantiate(swordShotEffect, player.transform.position, Quaternion.identity);
            currentEffect.transform.SetParent(player.transform);
            currentEffect.transform.position = player.transform.position;
            currentEffect.GetComponent<ParticleSystem>().Play();

            StartCoroutine(stopSword(currentEffect));
            StartCoroutine(abilitiesCooldown(3, ka3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Gunner && !shootingLaser)
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
            StartCoroutine(abilitiesCooldown(3, ga3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
        else if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            StartCoroutine(abilitiesCooldown(3, ea3Time));
            gameObject.GetComponent<masterInput>().abilityInUse = false;
        }
    }

    IEnumerator abilitiesCooldown(int ability, float time)
    {
        uiManager.StartCooldownSlider(ability, (0.9f/time));
        yield return new WaitForSeconds(time);

        switch (ability)
        {
            case 1:
                yield return new WaitForSeconds(1f);
                a1cooldown = false;
                acc1 = null;
                print("ability 1 done");
          
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                a2cooldown = false;
                acc2 = null;
                print("ability 2 done");
                break;
            case 3:
                yield return new WaitForSeconds(1f);
                a3cooldown = false;
                acc3 = null;
                print("ability 3 done");
                break;
        }
        uiManager.DeactivateCooldownOnAbility(ability);


        yield break;
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

    IEnumerator auraWait()
    {
        checkingAura = true;
        currentAura = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        GameObject tempEffect = Instantiate(combatAuraEffectStart, currentAura, Quaternion.identity);
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
        yield break;
    }

    IEnumerator buffWait(Collider c)
    {
        c.gameObject.GetComponent<CharacterBase>().buffPlayer(auraHealthBuff);
        yield return new WaitForSeconds(buffRate);
        buffingPlayer = false;
        yield break;
    }

    IEnumerator swordShooting()
    {
        shotSword = true;
        var sword = Instantiate(swordShot, swordSpawn.position, swordSpawn.rotation);
        sword.GetComponent<Rigidbody>().velocity = swordSpawn.transform.forward * swordSpeed;
        sword.GetComponent<swordShot>().damage = swordShotDamage;
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
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<masterInput>().throwingGrenade = false;
        yield return new WaitForSeconds(time);
        throwingGrenade = false;
        threwGrenade = false;
        yield break;
    }

    //Engineer

   
    void removeAllTowers()
    {
        for (int i = placedTowers.Count - 1; i >= 0; i--) // Start from the end of the list
        {
            GameObject tower = placedTowers[i];
            if (tower != null)
            {
                placedTowers.Remove(i); // Remove by index
                Destroy(tower); // Destroy the GameObject
            }
        }
    }

    void removeTower(int count)
    {
        if (placedTowers.ContainsKey(count))
        {
            GameObject temp = placedTowers[count];
            placedTowers.Remove(count); // Remove the specific tower
            Destroy(temp); // Destroy the GameObject
        }
    }

    void activateTesla()
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
            }
            else
            {
                currentTesla.gameObject.transform.position = player.transform.position + teslaDirection * turretPlacementRadius;
            }
        }
        if(teslaCount == 1)
        {
            print("updating 1");
            teslaDistance = Vector3.Distance(currentTesla.transform.position, mousePos);
            teslaDirection = (mousePos - currentTesla.transform.position).normalized;

            if (teslaDistance <= teslaPlacementRadius && teslaDistance > teslaMinPlacementRad)
            {
                nextCurrentTesla.gameObject.transform.position = mousePos;
                //nextCurrentTesla.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
            else if (teslaDistance <= teslaMinPlacementRad)
            {
                nextCurrentTesla.transform.position = currentTesla.transform.position + teslaDirection * (teslaMinPlacementRad + 0.1f);  // Small buffer to avoid overlap

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
                currentTesla = Instantiate(teslaPrefab, pos + new Vector3 (0,teslaSpawnHeight,0), Quaternion.identity);
            }
            

        }
        if (playerInput.actions["Attack"].triggered && teslaCount == 1 && !placingOne && placingTwo)
        {
            //print("activate 1");
            teslaCount = 0;
            if (nextCurrentTesla != null)// && teslaDistance <= turretPlacementRadius)
            {
                placingTesla = false;
                Vector3 pos = nextCurrentTesla.transform.position;
                Destroy(nextCurrentTesla);
                nextCurrentTesla = Instantiate(teslaPrefab, pos + new Vector3(0, teslaSpawnHeight, 0), Quaternion.identity);
            }
            placingOne = false;
            placingTwo = false;

            Vector3 difference = (nextCurrentTesla.transform.position - currentTesla.transform.position) / 2;

            GameObject tesWall = GameObject.Instantiate(teslaWall, nextCurrentTesla.transform.position - difference + new Vector3(0, 1, 0), Quaternion.identity);
            teslaParent = GameObject.Instantiate(teslaParentPrefab, tesWall.transform.position, Quaternion.identity);
            tesWall.transform.LookAt(new Vector3(nextCurrentTesla.transform.position.x, 1, nextCurrentTesla.transform.position.z));
            
            tesWall.transform.SetParent(teslaParent.transform);
            currentTesla.transform.SetParent(teslaParent.transform);
            nextCurrentTesla.transform.SetParent(teslaParent.transform);
            teslaParent.GetComponent<teslaTower>().assignVars(currentTesla, nextCurrentTesla, tesWall);
            teslaParent.GetComponent<teslaTower>().setParents();

            StartCoroutine(playerInputWait());
            gameObject.GetComponent<masterInput>().abilityInUse = false;
            placedTowers.Add(totalTowerCount, teslaParent);
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
        Vector3 mousePos = Vector3.zero;
        RaycastHit hit;

        // Determine input type (mouse vs. gamepad)
        if (playerInput.actions["MouseLook"].triggered)
        {
            usingMouseInput = true; // Set flag for mouse input
        }
        else if (Mathf.Abs(playerInput.actions["GamePadLook"].ReadValue<Vector2>().magnitude) > 0.1f) // Check for joystick movement
        {
            usingMouseInput = false; // Set flag for gamepad input
        }

        // Handle mouse input
        if (usingMouseInput)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                mousePos = hit.point;
            }
        }
        else // Handle gamepad input
        {
            Vector2 inputLook = playerInput.actions["GamePadLook"].ReadValue<Vector2>(); // Get joystick input
            Debug.Log($"Gamepad Input: {inputLook}"); // Log the joystick input values

            float distance = Mathf.Clamp(inputLook.magnitude, playerRad, turretPlacementRadius); // Clamp the distance

            if (inputLook != Vector2.zero) // Check for valid input
            {
                // Normalize the joystick input to get direction
                Vector3 directionT = new Vector3(inputLook.x, 0, inputLook.y).normalized;

                // Calculate the desired turret position based on player position and input direction
                mousePos = player.transform.position + directionT * distance;
            }
            else
            {
                // Default to player position if no input
                mousePos = player.transform.position;
            }
        }

        // Instantiate the transparent turret if instant is true
        if (instant)
        {
            instant = false;
            if (turretTransparentPrefab != null) // Check if prefab is assigned
            {
                currentTurret = GameObject.Instantiate(turretTransparentPrefab, player.transform.position + new Vector3(1,0,0), player.transform.rotation); // Match player's rotation
            }
            else
            {
                Debug.LogError("turretTransparentPrefab is null");
                return; // Early exit if prefab is null
            }
        }

        // Check if currentTurret is valid
        if (currentTurret == null)
        {
            Debug.LogError("currentTurret is not initialized");
            return; // Exit if currentTurret is null
        }

        // Calculate the distance from player to mousePos
        float distanceFromPlayer = Vector3.Distance(player.transform.position, mousePos);
        Vector3 direction = (mousePos - player.transform.position).normalized;

        // Position the turret based on distance from player
        if (distanceFromPlayer <= turretPlacementRadius && distanceFromPlayer > playerRad)
        {
            currentTurret.transform.position = mousePos;
        }
        else if (distanceFromPlayer <= playerRad)
        {
            currentTurret.transform.position = player.transform.position + direction * (playerRad + 0.1f); // Small buffer to avoid overlap
            currentTurret.transform.position = new Vector3(
                currentTurret.transform.position.x,
                Mathf.Max(currentTurret.transform.position.y, player.transform.position.y + 0.03f), // Ensure turret stays above player
                currentTurret.transform.position.z
            );
        }
        else
        {
            currentTurret.transform.position = player.transform.position + direction * turretPlacementRadius;
        }

        // Ensure turret faces the same direction as the player
        currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward); // Match turret rotation to player's forward direction

        // Handle the Attack action
        if (playerInput.actions["Attack"].triggered)
        {
            if (currentTurret != null) // Ensure currentTurret is valid
            {
                placing = false; // Update placing status
                Quaternion rot = currentTurret.transform.rotation; // Store current turret rotation
                Vector3 pos = currentTurret.transform.position; // Store current turret position

                Destroy(currentTurret); // Destroy the temporary turret

                // Instantiate the final turret
                if (turretPrefab != null) // Check if turretPrefab is assigned
                {
                    currentTurret = Instantiate(turretPrefab, pos + new Vector3(0, turretSpawnHeight, 0), rot);
                    StartCoroutine(playerInputWait()); // Wait for input
                }
                else
                {
                    Debug.LogError("turretPrefab is null");
                }
            }
            gameObject.GetComponent<masterInput>().abilityInUse = false; // Reset ability in use
            placedTowers.Add(totalTowerCount, currentTurret); // Add to placed towers
        }


        /*
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
        }
        else
        {
            currentTurret.gameObject.transform.position = player.transform.position + direction * turretPlacementRadius;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }

        if (playerInput.actions["Attack"].triggered)
        {
            if (currentTurret != null)// && distance <= turretPlacementRadius)
            {
                placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Vector3 pos = currentTurret.transform.position;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, pos + new Vector3(0, turretSpawnHeight, 0), rot);
                StartCoroutine(playerInputWait());
            }
            gameObject.GetComponent<masterInput>().abilityInUse = false;
            placedTowers.Add(totalTowerCount, currentTurret);
        }
        */
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
        //currentClass = gameObject.GetComponent<masterInput>().currentClass;
        player = GameObject.FindGameObjectWithTag("Player");
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        teslaNumCount = 0;
        turretNumCount = 0;

        skillTreeManagerObj = GameObject.FindGameObjectWithTag("skillTreeManager").GetComponent<SkillTreeManager>();

        placedTowers = new Dictionary<int, GameObject>();

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
        if(checkingAura && currentAura != null)
        {
            Collider[] colliders = Physics.OverlapSphere(currentAura, comatAuraRadius);

            foreach(Collider c in colliders)
            {
                if(c.gameObject.tag == "Player" && buffingPlayer == false)
                {
                    buffingPlayer = true;
                    //player.GetComponent<CharacterBase>().buffPlayer();
                    StartCoroutine(buffWait(c));
                }
            }
        }
        

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


    //modify functions

    public void modifyBubbleRad(float amount)
    {
        Debug.Log("Modified bubble radius by: " + amount);
        bubbleRadius += amount;
    }
}

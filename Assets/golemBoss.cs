using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class golemBoss : MonoBehaviour, enemyInt
{
    public Transform player;
    public Animator animator;
    private CameraFollow camera;
    public float attackCooldown = 2f;
    public float attackRange = 5f;
    public int maxAttacks = 3;
    public float turnSpeed = 15f; // Adjusted turn speed for smoother turning
    public float detectionRadius = 7f; // Detection range
    public float maxAngle = 4f;
    public bool unlocksDoor = false;
    [SerializeField] Door doorToUnlock;

    private NavMeshAgent agent;
    //public bool isAttacking = false;
    private bool isRecovering = false;
    private bool isTurning = false;

    public LayerMask playerLayer; // Player layer for detection

    [SerializeField] GameObject weapon;

    public Vector3 weaponOffset;
    public float weaponRad;
    public int weaponDmg;
    public bool hitPlayer = false;
    public float hitPlayerCooldown;


    //Health
    public int MAXHEALTH;
    [SerializeField] private int health;
    bool bossDying = false;
    private bool isHalfHealth = false; // Flag to track if half health event has triggered
    private bool isHalfHealthInProgress = false; // Flag to track if half health event is currently in progress
    private bool forceHalfHealthEvent = false; // Flag to force half health event to complete
    
    // Damage over time tracking
    public bool dmgOverTimeActivated = false;
    private bool takingDmgOT = false;
    private UIManager uiManager;

    //attack effects
    [SerializeField]
    private List<Vector3> attackList = new List<Vector3>();
    private List<Vector3> atckPos = new List<Vector3>();

    public float jumpSpeed, jumpLength, jumpCooldown, longJumpSpeed, longJumpLength, longJumpCooldown;
    public bool canJump = true, canLongJump = true;
    bool cameraAdjusted = false;


    //Damage
    public float atkRng1, atkRng2, atkRng3, atkRng4, atkRng5, atkRng6;
    public int atkDmg1, atkDmg2, atkDmg3, atkDmg4, atkDmg5, atkDmg6;

    // New attack properties
    public float lungeSpeed, lungeLength, lungeCooldown;
    public float backAttackRange, backAttackCooldown;
    public float spawnEnemiesCooldown = 15f;
    public float dashSpeed, dashLength, dashCooldown;
    public float medJumpSpeed, medJumpLength, medJumpCooldown;
    public bool canLunge = true, canBackAttack = true, canSpawnEnemies = true, canDash = true, canMedJump = true;
    public int lungeDamage, backAttackDamage, dashDamage, medJumpDamage;
    public float lungeRadius, backAttackRadius, dashRadius, medJumpRadius;

    //effects
    [SerializeField] GameObject slash1, slash2, slashSlam, slashSlam1, slashSlam2, jumpSlam;
    [SerializeField] GameObject lungeEffect, backAttackEffect, dashEffect, medJumpEffect;
    [SerializeField] GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn randomly
[SerializeField] Transform[] spawnPoints; // Optional predefined spawn points
    public GameObject healthRef;
    [SerializeField] GameObject enemyHealth;
    Slider enemyHealthBar;
    Slider delayedEnemyHealthBar;
    GameObject enemyUIRef;
    public float slash1Time = 1f, slash2Time = 1f, slashSlamTime = 1f, slashSlam2Time = 1f, slashSlam3Time = 1f;
    public float slamRadius = 2.5f, slamTime1, slamTime2;
    public Vector3 slamOffset = Vector3.zero;
    public Vector3 medJumpOffset = Vector3.zero;

    //Dialogue
    [SerializeField] DialogueObject halfway_dialogueObject;
    [SerializeField] DialogueObject finish_dialogueObject;

    //Enemy Interface 
    private bool _isAttacking;
    public bool isAttacking
    {
        get { return _isAttacking; }
        set
        {
            if (_isAttacking != value)  // Only set if the value is different
            {
                _isAttacking = value;
                // Do the other necessary actions
            }
        }
    }

    private bool _isActive;
    public bool isActive
    {
        get { return _isActive; }
        set
        {
            if (_isActive != value)  // Only set if the value is different
            {
                _isActive = value;
                // Do the other necessary actions
            }
        }
    }

    //-----------------Main Functions------------------------------

    private float spawnEnemiesTimer = 0f;

    void Start()
    {
        isActive = false;
        agent = GetComponent<NavMeshAgent>();
        slash1.GetComponent<ParticleSystem>().Stop();
        animator.GetComponent<Animator>();
        health = MAXHEALTH;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        animator.SetBool("death", false);

        // Initialize spawn enemies timer with a random offset
        spawnEnemiesTimer = Random.Range(0f, spawnEnemiesCooldown * 0.5f);
        // Get UI Manager reference
        uiManager = GameObject.Find("UIManager")?.GetComponent<UIManager>();
        enemyUIRef = GameObject.Find("DynamicEnemyUI");

        healthRef = Instantiate(enemyHealth);
        healthRef.transform.SetParent(enemyUIRef.transform, false);

        enemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().health;
        delayedEnemyHealthBar = healthRef.GetComponent<EnemyHealthPrefab>().delayedHealth;

        enemyHealthBar.maxValue = MAXHEALTH;
        delayedEnemyHealthBar.maxValue = MAXHEALTH;

        enemyHealthBar.value = health;
        delayedEnemyHealthBar.value = health;
        
        // Ensure attack radii have reasonable default values if not set in inspector
        if (medJumpRadius <= 0)
            medJumpRadius = 7.0f; // Increased default medium jump attack radius to ensure it hits the player
            
        // Ensure playerLayer is set correctly
        if (playerLayer.value == 0)
        {
            Debug.LogWarning("playerLayer not set in inspector, defaulting to layer 'Player'");
            playerLayer = LayerMask.GetMask("Player");
        }
        
        Debug.Log("Boss initialized with playerLayer: " + LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(playerLayer.value, 2))));
    }

    void Update()
    {
        if (bossDying || !isActive)
            return;
            
        // Check if half health event is in progress and stop all other processing
        if (isHalfHealthInProgress)
            return;
            
        // If we need to force the half health event (e.g., during a jump), stop everything and trigger it
        if (forceHalfHealthEvent && !isHalfHealthInProgress)
        {
            Debug.Log("Force triggering half health event from Update!");
            StopAllCoroutines();
            StartCoroutine(HalfHealthImplementation());
            return;
        }

        if (!cameraAdjusted)
        {
            adjustCameraOffset(new Vector3(0.0f, 13f, -4f));
            cameraAdjusted = true;
        }

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null) return;

        //if (hitPlayer)
            //StartCoroutine(hitPlayerWait());

        // Update spawn enemies timer
        if (canSpawnEnemies)
        {
            spawnEnemiesTimer += Time.deltaTime;
            if (spawnEnemiesTimer >= spawnEnemiesCooldown)
            {
                StartCoroutine(SpawnEnemies());
                spawnEnemiesTimer = 0f;
            }
        }

        // Check if player is within detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        bool playerDetected = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerDetected = true;
                break;
            }
        }

        if (!playerDetected)
        {
            animator.SetFloat("Forward", 0); // Stop movement animation
            return; // Stop processing if player is out of range
        }
        checkWeaponArea();
        checkAttackAreas();
        if (isAttacking || isRecovering)
        {
            agent.isStopped = true;
            return;
        }

        // Calculate the angle to the player
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        // If the angle exceeds the threshold, start turning
        if (Mathf.Abs(angle) > maxAngle && !isTurning)
        {
            StartCoroutine(TurnInPlace(angle));
            return; // Don't move while turning
        }

        // Move towards player if not turning
        if (!isTurning)
        {
            gameObject.transform.LookAt(player.position, Vector3.up);
            agent.isStopped = false;
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Forward", speed / agent.speed); // Normalize speed for animation
            animator.SetFloat("Turn", 0); // Reset turn animation when moving
        }

        // Attack if in range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            //StartCoroutine(AttackSequence());
        }

        
        
    }

    public void DeactivateHealthBar()
    {
        if (healthRef != null)
        {
            healthRef.gameObject.SetActive(false);
        }

    }

    public void ActivateHealthBar()
    {
        if (healthRef != null)
        {
            healthRef.gameObject.SetActive(true);
        }

    }

    IEnumerator hitPlayerWait()
    {
        yield return new WaitForSeconds(hitPlayerCooldown);
        hitPlayer = false;
        yield break;
    }

    public enemyInt getType()
    {
        return this;
    }

    public void onDeath()
    {
        var audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.ChangeTrack(GameObject.Find("SceneInformation").GetComponent<SceneInformation>().beginningTrack);
        audioManager.PlaySFX("BattleComplete");
        DeactivateHealthBar();
        camera.StartPan(this.transform.position, true, true, 0.05f);
        if (finish_dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(finish_dialogueObject));
        adjustCameraOffset(new Vector3(0.0f, -13f, 4f));
        StartCoroutine(unlockDoor());
    }

    public IEnumerator unlockDoor()
    {
        yield return new WaitForSeconds(5f);
        if (unlocksDoor && doorToUnlock != null)
        {
            camera.StartPan(doorToUnlock.transform.position, true, true, 0.05f);
            doorToUnlock.ToggleDoor();
        }
    }

    public void adjustCameraOffset(Vector3 adjustment)
    {
        camera.offset += adjustment;
    }



    public void takeDamage(int damage)
    {
        if (health - damage > 0)
        {
            health -= damage;
            StartCoroutine(updateHealthBarsNegative());

            // Check if health has dropped below half and the half health event hasn't triggered yet
            if (!isHalfHealth && health <= MAXHEALTH / 2)
            {
                isHalfHealth = true;
                // Force the half health event to happen regardless of current state
                StartCoroutine(HalfHealth());
            }
        }
        else
            StartCoroutine(bossDeath());
    }

    // List to track spawned enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    
    // Method to add enemy to tracking list
    public void TrackSpawnedEnemy(GameObject enemy)
    {
        if (enemy != null && !spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Add(enemy);
        }
    }
    
    public IEnumerator bossDeath()
    {
        if (bossDying)
            yield break;

        bossDying = true;

        // Kill all spawned enemies
        StartCoroutine(KillAllSpawnedEnemies());
        
        animator.SetBool("death", true);
        animator.Play("death");
        animator.SetBool("death", false);
        onDeath();
        yield break;
    }
    
    // Method to kill all spawned enemies when boss dies
    private IEnumerator KillAllSpawnedEnemies()
    {
        Debug.Log("Killing all " + spawnedEnemies.Count + " spawned enemies");
        
        // Create a copy of the list to avoid modification issues during iteration
        List<GameObject> enemiesToKill = new List<GameObject>(spawnedEnemies);
        
        foreach (GameObject enemy in enemiesToKill)
        {
            if (enemy != null)
            {
                // Get the EnemyFrame component and trigger normal death
                EnemyFrame enemyFrame = enemy.GetComponent<EnemyFrame>();
                if (enemyFrame != null)
                {
                    // Use the takeDamage method with a large amount of damage to ensure death
                    enemyFrame.takeDamage(99999, Vector3.zero, EnemyFrame.DamageSource.Enemy, EnemyFrame.DamageType.Sword);
                    Debug.Log("Killed spawned enemy: " + enemy.name);
                    
                    // Small delay between kills for visual effect
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    // Fallback if no EnemyFrame component
                    Destroy(enemy);
                }
            }
        }
        
        // Clear the list
        spawnedEnemies.Clear();
    }
    
    // Damage over time method for fire rune effects
    public IEnumerator dmgOverTime(int dmg, float statusTime, float dmgRate, EnemyFrame.DamageType dmgType)
    {
        // If already taking damage over time, don't start another instance
        if (takingDmgOT)
            yield break;
            
        takingDmgOT = true;
        dmgOverTimeActivated = true;
        
        float endTime = Time.time + statusTime;
        
        Debug.Log("Boss starting damage over time at: " + Time.time + " Until: " + endTime);
        
        // Continue applying damage over time until the statusTime expires
        while (Time.time < endTime && !bossDying)
        {
            // Apply damage once per dmgTime interval
            if (this == null)
            {
                yield break;
            }
            
            // Apply damage to the boss
            takeDamage(dmg);
            
            // Display damage number if UI manager is available
            if (uiManager != null)
            {
                uiManager.DisplayDamageNum(gameObject.transform, dmg);
            }
            
            Debug.Log("Boss damage taken: " + dmg + " at time: " + Time.time);
            
            // Wait for the next damage tick
            yield return new WaitForSeconds(dmgRate);
        }
        
        // Once the effect duration ends, reset flags and exit the coroutine
        Debug.Log("Boss finished dmgOverTime at: " + Time.time);
        dmgOverTimeActivated = false;
        takingDmgOT = false;
        
        yield break;
    }
    
    public IEnumerator HalfHealth()
    {
        // If already in progress, don't start again
        if (isHalfHealthInProgress)
            yield break;
            
        // Set flag to indicate half health event is in progress
        isHalfHealthInProgress = true;
        forceHalfHealthEvent = true;
        
        Debug.Log("Boss has reached half health! Stopping all attacks and entering rage mode!");
        
        // Force stop all attacks and movement
        StopAllCoroutines(); // Stop all running coroutines including attacks
        if (halfway_dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(halfway_dialogueObject));
        // Only keep this coroutine running
        StartCoroutine(HalfHealthImplementation());
        
        // We don't need to yield anything here since we're starting a new coroutine
        yield break;
    }
    
    private IEnumerator HalfHealthImplementation()
    {
        Debug.Log("Starting HalfHealthImplementation");
        
        // Stop current actions and reset state
        isAttacking = false;
        isRecovering = false;
        agent.isStopped = true;
        
        // Reset all attack flags to prevent any attacks from continuing
        canJump = false;
        canLongJump = false;
        canLunge = false;
        canBackAttack = false;
        canDash = false;
        canMedJump = false;
        canSpawnEnemies = false;
        
        // Wait a frame to ensure all other coroutines are stopped
        yield return null;
        
        // Set attacking and recovering flags to prevent new attacks
        isAttacking = true;
        isRecovering = true;
        
        // We don't need to reset position since animations handle the vertical movement
        // Just make sure we're on the NavMesh for movement after the event
        NavMeshHit navHit;
        if (!NavMesh.SamplePosition(transform.position, out navHit, 5f, NavMesh.AllAreas))
        {
            Debug.LogWarning("HalfHealth: Boss position not on NavMesh, attempting to find valid position");
            // Only adjust position if absolutely necessary
            if (NavMesh.SamplePosition(transform.position, out navHit, 10f, NavMesh.AllAreas))
            {
                transform.position = new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
            }
        }
        
        Debug.Log("Boss has reached half health! Entering rage mode!");
        
        // Play a special animation or effect
        animator.SetBool("halfHealth", true);
        animator.Play("halfHealth"); // Make sure this animation exists
        
        // Wait for animation to play
        yield return new WaitForSeconds(.5f);
        animator.SetBool("halfHealth", false);
        
        // Enhance boss abilities in second phase
        // For example, increase speed, damage, or unlock new attacks
        agent.speed *= 1.25f; // 25% speed increase
        turnSpeed *= 1.2f; // 20% turn speed increase
        
        // Reduce cooldowns for more aggressive attacks
        lungeCooldown *= 0.8f;
        backAttackCooldown *= 0.8f;
        dashCooldown *= 0.8f;
        jumpCooldown *= 0.8f;
        longJumpCooldown *= 0.8f;
        medJumpCooldown *= 0.8f;
        spawnEnemiesCooldown *= 0.7f; // More frequent enemy spawns
        
        // Increase damage
        weaponDmg = (int)(weaponDmg * 1.3f);
        lungeDamage = (int)(lungeDamage * 1.3f);
        backAttackDamage = (int)(backAttackDamage * 1.3f);
        dashDamage = (int)(dashDamage * 1.3f);
        medJumpDamage = (int)(medJumpDamage * 1.3f);
        
        // Spawn enemies as part of the phase transition
        if (enemyPrefabs != null && enemyPrefabs.Length > 0)
        {
            // Always spawn maximum number of enemies at half health
            int enemyCount = maxEnemyCount;
            Debug.Log("Half health event: Spawning " + enemyCount + " enemies");
            
            for (int i = 0; i < enemyCount; i++)
            {
                // Select a random enemy prefab from the array
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject selectedEnemyPrefab = enemyPrefabs[randomEnemyIndex];
                
                if (selectedEnemyPrefab != null)
                {
                    Vector3 spawnPosition;
                    
                    // Use predefined spawn points if available, otherwise spawn around the boss
                    if (spawnPoints != null && spawnPoints.Length > 0)
                    {
                        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
                        Transform spawnPoint = spawnPoints[randomSpawnPointIndex];
                        spawnPosition = spawnPoint.position;
                        Debug.Log("Half health: Spawning enemy at predefined point: " + randomSpawnPointIndex);
                    }
                    else
                    {
                        // Spawn in a circle around the boss for the half health event
                        float angle = i * (360f / enemyCount);
                        float radians = angle * Mathf.Deg2Rad;
                        Vector3 spawnOffset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * 4f;
                        spawnPosition = transform.position + spawnOffset;
                        
                        // Ensure position is on NavMesh
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(spawnPosition, out hit, 2f, NavMesh.AllAreas))
                        {
                            spawnPosition = hit.position;
                        }
                    }
                    
                    // Instantiate the enemy
                    GameObject enemy = Instantiate(selectedEnemyPrefab, spawnPosition, Quaternion.identity);
                    
                    if (enemy != null)
                    {
                        // Make enemies face the player if available, otherwise face the boss
                        if (player != null)
                            enemy.transform.LookAt(player.position);
                        else
                            enemy.transform.LookAt(transform.position);
                            
                        Debug.Log("Half health: Spawned enemy type: " + selectedEnemyPrefab.name);
                        
                        // Track this enemy so we can kill it when the boss dies
                        TrackSpawnedEnemy(enemy);
                    }
                }
                
                // Small delay between spawns
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        // Optional: Visual indication of rage mode
        // You could change the boss's color, add particle effects, etc.
        
        // Reset flags to allow normal behavior to resume
        isHalfHealthInProgress = false;
        forceHalfHealthEvent = false;
        isAttacking = false;
        isRecovering = false;
        agent.isStopped = false;
        
        // Reset all attack cooldowns to ensure they're available
        canJump = true;
        canLongJump = true;
        canLunge = true;
        canBackAttack = true;
        canDash = true;
        canMedJump = true;
        canSpawnEnemies = true;
        
        Debug.Log("Half health event completed, boss entering rage mode!");
        yield break;
    }

    // Offset vectors are already defined at the top of the class

    IEnumerator slamArea()
    {
        yield return new WaitForSeconds(slamTime1);
        jumpSlam.GetComponent<ParticleSystem>().Play();
        Collider[] temp = Physics.OverlapSphere(gameObject.transform.position + transform.rotation * (slamOffset + Vector3.up + Vector3.forward), slamRadius);
        foreach (Collider p in temp)
        {
            if (p.CompareTag("Player"))
            {
                // Use atkDmg2 for the regular jump attack
                p.GetComponent<CharacterBase>().takeDamage(atkDmg2, gameObject.transform.forward);
                UIManager.instance.DisplayDamageNum(p.transform, atkDmg2);
            }
        }

        yield break;
    }
    
    IEnumerator slamArea2()
    {
        yield return new WaitForSeconds(slamTime2);
        jumpSlam.GetComponent<ParticleSystem>().Play();
        Collider[] temp = Physics.OverlapSphere(gameObject.transform.position + transform.rotation * (slamOffset + Vector3.up + Vector3.forward), slamRadius);
        foreach (Collider p in temp)
        {
            if (p.CompareTag("Player"))
            {
                // Use atkDmg3 for the long jump attack
                p.GetComponent<CharacterBase>().takeDamage(atkDmg3, gameObject.transform.forward);
                UIManager.instance.DisplayDamageNum(p.transform, atkDmg3);
            }
        }
        yield break;
    }

    public float medJumpTime = 0.8f;
    IEnumerator medJumpSlice()
    {
        yield return new WaitForSeconds(medJumpTime);
        medJumpEffect.GetComponent<ParticleSystem>().Play();
        Collider[] temp = Physics.OverlapSphere(gameObject.transform.position + transform.rotation * (medJumpOffset + Vector3.up + Vector3.forward), slamRadius);
        foreach (Collider p in temp)
        {
            if (p.CompareTag("Player"))
            {
                // Use medJumpDamage for the medium jump attack
                p.GetComponent<CharacterBase>().takeDamage(medJumpDamage, gameObject.transform.forward);
                UIManager.instance.DisplayDamageNum(p.transform, medJumpDamage);
            }
        }
        yield break;
    }

    //-----------------Turning------------------------------

    IEnumerator TurnInPlace(float totalAngle)
    {
        isTurning = true;
        agent.isStopped = true;
        animator.SetFloat("Forward", 0); // No movement during turning
        animator.SetFloat("Turn", Mathf.Sign(totalAngle)); // Update turn animation based on angle direction

        float remainingAngle = Mathf.Abs(totalAngle);
        float turnDirection = Mathf.Sign(totalAngle);

        // Rotate until the remaining angle is small enough
        while (remainingAngle > 1) // Small buffer to stop rotation
        {
            float turnStep = turnSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * turnDirection * turnStep);
            remainingAngle -= turnStep;
            yield return null;
        }

        // After turning, set the rotation to face the player
        transform.rotation = Quaternion.LookRotation(transform.forward);
        isTurning = false;
    }







    //-----------------ATTACKING------------------------------

    void checkAttackAreas()
    {
        if (isAttacking || isRecovering)
            return;

        for(int i = 0; i < attackList.Count; i++)
        {
            switch(i)
            {
                case 0:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng1)
                        StartCoroutine(AttackSequence());
                    break;

                case 1:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng2 && canJump)
                        StartCoroutine(JumpAttack(jumpSpeed, jumpLength));
                    break;
                    
                case 2:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng3 && canLongJump)
                        StartCoroutine(longJumpAttack(longJumpSpeed, longJumpLength));
                    break;
                    
                case 3:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng4 && canLunge)
                        StartCoroutine(LungeAttack(lungeSpeed, lungeLength));
                    break;
                    
                case 4:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng5 && canBackAttack)
                        StartCoroutine(BackAttack());
                    break;
                    
                case 5:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng6 && canDash)
                        StartCoroutine(DashAttack(dashSpeed, dashLength));
                    break;
                    
                case 6:
                    if (Vector3.Distance(player.transform.position, gameObject.transform.position + transform.rotation * (attackList[i] + Vector3.up + Vector3.forward)) <= atkRng6 && canMedJump)
                        StartCoroutine(MedJumpAttack(medJumpSpeed, medJumpLength));
                    break;
            }
        }
    }

    // Add these variables for the weapon capsule
    public Vector3 weaponBaseOffset;
    
    void checkWeaponArea()
    {
        if (!isAttacking || hitPlayer || isRecovering)
            return;

        Debug.Log("golem: checkingWeapon");
        
        // Get the head and base positions for the capsule
        Vector3 headPos = weapon.GetComponent<golemBossWeapon>().headPosition.transform.position + weaponOffset;
        Vector3 basePos = weapon.transform.position + weaponBaseOffset;
        
        // Use OverlapCapsule to check for player hits along the weapon
        Collider[] player = Physics.OverlapCapsule(headPos, basePos, weaponRad, playerLayer);
        
        foreach(Collider p in player)
        {
            hitPlayer = true;
            p.GetComponent<CharacterBase>().takeDamage(weaponDmg, gameObject.transform.forward);
            UIManager.instance.DisplayDamageNum(p.transform, weaponDmg);
            StartCoroutine(hitPlayerWait());
        }
    }

    IEnumerator longJumpAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canLongJump || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
        canLongJump = false;
        isAttacking = true;
        agent.isStopped = true;
        // We'll handle the timing directly in this method instead of using slamArea2()
        // StartCoroutine(slamArea2());
        Debug.Log("golem: Starting longJump Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("jumpAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("jumpAttack");
        animator.SetBool("jumpAttack", false);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 initialDirection = transform.forward;
        
        // Track the player for the first half of the jump
        bool trackingPlayer = true;
        float trackingDuration = duration * .6f; // Track for half the duration
        
        // Define when to trigger the attack effect (percentage of the jump duration)
        float attackTriggerTime = 0.9f; // Trigger at 75% through the jump (slightly later for long jump)
        bool attackTriggered = false;

        while (elapsedTime < duration)
        {
            // Calculate normalized time
            float normalizedTime = elapsedTime / duration;
            
            // Update direction to track player during first half of jump
            if (trackingPlayer && elapsedTime < trackingDuration && player != null)
            {
                // Get direction to player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                // Keep y component the same to avoid tilting up/down
                directionToPlayer.y = 0;
                directionToPlayer.Normalize();
                
                // Smoothly rotate towards player
                transform.forward = Vector3.Lerp(transform.forward, directionToPlayer, Time.deltaTime * turnSpeed * 0.5f);
            }
            
            // Calculate target position based on current forward direction
            Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);
            
            // Move toward the target
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
            
            // Check if we've reached the attack trigger point and haven't triggered yet
            if (!attackTriggered && normalizedTime >= attackTriggerTime)
            {
                attackTriggered = true;
                
                // Play the effect at the specific time
                jumpSlam.GetComponent<ParticleSystem>().Play();
                
                // Check for player hit with Physics.OverlapSphere at this specific moment
                Collider[] hitPlayers = Physics.OverlapSphere(
                    gameObject.transform.position + transform.rotation * (slamOffset + Vector3.up + Vector3.forward), 
                    slamRadius, 
                    playerLayer);
                
                foreach (Collider p in hitPlayers)
                {
                    if (p.CompareTag("Player"))
                    {
                        p.GetComponent<CharacterBase>().takeDamage(atkDmg3, gameObject.transform.forward);
                        UIManager.instance.DisplayDamageNum(p.transform, atkDmg3);
                    }
                }
                
                Debug.Log("Long Jump Attack effect triggered at " + normalizedTime + " of jump duration");
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("golem: long Jump Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");

        isRecovering = false;
        
        agent.isStopped = false;
        yield return new WaitForSeconds(longJumpCooldown);

        //animator.SetTrigger("ReturnToIdle"); // Use a trigger for animation transition
        canLongJump = true;
        yield break;
    }

    IEnumerator JumpAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canJump || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
        // Check if half health event is in progress
        if (isHalfHealthInProgress || forceHalfHealthEvent)
        {
            Debug.Log("Jump attack prevented by half health event!");
            canJump = true;
            yield break;
        }

        canJump = false;
        isAttacking = true;
        agent.isStopped = true;
        // We'll handle the timing directly in this method instead of using slamArea()
        // StartCoroutine(slamArea());
        Debug.Log("golem: Starting Jump Attack");
        
        // Store initial position in case we need to return
        Vector3 initialPosition = transform.position;

        animator.SetFloat("Forward", 0);
        animator.SetBool("jump", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("jumpAtck");
        animator.SetBool("jump", false);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 initialDirection = transform.forward;
        
        // Track the player for the first half of the jump
        bool trackingPlayer = true;
        float trackingDuration = duration * 0.5f; // Track for half the duration
        
        // Define when to trigger the attack effect (percentage of the jump duration)
        float attackTriggerTime = 0.6f; // Trigger at 60% through the jump
        bool attackTriggered = false;
        
        // Check if half health event is triggered before starting the jump
        if (forceHalfHealthEvent)
        {
            // Abort the jump and let the half health event take over
            Debug.Log("Jump attack prevented by half health event!");
            isAttacking = false;
            canJump = true;
            yield break;
        }

        while (elapsedTime < duration)
        {
            // Calculate normalized time
            float normalizedTime = elapsedTime / duration;
            
            // Update direction to track player during first half of jump
            if (trackingPlayer && elapsedTime < trackingDuration && player != null)
            {
                // Get direction to player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                // Keep y component the same to avoid tilting up/down
                directionToPlayer.y = 0;
                directionToPlayer.Normalize();
                
                // Smoothly rotate towards player
                transform.forward = Vector3.Lerp(transform.forward, directionToPlayer, Time.deltaTime * turnSpeed * 0.5f);
            }
            
            // Calculate target position based on current forward direction
            Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);
            
            // Move toward the target
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
            
            // Check if we've reached the attack trigger point and haven't triggered yet
            if (!attackTriggered && normalizedTime >= attackTriggerTime)
            {
                attackTriggered = true;
                
                // Play the effect at the specific time
                jumpSlam.GetComponent<ParticleSystem>().Play();
                
                // Check for player hit with Physics.OverlapSphere at this specific moment
                Collider[] hitPlayers = Physics.OverlapSphere(
                    gameObject.transform.position + transform.rotation * (slamOffset + Vector3.up + Vector3.forward), 
                    slamRadius, 
                    playerLayer);
                
                foreach (Collider p in hitPlayers)
                {
                    if (p.CompareTag("Player"))
                    {
                        p.GetComponent<CharacterBase>().takeDamage(atkDmg2, gameObject.transform.forward);
                        UIManager.instance.DisplayDamageNum(p.transform, atkDmg2);
                    }
                }
                
                Debug.Log("Jump Attack effect triggered at " + normalizedTime + " of jump duration");
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("golem: Jump Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");

        isRecovering = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(jumpCooldown);

        //animator.SetTrigger("ReturnToIdle"); // Use a trigger for animation transition
        canJump = true;
    }


    IEnumerator AttackSequence()
    {
        if (isAttacking || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;

        isAttacking = true;
        agent.isStopped = true;
        
        animator.SetFloat("Forward", 0);

        int attackCount = Random.Range(1, maxAttacks + 1);
        
        // Store starting position for movement calculations
        Vector3 startPosition = transform.position;
        
        // Define forward movement parameters
        float forwardMovementDistance = 1.0f; // How far to move forward during attack
        float movementDuration = 0.0f; // Will be set based on attack type
        
        switch(attackCount)
        {
            case 1:
                animator.SetBool("Attack", true);
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack1");
                animator.SetBool("Attack", false);
                
                // Set movement duration for this attack
                movementDuration = slash1Time * 0.8f; // Move during 80% of the attack time
                
                // Move forward during attack
                StartCoroutine(MoveForwardDuringAttack(forwardMovementDistance, movementDuration));
                
                yield return new WaitForSeconds(slash1Time);
                slash1.GetComponent<ParticleSystem>().Play();
                break;
                
            case 2:
                animator.SetBool("Attack2", true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack2");
                animator.SetBool("Attack2", false);
                
                // Set movement duration for this attack
                movementDuration = slash2Time * 0.8f; // Move during 80% of the attack time
                
                // Move forward during attack
                StartCoroutine(MoveForwardDuringAttack(forwardMovementDistance, movementDuration));
                
                yield return new WaitForSeconds(slash2Time);
                slash2.GetComponent<ParticleSystem>().Play();
                break;
                
            case 3:
                animator.SetBool("Attack3", true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                    animator.Play("attack3");
                animator.SetBool("Attack3", false);
                
                // For combo attack, move forward in stages
                StartCoroutine(MoveForwardDuringAttack(forwardMovementDistance * 0.4f, slashSlamTime * 0.8f));
                
                yield return new WaitForSeconds(slashSlamTime);
                slashSlam.GetComponent<ParticleSystem>().Play();
                
                StartCoroutine(MoveForwardDuringAttack(forwardMovementDistance * 0.3f, slashSlam2Time * 0.8f));
                
                yield return new WaitForSeconds(slashSlam2Time);
                slashSlam1.GetComponent<ParticleSystem>().Play();
                
                StartCoroutine(MoveForwardDuringAttack(forwardMovementDistance * 0.3f, slashSlam3Time * 0.8f));
                
                yield return new WaitForSeconds(slashSlam3Time);
                slashSlam2.GetComponent<ParticleSystem>().Play();
                break;
        }
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
        
        //yield return new WaitForSeconds(attackCooldown);

        isRecovering = true;
        yield return new WaitForSeconds(1f);

        isRecovering = false;
        isAttacking = false;
        hitPlayer = false;
        agent.isStopped = false;
    }
    
    // Helper method to move forward during attacks
    private IEnumerator MoveForwardDuringAttack(float distance, float duration)
    {
        if (duration <= 0)
            yield break;
            
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * distance;
        
        // Check if the target position is valid on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, distance, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }
        else
        {
            // If not valid, reduce the distance
            targetPosition = startPosition + transform.forward * (distance * 0.5f);
        }
        
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // Calculate normalized time and position
            float normalizedTime = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we reach the exact target position
        transform.position = targetPosition;
    }




    //IEnumerator checkAttack(float attackTime,)

    // New attack methods
    IEnumerator LungeAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canLunge || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
            
        canLunge = false;
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("golem: Starting Lunge Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("lungeAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("lungeAttack"); // Make sure this animation exists
        animator.SetBool("lungeAttack", false);

        // Quick forward movement
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        
        // Check for player hit with Physics.OverlapSphere
        if (lungeEffect != null)
            lungeEffect.GetComponent<ParticleSystem>().Play();
            
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
        //Collider[] hitPlayers = Physics.OverlapSphere(
            //gameObject.transform.position + transform.rotation * (attackList[3] + Vector3.up + Vector3.forward), 
            //lungeRadius, 
            //playerLayer);
            
        //foreach (Collider p in hitPlayers)
        //{
            //p.GetComponent<CharacterBase>().takeDamage(lungeDamage, gameObject.transform.forward);
            //UIManager.instance.DisplayDamageNum(p.transform, lungeDamage);
        //}

        Debug.Log("golem: Lunge Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");
        isRecovering = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(lungeCooldown);

        canLunge = true;
        yield break;
    }

    public float rotationTime = 0.3f;

    IEnumerator BackAttack()
    {
        if (isAttacking || !canBackAttack || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
            
        canBackAttack = false;
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("golem: Starting Back Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("backAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("backAttack"); // Make sure this animation exists
        animator.SetBool("backAttack", false);
        
        // Quickly rotate 180 degrees to face behind
        float startRotation = transform.eulerAngles.y;
        float targetRotation = startRotation + 180f;
        //float rotationTime = 0.3f;
        float elapsedTime = 0f;
        
        while (elapsedTime < rotationTime)
        {
            float yRotation = Mathf.Lerp(startRotation, targetRotation, elapsedTime / rotationTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure exact rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotation, transform.eulerAngles.z);
        
        // Check for player hit with Physics.OverlapSphere
        //if (backAttackEffect != null)
            //backAttackEffect.GetComponent<ParticleSystem>().Play();
            
        //Collider[] hitPlayers = Physics.OverlapSphere(
            //gameObject.transform.position + transform.rotation * (attackList[4] + Vector3.up + Vector3.forward), 
            //backAttackRadius, 
            //playerLayer);
            
        //foreach (Collider p in hitPlayers)
        //{
           // p.GetComponent<CharacterBase>().takeDamage(backAttackDamage, gameObject.transform.forward);
            //UIManager.instance.DisplayDamageNum(p.transform, backAttackDamage);
        //}
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
        
        Debug.Log("golem: Back Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");
        isRecovering = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(backAttackCooldown);

        canBackAttack = true;
        yield break;
    }

    // Configuration for spawn positions
    public float minDistanceFromPlayer = 3f;  // Minimum distance from player
    public float maxDistanceFromPlayer = 8f;  // Maximum distance from player
    public int minEnemyCount = 2;            // Minimum number of enemies to spawn
    public int maxEnemyCount = 5;            // Maximum number of enemies to spawn
    public float spawnRadius = 1.5f;         // Radius to check if spawn position is clear
    public LayerMask obstacleLayer;          // Layer mask for obstacles to avoid spawning inside

    IEnumerator SpawnEnemies()
    {
        if (isAttacking || isRecovering || !canSpawnEnemies || player == null)
            yield break;
            
        canSpawnEnemies = false;
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("golem: Spawning Enemies");

        animator.SetFloat("Forward", 0);
        animator.SetBool("spawnEnemies", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("spawnEnemies"); // Make sure this animation exists
        animator.SetBool("spawnEnemies", false);
        
        // Check if we have any enemy prefabs to spawn
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned to spawn!");
            yield break;
        }
        
        // Determine how many enemies to spawn (random between min and max)
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);
        Debug.Log("Spawning " + enemyCount + " enemies");
        
        // Spawn each enemy
        for (int i = 0; i < enemyCount; i++)
        {
            // Select a random enemy prefab from the array
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject selectedEnemyPrefab = enemyPrefabs[randomEnemyIndex];
            
            if (selectedEnemyPrefab != null)
            {
                Vector3 spawnPosition;
                
                // Determine spawn position based on available options
                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    // Option 1: Use predefined spawn points if available
                    int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
                    Transform spawnPoint = spawnPoints[randomSpawnPointIndex];
                    spawnPosition = spawnPoint.position;
                    Debug.Log("Spawning enemy at predefined point: " + randomSpawnPointIndex);
                }
                else
                {
                    // Option 2: Spawn near player but not too close or far
                    spawnPosition = GetSpawnPositionNearPlayer();
                    Debug.Log("Spawning enemy near player at: " + spawnPosition);
                }
                
                // Instantiate the enemy
                GameObject enemy = Instantiate(selectedEnemyPrefab, spawnPosition, Quaternion.identity);
                
                // Make the enemy face the player
                if (enemy != null)
                {
                    enemy.transform.LookAt(player.position);
                    Debug.Log("Spawned enemy type: " + selectedEnemyPrefab.name);
                    
                    // Track this enemy so we can kill it when the boss dies
                    TrackSpawnedEnemy(enemy);
                }
            }
            
            // Small delay between spawns
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("golem: Enemies Spawned, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete");
        isRecovering = false;
        agent.isStopped = false;
        canSpawnEnemies = true;
        yield break;
    }
    
    // Helper method to find a valid spawn position near the player
    private Vector3 GetSpawnPositionNearPlayer()
    {
        // Try to find a valid position up to 10 times
        for (int attempt = 0; attempt < 10; attempt++)
        {
            // Generate a random direction around the player
            float randomAngle = Random.Range(0f, 360f);
            float randomDistance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
            
            // Calculate position in a circle around the player
            float radians = randomAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * randomDistance;
            Vector3 potentialPosition = player.position + offset;
            
            // Ensure the position is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(potentialPosition, out hit, 2f, NavMesh.AllAreas))
            {
                // Check if the position is clear of obstacles
                if (!Physics.CheckSphere(hit.position, spawnRadius, obstacleLayer))
                {
                    return hit.position;
                }
            }
        }
        
        // If all attempts fail, fall back to a position around the boss
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 fallbackOffset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 3f;
        return transform.position + fallbackOffset;
    }

    void LateUpdate()
    {
        
            healthRef.transform.position = new Vector3(this.transform.position.x - 4.5f,
            this.transform.position.y + 6f, this.transform.position.z);
    }

    IEnumerator DashAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canDash || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
            
        canDash = false;
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("golem: Starting Forward Dash Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("dashAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("dashAttack"); // Make sure this animation exists
        

        // Quick forward dash movement
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        
        // Dash straight ahead
        Vector3 targetPosition = startPosition + transform.forward * (moveSpeed * duration);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        animator.SetBool("dashAttack", false);
        // Check for player hit with Physics.OverlapSphere
        if (dashEffect != null)
            dashEffect.GetComponent<ParticleSystem>().Play();
            
        Collider[] hitPlayers = Physics.OverlapSphere(
            gameObject.transform.position + transform.rotation * (attackList[5] + Vector3.up + Vector3.forward), 
            dashRadius, 
            playerLayer);
            
        foreach (Collider p in hitPlayers)
        {
            p.GetComponent<CharacterBase>().takeDamage(dashDamage, gameObject.transform.forward);
            UIManager.instance.DisplayDamageNum(p.transform, dashDamage);
        }

        Debug.Log("golem: Dash Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");
        isRecovering = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        yield break;
    }

    public IEnumerator updateHealthBarsNegative()
    {
        //StopCoroutine(animateHealth());
        yield return animateHealth();
        yield return new WaitForSeconds(0.2f);
        //StopCoroutine(animateDelayedHealth());
        yield return animateDelayedHealth();
    }

    public IEnumerator animateHealth()
    {
        Debug.Log("Inside animate health");
        float reduceVal = 250f;
        while (enemyHealthBar.value != health)
        {
            if (Mathf.Abs(enemyHealthBar.value - health) <= 5)
            {
                enemyHealthBar.value = health;
            }
            else if (health < enemyHealthBar.value)
            {
                enemyHealthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                enemyHealthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }

    public IEnumerator animateDelayedHealth()
    {
        float reduceVal = 250f;
        while (delayedEnemyHealthBar.value != health)
        {
            if (Mathf.Abs(delayedEnemyHealthBar.value - health) <= 5)
            {
                delayedEnemyHealthBar.value = health;
            }
            else if (health < delayedEnemyHealthBar.value)
            {
                delayedEnemyHealthBar.value -= reduceVal * Time.deltaTime;
            }
            else
            {
                delayedEnemyHealthBar.value += reduceVal * Time.deltaTime;
            }

            yield return null;
        }
        yield break;
    }

    IEnumerator MedJumpAttack(float moveSpeed, float duration)
    {
        if (isAttacking || !canMedJump || isRecovering)
            yield break;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            yield break;
        // Check if half health event is in progress
        if (isHalfHealthInProgress || forceHalfHealthEvent)
        {
            Debug.Log("Medium Jump attack prevented by half health event!");
            canMedJump = true;
            yield break;
        }
            
        // We'll handle the attack timing directly in this method instead of using medJumpSlice
        canMedJump = false;
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("golem: Starting Medium Jump Attack");

        animator.SetFloat("Forward", 0);
        animator.SetBool("medJumpAttack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("medJumpAttack"); // Make sure this animation exists
        animator.SetBool("medJumpAttack", false);

        // Medium jump movement (between regular jump and long jump)
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        
        // Get direction and distance to player
        Vector3 directionToPlayer = Vector3.zero;
        float distanceToPlayer = 0f;
        Vector3 targetPosition;
        
        if (player != null)
        {
            // Calculate direction to player
            directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            directionToPlayer.Normalize();
            
            // Calculate distance to player
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            // Set target position to be closer to player
            // If player is far, jump directly toward them
            if (distanceToPlayer > medJumpRadius)
            {
                Debug.Log("MedJumpAttack: Player is far (" + distanceToPlayer + "), jumping directly toward them");
                targetPosition = player.position - directionToPlayer * (medJumpRadius * 0.5f); // Land closer to player
                targetPosition.y = startPosition.y; // Maintain same height
            }
            else
            {
                // If player is already in range, just jump forward
                Debug.Log("MedJumpAttack: Player already in range, jumping forward");
                targetPosition = startPosition + directionToPlayer * (moveSpeed * duration);
            }
        }
        else
        {
            // Fallback if player reference is null
            Debug.LogWarning("MedJumpAttack: Player reference is null, jumping forward");
            targetPosition = startPosition + transform.forward * (moveSpeed * duration);
            directionToPlayer = transform.forward;
        }
        
        // Track the player for the first half of the jump
        float trackingDuration = duration * 0.5f; // Track for half the duration
        
        // Add vertical movement for jump
        float jumpHeight = 3f; // Increased jump height
        
        // Define when to trigger the attack effect (percentage of the jump duration)
        float attackTriggerTime = 0.7f; // Trigger earlier in the jump
        bool attackTriggered = false;
        
        Debug.Log("MedJumpAttack: Jump from " + startPosition + " to " + targetPosition + ", distance: " + Vector3.Distance(startPosition, targetPosition));
        
        while (elapsedTime < duration)
        {
            // Check if half health event is triggered during the jump
            if (isHalfHealthInProgress || forceHalfHealthEvent)
            {
                // Abort the jump and let the half health event take over
                Debug.Log("Medium Jump attack interrupted by half health event!");
                isAttacking = false;
                canMedJump = true;
                yield break;
            }
            
            float normalizedTime = elapsedTime / duration;
            
            // Update direction to track player during first half of jump
            if (elapsedTime < trackingDuration && player != null)
            {
                // Get current direction to player
                Vector3 currentDirectionToPlayer = (player.position - transform.position).normalized;
                // Keep y component the same to avoid tilting up/down
                currentDirectionToPlayer.y = 0;
                currentDirectionToPlayer.Normalize();
                
                // Smoothly rotate towards player
                transform.forward = Vector3.Lerp(transform.forward, currentDirectionToPlayer, Time.deltaTime * turnSpeed);
                
                // Update target position based on current player position
                if (normalizedTime < 0.3f) // Only update target in early part of jump
                {
                    Vector3 newTarget = player.position - currentDirectionToPlayer * (medJumpRadius * 0.3f);
                    newTarget.y = startPosition.y;
                    targetPosition = Vector3.Lerp(targetPosition, newTarget, Time.deltaTime * 2f);
                }
            }
            
            // Calculate vertical offset for jump arc
            float verticalOffset = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;
            
            // Interpolate position with jump arc
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
            newPosition.y = startPosition.y + verticalOffset;
            
            transform.position = newPosition;
            
            // Check if we've reached the attack trigger point and haven't triggered yet
            if (!attackTriggered && normalizedTime >= attackTriggerTime)
            {
                attackTriggered = true;
                
                // Play the effect at the specific time
                if (medJumpEffect != null)
                    medJumpEffect.GetComponent<ParticleSystem>().Play();
                
                // Enhanced debugging for player detection
                Debug.Log("MedJumpAttack: Attack triggered at position " + transform.position);
                Debug.Log("MedJumpAttack: Using radius " + medJumpRadius + " and player layer " + LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(playerLayer.value, 2))));
                
                // Draw a debug sphere to visualize the attack area
                Debug.DrawRay(transform.position, Vector3.up * 5f, Color.red, 3f);
                
                // Try a direct approach - if we have a player reference, apply damage
                if (player != null)
                {
                    // Calculate current distance to player (reusing the variable from earlier)
                    distanceToPlayer = Vector3.Distance(transform.position, player.position);
                    Debug.Log("MedJumpAttack: Current distance to player: " + distanceToPlayer + ", Attack radius: " + medJumpRadius);
                    
                    // Always apply damage regardless of distance at this point
                    // This ensures the attack always hits when triggered
                    Debug.Log("MedJumpAttack: Applying " + medJumpDamage + " damage directly to player");
                    player.GetComponent<CharacterBase>().takeDamage(medJumpDamage, gameObject.transform.forward);
                    UIManager.instance.DisplayDamageNum(player.transform, medJumpDamage);
                }
                else
                {
                    Debug.LogWarning("MedJumpAttack: Player reference is null!");
                    
                    // Fallback to OverlapSphere if player reference is null
                    Collider[] hitPlayers = Physics.OverlapSphere(
                        transform.position, 
                        medJumpRadius * 2f, // Use a larger radius for the fallback
                        playerLayer);
                    
                    Debug.Log("MedJumpAttack: Fallback check found " + hitPlayers.Length + " potential targets");
                    
                    // Log all colliders found for debugging
                    foreach (Collider col in hitPlayers)
                    {
                        Debug.Log("MedJumpAttack: Found collider: " + col.name + " with tag: " + col.tag);
                        
                        if (col.CompareTag("Player"))
                        {
                            Debug.Log("MedJumpAttack: Hit player in fallback! Applying " + medJumpDamage + " damage to " + col.name);
                            col.GetComponent<CharacterBase>().takeDamage(medJumpDamage, gameObject.transform.forward);
                            UIManager.instance.DisplayDamageNum(col.transform, medJumpDamage);
                        }
                    }
                }
                
                Debug.Log("Medium Jump Attack effect triggered at " + normalizedTime + " of jump duration");
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we land at the correct height
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);

        Debug.Log("golem: Medium Jump Attack Complete, Entering Recovery");
        isRecovering = true;
        isAttacking = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Recovery Complete, Ready to Attack Again");
        isRecovering = false;
        agent.isStopped = false;
        yield return new WaitForSeconds(medJumpCooldown);

        canMedJump = true;
        yield break;
    }




    //-----------------Draw Gizmos------------------------------


    // Draw Gizmo Sphere to visualize detection range
    void OnDrawGizmos()
    {
        if (weapon == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Draw weapon capsule using two spheres
        Gizmos.color = Color.yellow;
        Vector3 headPos = weapon.GetComponent<golemBossWeapon>().headPosition.transform.position + weaponOffset;
        Vector3 basePos = weapon.transform.position + weaponBaseOffset;
        Gizmos.DrawWireSphere(headPos, weaponRad);
        Gizmos.DrawWireSphere(basePos, weaponRad);
        Gizmos.DrawLine(headPos, basePos);
        
        // Draw slam offset sphere in yellow
        Gizmos.DrawWireSphere(gameObject.transform.position + transform.rotation * (slamOffset + Vector3.up + Vector3.forward), slamRadius);
        
        // Draw medium jump offset sphere in yellow
        Gizmos.DrawWireSphere(gameObject.transform.position + transform.rotation * (medJumpOffset + Vector3.up + Vector3.forward), slamRadius);
        
        Gizmos.color = Color.green;
        for (int i = 0; i < attackList.Count; i++)
        {
            Vector3 vec = attackList[i];
            Vector3 attackPos = gameObject.transform.position + transform.rotation * (vec + Vector3.up + Vector3.forward);
            
            switch (i)
            {
                case 0:
                    Gizmos.DrawWireSphere(attackPos, atkRng1);
                    break;
                case 1:
                    Gizmos.DrawWireSphere(attackPos, atkRng2);
                    break;
                case 2:
                    Gizmos.DrawWireSphere(attackPos, atkRng3);
                    break;
                case 3:
                    // Lunge attack visualization
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(attackPos, atkRng4);
                    // Also visualize the lunge radius
                    Gizmos.DrawWireSphere(attackPos, lungeRadius);
                    Gizmos.color = Color.green;
                    break;
                case 4:
                    // Back attack visualization
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(attackPos, atkRng5);
                    // Also visualize the back attack radius
                    Gizmos.DrawWireSphere(attackPos, backAttackRadius);
                    Gizmos.color = Color.green;
                    break;
                case 5:
                    // Dash attack visualization
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(attackPos, atkRng6);
                    // Also visualize the dash radius
                    Gizmos.DrawWireSphere(attackPos, dashRadius);
                    Gizmos.color = Color.green;
                    break;
                case 6:
                    // Medium jump attack visualization
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(attackPos, atkRng6);
                    // Also visualize the medium jump radius
                    Gizmos.DrawWireSphere(attackPos, medJumpRadius);
                    Gizmos.color = Color.green;
                    break;
                default:
                    Gizmos.DrawWireSphere(attackPos, atkRng4); // Fallback if index > 6
                    break;
            }
        }
    }



}

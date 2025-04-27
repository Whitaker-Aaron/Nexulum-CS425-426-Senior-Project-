using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class spellCastManager : MonoBehaviour
{

    bool casting = false;
    GameObject currentEffect = null;
    Rune currentRune = null;
    private PlayerInput playerInput;


    [SerializeField] private GameObject lightningSpell, lightingStrike, waterSpell, waterSplash;

    Transform player;

    bool isGamepadLooking = false;
    bool isMouseLooking = false;

    public LayerMask ground;

    Vector3 lookPos = Vector3.zero;
    Vector3 lookDir = Vector3.zero;

    public float minPlacementDistance = .5f;
    public float maxPlacementDistance = 2.5f;

    // Lightning rune values
    public float lightningRad = 3f;
    public int lightningDamage = 50;
    
    // Water rune values
    public float waterRad = 3f;
    public int waterDamage = 40;
    
    // Wind rune values
    public float windRad = 4f;
    public int windDamage = 35;
    public float windVortexDuration = 5f;  // How long the vortex lasts
    public float windDamageInterval = 0.5f; // How often the vortex deals damage
    public float windPullForce = 15f;      // Force with which enemies are pulled
    [SerializeField] private GameObject windSpell, windBlast;

    public Vector3 spawnOffset = new Vector3(0, .2f, 0);
    UIManager uiManager;

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        if (masterInput.instance.inputPaused || isGamepadLooking)
            return;

        isMouseLooking = true;
        isGamepadLooking = false;

        if (casting && currentEffect != null && currentRune != null)
        {
            
            // Read mouse position input
            Vector2 mousePosition = context.ReadValue<Vector2>();

            // Perform raycast from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // Raycast onto the ground
            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                lookPos = hit.point; // Set look position to where the raycast hit the ground
            }


            // Calculate the distance from player to mousePos
            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            // Position the turret based on distance from player
            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                currentEffect.transform.position = lookPos + spawnOffset;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                currentEffect.transform.position = player.transform.position + direction * (minPlacementDistance + 0.1f); // Small buffer to avoid overlap
                currentEffect.transform.position = new Vector3(
                    currentEffect.transform.position.x,
                    spawnOffset.y,
                    currentEffect.transform.position.z
                );
            }
            else
            {
                currentEffect.transform.position = player.transform.position + direction * maxPlacementDistance + spawnOffset;
            }
        }
        
        


    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {

        //Vector3 mousePos = Vector3.zero;
        //RaycastHit hit;

        // Determine input type (mouse vs. gamepad)
        if (playerInput.actions["MouseLook"].triggered)
        {
            isMouseLooking = true; 
            isGamepadLooking = false;
        }
        else if (Mathf.Abs(playerInput.actions["GamePadLook"].ReadValue<Vector2>().magnitude) > 0.1f) // Check for joystick movement
        {
            isMouseLooking = false; // Set flag for gamepad input
            isGamepadLooking = true;
        }

        if (masterInput.instance.inputPaused || isMouseLooking)
            return;
        else if (casting && currentEffect != null && currentRune != null)
        {
            // Get the right stick input from the gamepad
            Vector2 rightStickInput = context.ReadValue<Vector2>();

            if (rightStickInput != Vector2.zero)
            {
                // Use camera direction to calculate look direction
                Vector3 playerForward = Camera.main.transform.forward;
                Vector3 playerRight = Camera.main.transform.right;

                // Ignore Y axis to keep player level
                playerForward.y = 0;
                playerRight.y = 0;

                // Normalize camera directions
                playerForward.Normalize();
                playerRight.Normalize();

                // Calculate look direction based on right stick input
                Vector3 lookDirection = (playerRight * rightStickInput.x) + (playerForward * rightStickInput.y);

                // Adjust the look position based on the right stick direction
                lookPos = player.transform.position + lookDirection * 5f; // Adjust distance as needed
            }

            // Calculate the direction to look at
            //lookDir = lookPos - player.transform.position;
            //lookDir.y = 0;

            // Calculate the distance from player to mousePos
            float distanceFromPlayer = Vector3.Distance(player.transform.position, lookPos);
            Vector3 direction = (lookPos - player.transform.position).normalized;

            // Position the turret based on distance from player
            if (distanceFromPlayer <= maxPlacementDistance && distanceFromPlayer > minPlacementDistance)
            {
                currentEffect.transform.position = lookPos + spawnOffset;
            }
            else if (distanceFromPlayer <= minPlacementDistance)
            {
                currentEffect.transform.position = player.transform.position + direction * (minPlacementDistance + 0.1f); // Small buffer to avoid overlap
                currentEffect.transform.position = new Vector3(
                    currentEffect.transform.position.x,
                    spawnOffset.y,
                    currentEffect.transform.position.z
                );
            }
            else
            {
                currentEffect.transform.position = player.transform.position + direction * maxPlacementDistance + spawnOffset;
            }
        }
        else
            return;
        //isGamepadLooking = true;

        

        //if (lookDir.magnitude > minLookDistance && !masterInput.instance.inputPaused)
        //{
        //player.transform.LookAt(player.transform.position + lookDir, Vector3.up); // Rotate towards the look direction
        //player.transform.rotation = Quaternion.Euler(lookDir.)
        //}
        //isGamepadLooking = false;

    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInput = GetComponent<PlayerInput>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

    }



    public void activateSpellCast(Rune rune, int abilityIndex)
    {
        print("spellcast: activating spellCast in SCManager");
        print("spellCast: Rune Name is: " + rune.runeName);
        print("spellCast: canCast is " + rune.canCast);
        
        // Check if the rune can be cast and we're not already casting
        if (!rune.canCast || casting) return;

        switch (rune.runeName)
        {
            case "Lightning":
                currentRune = rune;
                activateLightning();
                break;

            case "Water":
                currentRune = rune;
                activateWaterSplash();
                break;
                
            case "Wind":
                currentRune = rune;
                activateWind();
                break;
        }
        rune.canCast = false;
        StartCoroutine(abilityCooldown(rune.cooldownTime, abilityIndex, rune));



    }

    IEnumerator abilityCooldown(float time, int abilityIndex, Rune rune)
    {
        yield return StartCoroutine(uiManager.StartCooldownSlider(abilityIndex, (0.98f / time), true));
        yield return new WaitForSeconds(0.2f);
        rune.canCast = true;
        uiManager.DeactivateCooldownOnAbility(abilityIndex, true);
    }

    public void deactivateSpellCast()
    {
        gameObject.GetComponent<masterInput>().placing = false;
        gameObject.GetComponent<masterInput>().abilityInUse = false;
        casting = false;
        currentRune = null;
        currentEffect = null;
    }


    void activateLightning()
    {
        print("activating lightningSpellCast");
        gameObject.GetComponent<masterInput>().placing = true;
        casting = true;
        currentEffect = Instantiate(lightningSpell, player.position + spawnOffset, Quaternion.identity);
        currentEffect.transform.parent = player.transform;
        currentEffect.transform.position = player.position;
        
    }

    void activateWaterSplash()
    {
        print("Activating water splash");
        gameObject.GetComponent <masterInput>().placing = true;
        casting = true;
        // Create rotation with -90 degrees on X axis to fix the orientation
        Quaternion waterRotation = Quaternion.Euler(-90f, 0f, 0f);
        currentEffect = Instantiate(waterSpell, player.position + spawnOffset, waterRotation);
        currentEffect.transform.parent = player.transform;
        currentEffect.transform.position = player.position;
    }
    
    void activateWind()
    {
        print("Activating wind blast");
        gameObject.GetComponent<masterInput>().placing = true;
        casting = true;
        Quaternion windRotation = Quaternion.Euler(-90f, 0f, 0f);
        currentEffect = Instantiate(windSpell, player.position + spawnOffset, windRotation);
        currentEffect.transform.parent = player.transform;
        currentEffect.transform.position = player.position;
    }

    // Store the last damage sphere position and radius for Gizmos visualization
    private List<DamageSphereInfo> damageSphereInfos = new List<DamageSphereInfo>();
    
    // Class to store damage sphere information for Gizmos
    private class DamageSphereInfo
    {
        public Vector3 position;
        public float radius;
        public Color color;
        public float displayTime;

        public DamageSphereInfo(Vector3 pos, float rad, Color col)
        {
            position = pos;
            radius = rad;
            color = col;
            displayTime = 2.0f; // Display for 2 seconds
        }
    }

    void damageSphere(Vector3 pos, float rad, int dmg, EnemyFrame.DamageType type)
    {
        // Add sphere info for Gizmos visualization
        Color sphereColor = Color.white;
        switch (type)
        {
            case EnemyFrame.DamageType.Electric:
                sphereColor = Color.blue; // Blue for lightning
                break;
            case EnemyFrame.DamageType.Water:
                sphereColor = Color.cyan; // Cyan for water
                break;
            case EnemyFrame.DamageType.Wind:
                sphereColor = Color.green; // Green for wind
                break;
        }
        
        damageSphereInfos.Add(new DamageSphereInfo(pos, rad, sphereColor));

        // Apply damage to enemies and bosses
        Collider[] enemies = Physics.OverlapSphere(pos, rad);
        foreach(Collider c in enemies)
        {
            if(c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<EnemyFrame>().takeDamage(dmg, Vector3.zero, EnemyFrame.DamageSource.AOE, type);
            }
            else if(c.gameObject.tag == "Boss")
            {
                c.gameObject.GetComponent<golemBoss>().takeDamage(dmg);
            }
            else if(c.gameObject.tag == "bossPart")
            {
                c.gameObject.GetComponent<bossPart>().takeDamage(dmg);
            }
        }
    }
    
    // Draw the damage spheres in the scene view
    private void OnDrawGizmos()
    {
        // Draw current effect radius if casting
        if (casting && currentEffect != null && currentRune != null)
        {
            Gizmos.color = Color.yellow;
            switch (currentRune.runeName)
            {
                case "Lightning":
                    Gizmos.DrawWireSphere(currentEffect.transform.position + Vector3.up, lightningRad);
                    break;
                case "Water":
                    Gizmos.DrawWireSphere(currentEffect.transform.position + Vector3.up, waterRad);
                    break;
                case "Wind":
                    // For wind, draw a more complex gizmo to indicate the vortex effect
                    Gizmos.DrawWireSphere(currentEffect.transform.position, windRad);
                    
                    // Draw arrows pointing inward to indicate the pulling effect
                    Vector3 center = currentEffect.transform.position;
                    int arrowCount = 8;
                    float arrowLength = windRad * 0.4f;
                    
                    for (int i = 0; i < arrowCount; i++)
                    {
                        float angle = i * (360f / arrowCount) * Mathf.Deg2Rad;
                        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                        Vector3 start = center + direction * windRad;
                        Vector3 end = start - direction * arrowLength;
                        
                        Gizmos.DrawLine(start, end);
                        
                        // Draw arrowheads
                        Vector3 right = Quaternion.Euler(0, 30, 0) * direction;
                        Vector3 left = Quaternion.Euler(0, -30, 0) * direction;
                        Gizmos.DrawLine(end, end + right * (arrowLength * 0.2f));
                        Gizmos.DrawLine(end, end + left * (arrowLength * 0.2f));
                    }
                    break;
            }
        }
        
        // Draw recent damage spheres
        foreach (DamageSphereInfo info in damageSphereInfos)
        {
            Gizmos.color = info.color;
            Gizmos.DrawWireSphere(info.position, info.radius);
            
            // If it's a wind effect (green color), draw the inward arrows
            if (info.color == Color.green)
            {
                Vector3 center = info.position;
                int arrowCount = 8;
                float arrowLength = info.radius * 0.4f;
                
                for (int i = 0; i < arrowCount; i++)
                {
                    float angle = i * (360f / arrowCount) * Mathf.Deg2Rad;
                    Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                    Vector3 start = center + direction * info.radius;
                    Vector3 end = start - direction * arrowLength;
                    
                    Gizmos.DrawLine(start, end);
                    
                    // Draw arrowheads
                    Vector3 right = Quaternion.Euler(0, 30, 0) * direction;
                    Vector3 left = Quaternion.Euler(0, -30, 0) * direction;
                    Gizmos.DrawLine(end, end + right * (arrowLength * 0.2f));
                    Gizmos.DrawLine(end, end + left * (arrowLength * 0.2f));
                }
            }
        }
    }
    
    // Update damage sphere info lifetimes in Update
    private void Update()
    {
        // Update existing functionality
        if (casting && currentEffect != null && currentRune != null)
        {
            
            // Check for attack input to place the spell
            if (playerInput.actions["attack"].triggered)
            {
                print("spellCast: attack triggered");
                // Immediately set casting to false to prevent multiple casts
                casting = false;
                
                GameObject tempEffect = null;
                switch (currentRune.runeName)
                {
                    case "Lightning":
                        print("spellCast: lightning triggered");
                        tempEffect = Instantiate(lightingStrike, currentEffect.transform.position, Quaternion.identity);
                        damageSphere(tempEffect.transform.position + Vector3.up, lightningRad, lightningDamage, EnemyFrame.DamageType.Electric);
                        tempEffect.GetComponent<ParticleSystem>().Play();
                        currentEffect.gameObject.GetComponent<ParticleSystem>().Stop();
                        Destroy(tempEffect, 5f);
                        Destroy(currentEffect.gameObject);
                        deactivateSpellCast();
                        break;

                    case "Water":
                        // Create rotation with -90 degrees on X axis to fix the orientation
                        //Quaternion waterRotation = Quaternion.Euler(-90f, 0f, 0f);
                        tempEffect = Instantiate(waterSplash, currentEffect.transform.position, Quaternion.identity);
                        damageSphere(tempEffect.transform.position + Vector3.up, waterRad, waterDamage, EnemyFrame.DamageType.Water);
                        tempEffect.GetComponent<ParticleSystem>().Play();
                        currentEffect.gameObject.GetComponent<ParticleSystem>().Stop();
                        Destroy(tempEffect, 5f);
                        Destroy(currentEffect.gameObject);
                        deactivateSpellCast();
                        break;
                        
                    case "Wind":
                        tempEffect = Instantiate(windBlast, currentEffect.transform.position, Quaternion.identity);
                        tempEffect.GetComponent<ParticleSystem>().Play();
                        currentEffect.gameObject.GetComponent<ParticleSystem>().Stop();
                        
                        // Start the wind vortex effect
                        StartCoroutine(WindVortexEffect(tempEffect, windVortexDuration));
                        
                        // We don't destroy the effect immediately as it needs to persist
                        Destroy(currentEffect.gameObject);
                        deactivateSpellCast();
                        break;
                }
            }
        }
        
        // Update damage sphere info lifetimes
        for (int i = damageSphereInfos.Count - 1; i >= 0; i--)
        {
            DamageSphereInfo info = damageSphereInfos[i];
            info.displayTime -= Time.deltaTime;
            
            if (info.displayTime <= 0)
            {
                damageSphereInfos.RemoveAt(i);
            }
        }
    }
    
    // Coroutine to handle the wind vortex effect
    private IEnumerator WindVortexEffect(GameObject vortexEffect, float duration)
    {
        Vector3 vortexPosition = vortexEffect.transform.position;
        float elapsedTime = 0f;
        float nextDamageTime = 0f;
        
        // Add a visual indicator for the vortex area that lasts for the duration
        DamageSphereInfo vortexVisual = new DamageSphereInfo(vortexPosition, windRad, Color.green);
        vortexVisual.displayTime = duration + 1f; // Add a little extra time for visual clarity
        damageSphereInfos.Add(vortexVisual);
        
        // Continue the vortex effect for the specified duration
        while (elapsedTime < duration)
        {
            // Find all enemies within the wind radius
            Collider[] enemies = Physics.OverlapSphere(vortexPosition, windRad);
            
            foreach (Collider enemy in enemies)
            {
                if (enemy.CompareTag("Enemy") && enemy.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                    
                    // Calculate direction towards the vortex center
                    Vector3 directionToVortex = vortexPosition - enemy.transform.position;
                    float distanceToVortex = directionToVortex.magnitude;
                    
                    // Normalize the direction and apply force
                    directionToVortex.Normalize();
                    
                    // Apply stronger pull force when closer to the center
                    float pullMultiplier = 1f - (distanceToVortex / windRad);
                    pullMultiplier = Mathf.Clamp(pullMultiplier, 0.2f, 1f);
                    
                    // Apply the pull force
                    enemyRb.AddForce(directionToVortex * windPullForce * pullMultiplier, ForceMode.Force);
                }
            }
            
            // Deal damage at intervals
            if (Time.time >= nextDamageTime)
            {
                foreach (Collider enemy in enemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        // Apply smaller damage over time
                        EnemyFrame enemyFrame = enemy.GetComponent<EnemyFrame>();
                        if (enemyFrame != null)
                        {
                            int damageAmount = Mathf.RoundToInt(windDamage * 0.2f); // 20% of the base damage per tick
                            enemyFrame.takeDamage(damageAmount, Vector3.zero, EnemyFrame.DamageSource.AOE, EnemyFrame.DamageType.Wind);
                            uiManager.DisplayDamageNum(enemy.transform, damageAmount);
                        }
                    }
                }
                
                // Set the next damage time
                nextDamageTime = Time.time + windDamageInterval;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Destroy the vortex effect after duration
        Destroy(vortexEffect, 1f); // Give it a second to finish any particle effects
    }
}
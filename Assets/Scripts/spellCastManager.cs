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


    [SerializeField] private GameObject lightningSpell, lightingStrike, waterShield;

    Transform player;

    bool isGamepadLooking = false;
    bool isMouseLooking = false;

    public LayerMask ground;

    Vector3 lookPos = Vector3.zero;
    Vector3 lookDir = Vector3.zero;

    public float minPlacementDistance = .5f;
    public float maxPlacementDistance = 2.5f;

    public float lightningRad = 3f;
    public int lightningDamage = 50;

    public Vector3 spawnOffset = new Vector3(0, .2f, 0);

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
    }

    // Update is called once per frame
    void Update()
    {
        if (casting && currentEffect != null && currentRune != null)
        {
            if (playerInput.actions["attack"].triggered)
            {
                GameObject tempEffect = null;
                switch (currentRune.runeName)
                {
                    case "LightningCast":
                        tempEffect = Instantiate(lightingStrike, currentEffect.transform.position, Quaternion.identity);
                        damageSphere(tempEffect.transform.position, lightningRad, lightningDamage, EnemyFrame.DamageType.Electric);
                        tempEffect.GetComponent<ParticleSystem>().Play();
                        currentEffect.gameObject.GetComponent<ParticleSystem>().Stop();
                        Destroy(tempEffect, 5f);
                        Destroy(currentEffect.gameObject);
                        deactivateSpellCast();
                        break;

                    case "WaterCast":
                        tempEffect = Instantiate(waterShield, currentEffect.transform.position, Quaternion.identity);
                        //tempEffect.gameObject.GetComponent<>
                        break;
                }
            }
        }
    }

    public void activateSpellCast(Rune rune)
    {
        print("activating spellCast in SCManager");
        print("Rune Name is: " + rune.runeName);
        switch (rune.runeName)
        {
            case "LightningCast":
                currentRune = rune;
                activateLightning();
                break;

            case "WaterCast":
                currentRune = rune;
                activateWaterShield();
                break;
        }



    }

    public void deactivateSpellCast()
    {
        gameObject.GetComponent<masterInput>().placing = false;
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

    void activateWaterShield()
    {
        print("Activating water shield");
        gameObject.GetComponent <masterInput>().placing = true;
        casting = true;
        currentEffect = Instantiate(waterShield, player.position + spawnOffset, Quaternion.identity);
        currentEffect.transform.parent = player.transform;
        currentEffect.transform.position = player.position;
    }

    void damageSphere(Vector3 pos, float rad, int dmg, EnemyFrame.DamageType type)
    {

        Collider[] enemies = Physics.OverlapSphere(pos, rad);
        foreach(Collider c in enemies)
        {
            if(c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<EnemyFrame>().takeDamage(dmg, Vector3.zero, EnemyFrame.DamageSource.AOE, type);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SlenderManAI : MonoBehaviour
{
    public NavMeshAgent ai; // The NavMeshAgent component for AI movement
    public Transform[] teleportDestinations; // List of teleport locations
    public Transform player; // Reference to the player's position
    public float teleportCooldown = 5f; // Time between teleportations
    public float detectionRange = 10f; // Distance for Slenderman to detect the player
    public bool hasSeenPlayer = false; // Flag to check if Slenderman has seen the player
    public float healthDecreaseRate = 1f; // Rate at which health decreases when Slenderman sees the player
    private float teleportTimer = 0f; // Timer to handle teleport cooldown
    private bool isTeleporting = false; // Flag to prevent teleportation spamming
    private bool isBeingLitByFlashlight = false; // Flag to check if Slenderman is being lit by the flashlight
    private float flashlightTimer = 0f; // Timer for tracking flashlight duration on Slenderman
    private Coroutine flickerCoroutine; // To manage the flicker effect
    public GameObject[] lightObjects; // Array to store all light GameObjects
    private Light[] lights; // Array to store Light components
    private bool[] lightsOn; // Array to track whether each light is on or off
    public float healthRegenRate = 2f; // Rate at which health regenerates after Slenderman is dealt with
    private bool isRegeneratingHealth = false; // Flag to check if the player should be regenerating health

    public GameObject jumpscareCam;
    public GameObject blackscreen;
    public AudioSource staticSound;
    public AudioSource jumpscareSound;
    public bool enableCursorAfterDeath;
    public string scenename; // Name of the scene to load after the player dies

    public Renderer slendermanRenderer; // Assign this in the Inspector
    public Material normalMaterial; // Slenderman's default material
    public Material flickerMaterial; // The flicker effect material

    public PlayerHealth playerHealth; // Reference to PlayerHealth script

    void Start()
    {
        ai = GetComponent<NavMeshAgent>(); // Get NavMeshAgent
        playerHealth = player.GetComponent<PlayerHealth>(); // Get PlayerHealth component
        lights = new Light[lightObjects.Length];
        lightsOn = new bool[lightObjects.Length];
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i] = lightObjects[i].GetComponent<Light>();
            if (lights[i].enabled == true)
            {
                lightsOn[i] = true;
            }
            else
            {
                lightsOn[i] = false;
            }
        }
    }

    void Update()
    {
        if (hasSeenPlayer)
        {
            DrainHealth();
        }
        else
        {
            SearchForPlayer();
        }

        if (isBeingLitByFlashlight)
        {
            flashlightTimer += Time.deltaTime;

            // If flashlight is on Slenderman for 3 seconds, trigger teleportation, regen, and light
            if (flashlightTimer >= 3f)
            {
                if (teleportTimer <= 0 && !isTeleporting)
                {
                    TeleportToRandomLocation();
                    flashlightTimer = 0f;
                    isRegeneratingHealth = true;
                    TurnOnLights();
                }
            }
        }
        else
        {
            flashlightTimer = 0f;
        }

        if (isRegeneratingHealth && playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
        {
            playerHealth.TakeDamage(-healthRegenRate * Time.deltaTime); // Regenerate health (negative damage)
            if (playerHealth.currentHealth >= playerHealth.maxHealth)
            {
                playerHealth.ResetHealth(); // Ensure health doesn't go over max
                isRegeneratingHealth = false;
            }
        }

        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
        }
    }

    void SearchForPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
        {
            if (hit.transform == player)
            {
                hasSeenPlayer = true;
                ai.isStopped = true;
                TurnOffLights(); // Turn off lights when Slenderman detects the player
            }
        }
        else
        {
            if (!isTeleporting)
            {
                ai.SetDestination(teleportDestinations[Random.Range(0, teleportDestinations.Length)].position);
            }
        }
    }

    void TeleportToRandomLocation()
    {
        isTeleporting = true;
        transform.position = teleportDestinations[Random.Range(0, teleportDestinations.Length)].position;
        teleportTimer = teleportCooldown;
        ai.isStopped = false;
        isTeleporting = false;
    }

    void DrainHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(healthDecreaseRate * Time.deltaTime); // Drain player health
        }
    }

    public void StartBeingLitByFlashlight()
    {
        isBeingLitByFlashlight = true;
        if (flickerCoroutine == null)
        {
            flickerCoroutine = StartCoroutine(FlickerEffect());
        }
    }

    public void StopBeingLitByFlashlight()
    {
        isBeingLitByFlashlight = false;
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        slendermanRenderer.material = normalMaterial;
    }

    IEnumerator FlickerEffect()
    {
        while (isBeingLitByFlashlight)
        {
            slendermanRenderer.material = flickerMaterial;
            yield return new WaitForSeconds(0.1f);

            slendermanRenderer.material = normalMaterial;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TurnOffLights()
    {
        // Turn off all lights when Slenderman sees the player
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i].enabled = false; // Turn off light
            lightsOn[i] = false; // Track that this light is off
        }
    }

    void TurnOnLights()
    {
        // Turn on all lights when flashlight hits Slenderman for 3 seconds
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i].enabled = true; // Turn on light
            lightsOn[i] = true; // Track that this light is on
        }
    }
}



// public class SlenderAI : MonoBehaviour
// {
//     public NavMeshAgent ai; // The NavMeshAgent component for AI movement
//     public Transform[] teleportDestinations; // List of teleport locations
//     public Transform player; // Reference to the player's position
//     public float teleportCooldown = 5f; // Time between teleportations
//     public float detectionRange = 10f; // Distance for Slenderman to detect the player
//     public bool hasSeenPlayer = false; // Flag to check if Slenderman has seen the player
//     public float healthDecreaseRate = 1f; // Rate at which health decreases when Slenderman sees the player
//     private float teleportTimer = 0f; // Timer to handle teleport cooldown
//     private bool isTeleporting = false; // Flag to prevent teleportation spamming
//     private float health = 100f; // Player's health
//     private bool isBeingLitByFlashlight = false; // Flag to check if Slenderman is being lit by the flashlight
//     private float flashlightTimer = 0f; // Timer for tracking flashlight duration on Slenderman

//     public float healthRegenRate = 2f; // Rate at which health regenerates after Slenderman is dealt with
//     private bool isRegeneratingHealth = false; // Flag to check if the player should be regenerating health

//     public GameObject jumpscareCam;
//     public GameObject blackscreen;
//     float staticAmount;
//     float staticVolume;
//     public float staticIncreaseRate, staticDecreaseRate;
//     public AudioSource staticSound;
//     public bool enableCursorAfterDeath;
//     public AudioSource jumpscareSound;

//     // Name of the scene to load after the player dies
//     public string scenename;

//     void Start()
//     {
//         ai = GetComponent<NavMeshAgent>(); // Get NavMeshAgent
//     }

//     IEnumerator killPlayer()
//     {
//         yield return new WaitForSeconds(3.5f); // After 3.5 seconds
//         blackscreen.SetActive(true); // The black screen will be set active
//         AudioListener.pause = true; // Pause the game's Audio Listener
//         yield return new WaitForSeconds(6f); // After 6 seconds
//         if (enableCursorAfterDeath)
//         {
//             Cursor.visible = true; // Show the cursor
//             Cursor.lockState = CursorLockMode.None; // Unlock the cursor
//         }
//         SceneManager.LoadScene(scenename); // Load the scene
//     }

//     void Update()
//     {
//         if (hasSeenPlayer)
//         {
//             DrainHealth();
//         }
//         else
//         {
//             SearchForPlayer();
//         }

//         if (isBeingLitByFlashlight)
//         {
//             flashlightTimer += Time.deltaTime;
//             if (flashlightTimer >= 3f)
//             {
//                 if (teleportTimer <= 0 && !isTeleporting)
//                 {
//                     TeleportToRandomLocation();
//                     flashlightTimer = 0f;
//                     isRegeneratingHealth = true;
//                 }
//             }
//         }
//         else
//         {
//             flashlightTimer = 0f;
//         }

//         if (isRegeneratingHealth && health < 100f)
//         {
//             health += healthRegenRate * Time.deltaTime;
//             if (health >= 100f)
//             {
//                 health = 100f;
//                 isRegeneratingHealth = false;
//             }
//         }

//         if (teleportTimer > 0)
//         {
//             teleportTimer -= Time.deltaTime;
//         }

//         // Handle static sound and image during death
//         if (health <= 0)
//         {
//             staticVolume += staticIncreaseRate * Time.deltaTime;
//             staticAmount += staticIncreaseRate * Time.deltaTime;
//             if (staticVolume > 1) staticVolume = 1;
//             if (staticAmount > 0.9f) staticAmount = 0.9f;

//             staticSound.volume = staticVolume; // Apply volume to static sound
//             // Assuming you have an image component controlling the staticAmount
//             // e.g., staticImage.color = new Color(1, 1, 1, staticAmount);
//         }
//     }

//     void SearchForPlayer()
//     {
//         Vector3 direction = (player.position - transform.position).normalized;
//         RaycastHit hit;

//         if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
//         {
//             if (hit.transform == player)
//             {
//                 hasSeenPlayer = true;
//                 ai.isStopped = true;
//             }
//         }
//         else
//         {
//             if (!isTeleporting)
//             {
//                 ai.SetDestination(teleportDestinations[Random.Range(0, teleportDestinations.Length)].position);
//             }
//         }
//     }

//     void TeleportToRandomLocation()
//     {
//         isTeleporting = true;
//         transform.position = teleportDestinations[Random.Range(0, teleportDestinations.Length)].position;
//         teleportTimer = teleportCooldown;
//         ai.isStopped = false;
//         isTeleporting = false;
//     }

//     void DrainHealth()
//     {
//         health -= healthDecreaseRate * Time.deltaTime;
//         if (health <= 0)
//         {
//             StartCoroutine(killPlayer());
//             player.gameObject.SetActive(false);
//             jumpscareCam.SetActive(true);
//             ai.speed = 0;
//             ai.SetDestination(transform.position);
//         }
//     }

//     public void StartBeingLitByFlashlight()
//     {
//         isBeingLitByFlashlight = true;
//     }

//     public void StopBeingLitByFlashlight()
//     {
//         isBeingLitByFlashlight = false;
//     }
// }



// using System.Collections;
// using UnityEngine;

// public class SlenderManBehavior : MonoBehaviour
// {
//     public float healthDecreaseRate = 1f;
//     public float healthIncreaseRate = 0.5f;
//     public float health = 100f;
//     public Transform player;
//     public Light lightSource;
//     public float teleportCooldown = 5f; // Time before Slenderman can teleport again
//     public float teleportChance = 1f; // Probability of teleporting
//     public List<Transform> teleportDestinations;
//     private bool isInLight = false;
//     private bool isPlayerInLight = false;
//     private bool isTeleporting = false;
//     private float teleportTimer = 0f;
//     private float flashlightTimer = 0f;
//     private bool isDead = false;

//     private Renderer slenderRenderer;
//     private Material slenderMaterial;
//     private Collider playerCollider;

//     private void Start()
//     {
//         slenderRenderer = GetComponent<Renderer>();
//         slenderMaterial = slenderRenderer.material;
//         playerCollider = player.GetComponent<Collider>();
//     }

//     private void Update()
//     {
//         if (isDead)
//             return;

//         // Handle Slenderman teleportation if possible
//         HandleTeleportation();

//         // Check if Slenderman can see the player (raycast or frustum check)
//         bool canSeePlayer = CanSlendermanSeePlayer();

//         if (canSeePlayer)
//         {
//             HandlePlayerVisible();
//         }
//         else
//         {
//             HandlePlayerInvisible();
//         }

//         // Update the light effect
//         UpdateLightEffect();

//         // Update the player's health
//         UpdateHealth();
//     }

//     // Handle the teleportation of Slenderman
//     private void HandleTeleportation()
//     {
//         if (!isTeleporting)
//         {
//             teleportTimer += Time.deltaTime;
//             if (teleportTimer >= teleportCooldown)
//             {
//                 isTeleporting = true;
//                 teleportTimer = 0f;

//                 // Randomly teleport Slenderman to a new location
//                 TeleportSlenderMan();
//             }
//         }
//     }

//     // Teleports Slenderman to a random position from the teleport destinations
//     private void TeleportSlenderMan()
//     {
//         int randNum = Random.Range(0, teleportDestinations.Count);
//         transform.position = teleportDestinations[randNum].position;
//     }

//     // Check if Slenderman can see the player (simplified version with raycast)
//     private bool CanSlendermanSeePlayer()
//     {
//         RaycastHit hit;
//         Vector3 direction = player.position - transform.position;

//         if (Physics.Raycast(transform.position, direction, out hit))
//         {
//             if (hit.collider.CompareTag("Player"))
//             {
//                 return true;
//             }
//         }

//         return false;
//     }

//     // Handle the logic when Slenderman sees the player
//     private void HandlePlayerVisible()
//     {
//         // Lights go out if Slenderman sees the player
//         lightSource.enabled = false;

//         // Drain player's health
//         health -= healthDecreaseRate * Time.deltaTime;

//         // If the flashlight is on Slenderman for 3 seconds, the light turns back on
//         if (isInLight)
//         {
//             flashlightTimer += Time.deltaTime;

//             if (flashlightTimer >= 3f)
//             {
//                 // Turn lights back on
//                 lightSource.enabled = true;
//                 flashlightTimer = 0f;

//                 // Start recovering health
//                 health += healthIncreaseRate * Time.deltaTime;
//                 if (health > 100f) health = 100f;
//             }
//         }
//     }

//     // Handle the logic when Slenderman cannot see the player
//     private void HandlePlayerInvisible()
//     {
//         if (!isInLight) return;

//         // If Slenderman is not in light, allow teleporting
//         if (isTeleporting)
//         {
//             isTeleporting = false;
//             teleportTimer = 0f; // Reset teleport cooldown
//         }
//     }

//     // Update the light effect when Slenderman is either in or out of light
//     private void UpdateLightEffect()
//     {
//         if (isInLight)
//         {
//             // In light, Slenderman will stay in the same position and will not move
//             // You can add some additional effects here like transparency or glowing, etc.
//             slenderMaterial.SetColor("_Color", Color.white);
//         }
//         else
//         {
//             // Slenderman is in darkness, fade to black or change color to make him less visible
//             slenderMaterial.SetColor("_Color", Color.black);
//         }
//     }

//     // Update the player's health over time
//     private void UpdateHealth()
//     {
//         if (health <= 0f)
//         {
//             // Player is dead
//             killPlayer();
//         }
//     }

//     // Function to handle player death
//     private void killPlayer()
//     {
//         isDead = true;
//         // Call any game over or death logic
//         Debug.Log("Player is dead!");
//     }

//     // Trigger flashlight interaction
//     public void TriggerFlashlight(bool isShining)
//     {
//         isInLight = isShining;
//     }
// }

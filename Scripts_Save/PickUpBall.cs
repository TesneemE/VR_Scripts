using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // For Input System
using System.Collections;

public class PickUpBall : MonoBehaviour
{

    public Animator animator;
    public string animationStateName = "BallRoll";
    public AudioSource audioSource;
    public AudioClip clipDuringAnimation;
    public AudioClip loopingClipAfterAnimation;
    public AudioClip afterPickUp;

    private bool animationStarted = false;
    private bool animationEnded = false;

    private bool isDrainingHealth = false;
    public GameObject[] lightObjects; // Array to store all light GameObjects
    private Light[] lights; // Array to store Light components
    private bool[] lightsOn; // Array to track whether each light is on or off
    public GameObject enemy; // The enemy to activate
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // To detect ball pickup
    private Rigidbody ballRigidbody; // The Rigidbody to detect the throw
    public float healthDecreaseRate = 1f;
    public PlayerHealth playerHealth;
    // public Flashlight flashlight;
    // public GameObject flashlight_object;
    public GameObject player;
    public AudioSource slenderSource;
    public AudioClip slenderClip1;
    public AudioClip slenderClip2;
    public SlenderManAI slenderAI; // Reference to slender script
     public KeySound keySound;
    // public AudioClip pickupBallAudio; // The audio clip to play when near

    void Start()
    {
        // Initialize light and light tracking arrays
        lights = new Light[lightObjects.Length];
        lightsOn = new bool[lightObjects.Length];

        // Set all lights to off initially
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i] = lightObjects[i].GetComponent<Light>();
            lightsOn[i] = false; // Initialize all lights to off
        }

        if (enemy != null)
        {
            enemy.SetActive(false); // Enemy starts inactive
        }

        // Get XRGrabInteractable and Rigidbody components
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        ballRigidbody = GetComponent<Rigidbody>();

        if (grabInteractable != null)
        {
            // Correct method signature for the event listeners
            grabInteractable.selectEntered.AddListener(OnBallPickedUp);
            grabInteractable.selectExited.AddListener(OnBallThrown);
        }
        else
        {
            Debug.LogError("XRGrabInteractable component not found on ball.");
        }
    }

    void DrainHealth()
    {
        if (playerHealth != null && isDrainingHealth)
        {
            playerHealth.TakeDamage(healthDecreaseRate * Time.deltaTime); // Drain health continuously
        }
    }
    public void ActivateEnemy(SelectEnterEventArgs args) {

       
         if (enemy != null)
        {
            // Move enemy in front of the player
            // enemy.transform.position = args.interactorObject.transform.position + args.interactorObject.transform.forward * 2f;
                    Vector3 enemyPosition = enemy.transform.position;
        
        // Update only the x and z position (maintain the current y value)
        enemyPosition.x = args.interactorObject.transform.position.x + args.interactorObject.transform.forward.x * 2f; // Move in front on the X-axis
        enemyPosition.z = args.interactorObject.transform.position.z + args.interactorObject.transform.forward.z * 2f; // Move in front on the Z-axis
        
        // Set the enemy's position while keeping the original Y value
        enemy.transform.position = enemyPosition;
            enemy.SetActive(true); // Activate the enemy
            StartCoroutine(PlaySlenderAudioSequence());
            isDrainingHealth = true;
        }

        // makes sure lights are turned off when the ball is picked up
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (lightsOn[i])
            {
                lights[i].enabled = false; // Turn off light
                lightsOn[i] = false; // track that this light is off
            }
        }
    }
    // Corrected method signature to match the event expected delegate
    private void OnBallPickedUp(SelectEnterEventArgs args)
    {
         audioSource.Stop();
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (lightsOn[i])
            {
                lights[i].enabled = true; // Turn on light
                lightsOn[i] = true; // track that this light is on
            }
        }
         Invoke("ActivateEnemy", 2f);
    }
    public void ActivateScript(){
        if(slenderAI!=null)
        {
            slenderAI.enabled=true;
        }
    }

    // Corrected method signature to match the event expected delegate
    private void OnBallThrown(SelectExitEventArgs args)
    {
        if (ballRigidbody.linearVelocity.magnitude > 0.1f) // Check if ball is thrown
        {
            audioSource.Stop();
            isDrainingHealth = false;
            playerHealth.ResetHealth(); // Reset player health when ball is thrown
            StartCoroutine(DeactivateBallAndEnemy(1f)); // Deactivate ball and enemy after delay
        }
        // if (flashlight_object != null)
        // {
        //     flashlight_object.SetActive(true);
        // }
        //      if (flashlight != null)
        // {
        //     flashlight.enabled = true;
        // }
    }

//         // Turn on all lights after the enemy is deactivated
//         for (int i = 0; i < lightObjects.Length; i++)
//         {
//             if (!lightsOn[i])
//             {
//                 lights[i].enabled = true; // Turn on light
//                 lightsOn[i] = true; // Track that this light is on
//             }
//         }
//     }
//     }
    private IEnumerator DeactivateBallAndEnemy(float delay)
    {

        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false); // Deactivate the ball
        if (enemy != null)
        {
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0); //rotate enemy 180 deg
            enemy.SetActive(false); // Deactivate the enemy
        }
        slenderSource.Stop();
        // Turn on all lights after the enemy is deactivated
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (!lightsOn[i])
            {
                lights[i].enabled = true; // Turn on light
                lightsOn[i] = true; // Track that this light is on
            }
        }
                if(keySound!=null)
        {
            keySound.enabled=true;
        }
        Invoke("ActivateScript", 5f);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnBallPickedUp);
            grabInteractable.selectExited.RemoveListener(OnBallThrown);
        }
    }
    private IEnumerator PlaySlenderAudioSequence()
{
    if (slenderClip1 != null)
    {
        slenderSource.clip = slenderClip1;
        slenderSource.loop = false;
        slenderSource.Play();

        yield return new WaitForSeconds(slenderClip1.length);
    }

    if (slenderClip2 != null)
    {
         audioSource.clip=afterPickUp;
         audioSource.loop=true;
         audioSource.Play();

        slenderSource.clip = slenderClip2;
        slenderSource.loop = true;
        slenderSource.Play();
    }
}
        public void ActivateObject() {

  

         if (enemy != null)
        {
            // Move enemy in front of the player
            // enemy.transform.position = args.interactorObject.transform.position + args.interactorObject.transform.forward * 2f;
                    Vector3 enemyPosition = enemy.transform.position;
        
        // Update only the x and z position (maintain the current y value)
        enemyPosition.x =player.transform.position.x + player.transform.forward.x * 2f; // Move in front on the X-axis
        enemyPosition.z = player.transform.position.z + player.transform.forward.z * 2f; // Move in front on the Z-axis
        
        // Set the enemy's position while keeping the original Y value
        enemy.transform.position = enemyPosition;
            enemy.SetActive(true); // Activate the enemy
            StartCoroutine(PlaySlenderAudioSequence());
            isDrainingHealth = true;
        }

        // makes sure lights are turned off when the ball is picked up
        for (int i = 1; i < lightObjects.Length; i++)
        {
            if (lightsOn[i])
            {
                lights[i].enabled = false; // Turn off light
                lightsOn[i] = false; // track that this light is off
            }
        }
    }
    public void PlaySecondAudio(){
             animationEnded = true;
             if(!enemy.activeInHierarchy){
            audioSource.clip = loopingClipAfterAnimation;
            audioSource.loop = true;
            audioSource.Play();
             }
    }
    void Update(){
              AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Start playing Clip A when animation starts
        if (!animationStarted && stateInfo.IsName(animationStateName))
        {
            animationStarted = true;
            audioSource.clip = clipDuringAnimation;
            audioSource.loop = false;
            audioSource.Play();
        }

        // After animation ends, play Clip B on loop
        if (animationStarted && !animationEnded && stateInfo.normalizedTime >= 1f)
        {
              Invoke("PlaySecondAudio", 2.2f);
            // animationEnded = true;
            // audioSource.clip = loopingClipAfterAnimation;
            // audioSource.loop = true;
            // audioSource.Play();
        }
                // Check if the "A" key is pressed (simulating the ball being picked up)
        if (Input.GetKeyDown(KeyCode.A))
        {
            audioSource.Stop();
        // for (int i = 0; i < lightObjects.Length; i++)
        // {
        // //     if (lightsOn[i])
        // //     {
        //         lights[i].enabled = true; // Turn on light
        //         lightsOn[i] = true; // track that this light is on
        //     // }
        // }
            Invoke("ActivateObject", 2f);
            // Activate the enemy
        //     if (enemy != null)
        //     {
        // Vector3 enemyPosition = enemy.transform.position;
        
        // // Update only the x and z position (maintain the current y value)
        // enemyPosition.x = player.transform.position.x + player.transform.forward.x * 2f; // Move in front on the X-axis
        // enemyPosition.z = player.transform.position.z + player.transform.forward.z * 2f; // Move in front on the Z-axis
        
        // // Set the enemy's position while keeping the original Y value
        // enemy.transform.position = enemyPosition;
        //     enemy.SetActive(true); 
        //     }
        //      isDrainingHealth = true;
        }
                if (Input.GetKeyDown(KeyCode.B))
        {
            audioSource.Stop();
            isDrainingHealth = false;
            playerHealth.ResetHealth(); // Reset player health when ball is thrown
            StartCoroutine(DeactivateBallAndEnemy(1f)); // Deactivate ball and enemy after delay
        slenderSource.Stop();
        // if (flashlight_object != null)
        // {
        //     flashlight_object.SetActive(true);
        // }
        //      if (flashlight != null)
        // {
        //     flashlight.enabled = true;
        // }
        // }
    DrainHealth();
    }
}
}



//     // public void OnFlashlightPickedUp()
//     // {
//     //     hasPickedUpFlashlight = true;

//     //     // Enable SlenderAI when flashlight is picked up
//     //     SlenderAI slenderAI = FindObjectOfType<SlenderAI>();
//     //     if (slenderAI != null)
//     //     {
//     //         slenderAI.gameObject.SetActive(true);
//     //     }
//     // }
// using UnityEngine;

// public class PickUpBall : MonoBehaviour
// {
//     public GameObject[] lightObjects; // Array to store all light GameObjects
//     private Light[] lights; // Array to store Light components
//     private bool[] lightsOn; // Array to track whether each light is on or off
//     public GameObject enemy; // The enemy to activate
//     public GameObject ball;
//     public GameObject player;

//     void Start()
//     {
//         // Initialize arrays based on the length of lightObjects
//         lights = new Light[lightObjects.Length];
//         lightsOn = new bool[lightObjects.Length];

//         // Get Light components for each object in the lightObjects array
//         for (int i = 0; i < lightObjects.Length; i++)
//         {
//             lights[i] = lightObjects[i].GetComponent<Light>();
//             lightsOn[i] = false; // Initialize all lights to off
//         }

//         // Ensure enemy is inactive at start
//         if (enemy != null)
//         {
//             enemy.SetActive(false);
//         }
//     }

//     void Update()
//     {
//         // Check if the "A" key is pressed (simulating the ball being picked up)
//         if (Input.GetKeyDown(KeyCode.A))
//         {
//             // Turn on all lights
//             // for (int i = 0; i < lightObjects.Length; i++)
//             // {
//             //     if (!lightsOn[i])
//             //     {
//             //         lights[i].enabled = true; // Turn on light
//             //         lightsOn[i] = true; // Track that this light is on
//             //     }
//             // }

//             // Activate the enemy
//             if (enemy != null)
//             {
//             // enemy.transform.position = player.transform.position + player.transform.forward * 2f; // 2 units in front of the player
//             enemy.SetActive(true); 
//             }
//         }
//                 if (Input.GetKeyDown(KeyCode.B))
//         {
//         ball.SetActive(false); // Ball disappears after being thrown

//         // Deactivate the enemy
//         if (enemy != null)
//         {
//             enemy.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate the enemy 180 degrees on the Y-axis
//             enemy.SetActive(false); // Enemy disappears
//         }

//         // Turn on all lights after the enemy is deactivated
//         for (int i = 0; i < lightObjects.Length; i++)
//         {
//             if (!lightsOn[i])
//             {
//                 lights[i].enabled = true; // Turn on light
//                 lightsOn[i] = true; // Track that this light is on
//             }
//         }
//     }
//     }
// }

// // using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using System.Collections;

// public class PickUpBall : MonoBehaviour
// {
//     public GameObject[] lightObjects; // Array to store all light GameObjects
//     private Light[] lights; // Array to store Light components
//     private bool[] lightsOn; // Array to track whether each light is on or off
//     public GameObject enemy; // The enemy to activate
//     private XRGrabInteractable grabInteractable; // To detect ball pickup
//     private Rigidbody ballRigidbody; // The Rigidbody to detect the throw

//     void Start()
//     {
//         // Initialize arrays based on the length of lightObjects
//         lights = new Light[lightObjects.Length];
//         lightsOn = new bool[lightObjects.Length];

//         // Get Light components for each object in the lightObjects array
//         for (int i = 0; i < lightObjects.Length; i++)
//         {
//             lights[i] = lightObjects[i].GetComponent<Light>();
//             lightsOn[i] = false; // Initialize all lights to off
//         }

//         // Ensure enemy is inactive at start
//         if (enemy != null)
//         {
//             enemy.SetActive(false); // Enemy starts inactive
//         }

//         // Get the XRGrabInteractable component to detect pickup
//         grabInteractable = GetComponent<XRGrabInteractable>();
//         ballRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody component

//         if (grabInteractable != null)
//         {
//             // Subscribe to the event when the object is picked up
//             grabInteractable.onSelectEntered.AddListener(OnBallPickedUp);
//             grabInteractable.onSelectExited.AddListener(OnBallThrown); // Listen for when the ball is thrown
//         }
//         else
//         {
//             Debug.LogError("XRGrabInteractable component not found on ball.");
//         }
//     }

//     // This method will be triggered when the ball is picked up in VR
//     private void OnBallPickedUp(XRBaseInteractor interactor)
//     {
//         // Optionally, place the enemy in front of the player
//         if (enemy != null)
//         {
//             // Adjust the position of the enemy to appear in front of the player
//             enemy.transform.position = interactor.transform.position + interactor.transform.forward * 2f; // 2 units in front of the player
//             enemy.SetActive(true); // Activate the enemy
//         }
//     }

//     // This method will be triggered when the ball is thrown in VR
//     private void OnBallThrown(XRBaseInteractor interactor)
//     {
//         // Check if the ball is moving fast enough to be considered thrown
//         if (ballRigidbody.velocity.magnitude > 0.1f)
//         {
//             // Deactivate the ball and enemy after a delay
//             StartCoroutine(DeactivateBallAndEnemy(1f)); // Ball and enemy will disappear after 1 second
//         }
//     }

//     // Coroutine to delay deactivating the ball and enemy, then turn on lights
//     private IEnumerator DeactivateBallAndEnemy(float delay)
//     {
//         // Wait for the specified delay
//         yield return new WaitForSeconds(delay);

//         // Deactivate the ball
//         gameObject.SetActive(false); // Ball disappears after being thrown

//         // Deactivate the enemy
//         if (enemy != null)
//         {
//             enemy.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate the enemy 180 degrees on the Y-axis
//             enemy.SetActive(false); // Enemy disappears
//         }

//         // Turn on all lights after the enemy is deactivated
//         for (int i = 0; i < lightObjects.Length; i++)
//         {
//             if (!lightsOn[i])
//             {
//                 lights[i].enabled = true; // Turn on light
//                 lightsOn[i] = true; // Track that this light is on
//             }
//         }
//     }

//     void OnDestroy()
//     {
//         // Unsubscribe from the event when the object is destroyed to prevent memory leaks
//         if (grabInteractable != null)
//         {
//             grabInteractable.onSelectEntered.RemoveListener(OnBallPickedUp);
//             grabInteractable.onSelectExited.RemoveListener(OnBallThrown);
//         }
//     }
// }


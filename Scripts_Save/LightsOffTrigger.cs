// using UnityEngine;

// public class LightsOffTrigger : MonoBehaviour
// {
    // public GameObject lightObject1;
    // public GameObject lightObject2;
    // public GameObject lightObject3;
    // public GameObject lightObject4;
    // public GameObject lightObject5;
    // public GameObject lightObject6;
    // public GameObject lightObject7;
    // private Light light1; // Assign first light in the Inspector
    // private Light light2; // Assign second light in the Inspector
    // private Light light3;
    // private Light light4;
    // private Light light5;
    // private Light light6;
    // private Light light7;
    // public Transform ball; // Assign the Ball in the Inspector
    // private Animator ballAnimator; // To access the ball's animation state

    // private bool lightsOff1 = false;
    // private bool lightsOff2 = false;

//     void Start()
//     {
//         // Get the Light components from the GameObjects
//         light1 = lightObject1.GetComponent<Light>();
//         light2 = lightObject2.GetComponent<Light>();

//         // Get the Animator component of the ball
//         ballAnimator = ball.GetComponent<Animator>();
//     }

//     void Update()
//     {
        // Debug.Log(ball.position.x);
        // Debug.Log(lightObject1.transform.position.x);
//         // Check if the ball's animation is playing
//         if (ballAnimator != null)
//         {
//             // Get the current animation state from the Animator
//             AnimatorStateInfo stateInfo = ballAnimator.GetCurrentAnimatorStateInfo(0);

//             // Check if the animation is playing (replace "BallRoll" with your animation's name)
//             if (stateInfo.IsName("BallRoll"))
//             {
//                 // Check if the ball has passed the lights (assuming movement on the Z-axis)
//                 if (!lightsOff1 && ball.position.x > lightObject1.transform.position.x)
//                 {
//                     light1.enabled = false;
//                     lightsOff1 = true; // Prevents turning them off multiple times
//                 }

//                 if (!lightsOff2 && ball.position.x > lightObject2.transform.position.x)
//                 {
//                     light2.enabled = false;
//                     lightsOff2 = true; // Prevents turning them off multiple times
//                 }
//             }
//         }
//     }
// }


using UnityEngine;

public class LightsOffTrigger : MonoBehaviour
{
    public GameObject[] lightObjects; // Array for all light GameObjects
    private Light[] lights; // Array to store Light components
    private bool[] lightsOff; // Array to track the state of each light
    public GameObject ball; // Assign the Ball in the Inspector

    void Start()
    {
        // Initialize arrays based on the length of lightObjects
        lights = new Light[lightObjects.Length];
        lightsOff = new bool[lightObjects.Length];

        // Get the Light components for each object in the array
        for (int i = 0; i < lightObjects.Length; i++)
        {
            lights[i] = lightObjects[i].GetComponent<Light>();
            lightsOff[i] = false; // Set all lights to not be off initially
        }
    }

    void Update()
    {
        // Iterate through all lights and turn them off when the ball passes them
        for (int i = 0; i < lightObjects.Length; i++)
        {
            // Check if the ball's position has passed the light's position
            if (!lightsOff[i] && ball.transform.position.x > lightObjects[i].transform.position.x)
            {
                lights[i].enabled = false; // Turn off the light
                lightsOff[i] = true; // Prevent turning it off multiple times
            }
        }
    }
}


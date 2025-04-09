using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // Include this to use the new Input System

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // ref to flashlight
    public float flashlightDetectionRange = 5f; // range for the flashlight raycast
    public Transform slendermanTransform; // ref to slenderman's pos
    public SlenderManAI slendermanAI; // Reference to Slenderman ai script

    private InputAction flashlightToggleAction; // action for toggling the flashlight
    private bool isFlashlightOn = false;  // track if the flashlight is on or off

    void Start()
    {

        // intalize the flashlight toggle action
        flashlightToggleAction = new InputAction("FlashlightToggle", binding: "<XRController>{LeftHand}/primaryButton"); // Left controller primary button

        // add another binding for the right controller (optional)
        flashlightToggleAction.AddBinding("<XRController>{RightHand}/primaryButton");

        flashlightToggleAction.Enable(); // Enable the action
    }

    void Update()
    {
        // check if the flashlight toggle action was triggered
        if (flashlightToggleAction.triggered)
        {
            ToggleFlashlight(); // Toggle flashlight state on button press
        }

        // only perform flashlight logic if the flashlight is on
        if (flashlight.enabled)
        {
            Ray ray = new Ray(flashlight.transform.position, flashlight.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, flashlightDetectionRange))
            {
                // check if the flashlight is pointing directly at Slenderman
                if (hit.transform.CompareTag("Slenderman"))
                {
                    // if the flashlight is pointed at Slenderman, start the flicker effect from script
                    slendermanAI.StartBeingLitByFlashlight();
                }
                else
                {
                    // if the flashlight is not pointing at Slenderman, stop the flicker effect 
                    slendermanAI.StopBeingLitByFlashlight();
                }
            }
            else
            {
                // if the flashlight is not pointing at any object, stop the flicker effect
                slendermanAI.StopBeingLitByFlashlight();
            }
        }
        else
        {
            // if flashlight is turned off, stop the flicker effect
            slendermanAI.StopBeingLitByFlashlight();
        }
    }

    // toggle the flashlight on or off
    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn; // Turn the flashlight on or off
    }

    // cleanup on destroy
    private void OnDestroy()
    {
        flashlightToggleAction.Disable(); // Disable the action when no longer needed
    }
}

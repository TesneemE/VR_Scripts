using UnityEngine;

public class XRMovementSimulator : MonoBehaviour
{
    public float speed = 3.0f; // Movement speed
    public float rotationSpeed = 100.0f; // Rotation speed
    public Transform playerCamera; // Reference to the camera (if you want to move based on camera direction)

    private void Update()
    {
        // Get input for movement (W, A, S, D keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow

        // Move the XR Rig based on input
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        transform.position += moveDirection * speed * Time.deltaTime;

        // Get input for rotation (arrow keys or mouse)
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
    }
}

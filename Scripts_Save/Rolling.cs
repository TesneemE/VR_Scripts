// using UnityEngine;

// public class Rolling : MonoBehaviour
// {
//     public float speed = 2;
//     private Rigidbody = rb;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         rb= GetComponent<Rigidbody>();
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         float movementHorizontal= Input.GetAxis("Horizontal");
//         float movementVertical= Input.GetAxis("Vertical");

//         Vector3 movement = new Vector3 (movementHorizontal, 0.0f, movementVertical);

//         rb.AddForce(movement * speed);
//     }
// }
using UnityEngine;

public class Rolling : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool rollingToPlayer = true;

    public Transform player; // Assign the player's transform in the Inspector
    public float stopDistance = 1.5f; // Distance at which the ball stops in front of the player

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Start ball a few units away from the player
        Vector3 startOffset = new Vector3(-10.0f, 0.1f, 0.0f); // Adjust as needed
        // transform.position = player.position + startOffset;
        transform.position = startOffset;

        // Set target position in front of the player
        targetPosition = player.position + new Vector3(0, 0, stopDistance);
    }

    void FixedUpdate()
    {
        if (rollingToPlayer)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            // Stop rolling when close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                rollingToPlayer = false;
                rb.linearVelocity = Vector3.zero; // Stop any remaining movement
            }
        }
    }
}

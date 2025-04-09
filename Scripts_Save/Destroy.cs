using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Log the object name
        Destroy(collision.gameObject); // Destroy the object it collides with
    }
}

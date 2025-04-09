using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteCollider : MonoBehaviour
{
    public GameObject levelCompleteCollider;

    void OnTriggerEnter(Collider collider)
    {  
        if (collider.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);  // load first scene (menu)
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  // load next scene
        }
    }
}

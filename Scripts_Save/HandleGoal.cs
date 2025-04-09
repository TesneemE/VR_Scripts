
using UnityEngine;

public class HandleGoal : MonoBehaviour
{
    public GameObject levelCompleteCollider;

    public void completeGoal()
    {
        levelCompleteCollider.SetActive(true);  // activate collider for leaving scene
        gameObject.SetActive(false);  // deactivate goal object
    }
}

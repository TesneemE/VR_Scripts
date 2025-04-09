using UnityEngine;

public class FacingCheck : MonoBehaviour
{
    public GameObject target; // target object -assigned in inspector
    public float threshold = 0.99f; // 1 means perfectly aligned- used a threshold of .99
    public float nearDistance = 1.0f; // distance for checking if object is near target
    bool checkNear = false;

    void Update()
    {
        // update checkNear based on distance to target
        checkNear = IsNearTarget(target);

        if(checkNear)
        {
             target.SetActive(true);
             Destroy(target, 3f); //if near, Activate Target Object, and destroy with 3 second delay
        }
        if (IsFacingTarget(target) || checkNear)
        {
            target.SetActive(true);  //if facing, activate target object
        }
        else
        {
            target.SetActive(false); //if neither, target object is deactivated
        }
    }

    bool IsFacingTarget(GameObject target)
    {
        if (target == null) return false;

        // get direction from this object to the target
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        // get the forward direction of this object
        Vector3 forwardDirection = transform.forward;

        // use the dot product to check alignment
        float dotProduct = Vector3.Dot(forwardDirection, directionToTarget);
        return dotProduct >= threshold; // true if facing, means it's >=.99
    }

    bool IsNearTarget(GameObject target)
    {
        if (target == null) return false;

        // check if the distance is within nearDistance
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= nearDistance; //returns true if <=1
    }
}


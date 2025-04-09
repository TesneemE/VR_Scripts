using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastSlender : MonoBehaviour
{
    //The player object
    public GameObject playerObj;
    
    //Slender's Transform
    public Transform slenderTransform;
    
    //Bool that determines if the raycast is hitting the player or not
    public bool detected;
    
    //Offset that helps position the raycast if it's not positioned properly
    public Vector3 offset;

    //The Update() void makes stuff hapen every frame
    void Update()
    {
        Vector3 direction = (playerObj.transform.position - slenderTransform.position).normalized; //Slender's raycast direction will point towards the player
        RaycastHit hit; //RaycastHit variable
        
        //If the raycast hits something,
        if (Physics.Raycast(slenderTransform.position + offset, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(slenderTransform.position + offset, hit.point, Color.red, Mathf.Infinity); //The raycast is drawn for visualization purposes in the Unity Editor
            if (hit.collider.gameObject == playerObj) //If the raycast hits the player object,
            {
                detected = true; //detected equals true
                //call turn off light function?
            }
            else //else,
            {
                detected = false; //detected equals false
            }
        }
    }
}

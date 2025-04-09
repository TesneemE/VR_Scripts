using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;     // Move forward
        if (Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;  // Move backward
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;  // Move left
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;  // Move right

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
using UnityEngine;
using System.Collections;

public class Seminar : MonoBehaviour {

	public float moveSpeed = 10f;  // variable can be modified in Inspection window, helpful for letting users change settings in-game
	public float turnSpeed = 10f;

	// Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
		Translate();  // _-axis moves by [below] in each frame
		Rotate();  
	}

	void Translate() {

    	// transform.Translate(new Vector3(0, 0, 1f));  // will go forward very fast
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);  // moves meters per second, not frame

	}

	void Rotate() {

		// transform.Rotate(new Vector3(0, -1f, 0));
		transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);

	}

}
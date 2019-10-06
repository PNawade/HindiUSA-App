using UnityEngine;
using System.Collections;

public class fcColloider : MonoBehaviour {
	private float dir;
	private fcGameController gameControllerScript;


	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		if (go != null)
		{
			gameControllerScript = go.GetComponent <fcGameController>();
		}
		if (gameControllerScript == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerExit(Collider other) {
		GameObject go = other.gameObject;

//		if (Mathf.Abs (go.GetComponent<Rigidbody> ().velocity.y) == 0)
//			return;
		
		dir = go.GetComponent<Rigidbody> ().velocity.y;
		dir = go.transform.position.y - transform.position.y;
		go.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
		go.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);

		// tags are assigned to the four barriers
		// the topmost barrier is assigned tag of 1

		if (name == "quadUBoundry1") {
			if (dir > 0) {  // if the card was sliding up before completely exiting barrier, move it to the bottom and display next image
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
				gameControllerScript.showNextCard (other.gameObject);
			}
			if (dir < 0) { //if the card was sliding down before completely exiting barrier, snap it to the top slot
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
			}
		}
		if (name == "quadLBoundry4") {
			if (dir < 0) { //if the card was sliding down before completely exiting barrier, move it to the top and display prev image
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
				gameControllerScript.showPrevCard (other.gameObject);
			}
			if (dir > 0) { // if the card was sliding up before completely exiting barrier, snap it to the bottom slot
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
			}
		}
		if (name == "quadUBoundry2") {
		if (dir > 0) { // if the card was sliding up before completely exiting barrier, snap it to the top slot
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
				other.transform.rotation = gameControllerScript.getPH1 ().transform.rotation;	
				other.GetComponent<flip>().ResetRotation();
			} 
			if (dir < 0) { //if the card was sliding down before completely exiting barrier, snap it to the middle slot
				other.transform.position = gameControllerScript.getPH2 ().transform.position;
			}
		}
		if (name == "quadLBoundry3") {
		if (dir > 0) { // if the card was sliding up before completely exiting barrier
				other.transform.position = gameControllerScript.getPH2 ().transform.position;
			} 
			if (dir < 0) { //if the card was sliding down before completely exiting barrier
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
				other.transform.rotation = gameControllerScript.getPH3 ().transform.rotation;	
				other.GetComponent<flip>().ResetRotation();
			}
		}
	}
}

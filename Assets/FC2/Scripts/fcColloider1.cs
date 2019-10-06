using UnityEngine;
using System.Collections;

public class fcColloider1 : MonoBehaviour {
	private float dir;
	private fcGameController1 gameControllerScript;


	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		if (go != null)
		{
			gameControllerScript = go.GetComponent <fcGameController1>();
		}
		if (gameControllerScript == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other) {
		//GameObject pnl;
		//pnl = (GameObject)Instantiate (gameControllerScript.pnlFlashCard);
		//pnl.transform.SetParent (gameControllerScript.pnlSet.transform, false);
		//pnl.GetComponent<fcPanel> ().showPanel (3);
	}

	void OnTriggerExit(Collider other) {
		GameObject go = other.gameObject;
		dir = go.GetComponent<Rigidbody> ().velocity.y;
		go.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);

		go.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);

		// tags are assigned to the four barriers
		// the topmost barrier is assigned tag of 1

//		if (tag == "1") {
		if (name == "quadUBoundry1") {
			if (dir > 0) {  // if the card was sliding up before completely exiting barrier
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
				gameControllerScript.showNextCard (other.gameObject);
			}
			if (dir < 0) { //if the card was sliding down before completely exiting barrier
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
			}
		}
//		if (tag == "4") { 
		if (name == "quadLBoundry4") {
			if (dir < 0) { //if the card was sliding down before completely exiting barrier
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
				gameControllerScript.showPrevCard (other.gameObject);
			}
			if (dir > 0) { // if the card was sliding up before completely exiting barrier
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
			}
		}
//		if (tag == "2") {
		if (name == "quadUBoundry2") {
		if (dir > 0) { // if the card was sliding up before completely exiting barrier
				other.transform.position = gameControllerScript.getPH1 ().transform.position;
				other.transform.rotation = gameControllerScript.getPH1 ().transform.rotation;	
				other.GetComponent<flip1>().ResetRotation();
			} 
			if (dir < 0) { //if the card was sliding down before completely exiting barrier
				other.transform.position = gameControllerScript.getPH2 ().transform.position;
			}
		}
//		if (tag == "3") {
		if (name == "quadLBoundry3") {
		if (dir > 0) { // if the card was sliding up before completely exiting barrier
				other.transform.position = gameControllerScript.getPH2 ().transform.position;
			} 
			if (dir < 0) { //if the card was sliding down before completely exiting barrier
				other.transform.position = gameControllerScript.getPH3 ().transform.position;
				other.transform.rotation = gameControllerScript.getPH3 ().transform.rotation;	
				other.GetComponent<flip1>().ResetRotation();
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class flip : MonoBehaviour, IPointerClickHandler
{
	private fcGameController gameControllerScript;
	public Text uiTxtCardHindi;
	public Text uiTxtCardEng;
	public GameObject txt3DHindi;
	public GameObject txt3DEng;
	//public static Vector3 v;

	private float maxMove;
	private Rect touchRect;
	// Use this for initialization
	void Start ()
	{
		GameObject go = GameObject.Find ("GameController");
		if (go != null) {
			gameControllerScript = go.GetComponent <fcGameController> ();
		}
		if (gameControllerScript == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}

		maxMove = gameControllerScript.getPH2 ().transform.position.y - gameControllerScript.getPH3 ().transform.position.y;
		maxMove = 0.40f * Mathf.Abs (maxMove);
		touchRect = new Rect (0, 0, Screen.width, Screen.height * 0.80f);
	}

	public void ResetRotation ()
	{
		yInitialPos = 0;
		rotateAngle = 0;
		activeTouch = false;
		touchEnded = false;
		direction = new Vector2 (0, 0);
		transform.rotation = gameControllerScript.getPH2 ().transform.rotation;
	}


	private Vector2 startPos;
	private Vector2 direction;
	float yInitialPos = 0;
	float rotateAngle = 0;
	bool activeTouch = false;
	bool touchEnded = false;

	/*
	void Update ()
	{	
		//GetComponent<Rigidbody> ().velocity = v;
		Touch touch;
		if (Input.touchCount > 0) {
			touch = Input.GetTouch (0);

			// Handle finger movements based on touch phase.
			switch (touch.phase) {
			// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
				yInitialPos = transform.position.y;
				rotateAngle = transform.rotation.eulerAngles.y;
				activeTouch = true;
				touchEnded = false;
				break;

			case TouchPhase.Moved:
				direction = touch.position - startPos;
				break;

			// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				activeTouch = false;
				touchEnded = true;

				if (Mathf.Abs (direction.y) > Mathf.Abs (direction.x)) {
						GetComponent<Rigidbody> ().velocity = new Vector3 (0, 1.0f * (direction.y / Mathf.Abs (direction.y)), 0);	
						//v = new Vector3 (0, 1.0f * (direction.y / Mathf.Abs (direction.y)), 0);
				}
				break;
			}
		
			// do the following only if activeTouch is true
			// if a card is dragged too much, the card gets a velocity and activeTouch is switched off
			// this stops further response to the dragging, until a new touch is started
			if (activeTouch == true) {
				// first check if the swipe is vertical, then slide the cards
				if (Mathf.Abs (direction.y) > Mathf.Abs (direction.x)) {
					float yDisplacement = transform.position.y - yInitialPos;
					if (Mathf.Abs (yDisplacement) < maxMove)
						transform.position = new Vector3 (transform.position.x, transform.position.y + touch.deltaPosition.y / 200, transform.position.z);
					else {
						GetComponent<Rigidbody> ().velocity = new Vector3 (0, direction.y / 50, 0);
						//v = new Vector3 (0, direction.y / 50, 0);
						activeTouch = false;
					}
				} 
				// if the swipe is horizontal then rotate the middle card only
				if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
					if (transform.position == gameControllerScript.getPH2 ().transform.position) {	// this makes ure that only the middle card is rotated
						transform.Rotate (0, -touch.deltaPosition.x, 0);
					}			
				}
			}
		}

		if (touchEnded == true) {
			if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
				if (transform.position == gameControllerScript.getPH2 ().transform.position) {	// this makes sure that only the middle card is rotated
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, rotateAngle + 180.0f, 0), 2.0f * Time.deltaTime);
				}
			}
		}
	}

*/
	public void OnPointerClick (PointerEventData ped)
	{
//		ResetRotation ();
//		rotateAngle = 180;
//		touchEnded = true;
//		direction = new Vector2 (1, 0);
	}

	void Update ()
	{	
		if (Mathf.Abs (GetComponent<Rigidbody> ().velocity.y) > 0)
			return;
		if (transform.position != gameControllerScript.getPH2 ().transform.position)
			return;
		AudioSource audio = GameObject.Find ("GameController").gameObject.GetComponent<AudioSource> ();

		if ((Input.touchCount > 0) && (touchRect.Contains (Input.GetTouch (0).position))) {
			Touch touch = Input.GetTouch (0);

			// Handle finger movements based on touch phase.
			switch (touch.phase) {
			// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
				yInitialPos = transform.position.y;
				rotateAngle = transform.rotation.eulerAngles.y;
				activeTouch = true;
				touchEnded = false;
				break;

			case TouchPhase.Moved:
				direction = touch.position - startPos;
				break;

			// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				activeTouch = false;
				touchEnded = true;
			
				if ((Mathf.Abs (direction.y) > Mathf.Abs (direction.x) * 1.1f) && !(audio.isPlaying)) {
					//GetComponent<Rigidbody> ().velocity = new Vector3 (0, Mathf.Sign(direction.y) * 4, 0);	
					gameControllerScript.SetVelocity (Mathf.Sign (direction.y) * 4);
				}
				break;
			} // END OF CASE
			if (activeTouch == true) {
				/*
				// first check if the swipe is vertical, then slide the cards
				if (Mathf.Abs (direction.y) > Mathf.Abs (direction.x)) {
					if (Mathf.Abs(transform.position.y - yInitialPos) < maxMove)
						transform.position = new Vector3 (transform.position.x, transform.position.y + touch.deltaPosition.y / 200, transform.position.z);
				} 
				*/

				// if the swipe is horizontal then rotate the middle card only
				if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
					if (transform.position == gameControllerScript.getPH2 ().transform.position) {	// this makes ure that only the middle card is rotated
						transform.Rotate (0, -touch.deltaPosition.x, 0);
					}			
				}
			}
		} // END OF TOUCH
		if ((touchEnded == true) & (activeTouch == false)) {
			if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y) * 1.1f) {
				if (transform.position == gameControllerScript.getPH2 ().transform.position) {	// this makes sure that only the middle card is rotated
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, rotateAngle + 180.0f, 0), 3.0f * Time.deltaTime);
				}
			} 	
		}
	}
	//END OF UPDATE
}
//END OF CLASS


	
/*
	void Update_old()
	{
	 	Vector2 touchDelta;

		if (Input.touchCount> 0 && Input.GetTouch(0).phase==TouchPhase.Moved) {
			touchDelta  = Input.GetTouch(0).deltaPosition;

			// do not rotate if the card is sliding vertically
			if (gameObject.GetComponent<Rigidbody> ().velocity.magnitude == 0) {
				targetRotation *= Quaternion.AngleAxis (-touchDelta.x / 5, Vector3.up);
			}

			// do not set the card in motion for minor swipes
			if (gameObject.GetComponent<Rigidbody> ().velocity.magnitude == 0)
				if (Mathf.Abs((touchDelta.y/Time.deltaTime)) > 300) 
					GetComponent<Rigidbody>().velocity = new Vector3 (0,touchDelta.y *0.1f,0);
		}

//		if (gameObject.GetComponent<Rigidbody> ().velocity.magnitude == 0) { 
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				touchDelta = Input.GetTouch (0).deltaPosition;
				if (Mathf.Abs (touchDelta.x) < 5) {
					targetRotation = iniRotation;
				}
				
				if (touchDelta.x < -5) {
					targetRotation = iniRotation * Quaternion.AngleAxis (180, Vector3.up);
					iniRotation = targetRotation;
				} //This was a flick to the left with magnitude of 5 or more
				if (touchDelta.x > 5) {
					targetRotation = iniRotation * Quaternion.AngleAxis (-180, Vector3.up);
					iniRotation = targetRotation;
				} //This was a flick to the right with magnitude of 5 or more
			}

			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 5.0f * Time.deltaTime); 

//		}
	}

*/

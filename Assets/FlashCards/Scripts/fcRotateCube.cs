using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CnControls;

public class fcRotateCube : MonoBehaviour {

	public float smooth = 100f;
	public Text uiTxtCard;
	private float iniAngle; 
	private fcGameController gameController;

	private Quaternion targetRotation;
	void Start(){
		targetRotation = transform.rotation;
		GameObject gameControllerObject = GameObject.Find ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <fcGameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

void Update () {
	if (CnInputManager.TouchCount ==1) {
			targetRotation *=  Quaternion.AngleAxis(180, Vector3.up);
	}
		transform.rotation= Quaternion.Lerp (transform.rotation, targetRotation , 10.0f * Time.deltaTime); 
//		if ((transform.rotation.eulerAngles.y >175) && (transform.rotation.eulerAngles.y < 185))
//			uiTxtCard.text = gameController.getCardNameHindi();
//		else
//			uiTxtCard.text = "";
		//uiText.text = transform.rotation.eulerAngles.y.ToString();
			
}
}
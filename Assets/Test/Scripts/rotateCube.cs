using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class rotateCube : MonoBehaviour
{
	public Text txtScore;
	public Text txtCardsShown;
	public Text txtCardsAttempted;
	public Text txtCardsSkipped;
	public Text txtCardsCorrect;
	public Text txtCardsWrong;
	public Text txtTime;
	private float startTime;
	private float elapsedTime;
	private bool fGameOver;

	// Use this for initialization
	void Start ()
	{
		startRotation ();
		startTime = Time.time;
		fGameOver = false;
	}

	public void startRotation ()
	{
		gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 1, 0);
	}

	public void stopRotation ()
	{
		gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);
	}

	void Update ()
	{
		if (fGameOver == false)
			elapsedTime = Time.time - startTime;
		//txtTime.text = elapsedTime.ToString ();
		txtTime.text = string.Format ("{0:0}:{1:00}", Mathf.Floor (elapsedTime / 60), elapsedTime % 60);
	}

	public void DisplayScore ()
	{
		txtScore.text = testScore.getScore ().ToString ();
		txtCardsShown.text = "Cards Shown : " + testScore.getCardsShown ().ToString ();
		txtCardsAttempted.text = "Cards Attempted : " + testScore.getCardsAttempted ().ToString ();
		txtCardsSkipped.text = "Cards Skipped : " + testScore.getCardsSkipped ().ToString ();
		txtCardsCorrect.text = "Cards Correct : " + testScore.getCardsCorrect ().ToString ();
		txtCardsWrong.text = "Cards Wrong : " + testScore.getCardsWrong ().ToString ();
	}

	public void GameOver ()
	{
		fGameOver = true;
		gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);
		//gameObject.transform.rotation = new Quaternion 
		transform.eulerAngles = new Vector3 (0, 270, 0);
		transform.position = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1.24f, 1.45f, 1.06f);
		return;

		Transform tr = transform.Find ("quadSide1/Canvas/imgBlue");
		if (tr == null)
			Debug.Log ("Score Object Not Found");
		//GameObject goParent = GameObject.Find ("imgBackground");
		//if (goParent == null)
		//	Debug.Log ("Parent not found");
		tr.localScale = new Vector3 (2, 2, 2);
		//tr.position = new Vector3 (
		//tr.SetParent(goParent.transform,false);

	}
}

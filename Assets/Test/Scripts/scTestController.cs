using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;


public class scTestController : MonoBehaviour
{
	public Text uiTxtLevelName;
	private int selectedCard;
	public GameObject testCard;
	public GameObject[] ansCardSo;
	//Scene Object
	public bool clickEnabled = true;
	public Image img1;
	public Image img2;
	public Text txtCardNo;
	private uniqueRand urTestCard;
	public GameObject controller;

	public txtList ansCards;
	private txtList txtListComponent;
	private int cardCount;
	private string categoryName;

	void Start ()
	{
		categoryName = globalData.getCategory ();
		uiTxtLevelName.text = categoryName;

		ansCards = GetComponent<txtList> ();
		StartCoroutine (Init ());
	}

	IEnumerator Init ()
	{
		while (ansCards.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		} 

		ansCards.loadCardList ("Cards");
		cardCount = ansCards.cardList.Count;
		if (cardCount == 0) {
			Debug.Log ("No configuration available for selected option");
		} else {
			uiTxtLevelName.text = globalData.path [globalData.pathDepth];	
			urTestCard = new uniqueRand (0, cardCount - 1);
			testScore.resetScore ();
			ansCardSo [0].GetComponent<ansCardGo> ().init (0); 
			ansCardSo [1].GetComponent<ansCardGo> ().init (1); 
			ansCardSo [2].GetComponent<ansCardGo> ().init (2); 
			ansCardSo [3].GetComponent<ansCardGo> ().init (3); 

			showNextCard ();
		}
	}

	void showNextCard ()
	{
		selectedCard = urTestCard.nextRandom ();
		//StartCoroutine (showImage (selectedCard.ToString()));
		img1.GetComponent<showImage> ().show (selectedCard.ToString ());
		img2.GetComponent<showImage> ().show (selectedCard.ToString ());
		showAnswers ();
		testScore.UpdateScore ("SHOWN");
		txtCardNo.text = testScore.getCardsShown () + "/" + cardCount;
	}

	void showAnswers ()
	{
		int randomAnsCard;
		int randomPos;
		int fillCount = 0;
		uniqueRand urPosCard = new uniqueRand (0, 3);
		uniqueRand urAnsCard = new uniqueRand (0, cardCount - 1);

		randomPos = urPosCard.nextRandom ();
		ansCardSo [randomPos].GetComponent<ansCardGo> ().show (selectedCard);
		fillCount = fillCount + 1;

		while (fillCount < 4) {
			randomAnsCard = urAnsCard.nextRandom ();
			if (randomAnsCard != selectedCard) {
				randomPos = urPosCard.nextRandom ();
				ansCardSo [randomPos].GetComponent<ansCardGo> ().show (randomAnsCard);
				fillCount = fillCount + 1;
			}
		}

	}

	public void checkCard (int i, int p)
	{
		if (i == selectedCard) {
			ansCardSo [p].GetComponent<ansCardGo> ().flashGreen ();
			testScore.UpdateScore ("CORRECT");

			controller.GetComponent<AudioControllerScript> ().playClapSound ();
		} else {
			testScore.UpdateScore ("WRONG");
			ansCardSo [p].GetComponent<ansCardGo> ().flashRed (); 

			controller.GetComponent<AudioControllerScript> ().playWrongSound ();
		}
		testCard.GetComponent<rotateCube> ().DisplayScore ();
		if (testScore.getCardsShown () >= cardCount) {
			testCard.GetComponent<rotateCube> ().GameOver ();
			return;
		}

		testCard.GetComponent<rotateCube> ().stopRotation ();

		StartCoroutine (ShowAfterSomeTime (3f));
	}

	IEnumerator ShowAfterSomeTime (float t)
	{
		clickEnabled = false;
		yield return new WaitForSecondsRealtime (t);

		testCard.GetComponent<rotateCube> ().startRotation ();
		showNextCard ();
		clickEnabled = true;
	}

	public void back ()
	{
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

}

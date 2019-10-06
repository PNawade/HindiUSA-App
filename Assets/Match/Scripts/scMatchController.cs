using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class scMatchController : MonoBehaviour
{
	/*
	public class matchCard {
		public string hindiName;
		public string englishName;
		public matchCard (string hn, string en)
		{
			hindiName = hn;
			englishName = en;
		}
	}
*/
	public Text txtScore;
	public Text uiTxtLevelName;
	public GameObject prefabCard;
	private List<GameObject> matchCardsGo = new List<GameObject> ();
	public txtList matchCards;
	private int cardCount;
	private int score = 0;
	private string categoryName;
	public GameObject controller;
	private bool clickEnabled = true;

	// Use this for initialization
	void Start ()
	{
		categoryName = globalData.getCategory ();

		txtScore.text = "Score: " + score;

		uiTxtLevelName.text = categoryName;
		matchCards = GetComponent<txtList> ();
		StartCoroutine (Init ());
	}

	IEnumerator Init ()
	{
		while (matchCards.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		} 
		matchCards.loadCardList ("Cards");
		cardCount = matchCards.cardList.Count;

		if (cardCount == 0) {
			Debug.Log ("No configuration available for selected option");
			yield break;
		}

		uiTxtLevelName.text = globalData.path [globalData.pathDepth];	

		createCards ();
		showGame ();

	}

	void createCards ()
	{
		for (int i = matchCardsGo.Count - 1; i >= 0; i--) {
			Destroy (matchCardsGo [i]);
			matchCardsGo.RemoveAt (i);
		}

		for (int i = 0; i < 12; i++) {
			//destroy and delete all objects from the list
			GameObject go = Instantiate (prefabCard);
			go.name = i.ToString ();
			matchCardsGo.Add (go);
			go.GetComponent<matchCardGo> ().Init (i);
		}
	}

	void showGame ()
	{
		uniqueRand urTerm = new uniqueRand (0, cardCount - 1);
		int randomTerm;

		uniqueRand urCardPos = new uniqueRand (0, 11);
		int randomPos;

		createCards ();
		clickNo = 0;
		cardsMatched = 0;
			
		for (int i = 0; i < 6; i++) {
			randomTerm = urTerm.nextRandom ();

			randomPos = urCardPos.nextRandom ();
			matchCardsGo [randomPos].GetComponent<matchCardGo> ().show (randomTerm, "IMAGE");

			randomPos = urCardPos.nextRandom ();
			matchCardsGo [randomPos].GetComponent<matchCardGo> ().show (randomTerm, "TEXT");
		}
	}


	private int clickNo;
	private int firstCard;
	private int secondCard;
	private matchCardGo firstCardObject;
	private matchCardGo secondCardObject;
	private int cardsMatched;
	private float resetTime = 0.5f;

	public void registerCard (int cardPos)
	{
		clickNo = clickNo + 1;

		if (clickNo == 1) {
			firstCard = cardPos;
			firstCardObject = matchCardsGo [firstCard].GetComponent<matchCardGo> ();
		}

		if (clickNo == 2) {
			secondCard = cardPos;
			secondCardObject = matchCardsGo [secondCard].GetComponent<matchCardGo> ();
			if ((firstCardObject.getCardType ().Equals (secondCardObject.getCardType ())) && (firstCardObject.getCardIndex () == secondCardObject.getCardIndex ())) {
				clickNo = 0;
				firstCardObject.resetCardAfterTime (0f);
			}
		}

		if (clickNo == 1)
			firstCardObject.setColor (210, 210, 10);  // set the card yellow

		if (clickNo == 2) {
			// check the match
			// if match set both cards to green and close them
			// if no match set both card to red and make them normal
			// set the clickNo = 0
			//bool correct = ((firstCardObject.et.text.Equals (secondCardObject.et.text)) && (firstCardObject.ht.text.Equals (secondCardObject.ht.text)));

			if (firstCardObject.getCardIndex () == secondCardObject.getCardIndex ()) {
				firstCardObject.setColor (10, 210, 10);  // set the card green
				secondCardObject.setColor (10, 210, 10);  // set the card green
				firstCardObject.closeCardAfterTime (resetTime);
				secondCardObject.closeCardAfterTime (resetTime);
				cardsMatched = cardsMatched + 1;
				controller.GetComponent<AudioControllerScript> ().playClapSound ();
				changeScore (2);
			} else {
				firstCardObject.setColor (210, 10, 10);  // set the card red
				secondCardObject.setColor (210, 10, 10);  // set the card red
				firstCardObject.resetCardAfterTime (resetTime);
				secondCardObject.resetCardAfterTime (resetTime);
				controller.GetComponent<AudioControllerScript> ().playWrongSound ();
				changeScore (-2);
			}
			clickNo = 0;
			if (cardsMatched == 6) {
				showGame ();
				changeScore (3);
			}
			StartCoroutine (ShowAfterSomeTime (2f));
		}
	}

	void changeScore (int changeInScore)
	{
		score = score + changeInScore;
		txtScore.text = "Score: " + score;
	}

	IEnumerator ShowAfterSomeTime (float t)
	{
		clickEnabled = false;
		yield return new WaitForSecondsRealtime (t);
		clickEnabled = true;
	}

	public void back ()
	{
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

}

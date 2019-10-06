using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class fcGameController1 : MonoBehaviour {

	public txtList flashCards;
	private int[] rIndexArray;  //random index array used to access the card list
	private int cardCount; 
	public string categoryName;
	public GameObject cubeFlashCard;
	public GameObject pnlFlashCard;
	public GameObject fcPlaceHolder1;
	public GameObject fcPlaceHolder2;
	public GameObject fcPlaceHolder3;

	public GameObject fc1;
	public GameObject fc2;
	public GameObject fc3;

	public GameObject pnl1;
	public GameObject pnl2;
	public GameObject pnl3;
	public GameObject pnlSet;

	private int topCard;


	void Start () {
		categoryName = globalData.getCategory ();
		flashCards = GetComponent<txtList> ();
		StartCoroutine (Init ());
	}

	IEnumerator Init() {
		while (flashCards.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		} 
		flashCards.loadCardList ("Cards");
		cardCount = flashCards.cardList.Count;
		if (cardCount == 0) {
			Debug.Log ("No configuration available for selected option");
			yield break ;
		}

		randomizeArray ();
		/*
		pnl1 = (GameObject)Instantiate (pnlFlashCard);
		pnl2 = (GameObject)Instantiate (pnlFlashCard);
		pnl3 = (GameObject)Instantiate (pnlFlashCard);

		pnl1.transform.SetParent (pnlSet.transform, false);
		pnl2.transform.SetParent (pnlSet.transform, false);
		pnl3.transform.SetParent (pnlSet.transform, false);

		pnl1.GetComponent<fcPanel>().showPanel(rIndexArray[0]);
		pnl2.GetComponent<fcPanel>().showPanel(rIndexArray[1]);
		pnl3.GetComponent<fcPanel>().showPanel(rIndexArray[2]);

*/
		//fc1 = (GameObject) Instantiate (cubeFlashCard, fcPlaceHolder1.transform.position, fcPlaceHolder1.transform.rotation);
		//fc2 = (GameObject) Instantiate (cubeFlashCard, fcPlaceHolder2.transform.position, fcPlaceHolder2.transform.rotation);
		//fc3 = (GameObject) Instantiate (cubeFlashCard, fcPlaceHolder3.transform.position, fcPlaceHolder3.transform.rotation);

//		fc1.GetComponent<showFlashCard1>().show (rIndexArray[0]);
//		fc2.GetComponent<showFlashCard1>().show (rIndexArray[1]);
//		fc3.GetComponent<showFlashCard1>().show (rIndexArray[2]);
		topCard = 0;
	}

	private void randomizeArray () {
		rIndexArray = new int[cardCount];
		uniqueRand r  = new uniqueRand (0, cardCount - 1);
		r.resetUniqueRandomInt ();

		for (int i = 0; i < cardCount; i++)
			rIndexArray[i] = r.nextRandom();
	}

	public GameObject getPH1()
	{
		return fcPlaceHolder1;
	}

	public GameObject getPH2()
	{
		return fcPlaceHolder2;
	}

	public GameObject getPH3()
	{
		return fcPlaceHolder3;
	}

	// this displays image on the card that is at the bottom
	public void showNextCard(GameObject fc) 
	{
		topCard = topCard + 1;
		if (topCard == cardCount) 
			topCard = 0;
	
		int i = topCard + 2;
		if (i >= cardCount) 
			i = i - cardCount;
		
		fc.GetComponent<showFlashCard1>().show (rIndexArray[i]);
	}

	//this displays image on the card that is at the top
	public void showPrevCard(GameObject fc) 
	{
		topCard = topCard - 1;
		if (topCard == -1)
			topCard = cardCount - 1;
		fc.GetComponent<showFlashCard1>().show (rIndexArray[topCard]);
	}
		

	public void back() {
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

	public void playClip ()
	{
		int i = topCard + 1;
		i = rIndexArray [i];
		AudioClip ac = Resources.Load (categoryName + "/Audio/" + i.ToString()) as AudioClip;
		AudioSource audio = gameObject.GetComponent<AudioSource>();
		audio.PlayOneShot (ac);
	}

}

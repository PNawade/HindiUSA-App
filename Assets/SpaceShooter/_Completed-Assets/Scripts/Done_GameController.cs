using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;



public class Done_GameController : MonoBehaviour
{
	public GameObject basicCard;
	private int cardCount;
	public float minX;
	public float maxX;
	public float maxZ;

	public int waveCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text gameOverText;

	public GUIText scoreText;
	//public GUIText gameOverText;
	public Text ht;
	public Text et;
	public Text uiTxtScore;
	public Text uiTxtWrongCard;

	public Button b;
	public string categoryName;
	public GameObject quadWrongCard;

	private bool gameOver;
	private int score;

	//private cardList spaceCards;
	public txtList spaceCards;
	private int selectedCard;

	void Start ()
	{
		minX = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 9)).x;
		maxX = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 9)).x;

		gameOver = false;

		b.gameObject.SetActive (false);

		score = 0;
		UpdateScore ();
		categoryName = globalData.getCategory ();
		spaceCards = GetComponent<txtList> ();
		StartCoroutine (Init ());
		gameOverText.gameObject.SetActive (false);
	}

	IEnumerator Init ()
	{
		while (spaceCards.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		} 
		spaceCards.loadCardList ("Cards");
		cardCount = spaceCards.cardList.Count;
		if (cardCount == 0) {
			Debug.Log ("No configuration available for selected option");
			yield break;
		}
		newCard ();
		StartCoroutine (SpawnWaves ());
	}

	public void Restart ()
	{
		SceneManager.LoadScene ("SpaceShooter", LoadSceneMode.Single);
	}


	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true) {
			int forced_card_seq;
			forced_card_seq = UnityEngine.Random.Range (0, waveCount - 1);

			for (int i = 0; i < waveCount; i++) {
				int r;
				string rStr;

				if (i == forced_card_seq)
					r = selectedCard;
				else
					r = UnityEngine.Random.Range (0, cardCount - 1);
				rStr = r.ToString ();
				
				Vector3 spawnPosition = new Vector3 (UnityEngine.Random.Range (minX, maxX), 0, 15);
				Quaternion spawnRotation = Quaternion.identity;
				GameObject go = (GameObject)Instantiate (basicCard, spawnPosition, spawnRotation);
				go.name = rStr;
				yield return new WaitForSeconds (spawnWait);

			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver) {
				break;
			}
		}
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		uiTxtScore.text = "Score: " + score;
	}

	public void GameOver ()
	{
		gameOverText.gameObject.SetActive (true);
		gameOver = true;
		b.gameObject.SetActive (true);
	}

	public bool isMatch (string tagName)
	{
		int i;
		i = int.Parse (tagName);

		if (selectedCard == i)
			return true;
		else
			return false;
	}

	public void newCard ()
	{
		selectedCard = UnityEngine.Random.Range (0, cardCount - 1);
		et.text = spaceCards.cardList [selectedCard].hindiRomanName;
		ht.text = spaceCards.cardList [selectedCard].hindiName;

		/*
		if (spaceCards.cardList [selectedCard].hindiName == "") {
			ht.text = "";
			et.text = spaceCards.cardList [selectedCard].englishName;
		} else {
			ht.text = spaceCards.cardList [selectedCard].hindiName;
			et.text = "";
		}
		*/
	}

	public void showWrongCard (string tag, GameObject go)
	{
		int i;
		i = int.Parse (tag);

		quadWrongCard.transform.position = go.transform.position;
		uiTxtWrongCard.text = spaceCards.cardList [i].hindiName;
		StartCoroutine (resetText ());
	}

	IEnumerator resetText ()
	{
		yield return new WaitForSeconds (0.4f);
		uiTxtWrongCard.text = "";
	}

	public void back ()
	{
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}
}
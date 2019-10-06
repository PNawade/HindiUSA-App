using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class fcGameController : MonoBehaviour
{
	public Button shuffleButton;

	public txtList flashCards;
	private int[] rIndexArray;
	//random index array used to access the card list
	private int cardCount;
	public string categoryName;
	public GameObject cubeFlashCard;
	public GameObject fcPlaceHolder1;
	public GameObject fcPlaceHolder2;
	public GameObject fcPlaceHolder3;
	public Button btnSound;
	public Image[] images;
	public GameObject panel;

	private GameObject fc1;
	private GameObject fc2;
	private GameObject fc3;

	public AudioClip ac;

	private int topCard;


	void Start ()
	{
		categoryName = globalData.getCategory ();
		flashCards = GetComponent<txtList> ();
		StartCoroutine (Init ());
		images = btnSound.GetComponentsInChildren<Image> ();

		panel.SetActive (false);
	}

	public int getCurrentCardNo ()
	{
		int i;
		i = topCard + 1;
		if (i == cardCount)
			i = 0;
		
		return rIndexArray [i];
	}

	IEnumerator Init ()
	{
		while (flashCards.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		}
		flashCards.loadCardList ("Cards");
		cardCount = flashCards.cardList.Count;
		if (cardCount == 0) {
			Debug.Log ("No configuration available for selected option");
			yield break;
		}

		rIndexArray = new int[cardCount]; 
		//Order in which to show the cards


		//if shuffle flag is set, the array is set in a random order
		if (flashCards.fShuffle) {
			uniqueRand r = new uniqueRand (0, cardCount - 1);
			r.resetUniqueRandomInt ();
			for (int i = 0; i < cardCount; i++) {
				rIndexArray [i] = r.nextRandom ();
			}
		} else {
			//if shuffle flag is not set, the array is set in a sequential order
			for (int i = 0; i < cardCount; i++)
				rIndexArray [i] = i;
		}

		fc1 = (GameObject)Instantiate (cubeFlashCard, fcPlaceHolder1.transform.position, fcPlaceHolder1.transform.rotation);
		fc2 = (GameObject)Instantiate (cubeFlashCard, fcPlaceHolder2.transform.position, fcPlaceHolder2.transform.rotation);
		fc3 = (GameObject)Instantiate (cubeFlashCard, fcPlaceHolder3.transform.position, fcPlaceHolder3.transform.rotation);

		fc1.GetComponent<showFlashCard> ().show (rIndexArray [0]);
		fc2.GetComponent<showFlashCard> ().show (rIndexArray [1]);
		fc3.GetComponent<showFlashCard> ().show (rIndexArray [2]);
		topCard = 0;


	}


	/*private void randomizeArray ()
	{
		rIndexArray = new int[cardCount];
		uniqueRand r = new uniqueRand (0, cardCount - 1);
		r.resetUniqueRandomInt ();
		if (!shuffle) {
			for (int i = 0; i < cardCount; i++)
				rIndexArray [i] = i;
		} else {
			for (int i = 0; i < cardCount; i++) {
				rIndexArray [i] = r.nextRandom ();
			}
		}
	}*/

	public GameObject getPH1 ()
	{
		return fcPlaceHolder1;
	}

	public GameObject getPH2 ()
	{
		return fcPlaceHolder2;
	}

	public GameObject getPH3 ()
	{
		return fcPlaceHolder3;
	}

	public void SetVelocity (float v)
	{
		fc1.GetComponent<Rigidbody> ().velocity = new Vector3 (0, v, 0);
		fc2.GetComponent<Rigidbody> ().velocity = new Vector3 (0, v, 0);
		fc3.GetComponent<Rigidbody> ().velocity = new Vector3 (0, v, 0);
	}

	// this displays image on the card that is at the bottom
	public void showNextCard (GameObject fc)
	{
		topCard = topCard + 1;
		if (topCard == cardCount)
			topCard = 0;
	
		int i = topCard + 2;
		if (i >= cardCount)
			i = i - cardCount;
		
		fc.GetComponent<showFlashCard> ().show (rIndexArray [i]);
	}

	//this displays image on the card that is at the top
	public void showPrevCard (GameObject fc)
	{
		topCard = topCard - 1;
		if (topCard == -1)
			topCard = cardCount - 1;
		fc.GetComponent<showFlashCard> ().show (rIndexArray [topCard]);
	}


	public void back ()
	{
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

	/*private void shuffleCards ()
	{
		shuffle = true;

		randomizeArray ();

		fc1.GetComponent<showFlashCard> ().show (rIndexArray [0]);
		fc2.GetComponent<showFlashCard> ().show (rIndexArray [1]);
		fc3.GetComponent<showFlashCard> ().show (rIndexArray [2]);
		topCard = 0;
	}*/

	public void playClip ()
	{
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		if (!audio.isPlaying) {
			StartCoroutine (asyncLoadClip1 ());

		}
	}

	IEnumerator changeBtnColor ()
	{
		panel.SetActive (true);

		AudioSource audio = gameObject.GetComponent<AudioSource> ();

		while (audio.isPlaying) {
			yield return new WaitForSeconds (0.05f);
		}

		panel.SetActive (false);
	}

	IEnumerator asyncLoadClip1 ()
	{
		string url = globalData.url;
		int i = getCurrentCardNo ();
		url = url + globalData.getUnixPath () + "/" + i.ToString () + ".mp3";
		url = url + "?p=" + globalData.sessionId;
		//url = "file://" + Application.persistentDataPath + "/test.wav";

		string authorization = authenticate ("hadmin", "hindiusa123");
		//UnityWebRequest www = UnityWebRequest.GetTexture (url);

		

		UnityWebRequest www = UnityWebRequest.GetAudioClip (url, AudioType.MPEG);
		www.SetRequestHeader ("AUTHORIZATION", authorization);
		yield return www.Send ();

		if (www.isError) {
			Debug.Log ("Error accessing the url : " + url + " : " + www.error);
		} else {
			AudioSource audio = gameObject.GetComponent<AudioSource> ();
			
			ac = DownloadHandlerAudioClip.GetContent (www);
			audio.PlayOneShot (ac);
			StartCoroutine (changeBtnColor ());
		}

		www.Dispose ();
		/*WWW www = new WWW (url);

		yield return www;
		if (string.IsNullOrEmpty(www.error)) {
			AudioSource audio = gameObject.GetComponent<AudioSource>();
			ac = www.GetAudioClip (false,true, AudioType.MPEG);
			audio.PlayOneShot (ac);
		}*/

	}

	string authenticate (string username, string password)
	{
		string auth = username + ":" + password;
		auth = System.Convert.ToBase64String (System.Text.Encoding.GetEncoding ("ISO-8859-1").GetBytes (auth));
		auth = "Basic " + auth;
		return auth;
	}
}

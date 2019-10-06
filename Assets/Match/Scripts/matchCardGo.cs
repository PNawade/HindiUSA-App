using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class matchCardGo : MonoBehaviour,IPointerClickHandler
{
	public UnityEngine.UI.Text ht;
	// this is the hindi text shown on the card
	public UnityEngine.UI.Text et;
	// this is the english text shown on the card
	public Image img;
	// this is the image shown on the card

	private GameObject go;
	private scMatchController mc;
	private Image imgGo;
	// this is the sprite image on the card itself (parent of img above)

	private int cardIndex;
	// this is the index of the cardImage displayed on this card
	private int cardPos;
	// this is the position of the card on screen (0 to 11)
	private string cardType;
	// this could be IMAGE or TEXT
	private string categoryName;

	public void Init (int i)
	{
		cardPos = i;
		GameObject matchControllerGo = GameObject.Find ("matchController");
		mc = matchControllerGo.GetComponent<scMatchController> ();

		categoryName = globalData.getCategory ();

		imgGo = gameObject.GetComponent<Image> ();

		if (cardPos < 3)
			gameObject.transform.SetParent (GameObject.Find ("imgRow1").transform, false);
		if (cardPos >= 3 && cardPos <= 5)
			gameObject.transform.SetParent (GameObject.Find ("imgRow2").transform, false);
		if (cardPos >= 6 && cardPos <= 8)
			gameObject.transform.SetParent (GameObject.Find ("imgRow3").transform, false);
		if (cardPos >= 9)
			gameObject.transform.SetParent (GameObject.Find ("imgRow4").transform, false);

	}

	public void setCardIndex (int i)
	{
		cardIndex = i;
	}

	public int getCardIndex ()
	{
		return cardIndex;
	}

	public void setCardType (string str)
	{
		cardType = str;
	}

	public string getCardType ()
	{
		return cardType;
	}

	public void show (int i, string strType)
	{
		cardType = strType;
		cardIndex = i;

		if (cardType == "TEXT") {
			et.text = mc.matchCards.cardList [cardIndex].hindiRomanName;
			ht.text = mc.matchCards.cardList [cardIndex].hindiName;
			/*
			if (mc.matchCards.cardList [cardIndex].hindiName == "") {
				ht.text = "";
				et.text = mc.matchCards.cardList [cardIndex].englishName;
			} else {
				et.text = "";
				ht.text = ht.text = mc.matchCards.cardList [cardIndex].hindiName;
			}
			*/
			img.enabled = false;
		}
		if (cardType == "IMAGE") {
			et.text = mc.matchCards.cardList [cardIndex].hindiRomanName;
			ht.text = mc.matchCards.cardList [cardIndex].hindiName;
			ht.enabled = false;
			et.enabled = false;
			img.enabled = true;
			setColor (107, 117, 168);
			img.GetComponent<showImage> ().show (cardIndex.ToString ());
		}
	}

	public void setColor (byte R, byte G, byte B)
	{
		imgGo.color = new Color32 (R, G, B, 255);
	}

	public void OnPointerClick (PointerEventData ped)
	{
		mc.registerCard (cardPos); 
	}

	public void closeCardAfterTime (float t)
	{
		StartCoroutine (closeCard (t));
	}

	public void resetCardAfterTime (float t)
	{
		StartCoroutine (resetCard (t));
	}

	IEnumerator closeCard (float t)
	{
		yield return new WaitForSecondsRealtime (t);
		img.enabled = false;
		ht.text = "";
		et.text = "";
		setColor (255, 255, 255);
	}

	IEnumerator resetCard (float t)
	{
		yield return new WaitForSecondsRealtime (t);
		setColor (107, 117, 168);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ansCardGo : MonoBehaviour,IPointerClickHandler
{
	private Text ht;
	// this is the hindi text shown on the card
	private Text et;
	private scTestController tc;
	private Image imgGo;
	// this is image associated with the answer card (outer image that shows border)

	private int cardIndex;
	// this is the index of the cardImage displayed on this card
	private int cardPos;
	// this is the position of the card on screen (0 to 11)

	public void init (int p)
	{
		GameObject testControllerGo = GameObject.Find ("testController");
		tc = testControllerGo.GetComponent<scTestController> ();
		imgGo = gameObject.GetComponent<Image> ();
		GameObject go = gameObject.transform.Find ("txtHindiName").gameObject;
		ht = go.GetComponent<Text> ();
		et = gameObject.transform.Find ("txtEnglishName").gameObject.GetComponent<Text> ();
		cardPos = p;
	}

	public void setCardIndex (int i)
	{
		cardIndex = i;
	}

	public int getCardIndex ()
	{
		return cardIndex;
	}

	public void show (int i)
	{
		cardIndex = i;
		resetCard ();
		ht.text = tc.ansCards.cardList [cardIndex].hindiName;
		et.text = tc.ansCards.cardList [cardIndex].hindiRomanName;

		/*
		if (tc.ansCards.cardList [cardIndex].hindiName == "") {
			ht.text = "";
			et.text = tc.ansCards.cardList [cardIndex].englishName;
		} else {
			et.text = "";
			ht.text = tc.ansCards.cardList [cardIndex].hindiName;
		}
		*/
	}

	public void flashGreen ()
	{
		setColor (10, 210, 10);
//		resetCardAfterTime (0.2f);
	}

	public void flashRed ()
	{
		setColor (210, 10, 10);
//		resetCardAfterTime (0.2f);
	}

	private void setColor (byte R, byte G, byte B)
	{
		imgGo.color = new Color32 (R, G, B, 255);
	}

	public void OnPointerClick (PointerEventData ped)
	{
		if (tc.clickEnabled) {
			tc.checkCard (cardIndex, cardPos);
		}
	}

	private void resetCardAfterTime (float t)
	{
		StartCoroutine (resetCard (t));
	}

	IEnumerator resetCard (float t)
	{
		yield return new WaitForSecondsRealtime (t);
		setColor (107, 117, 168);
	}

	public void resetCard ()
	{
		setColor (107, 117, 168);
	}
}

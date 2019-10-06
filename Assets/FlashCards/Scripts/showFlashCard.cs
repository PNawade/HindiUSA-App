using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showFlashCard : MonoBehaviour
{

	public Image img;
	public Text uiTxtHindiName;
	public Text uiTxtHindiRomanName;
	public Text uiTxtEnglishName;
	private GameObject gc;
	//Game Controller Object
	private fcGameController fcgc;

	void Start ()
	{
		gc = GameObject.Find ("GameController");
		fcgc = gc.GetComponent<fcGameController> ();
	}

	public void show (int randomizedIndex)
	{
		gc = GameObject.Find ("GameController");
		fcgc = gc.GetComponent<fcGameController> ();

		img.GetComponent<showImage> ().show (randomizedIndex.ToString ());
		uiTxtHindiName.text = fcgc.flashCards.cardList [randomizedIndex].hindiName;
		uiTxtEnglishName.text = fcgc.flashCards.cardList [randomizedIndex].englishName;
		uiTxtHindiRomanName.text = fcgc.flashCards.cardList [randomizedIndex].hindiRomanName;
	}
}

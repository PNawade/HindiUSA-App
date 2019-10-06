using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fcPanel : MonoBehaviour {

	public Image img;
	public Text uiTxtHindiName;
	public Text uiTxtEnglishName;
	private GameObject gc;  //Game Controller Object
	private fcGameController1 fcgc;

	void Start() {
		gc = GameObject.Find ("GameController");
		fcgc = gc.GetComponent<fcGameController1> ();
	}
	public void showPanel (int randomizedIndex) {
		gc = GameObject.Find ("GameController");
		fcgc = gc.GetComponent<fcGameController1> ();

		img.GetComponent<showImage> ().show (randomizedIndex.ToString ());
		uiTxtHindiName.text = fcgc.flashCards.cardList [randomizedIndex].hindiName;
		uiTxtEnglishName.text = fcgc.flashCards.cardList [randomizedIndex].englishName;
	}
}

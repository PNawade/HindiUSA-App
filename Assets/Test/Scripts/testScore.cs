using UnityEngine;
using System.Collections;

public class testScore  {

	static private int score;
	static private int cardsShown;
	static private int cardsAttempted;
	static private int cardsSkipped;
	static private int cardsCorrect;
	static private int cardsWrong;

	// Use this for initialization
	static public void resetScore () {
		score = 0;
		cardsShown = 0;
		cardsAttempted = 0;
		cardsSkipped = 0;
		cardsCorrect = 0;
		cardsWrong = 0;

	}

	static public void UpdateScore (string strResult) {
		switch (strResult) {
		case "SHOWN":
			cardsShown++;
			cardsAttempted++;
			break;
		case "CORRECT":
			score = score + 10;
			cardsCorrect++;
			break;
		case "WRONG":
			score = score - 10;
			cardsWrong++;
			break;
		case "SKIPPED":
			cardsSkipped++;
			break;
		}
	}
	static public int getScore() {
		return score;
	}
	static public int getCardsShown() {
		return cardsShown;
	}
	static public int getCardsAttempted () {
		return cardsAttempted;
	}
	static public int getCardsSkipped() {
		return cardsSkipped;
	}
	static public int getCardsCorrect(){
		return cardsCorrect;
	}
	static public int getCardsWrong() {
		return cardsWrong;
	}
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class cardList  {

	public class card {
		public string hindiName;
		public string englishName;

		public card (string hn, string en)
		{
			hindiName = hn;
			englishName = en;
		}
	}
		
	public List <card> cards;

	public cardList (string fileName, string sectionName) {
		loadList (fileName, sectionName);
	}
	// Use this for initialization

	public void loadList (string fileName, string sectionName) {
		cards = new List<card> ();
		INIParser ini = new INIParser ();
		TextAsset fileData = Resources.Load(fileName) as TextAsset;
		ini.Open(fileData);

		for (int i = 0; i < 100 ; i++) 
		{
			string line = ini.ReadValue(sectionName, i.ToString(),"-1");
			if (line == "-1")
				break;
			string[] tokens = line.Split(',');
			cards.Add (new card (tokens [0], tokens [1]));
		}
		ini.Close ();
	}

	public int Count() {
		return cards.Count;
	}

}

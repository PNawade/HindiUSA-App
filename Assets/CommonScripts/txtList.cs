using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class txtList:MonoBehaviour
{
	public bool fLoaded = false;
	public List <string> lstLines;

	public class lvlMenu
	{
		public string lvlNameHindi;
		public string lvlNameHindiRoman;
		public string lvlNameEnglish;
		public string termCount;
		public string subMenu;
		public string unixDirName;

		// the following class is for storing the structure of menuconfig file
		public lvlMenu (string str1, string str2, string str3, string str4, string str5, string str6)
		{
			lvlNameHindi = str1.Trim ();
			lvlNameHindiRoman = str2.Trim ();
			lvlNameEnglish = str3.Trim ();
			termCount = str4.Trim ();
			subMenu = str5.Trim ();
			unixDirName = str6.Trim ();
		}
	}

	public List <lvlMenu> lvlMenuList = new List<lvlMenu> ();

	// the following class is for storing the structure of cardlist file
	public class card
	{
		public string hindiName;
		public string hindiRomanName;
		public string englishName;
		public string imgName;
		public string audioName;

		public card (string hn, string hrn, string en)
		{
			hindiName = hn;
			hindiRomanName = hrn;
			englishName = en;
		}
	}

	public List <card> cardList = new List<card> ();
	public bool fShuffle;

	// the following class is for storing the strcture of userconfig file
	public class user
	{
		public string[] userArray;
		public string userName;
		public string token;
		public string deviceID1;
		public string deviceID2;
		public string schoolName;
		public string S1;
		public string S2;
		public string B1;
		public string B2;
		public string I1;
		public string I2;
		public string I3;
		public string A1;
		public string A2;
		public string admin;
		public string levelCoordinator;

		public user (string[] userProfile)
		{
			userArray = userProfile;
			userName = userProfile [0];
			token = userProfile [1];
			deviceID1 = userProfile [2];
			deviceID2 = userProfile [3];
			schoolName = userProfile [4];
			S1 = userProfile [5];
			S2 = userProfile [6];
			B1 = userProfile [7];
			B2 = userProfile [8];
			I1 = userProfile [9];
			I2 = userProfile [10];
			I3 = userProfile [11];
			A1 = userProfile [12];
			A2 = userProfile [13];
			admin = userProfile [14];
			levelCoordinator = userProfile [15];
		}
	}

	public List <user> userList = new List<user> ();

	void Start ()
	{
		fLoaded = false;
		StartCoroutine (loadTxtLines ());
	}

	public void Refresh ()
	{
		fLoaded = false;
		globalData.sessionId = UnityEngine.Random.Range (1000000, 9999999).ToString ();
		StartCoroutine (loadTxtLines ());
	}

	IEnumerator loadTxtLines ()
	{
		string strAllText;
		string url = globalData.url;
		string fileName;

		if (SceneManager.GetActiveScene ().name == "ListLevel")
		//if (globalData.getCategory () == "")
			fileName = "menuConfig.txt";
		else if (SceneManager.GetActiveScene ().name == "LogIn")
			fileName = "userList.csv";
		else
			fileName = globalData.getUnixPath () + "/" + "cardConfig.txt";
		url = url + fileName + "?p=" + globalData.sessionId;

		string authorization = authenticate ("hadmin", "hindiusa123");
		UnityWebRequest www = UnityWebRequest.GetTexture (url);
		www.SetRequestHeader ("AUTHORIZATION", authorization);

		yield return www.Send ();

		if (www.responseCode == 403) {
			print ("Unauthorized!");
		}

        if (www.isError)
        {
            Debug.Log("Error accessing the url : " + url + " : " + www.error);
        }
        else
        {
            strAllText = www.downloadHandler.text;
            lstLines = new List<string>(strAllText.Split(new string[] { "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries));
            print(strAllText);
        }
		print (url);
		www.Dispose ();
		fLoaded = true;
	}

	string authenticate (string username, string password)
	{
		string auth = username + ":" + password;
		auth = System.Convert.ToBase64String (System.Text.Encoding.GetEncoding ("ISO-8859-1").GetBytes (auth));
		auth = "Basic " + auth;
		return auth;
	}

	public void loadMenuList (string mnuName)
	{
		string[] tokens = new string[5];
		lvlMenuList.Clear ();
		
		for (int i = 0; i < lstLines.Count; i++) {
			if (lstLines [i].Trim () == "[" + mnuName + "]") {
				for (int j = i + 1; j < lstLines.Count; j++) {
					tokens = lstLines [j].Split (',');
					if (tokens.Length != 6)
						break;
					lvlMenuList.Add (new lvlMenu (tokens [0].Trim (), tokens [1].Trim (), tokens [2].Trim (), tokens [3].Trim (), tokens [4].Trim (), tokens [5].Trim ()));
				}
			}
		}
	}

	public void loadCardList (string mnuName)
	{
		string[] parts = new string[2];
		string[] tokens = new string[2];
		cardList.Clear ();

		fShuffle = true;

		for (int i = 0; i < lstLines.Count; i++) {

			if (lstLines [i].Trim () == "Shuffle=0") {
				fShuffle = false;
			}

			if (lstLines [i].Trim () == "[" + mnuName + "]") {
				for (int j = i + 1; j < lstLines.Count; j++) {
					parts = lstLines [j].Split ('=');
					if (parts.Length != 2)
						break;
					tokens = parts [1].Split (',');
					if (tokens.Length == 3)
						cardList.Add (new card (tokens [0].Trim (), tokens [1].Trim (), tokens [2].Trim ()));
				}
			}
		}
	}

	public void loadUserList ()
	{
		for (int i = 0; i < lstLines.Count; i++) {
			string[] tokens = lstLines [i].Split (',');
			if (tokens.Length == 16) {
				userList.Add (new user (tokens));
			}
		}
	}
}

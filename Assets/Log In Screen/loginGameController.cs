using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class loginGameController : MonoBehaviour
{
    public Image logo;
	public txtList userProfileList;
	public Button button;
	public Canvas inputCanvas;
	public InputField usernameField;
	public InputField passwordField;
	public static string storeIDScript = "sendUUID.php";
	public static string storeLoginAttempt = "LoginScript.php";
	public Text messagetext;
	public Text txtVersion;
	public bool storeIDFinished;
	public bool logAttemptFinished;


	// Use this for initialization
	void Start ()
	{
        
		globalData.Init ("hindi");
		messagetext.text = "Please enter your registered email address and the assigned six digit token to log in.";
		StartCoroutine (Init ());
		txtVersion.text = "Version: " + globalData.version;
	}

	IEnumerator Init ()
	{
		while (userProfileList.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		}
		userProfileList.loadUserList ();

		if (checkForDeviceID ()) {
            globalData.loggedIn = true;
			SceneManager.LoadScene ("ListLevel", LoadSceneMode.Single);
		}
	}

	bool checkForDeviceID ()
	{
		for (int i = 0; i < userProfileList.userList.Count; i++) {
			if (globalData.deviceID == userProfileList.userList [i].deviceID1) {
				globalData.loggedinUser = userProfileList.userList [i];
				return true;
			}
		}
		return false;
	}

	bool CheckCredentials ()
	{
		for (int i = 0; i < userProfileList.userList.Count; i++) {
			if (usernameField.text.Equals (userProfileList.userList [i].userName) && passwordField.text.Equals (userProfileList.userList [i].token)) {
				if (userProfileList.userList [i].deviceID1.Equals ("")) {
					StartCoroutine (loginAttempt ("SUCCESS    TIME: " + System.DateTime.Now +
					"    DEVICE: " + globalData.deviceID +
					"    EMAIL: " + usernameField.text +
					"    PASSWORD: " + passwordField.text));
					globalData.loggedinUser = userProfileList.userList [i];
					StartCoroutine (RegisterDevice ());
					return true;
				} else {
					StartCoroutine (loginAttempt ("FAILURE (Token is already being used)    TIME: " + System.DateTime.Now +
					"    DEVICE: " + globalData.deviceID +
					"    EMAIL: " + usernameField.text +
					"    PASSWORD: " + passwordField.text));
					messagetext.text = "This token is already registered with another device.";
					return false;
				}
			}
		}
		StartCoroutine (loginAttempt ("FAILURE (Email or token is incorrect)    TIME: " + System.DateTime.Now +
		"    DEVICE: " + globalData.deviceID +
		"    EMAIL: " + usernameField.text +
		"    PASSWORD: " + passwordField.text));
		
		messagetext.text = "Incorrect Username or Password";
		return false;
	}

	IEnumerator RegisterDevice ()
	{
		/*
		 * for each element of the userprofile array
		 * if the username and token = globaldata.user and globaldata.token then
		 *      update the device id in the array
		 *      break
		 * end if
		 * write back the entire array using php call
		 */
		globalData.loggedinUser.userArray [2] = globalData.deviceID;
		for (int i = 0; i < userProfileList.userList.Count; i++) {
			if (usernameField.text.Equals (userProfileList.userList [i].userName) && passwordField.text.Equals (userProfileList.userList [i].token)) {
				userProfileList.userList [i] = globalData.loggedinUser;
			}
		}

		string newText = "";

		for (int i = 0; i < userProfileList.userList.Count; i++) {
			for (int j = 0; j < userProfileList.userList [i].userArray.Length; j++) {
				newText = newText + userProfileList.userList [i].userArray [j];
				if (j != userProfileList.userList [i].userArray.Length - 1)
					newText = newText + ",";
			}
			if (i != userProfileList.userList.Count - 1)
				newText = newText + "\n";
		}

		print (newText);
		WWWForm form = new WWWForm ();
		form.AddField ("newText", newText);

		UnityWebRequest www = UnityWebRequest.Post (globalData.url + storeIDScript, form);
		www.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());

		print (globalData.url + storeIDScript);

		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {
			Debug.Log ("Data: " + www.downloadHandler.text);
		}
		www.Dispose ();
		storeIDFinished = true;
	}

	public void ButtonClicked ()
	{
		userProfileList.Refresh ();
		StartCoroutine (waitToRefresh ());
	}

	IEnumerator waitToRefresh ()
	{
		while (userProfileList.fLoaded == false) {
			yield return new WaitForSeconds (0.05f);
		}
		bool correctCredential = CheckCredentials ();
		logAttemptFinished = false;
		if (correctCredential) {
			storeIDFinished = false;
			while (!storeIDFinished || !logAttemptFinished) {
				yield return new WaitForSeconds (0.05f);
			}
            globalData.loggedIn = true;
			SceneManager.LoadScene ("ListLevel", LoadSceneMode.Single);
		}
	}

	IEnumerator loginAttempt (string message)
	{
		WWWForm form = new WWWForm ();
		form.AddField ("newLine", "\n" + message);

		UnityWebRequest www = UnityWebRequest.Post (globalData.url + storeLoginAttempt, form);
		www.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());

		print (globalData.url + storeIDScript);

		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {
			Debug.Log ("Data: " + www.downloadHandler.text);
		}
		www.Dispose ();
		logAttemptFinished = true;
	}

    public void showAboutScreen ()
    {
        SceneManager.LoadScene("AboutLogIn", LoadSceneMode.Single);
    }


    /*IEnumerator GetText ()
	{
		WWW w = new WWW (globalData.url + passFile);
		yield return w;
		if (w.error != null)
			Debug.Log ("Error accessing the url : " + globalData.url + passFile + " : " + w.error);
		else {
			passText = w.text;
		}
		w.Dispose ();
		ReadText ();
	}

	private void ReadText ()
	{
		pswrdArray = passText.Split ('\n');
		IDArray = new string[pswrdArray.Length];
		usrnmArray = new string[pswrdArray.Length];
		roleArr = new string[pswrdArray.Length];

		for (int i = 0; i < pswrdArray.Length; i++) {
			string[] tempArr = pswrdArray [i].Split (',');
			if (tempArr.Length == 7) {
				roleArr [i] = tempArr [0];
				usrnmArray [i] = tempArr [1];
				pswrdArray [i] = tempArr [2];
				IDArray [i] = tempArr [3];
				if (!accessGranted && IDArray [i].Equals (globalData.deviceID)) {
					if (tempArr [0].Equals ("admin")) {
						admin = true;
					} else {
						admin = false;
					}
					globalData.setUser (roleArr [i], usrnmArray [i], pswrdArray [i], admin);
					StartCoroutine (Grant ("Welcome back " + usrnmArray [i]));
					index = i;
				}
			} else {
				Debug.Log ("Error in reading password and username at entry: " + i);
				print (pswrdArray [i]);
			}
		}
	}

	public void ButtonClicked ()
	{
		if (!accessGranted) {
			Validate ();
			Debug.Log ("Attempting to log on");
		}
	}


	public void Validate ()
	{
		if (!clicked) {
			clicked = true;
			bool validate = false;
			for (int i = 0; i < pswrdArray.Length; i++) {
				if ((pswrdArray [i].Equals (passwordField.text) && usrnmArray [i].Equals (usernameField.text))) {
					validate = true;
					index = i;
					break;
				}
			}
			// scenario 1: username/password does not exist in the list
			if (!validate) {
				Deny ("Incorrect Username and Password");
				clicked = false;
				return;
			}
			
			// scenario2 - username and password matches but the device is already associated with this login
			if (!IDArray [index].Equals ("")) {
				Deny ("This account is already logged onto another device.");
				clicked = false;
				return;
			} 
			if (roleArr [index].Equals ("admin")) {
				admin = true;
			} else {
				admin = false;
			}
			// scenario3 - username and password match and the device is not associated with this login
			StartCoroutine (SaveDevice (usrnmArray [index], pswrdArray [index]));
		}
	}

	IEnumerator SaveDevice (string u, string p)
	{
		WWWForm deviceIDForm = new WWWForm ();
		deviceIDForm.AddField ("UUID", globalData.deviceID);
		deviceIDForm.AddField ("email", u);
		deviceIDForm.AddField ("pswrd", p);
		deviceIDForm.AddField ("oldUUID", "");
		deviceIDForm.AddField ("role", roleArr [index]);
		globalData.setUser (roleArr [index], u, p, admin);
		WWW saveID = new WWW (globalData.url + storeIDScript, deviceIDForm);
		yield return saveID;
		if (saveID.error == null) {
			Debug.Log ("Save done");
			Debug.Log ("Text: " + saveID.text);
		} else {
			Debug.Log ("Error during Save: " + saveID.error);
			Debug.Log ("Text: " + saveID.text);
		}
		saveID.Dispose ();
		StartCoroutine (GetText ());
		StartCoroutine (Grant ("You are logged in as " + u));
	}*/

    /*IEnumerator Grant (string message)
	{
		Debug.Log ("Granted");
		messagetext.text = ("Access Granted: " + message);
		usernameField.gameObject.SetActive (false);
		passwordField.gameObject.SetActive (false);
		button.gameObject.GetComponentInChildren<Text> ().text = "Log Out";
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene ("ListLevel", LoadSceneMode.Single);
	}*/


}
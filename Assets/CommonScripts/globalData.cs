using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;



public class globalData: MonoBehaviour
{
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	extern static public void showNativeAlert (string title, string message);
	#endif


	static public string projectName;
	static public string[] path = { "Main", "", "" };
	static public string[] arrUnixPath = { "Main", "", "" };
	static public string deviceID = "";
	static public int pathDepth = 0;
	static public string url;
	static public string uploadURL;
	static public string sessionId = "0";
    static public bool loggedIn = false;
	static public txtList.user loggedinUser;
	static public string version = "1.05";

	public static void Init (string pn)
	{
		sessionId = UnityEngine.Random.Range (1000000, 9999999).ToString ();
		projectName = pn;
		url = "http://34.238.119.210/" + projectName + "/";
		uploadURL = "http://34.238.119.210/" + projectName + "/upload.php";
		deviceID = SystemInfo.deviceUniqueIdentifier;
	}

	private static string authenticate (string username, string password)
	{
		string auth = username + ":" + password;
		auth = System.Convert.ToBase64String (System.Text.Encoding.GetEncoding ("ISO-8859-1").GetBytes (auth));
		auth = "Basic " + auth;
		return auth;
	}

	public static string getHeader ()
	{
		return authenticate ("hadmin", "hindiusa123");
	}

	/*public static WWWForm signOutForm ()
	{
		WWWForm deviceIDForm = new WWWForm ();
		//deviceIDForm.AddField ("role", role);
		deviceIDForm.AddField ("UUID", "");
		deviceIDForm.AddField ("oldUUID", deviceID);
		deviceIDForm.AddField ("pswrd", pswrd);
		deviceIDForm.AddField ("email", usrnm);
		return deviceIDForm;
	}

	public IEnumerator RemoveDeviceID ()
	{
		WWWForm deviceIDForm = new WWWForm ();
		deviceIDForm.AddField ("UUID", "");
		deviceIDForm.AddField ("oldUUID", deviceID);
		deviceIDForm.AddField ("pswrd", pswrd);
		deviceIDForm.AddField ("email", usrnm);
		//WWW saveID = new WWW (globalData.url + ValidatePassword.storeIDScript, deviceIDForm);
		yield return saveID;
		if (saveID.error == null) {
			Debug.Log ("Save done");
			Debug.Log ("Text: " + saveID.text);
		} else {
			Debug.Log ("Error during Save: " + saveID.error);
			Debug.Log ("Text: " + saveID.text);
		}
		saveID.Dispose ();
		SceneManager.LoadScene ("ListLevel", LoadSceneMode.Single);
	}*/

	static public string getCategory ()
	{
		string categoryName = "";

		if (pathDepth == 0)
			categoryName = "";

		if (pathDepth == 1)
			categoryName = path [1];

		if (pathDepth > 1) {
			categoryName = path [1];
			for (int n = 2; n <= pathDepth; n++) {
				categoryName = categoryName + "/" + path [n];
			}
		}
		return categoryName;
	}

	static public string getUnixPath ()
	{
		string unixPath = "";

		if (pathDepth == 0)
			unixPath = "";

		if (pathDepth == 1)
			unixPath = arrUnixPath [1];

		if (pathDepth > 1) {
			unixPath = arrUnixPath [1];
			for (int n = 2; n <= pathDepth; n++) {
				unixPath = unixPath + "/" + arrUnixPath [n];
			}
		}
		return unixPath;		
	}
}

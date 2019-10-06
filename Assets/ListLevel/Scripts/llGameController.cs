using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;


public class llGameController : MonoBehaviour
{
	public GameObject prefabLevel;
	public Text uiTxtLevelName;
	private List<GameObject> itemList = new List<GameObject> ();
	private int lstCount;
	private GameObject btnBackGo;
	private string categoryName;
	private txtList txtListComponent;
	public string projectName;

	void Awake ()
	{
		btnBackGo = GameObject.Find ("btnBack");
		txtListComponent = GetComponent<txtList> ();
		StartCoroutine (ShowList ("Main"));
	}

	IEnumerator ShowList (string mnuName)
	{
		while (txtListComponent.fLoaded == false) {
			yield return new WaitForSeconds (0.1f);
		}
		mnuName = globalData.path [globalData.pathDepth];
		if (mnuName == "")
			mnuName = "Main";
		txtListComponent.loadMenuList (mnuName);

		for (int i = 0; i < txtListComponent.lvlMenuList.Count; i++) {
			GameObject go = (GameObject)Instantiate (prefabLevel);
			go.name = i.ToString ();
			go.GetComponent<pnlLevel> ().Init (txtListComponent.lvlMenuList [i]);
			itemList.Add (go);
		}
		lstCount = itemList.Count;
		categoryName = globalData.getCategory ();
		uiTxtLevelName.text = categoryName;
		if (categoryName == "")
			btnBackGo.SetActive (false);
		else
			btnBackGo.SetActive (true);
	}

	void DestroyAll ()
	{
		for (int i = lstCount - 1; i >= 0; i--) {
			Destroy (itemList [i]);
			itemList.RemoveAt (i);
		}
	}

	public void GoForward (string lvlName, string unixDirname, bool fSubMenu)
	{
		print (lvlName);
		DestroyAll ();
		globalData.pathDepth = globalData.pathDepth + 1;
		globalData.path [globalData.pathDepth] = lvlName;
		globalData.arrUnixPath [globalData.pathDepth] = unixDirname;
		if (fSubMenu == true) {
			StartCoroutine (ShowList (lvlName));
		} else {
			SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
		}
	}

	public void GoBack ()
	{
		DestroyAll ();
		globalData.pathDepth = globalData.pathDepth - 1;
		StartCoroutine (ShowList (globalData.path [globalData.pathDepth]));
	}

	public void AboutButtonClicked ()
	{
		SceneManager.LoadScene ("About", LoadSceneMode.Single);
	}

	/*public void Exit ()
	{
		WWW saveID = new WWW (globalData.url + ValidatePassword.storeIDScript, globalData.signOutForm ());
		StartCoroutine (removeDeviceID (saveID));
	}

	IEnumerator removeDeviceID (WWW saveID)
	{
		yield return saveID;
		if (saveID.error == null) {
			Debug.Log ("Save done");
			Debug.Log ("Text: " + saveID.text);
		} else {
			Debug.Log ("Error during Save: " + saveID.error);
			Debug.Log ("Text: " + saveID.text);
		}
		saveID.Dispose ();
		SceneManager.LoadScene ("LogIn", LoadSceneMode.Single);
	}*/
}

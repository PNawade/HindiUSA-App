using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class loadImage:MonoBehaviour  { 
public bool fLoaded = false; 
public Texture texture;

	public void load (string imgName) {
		fLoaded = false;
		StartCoroutine (asyncLoad (imgName));
	}

	IEnumerator asyncLoad (string imgName) {
		string url = globalData.url;
		//url = url + globalData.getCategory () + "/" + imgName + ".png";
		url = url + globalData.getCategory () + "10.jpg";

		UnityWebRequest www = UnityWebRequest.GetTexture (url);
		yield return www.Send();

		if (www.isError) {
			Debug.Log ("Error accessing the url : " + url + " : " + www.error);
		} else {
			texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
		}
		fLoaded = true;
	}
}
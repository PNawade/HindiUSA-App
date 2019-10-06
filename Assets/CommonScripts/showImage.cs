using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class showImage:MonoBehaviour
{
	public bool fLoaded = false;
	private Material mat;
	private Image img;
	private GameObject go;

	void Awake ()
	{
		img = gameObject.GetComponent<Image> ();
		go = GameObject.Find ("GameController");
	}

	public void show (string imgName)
	{
		fLoaded = false;
		img = gameObject.GetComponent<Image> ();
		StartCoroutine (asyncLoad (imgName));
		StartCoroutine (asyncLoadClip (imgName));
	}

	IEnumerator asyncLoad (string imgName)
	{
		string url = globalData.url;
		url = url + globalData.getUnixPath () + "/" + imgName + ".jpg" + "?p=" + globalData.sessionId;

		UnityWebRequest www = UnityWebRequest.GetTexture (url);
		www.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());

		yield return www.Send ();
		print (url);
		if (www.responseCode == 404) {
			url = globalData.url + globalData.getUnixPath () + "/" + imgName + ".png" + "?p=" + globalData.sessionId;
			www = UnityWebRequest.GetTexture (url);
			www.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());
			yield return www.Send ();
		}
		if (www.isError) {
			Debug.Log ("Error accessing the url : " + url + " : " + www.error);
		} else {
			DestroyImmediate (mat);
			mat = new Material (Shader.Find ("Sprites/Default"));
			mat.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			img.material = mat;
		}
		print(url);
		www.Dispose();
		fLoaded = true;
	}

	IEnumerator asyncLoadClip (string imgName)
	{
		string url = globalData.url;
		url = url + globalData.getUnixPath () + "/Audio/" + imgName + ".mp3";
		WWW www = new WWW (url);
		fcGameController fcg;

		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			fcg = go.GetComponent<fcGameController> (); 
			fcg.ac = www.GetAudioClip (false, false);
		}
	}
}
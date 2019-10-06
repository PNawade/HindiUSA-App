using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


[RequireComponent (typeof(AudioSource))]
public class MicControlC : MonoBehaviour
{

	public float sensitivity = 100;
	public UnityEngine.UI.Text txtRecording;
	public float ramFlushSpeed = 5;
	//The smaller the number the faster it flush's the ram, but there might be performance issues...
	[Range (0, 100)]
	public float sourceVolume = 100;
	//Between 0 and 100
	public bool GuiSelectDevice = true;
	//
	public string selectedDevice { get; private set; }

	public float loudness { get; private set; }
	//dont touch
	//
	private float ramFlushTimer;
	private int amountSamples = 256;
	//increase to get better average, but will decrease performance. Best to leave it
	private int minFreq, maxFreq;
	public Button recordButton;

	private GameObject go;

	void Start ()
	{
		GetComponent<AudioSource> ().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource> ().mute = false; // Mute the sound, we don't want the player to hear it
		selectedDevice = Microphone.devices [0].ToString ();
		GetMicCaps ();
		txtRecording.gameObject.SetActive (false);
		go = GameObject.Find ("GameController");
		recordButton.gameObject.SetActive (false);
		if (globalData.loggedinUser.admin.Equals ("1")) {
			recordButton.gameObject.SetActive (true);
		}
	}

	public void GetMicCaps ()
	{
		Microphone.GetDeviceCaps (selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)//These 2 lines of code are mainly for windows computers
			maxFreq = 44100;
	}

	public void StartMicrophone ()
	{
		
		GetComponent<AudioSource> ().clip = Microphone.Start (selectedDevice, true, 10, maxFreq);  //Starts recording
		while (!(Microphone.GetPosition (selectedDevice) > 0)) {
		}                                  // Wait until the recording has started
	}

	public void StopMicrophone ()
	{
		GetComponent<AudioSource> ().Stop ();  //Stops the audio
		Microphone.End (selectedDevice);      //Stops the recording of the device	
	}

	private void RamFlush ()
	{
		if (ramFlushTimer >= ramFlushSpeed && Microphone.IsRecording (selectedDevice)) {
			StopMicrophone ();
			StartMicrophone ();
			ramFlushTimer = 0;
		}
	}

	float GetAveragedVolume ()
	{
		float[] data = new float[amountSamples];
		float a = 0;
		GetComponent<AudioSource> ().GetOutputData (data, 0);
		foreach (float s in data) {
			a += Mathf.Abs (s);
		}
		return a / amountSamples;
	}

	public void recordClip ()
	{
		string filepath;

		//generate new session id so that recorded clips are played (instead of cached one)
		globalData.sessionId = UnityEngine.Random.Range (1000000, 9999999).ToString ();

		int i = go.GetComponent<fcGameController> ().getCurrentCardNo ();
		filepath = Path.Combine (Application.persistentDataPath, i.ToString () + ".wav");

		GetComponent<AudioSource> ().volume = (sourceVolume / 1);
		loudness = GetAveragedVolume () * sensitivity * (sourceVolume / 1);

		if (Microphone.IsRecording (selectedDevice)) {
			StopMicrophone ();
			AudioClip ac = SavWav.TrimSilence (GetComponent<AudioSource> ().clip, 0.001f);
			SavWav.Save (filepath, ac);
			txtRecording.text = "Uploading ...";
			StartCoroutine (UploadFileCo (filepath));
		} else {
			StartMicrophone ();
			txtRecording.text = "Recording ...";
			txtRecording.gameObject.SetActive (true);
		}
		//AudioSource aud = gameObject.GetComponent<AudioSource>();
		//aud.clip = Microphone.Start(null, false, 1, 44100);
		//aud.Play();	
	}

	IEnumerator UploadFileCo (string localFileName)
	{
		UnityWebRequest localFile = UnityWebRequest.GetTexture ("file://" + localFileName);
		localFile.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());
		//WWW localFile = new WWW ("file://" + localFileName);
		yield return localFile.Send ();

		if (localFile.error == null)
			Debug.Log ("Loaded file successfully");
		else {
			Debug.Log ("Open file error: " + localFile.error);
			yield break;           // stop the coroutine here
		}
		WWWForm postForm = new WWWForm ();
		// version 1
		//postForm.AddBinaryData("theFile",localFile.bytes);
		// version 2
		postForm.AddField ("UnixPath", globalData.getUnixPath ());
		postForm.AddBinaryData ("theFile", localFile.downloadHandler.data, localFileName, "audio/x-wav");


		//WWW upload = new WWW (globalData.uploadURL, postForm);  

		UnityWebRequest upload = UnityWebRequest.Post (globalData.uploadURL, postForm);
		upload.SetRequestHeader ("AUTHORIZATION", globalData.getHeader ());

		yield return upload.Send ();

		// hide the text that is displaying "Uploading"
		txtRecording.gameObject.SetActive (false);
		if (upload.error == null)
			Debug.Log ("upload done :" + upload.downloadHandler.text);
		else
			Debug.Log ("Error during upload: " + upload.error);
	}
}
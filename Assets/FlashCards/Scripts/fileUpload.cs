// C# file names: "FileUpload.cs"
using UnityEngine;
using System.Collections;


public class fileUpload : MonoBehaviour
{
	private string m_LocalFileName = "C:/boot.ini";
	private string m_URL = "http://192.168.178.29/php/upload.php";

	IEnumerator UploadFileCo(string localFileName, string uploadURL)
	{
		WWW localFile = new WWW("file:///" + localFileName);
		yield return localFile;
		if (localFile.error == null)
			Debug.Log("Loaded file successfully");
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			yield break; // stop the coroutine here
		}
		WWWForm postForm = new WWWForm();
		// version 1
		//postForm.AddBinaryData("theFile",localFile.bytes);
		// version 2
		postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"text/plain");
		WWW upload = new WWW(uploadURL,postForm);        
		yield return upload;
		if (upload.error == null)
			Debug.Log("upload done :" + upload.text);
		else
			Debug.Log("Error during upload: " + upload.error);
	}
	void UploadFile(string localFileName, string uploadURL)
	{
		StartCoroutine(UploadFileCo(localFileName, uploadURL));
	}
}

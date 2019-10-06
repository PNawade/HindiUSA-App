using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour {
	public UnityEngine.UI.Text txtSetName;
	public UnityEngine.UI.Text txtLevelName;

	// Use this for initialization
	void Start () {
		txtSetName.text = globalData.path[globalData.pathDepth];
		txtLevelName.text = globalData.path [1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void goBack() {
		globalData.pathDepth = globalData.pathDepth - 1;
		SceneManager.LoadScene ("ListLevel", LoadSceneMode.Single);
	}

	public void cards() {
		SceneManager.LoadScene ("FlashCards", LoadSceneMode.Single);
	}

	public void space() {
		SceneManager.LoadScene ("SpaceShooter", LoadSceneMode.Single);
	}

	public void match() {
		SceneManager.LoadScene ("Match", LoadSceneMode.Single);
	}
	public void test() {
		SceneManager.LoadScene ("Test", LoadSceneMode.Single);
	}

}

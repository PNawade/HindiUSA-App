using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Done_DestroyByTime : MonoBehaviour
{
	public float lifetime;

	IEnumerator Start ()
	{
		yield return new WaitForSeconds (0.4f);
		//GameObject go = GameObject.Find ("txtWrongCard");
		//TextMesh m = go.GetComponent<TextMesh> ();
		//m.text = "";
		GameObject go = GameObject.Find ("uiTxtWrongCard");
		Text t = go.GetComponent<Text>();
		t.text = "";
		Destroy (gameObject, lifetime);
	}
}

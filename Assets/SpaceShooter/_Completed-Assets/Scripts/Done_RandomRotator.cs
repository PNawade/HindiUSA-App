using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Done_RandomRotator : MonoBehaviour 
{
	public float tumble;
	public Image imgFront;
	public Image imgBack;
	
	void Start ()
	{
		imgFront.GetComponent<showImage>().show (name);
		imgBack.GetComponent<showImage>().show (name);
		GetComponent<Rigidbody> ().angularVelocity = new Vector3 (1f,1f,0.2f) ;//Random.insideUnitSphere * tumble;
	}
}  
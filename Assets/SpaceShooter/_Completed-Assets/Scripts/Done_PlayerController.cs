﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using CnControls;

[System.Serializable]
public class Done_Boundary
{
	public float xMin, xMax, zMin, zMax;

}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;

	private Vector2 dir;
	public GameObject t;


	void Update ()
	{
		if (CnInputManager.GetButton ("Jump") && Time.time > nextFire) {		//if (Input.GetButton("Fire1") && Time.time > nextFire) 
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource> ().Play ();
		}
	}

	void FixedUpdate ()
	{
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");

//		Vector3 a = Input.acceleration;
			
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

//		Vector3 movement = new Vector3 (a.x, 0.0f, a.y);
//		dir = t.GetComponent<move>().getDir();

//		Vector3 movement = new Vector3 (dir.x, 0.0f, dir.y);
		Vector3 movement = new Vector3 (CnInputManager.GetAxis ("Horizontal"), 0f, CnInputManager.GetAxis ("Vertical"));

		boundary.xMin = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 9)).x;
		boundary.xMax = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 9)).x;
		boundary.zMax = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.65f, 9.0f)).z;
		boundary.zMin = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.0f, 9.0f)).z;

		GetComponent<Rigidbody> ().velocity = movement * speed;

		GetComponent<Rigidbody> ().position = new Vector3 (
			Mathf.Clamp (GetComponent<Rigidbody> ().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody> ().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody> ().velocity.x * -tilt);
	}
}

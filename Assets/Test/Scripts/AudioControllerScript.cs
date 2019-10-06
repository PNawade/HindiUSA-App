using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class AudioControllerScript : MonoBehaviour
{
	public AudioClip wrongAnswer;
	public AudioClip rightAnswer;

	public void playClapSound ()
	{
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		audio.PlayOneShot (rightAnswer);
	}

	public void playWrongSound ()
	{
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		audio.PlayOneShot (wrongAnswer);
	}
}

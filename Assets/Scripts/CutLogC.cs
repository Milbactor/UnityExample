using UnityEngine;
using System.Collections;

public class CutLogC : MonoBehaviour {


	public AudioClip WaterSound;
	private bool alreadyPlayed = false;
	Vector3  vec; 
	// Use this for initialization
	void Start () {

		if (Random.value > .5) {
			vec = Vector3.forward;
		}
		else {
			vec = Vector3.back;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(
			vec );
		if(transform.position.y < -24 && alreadyPlayed == false)
		{
			playWaterSound();
			alreadyPlayed = true;
			Destroy(this.gameObject, WaterSound.length);
		}
	}

	
	void playWaterSound()
	{
		if( WaterSound != null  && audio.isPlaying == false)
		{
			audio.PlayOneShot(WaterSound);
		}
		
	}
}

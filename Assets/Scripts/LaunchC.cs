using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LaunchC : MonoBehaviour {

	LiteTimer timer;
	float maxTimer = 0.5f;
	float speed = 50f;

	// Use this for initialization
	void Start () {

		timer = new LiteTimer(maxTimer );
		timer.onElapsed += timerElapsed;
	
	}

	void timerElapsed( LiteTimer timer)
	{	
		collider2D.isTrigger = false;
		timer.stop();
		GetComponent<StatusC>().OnSaveMoveBounds(this.gameObject);
	}

	
	// Update is called once per frame
	void Update () {
		timer.Update();

		if( timer.playing)
		{
			rigidbody2D.velocity = new Vector2(0,speed);
	
		}
	
	}

	public void LaunchStart ()
	{
		timer.start();
	}
}

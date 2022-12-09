using UnityEngine;
using System.Collections;
using AssemblyCSharp; 

public class BambooPlatformC : MonoBehaviour {

	bool moving = false;
	LiteTimer movingTimer;
	float movingTime = 3.5f;

	// Use this for initialization
	void Start () {
		movingTimer = new LiteTimer(movingTime);
		movingTimer.onElapsed += timeElapsed;
	}
	
	// Update is called once per frame
	void Update () {
		movingTimer.Update();
	}

	
	void timeElapsed( LiteTimer timer)
	{	
		moving = false;
		movingTimer.stop();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player" && moving == false)
		{
			GetComponent<Animator>().SetTrigger("Move");
			moving = true;
			movingTimer.start();

		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		print ("adsa");

	}
}

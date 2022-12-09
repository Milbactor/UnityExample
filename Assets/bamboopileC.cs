using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class bamboopileC : MonoBehaviour {

	LiteTimer pushTimer;
	float pushTime = 20f;
	float speed = 0.005f;

	// Use this for initialization
	void Start ()
	{
		pushTimer = new LiteTimer(pushTime);
		pushTimer.onElapsed += HandleonElapsed;
		pushTimer.start();
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("bambooPath"), "time", 20));
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		pushTimer.stop();
		//collider2D.isTrigger = false;
		
	}
	// Update is called once per frame
	void Update () {
		pushTimer.Update();

	if(pushTimer.playing && transform.localScale.x < 1){
			transform.localScale = new Vector3
				(transform.localScale.x + speed, 
				 transform.localScale.y + speed, 0);
		}
	
	}
}

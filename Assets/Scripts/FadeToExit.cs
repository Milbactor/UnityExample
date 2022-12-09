using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FadeToExit : MonoBehaviour {

	LiteTimer countDownTimer;
	LiteTimer fadeOutTimer;
	// Use this for initialization
	void Start () {
		
		fadeOutTimer = new LiteTimer(4f);
		countDownTimer = new LiteTimer(10f);
		countDownTimer.start();
		countDownTimer.onElapsed += startFade;
	}

	void startFade ( LiteTimer timer)
	{
		fadeOutTimer.start();
		
		fadeOutTimer.onElapsed += OnFadeOutcomplete;
	}

	void OnFadeOutcomplete ( LiteTimer timer)
	{
		Application.LoadLevel("score");
	}
	
	// Update is called once per frame
	void Update () {
		
		countDownTimer.Update();
		fadeOutTimer.Update();
		
		if(fadeOutTimer.playing) GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, fadeOutTimer.time / fadeOutTimer.duration);
	}
}

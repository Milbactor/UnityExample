using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class fogc : MonoBehaviour {

	LiteTimer appearTimer;
	LiteTimer durationTimer;
	public float appearTime;
	public float durationTime;
	// Use this for initialization
	void Start () {
		appearTimer = new LiteTimer (appearTime);
		durationTimer = new LiteTimer (durationTime);

		durationTimer.onElapsed += durationElapsed;
		appearTimer.onElapsed += HandleonElapsed;
		particleSystem.enableEmission = false;
		appearTimer.start();
	}

	void durationElapsed (LiteTimer timer)
	{
		appearTimer.start ();
		particleSystem.enableEmission = false ;
	}

	void HandleonElapsed(LiteTimer timer)
	{
		durationTimer.start ();
		particleSystem.enableEmission = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		appearTimer.Update();
		durationTimer.Update();
	}
}

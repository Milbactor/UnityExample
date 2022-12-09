using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LavaBurstC : GeekBehaviour {

	public LiteTimer durationTimer;
	public float burstTime = 1f;
	public GameObject cap;
	// Use this for initialization
	void Start () {
	
		base.Start();
		
		durationTimer = new LiteTimer( burstTime );
		//durationTimer.onElapsed += HandleonElapsed;
	}

	public void HandleonElapsed ( LiteTimer timer)
	{
		particleSystem.Stop();
		cap.rigidbody2D.gravityScale = 1f;
	}
	
	public void Activate( float duration )
	{
		durationTimer.duration = burstTime;
		durationTimer.start();
		particleSystem.Play();
		
		/*iTween.MoveTo( cap, iTween.Hash(
			"position", cap.transform.position + new Vector3(0, 20 ),
			"time", 2f 
		));*/
		
	}
	
	void FixedUpdate()
	{
		if( durationTimer.playing )
		{
			cap.rigidbody2D.velocity = new Vector2(0f, 20f );
		}
		else if( particleSystem.isPlaying )
		{
			cap.rigidbody2D.velocity = Vector2.zero;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		durationTimer.Update();
	}
}

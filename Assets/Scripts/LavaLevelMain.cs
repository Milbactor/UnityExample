using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LavaLevelMain : GeekLevel {

	public LavaBurstC[] bursts;
	public int maxActiveBursts = 1;
	public float minBurstDuration = 1f;
	public float maxBurstDuration = 1f;
	
	public float minBurstPauseDuration = 3f;
	public float maxBurstPauseDuration = 6f;
	
	private LiteTimer[] burstTimers; 
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		burstTimers = new LiteTimer[ bursts.Length ];
		for( int i = 0; i < burstTimers.Length; i++)
		{
			burstTimers[i] = new LiteTimer( Random.Range(minBurstPauseDuration, maxBurstPauseDuration) );
			burstTimers[i].onElapsed += HandleonElapsed;
			//burstTimers[i].start();
		}
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		int burstID = 0;
		foreach( LiteTimer burstTimer in burstTimers){
			if( burstTimer == timer ) break;
			burstID++;
		}
		
		//start pause duration
		if( bursts[burstID].particleSystem.isPlaying)
		{
			timer.duration = Random.Range(minBurstPauseDuration, maxBurstPauseDuration);
			timer.start();
			bursts[burstID].HandleonElapsed(null);
		}
		else
		{ //start bursting
		
			timer.duration = Random.Range( minBurstDuration, maxBurstDuration );
			timer.start();
			bursts[burstID].Activate(0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		
		foreach(LiteTimer burstTimer in burstTimers)
		{
			burstTimer.Update();
		}
	}
}

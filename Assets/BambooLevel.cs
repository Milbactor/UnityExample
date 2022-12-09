using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BambooLevel : GeekLevel {

	int curRaft = 0;
	public raftC2[] rafts;
	LiteTimer[] timers;
	
	public float raftRespawn = 5f;
	// Use this for initialization
	void Start () {
		
		base.Start();
		
		print ("bamboolevel Start");
		timers = new LiteTimer[ rafts.Length];
		
		for(int i = 0; i < rafts.Length; i++)
		{
			raftC2 raft = rafts[i];
			
			timers[i] = new LiteTimer( raftRespawn );
			timers[i].onElapsed += RespawnRaft;

			//addMessageListener( args => raftDissapeared( (raftC) args[0]), raftC2.M_RAFT_DISSAPEAR );
			//raft.gameObject.SetActive( false );
		}

	}

	void raftDissapeared (raftC raftC)
	{
	
	}

	void RespawnRaft (LiteTimer timer)
	{
		//rafts[ getTimerID(timer) ].startPath();
		rafts[ getTimerID(timer) ].appear();
	}

	// Update is called once per frame
	void Update () {
		
		base.Update();
		
		foreach( LiteTimer t in timers )
		{
			t.Update();
		}
	}
	
	int getRaftID( raftC raft)
	{
		for(int i = 0; i < rafts.Length; i++)
		{
			if( rafts[ i ] == raft ) return i;
		}
		
		return -1;
	}
	
	int getTimerID( LiteTimer timer)
	{
		for(int i = 0; i < timers.Length; i++)
		{
			if( timers[ i ] == timer ) return i;
		}
		
		return -1;
	}

}

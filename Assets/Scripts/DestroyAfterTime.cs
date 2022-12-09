using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DestroyAfterTime : GeekBehaviour {

	public static string M_ELAPSED = "M_ELAPSED";
	public float time;
	private LiteTimer timer;
	// Use this for initialization
	void Start () {
	
		base.Start();
		timer = new LiteTimer( time );
		timer.onElapsed += HandleonElapsed;
		timer.start();
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		dispatchMessage(M_ELAPSED);
		GameObject.Destroy( this.gameObject );
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		timer.Update();
	}
}

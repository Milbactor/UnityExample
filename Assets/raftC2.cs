using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class raftC2 : GeekBehaviour {

	float sinTimer = 0;
	public float sinkDuration = 5f;
	LiteTimer sinkTimer;
	public static string M_RAFT_DISSAPEAR = "RAFT_DISSAPEAR";
	// Use this for initialization
	void Start () {
	
		base.Start();
		particleSystem.Play();

		sinkTimer = new LiteTimer( sinkDuration );
		sinkTimer.onElapsed += HandleonElapsed;
	}

	void HandleonElapsed (LiteTimer timer)
	{
		appear();

	}
	
	// Update is called once per frame
	void Update () {
	
		base.Update();
		transform.position += new Vector3(Mathf.Sin( sinTimer ) / 200  ,  0);
		sinTimer += Time.deltaTime * 2;

		sinkTimer.Update();

		if(sinkTimer.playing) print ("sinkTimer: " + sinkTimer.time );
	}

	void OnCollisionExit2D( Collision2D collision )
	{	
		print ("raft collision: " + collision.gameObject.name );

		dissapear();
		//dispatchMessage( M_RAFT_DISSAPEAR, this );
		//collider2D.enabled = false;
	}

	void dissapear()
	{
		sinkTimer.start();
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0 );
		particleSystem.Stop();

		GameObject splash = GameObject.Instantiate( Resources.Load( "Splash") ) as GameObject;
		splash.transform.position = tf.position;
	}

	public void appear()
	{
		print (this.name +  "appear" );
		gameObject.SetActive( true );

		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 );
		particleSystem.Play();

		GameObject splash = GameObject.Instantiate( Resources.Load( "Splash")) as GameObject;
		splash.transform.position = tf.position;
	}
}

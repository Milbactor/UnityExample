using UnityEngine;
using System.Collections;

public class raftC : GeekBehaviour {

	public static string M_RAFT_BEGINPATH_COMPLETE = "RAFT_BEGINPATH_COMPLETE";
	public static string M_RAFT_ENDPATH_COMPLETE = "RAFT_ENDPATH_COMPLETE";

	public string enterPath;
	public string exitPath;
	public float time;
	
	float sinTimer = 0;
	bool complete = false;
	// Use this for initialization
	void Start () {
			                                       
		base.Start();
		
		print ("raft start");
	}
	
	public void startPath()
	{
		Hashtable tweenParams = new Hashtable();
		tweenParams.Add("from", GetComponent<SpriteRenderer>().color);
		tweenParams.Add("to", Color.white);
		tweenParams.Add("time", 0.5f);
		tweenParams.Add("onupdate", "OnColorUpdated");
		iTween.ValueTo(gameObject, tweenParams);
		
		iTween.MoveTo( gameObject, iTween.Hash("path", iTweenPath.GetPath(enterPath), "time", time, "easetype", iTween.EaseType.linear, 
		                                       "oncomplete", "startPathComplete"));
		                                       
		particleSystem.Play();

		gameObject.SetActive( true );
	}
	
	private void OnColorUpdated(Color color)
	{
		GetComponent<SpriteRenderer>().color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
		base.Update();
		
		transform.position += new Vector3(Mathf.Sin( sinTimer ) / 200  ,  0);
		sinTimer += Time.deltaTime * 2;
	}
	
	void startPathComplete()
	{
		dispatchMessage( M_RAFT_BEGINPATH_COMPLETE, this );
		complete = true;
	}
	
	void exitPathComplete()
	{
		dispatchMessage( M_RAFT_ENDPATH_COMPLETE, this );
		GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0 );
		
		particleSystem.Stop();
	}
	
	void OnCollisionExit2D( Collision2D collision )
	{	
		if( complete == false ) return;
		print ("raft collision: " + collision.gameObject.name );
		iTween.MoveTo( gameObject, iTween.Hash("path", iTweenPath.GetPath(exitPath), "time", time,"easetype", iTween.EaseType.linear,
		                                       "oncomplete", "exitPathComplete"));

		complete = true;
		//collider2D.enabled = false;
	}
}

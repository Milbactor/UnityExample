using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LavaC : MonoBehaviour {


	public float risingSpeed;
	bool isRising = false;
	bool isDecenting = false;
	public float pauseTime = 10f; 
	LiteTimer pauseTimer;
	public GameObject stopPosition;
	float originalPosition;
	
	public GameObject splashPrefab;



	// Use this for initialization
	void Start () {
		originalPosition = transform.position.y;
		pauseTimer = new LiteTimer(pauseTime);
		pauseTimer.onElapsed += PauseElapsed;
		pauseTimer.start ();
	}

	
	void PauseElapsed( LiteTimer timer)
	{	
		pauseTimer.stop ();
		isRising = true;
		
		iTween.ShakePosition (Camera.main.gameObject, new Hashtable{
			{ "time", 12 },
			{"amount", new Vector3( 0.3f, 0.3f ) }
		});
	}
	// Update is called once per frame
	void Update () {

		pauseTimer.Update();

		if( isRising == true)
		{
			transform.position = new Vector3(
				transform.position.x, transform.position.y + risingSpeed * Time.deltaTime, 0);
				
		}
		if( transform.position.y >= stopPosition.transform.position.y)
		{
			isRising = false;
			isDecenting = true;
		}

		if( isDecenting == true)
		{
			transform.position = new Vector3(
				transform.position.x, transform.position.y - risingSpeed * Time.deltaTime, 0);
		}

		if(isDecenting == true && transform.position.y <= originalPosition)
		{
			isDecenting = false;
			pauseTimer.start();
		}
	
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag != "Player" || collider.gameObject.GetComponent<FellStatusEffect>() != null) return;
		
		print ("hit lava: " + collider.gameObject.name );
		collider.gameObject.GetComponent<MessageDispatcher>().dispatchMessage(StatusC.M_FELL_WATER);
		
		GameObject splash = Instantiate( splashPrefab ) as GameObject ;
		splash.transform.position = collider.transform.position;
	}
}

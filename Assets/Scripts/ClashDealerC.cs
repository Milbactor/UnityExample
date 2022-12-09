using UnityEngine;
using System.Collections;

public class ClashDealerC : GeekBehaviour  {

	public static string M_CLASH_RECEIVED = "M_CLASH_RECEIVED";
	public static string M_CLASH_FINISHED = "M_CLASH_FINISHED";
	public GameObject player1;
	public GameObject player2;
	public bool clash = false;//private
	public AudioClip clashSound = null;


	// Use this for initialization
	void Start ()
	{
		base.Start();

		if(GameManager.instance.loadPlayersFromCharacterSelection == true)
		{
			if( GameManager.instance.players[0].character != null)
			{
				player1 = GameManager.instance.players[0].character;
				
			}
			if( GameManager.instance.players[1].character != null)
			{
				player2 = GameManager.instance.players[1].character;
				
			}
		}
		/*else
		{
			if( GameManager.instance.defaultHero != null)
			{
				player1 = GameManager.instance.defaultHero;
				
			}
			if( GameManager.instance.defaultVillain != null)
			{
				player2 = GameManager.instance.defaultVillain;
			}
		}*/	
		addMessageListener( (arguments) => OnClashReceived(), M_CLASH_RECEIVED );
		addMessageListener( (arguments) => OnClashFinished(), M_CLASH_FINISHED );
	}

	// Update is called once per frame
	void Update () {
	
	}

	//recieves from slashHitbox 
	public void OnClashReceived ()
	{
		//clash flag is to make sure the function calling only one time
		if(clash == false)
		{
			clash = true;
			FacingAdjust();
			player1.GetComponent<GeekBehaviour>().dispatchMessage(ClashC.M_CLASH_RECEIVED, player2);
			player2.GetComponent<GeekBehaviour>().dispatchMessage(ClashC.M_CLASH_RECEIVED, player1);
			playClashSound();

		}


	}

	void FacingAdjust ()
	{
		player1.rigidbody2D.velocity = new Vector2(0,0);
		player2.rigidbody2D.velocity =  new Vector2(0,0);

		GameObject leftPlayer = player1;
		GameObject rightPlayer = player2;
		if( rightPlayer.transform.position.x <  leftPlayer.transform.position.x ){
			leftPlayer = player2;
			rightPlayer =  player1;
		}

		leftPlayer.transform.localScale = new Vector3(1,1,1);
		rightPlayer.transform.localScale = new Vector3(-1,1,1);

		Vector3 distance = rightPlayer.transform.position - leftPlayer.transform.position;
		print (distance);

		leftPlayer.transform.position += ( distance / 4f ) ;
		rightPlayer.transform.position -= ( distance / 4f ) ;
	}

	//called from each player in the ending of clash
	public void OnClashFinished ()
	{
		clash = false;
	}
	
	public void playClashSound ()
	{
		if( clashSound != null )
		{
			audio.clip = clashSound;
			audio.Play();
		}
	}
}

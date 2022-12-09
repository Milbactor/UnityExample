
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

[RequireComponent( typeof(InventoryC) ) ]
public class SuperMoveC : GeekBehaviour {

	public static string M_USE_SUPERMOVE = "M_USE_SUPERMOVE";
	public static string M_PLAY_SOUND = "M_PLAY_SOUND";
	
	public AudioClip superMoveSound = null;
	public AudioClip superMoveCatSound = null;
	bool showBanner = false;
	public bool UsingSuperMove = false;
	
	InventoryC inventory;
	LiteTimer bannerTimer;
	
	GameObject banner;
	GameObject trail;

	private int moveingDirectionX = 1;
	
	LiteTimer exitLevelTimer;
	LiteTimer exitLevelDelay;
	Vector3[] exitLevelPath; 
	// Use this for initialization
	override protected void Start () {
	
		base.Start();
		
		inventory = GetComponent<InventoryC>();
		bannerTimer = new LiteTimer( 0.5f );
		bannerTimer.onElapsed += OnBannerTimerElapsed;
		
		exitLevelTimer = new LiteTimer( 1f );
		exitLevelDelay = new LiteTimer( 1f );
		//exitLevelDelay.onElapsed += HandleOnDelayFinished;
		
		addMessageListener( args => onUseSuperMove(), M_USE_SUPERMOVE );
		addMessageListener( args => PlaySuperMoveCatSound (), M_PLAY_SOUND );
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE);
	}

	void HandleOnDelayFinished ()
	{
		StartExitLevelAnimation();
	}

	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(curHash == Animator.StringToHash("SuperMove.SuperMoveFlying") && showBanner)
		{
			OnBannerTimerElapsed( null );
			//bannerTimer.start();
		}
		else if ( prevHash == Animator.StringToHash("JumpState.BackFlip" ) && exitLevelPath == null)
		{
			StartExitLevelAnimation();
		} 
	}

	void StartExitLevelAnimation ()
	{
		int side = GameManager.instance.getPlayerByCharacter( gameObject).side * -1;
		Vector3 destPos = new Vector2( ( LevelBounds.instance.bounds.center.x + (LevelBounds.instance.bounds.width / 2)  ) * side, tf.position.y );
		
		//dispatchMessage(JumpC.M_JUMP_IMPULSE );
		Vector3 distance = destPos - transform.position;//Vector3.Distance( opponent.transform.position, transform.position ); 
		float time = Mathf.Abs(distance.x) * 0.025f;
		
		int direction = -1;
		if( distance.x > 0 ) direction = 1;
		
		/*iTween.MoveTo( this.gameObject, new Hashtable {
			{ "ignoretimescale", true },
			{ "position", destPos },
			{ "time", time},
			{ "oncomplete", "MoveToNextLevel" },
			{ "oncompleteparams", direction },
			{ "easetype", iTween.EaseType.linear }
		} ); */
		
		exitLevelPath = new Vector3[ 10 ];
		for(int i = 0; i < 10; i++)
		{
			exitLevelPath[i] = new Vector3( transform.position.x + ( distance.x * (0.06f * i ) ), 
			                               transform.position.y + ( Mathf.Sin( i / (Mathf.Abs( distance.x / 12.5f )) ) * 10 ) ); 
		}
		
		exitLevelTimer.start();

		rb2D.velocity = new Vector2(0, -1);
		anim.SetBool(JumpC.M_JUMP, true ); //somehow doesn't trigger jump
	}
	
	void OnDrawGizmos()
	{
		if(exitLevelPath != null )
		{
			iTween.DrawPath( exitLevelPath, Color.blue );
		}
	}

	void MoveToNextLevel( int side )
	{
		GameManager.instance.LoadNextLevel( side );
		//dispatchMessage(Portalc.M_ON_LEVEL_BOUNDARIES_PASSED, gameObject, new Vector2(1, 0), true );
	}
	void OnBannerTimerElapsed ( LiteTimer timer)
	{
		showBanner = false;
		GameObject.Destroy( banner );
		
		trail = GameObject.Instantiate( Resources.Load("SuperMoveTrail") ) as GameObject;
		TrackC track = trail.AddComponent<TrackC>();
		track.focusObj = gameObject;
		
		anim.SetTrigger(AnimatorConstants.SUPERMOVE_FLY );
		List<GameObject> opponents = GameManager.instance.GetOtherPlayers( this.gameObject );
		
		for(int i = 0; i < opponents.Count; i++)
		{
			GameObject opponent = opponents[i];
			Vector3 distance = opponent.transform.position - transform.position;//Vector3.Distance( opponent.transform.position, transform.position ); 
			GeekBehaviour d = opponent.GetComponent<GeekBehaviour>();
			//print ("messageDispatcher: " + d );
			
			float time = Mathf.Abs(distance.x) * 0.025f;
			
			//print ("time: " + time );

			if( distance.x < 0 ) moveingDirectionX = -1;
			Time.timeScale = 1f;
			
			iTween.MoveTo( this.gameObject, new Hashtable {
				{ "ignoretimescale", true },
				{ "position", opponent.transform.position + (new Vector3(1, 0) * (moveingDirectionX * 2) ) },
				{ "time", time},
				{ "oncomplete", "superMoveComplete" },
				{ "oncompleteparams", opponent },
				{ "easetype", iTween.EaseType.linear }
			} );;
			
			
		}
	}

	void onUseSuperMove ()
	{
		if( inventory.getItemCount( typeof(ScrollC) ) < 3 ) return;
		UsingSuperMove = true;
		PlaySuperMoveSound ();
		//TODO HARDCODED FOR 1 OPPONENT
		anim.SetTrigger( AnimatorConstants.SUPERMOVE_START );
		showBanner = true;
		
		GeekTools.Stun( gameObject );
		
		foreach( GameObject opponent in GameManager.instance.GetOtherPlayers( this.gameObject ) )
		{
			GeekTools.Stun( opponent );
		}

		print ("loading banner: Samudai_Banner_" + gameObject.name + " _Prefab");		
		//Time.timeScale = 0; //animations are not played with timescale 0 :(
		dispatchMessage( PlayerC.M_INPUT_DISABLE );
		collider2D.enabled = false;

		foreach( Player player in  GameManager.instance.GetOtherPlayersObjs( gameObject ) ) 
		{
			player.character.GetComponent<MessageDispatcher>().dispatchMessage( PlayerC.M_INPUT_DISABLE);
		}

	}
	
	void OnGUI()
	{
		Texture2D texture = Resources.Load("Samudai_Banner_" + gameObject.name ) as Texture2D ;
		Camera cam = Camera.main;
		int side = GameManager.instance.getPlayerByCharacter( gameObject).side;

		float x = 0;
		if( side > 0 ) x = cam.pixelWidth - texture.width / 2;

		if( showBanner == true ) GUI.DrawTexture( new Rect(x, (cam.pixelHeight / 2) - 279, texture.width/2, texture.height/2 ),//new Rect(0, cam.pixelHeight / 2, cam.pixelWidth, cam.pixelHeight / 2 ), 
		texture); 
	}
	
	void superMoveComplete( GameObject opponent)
	{

		Time.timeScale = 1f;
		dispatchMessage( PlayerC.M_INPUT_ENABLE );

		GameObject.Destroy( trail );
		
		anim.SetTrigger( AnimatorConstants.SUPERMOVE_ATTACK );
		opponent.GetComponent<MessageDispatcher>().dispatchMessage(SuperMoveC.M_PLAY_SOUND);
		
		//DestroyImmediate( GetComponent<StunC>() );
		GetComponent<GeekPhysicsC>().gravity = 0.0f;
		dispatchMessage(PlayerC.M_INPUT_DISABLE );
		collider2D.isTrigger = true;

		Destroy( opponent.GetComponent<StunC>() );
		
		opponent.layer = LayerMask.NameToLayer("ignoreEnvironment");
		//this allows sound of normal damage to play... 
		opponent.GetComponent<MessageDispatcher>().dispatchMessage( DamageC.M_DAMAGE_RECEIVED,
		           this.gameObject, 
		           new Vector2(moveingDirectionX * 10, 0 ),
		           0,
		           "SuperMove" 
		                                                           );
		opponent.GetComponent<MessageDispatcher>().addMessageListener( args => Ringout( (GameObject) args[0], (Vector2) args[1] ), Portalc.M_ON_LEVEL_BOUNDARIES_PASSED );
		opponent.GetComponent<MessageDispatcher>().SendMessage( PlayerC.M_INPUT_DISABLE );
	}

	void Ringout ( GameObject obj, Vector2 direction)
	{
		obj.layer = LayerMask.NameToLayer("players");

	}
	
	// Update is called once per frame
	override protected void Update () {
	
		base.Update();

		bannerTimer.Update();
		exitLevelTimer.Update();
		exitLevelDelay.Update();
		
		if(exitLevelPath != null && exitLevelTimer.playing)
		{
			float percent = (exitLevelTimer.time / exitLevelTimer.duration);
			iTween.PutOnPath( this.gameObject, exitLevelPath, percent );
			
			//print ("put on path %" + percent);
			if( percent < 0.3f)  anim.SetBool(JumpC.M_JUMP, true );
			
			if(percent >= 0.975f) //BUG PRONE
			{
				MoveToNextLevel( moveingDirectionX );
			}
		}
	}


	void PlaySuperMoveSound ()
	{
		if( superMoveSound != null )
		{
			audio.PlayOneShot(superMoveSound);
		}
	}

	void PlaySuperMoveCatSound ()
	{
		if( superMoveCatSound != null )
		{
			print ("ae");
			audio.PlayOneShot(superMoveCatSound);
		}
	}


}

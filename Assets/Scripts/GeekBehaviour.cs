using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeekBehaviour : MonoBehaviour {

	public static string M_ENTER_ANIMATION_STATE = "M_ENTER_ANIMATION_STATE";
	protected MessageDispatcher dispatcher = null;
	protected Animator anim;
	public int animationHash = 0;
	
	protected Rigidbody2D rb2D;
	protected Transform tf;

	public List<int> illegalStates = new List<int>();

	// Use this for initialization
	protected virtual void Start () {
	
		dispatcher = gameObject.GetComponent<MessageDispatcher>();
		if( dispatcher == null )
		{
			gameObject.AddComponent<MessageDispatcher>();
			//print ( gameObject.name + " adds messageDispatcher");
		}

		anim = GetComponent<Animator> ();
		dispatcher = gameObject.GetComponent<MessageDispatcher>();
		
		rb2D = rigidbody2D;
		tf = transform;
	}

	protected bool InIllegalState()
	{
		foreach( int illegalState in illegalStates )
		{
			if( illegalState == animationHash )
				return true;
		}

		return false;
	}
	
	protected virtual void Awake()
	{
		dispatcher = gameObject.GetComponent<MessageDispatcher>();
		if( dispatcher == null )
		{
			gameObject.AddComponent<MessageDispatcher>();
			//print ( gameObject.name + " adds messageDispatcher");
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		//dispatcher.Update();
		if( anim == null ) return;
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		
		if ( stateInfo.nameHash != animationHash){
			
			dispatchMessage( M_ENTER_ANIMATION_STATE, animationHash, stateInfo.nameHash );
			animationHash = stateInfo.nameHash;
		}

		if( InIllegalState()) return;
	}

	public void addMessageListener( MessageDispatcher.Callback callback, string message)
	{
		//print ( gameObject.name + " adds MessageListener : " + message);
		dispatcher.addMessageListener( callback, message );
		/*
		print ("current listeners after add: " );
		
		foreach(string key in dispatcher.listeners.Keys)
		{
			print ("[ " + key + " ]");
		}
		
		print ("--------------"); */
	}

	public void dispatchMessage( string message, params object[] arguments)
	{
		if( message == "M_TOGGLE_MOVEMENT") {
			int breakpoint = 0;
		}
		if( enabled ) dispatcher.dispatchMessage( message, arguments );
	}

	public void AudioPlay(AudioClip clip)
	{
		AudioSource[] sounds;
		sounds = GetComponents<AudioSource>();
		foreach(AudioSource audio in sounds )
		{
			if( audio.isPlaying == false ){
				audio.clip = clip;
				audio.Play();
				return;
			}

		}

		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.Play();

	}

}

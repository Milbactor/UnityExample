using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ThrowC : GeekBehaviour {
	
	public static string M_THROW = "TriggerThrow";
	public static string M_THROW_UP = "TriggerThrowUp";
	public static string M_THROW_DISABLE = "ThrowDisable";
	public static string M_THROW_ENABLE = "ThrowEnable";
	public static string M_THROW_SUCCESS = "M_THROW_SUCCESS";
	private bool enabledThrow = false; 

	protected Quaternion qRight;
	protected Quaternion qLeft;
	private Quaternion qUp;
	public GameObject ThrowHitBoxObject;
	public float offsetY = 0.0f;
	public AudioClip throwSound = null;

	GameObject spawnPos;
	GameObject spawnPosUp;

	// Use this for initialization
	 protected virtual void Start () {
		base.Start ();
		addMessageListener((arguments) => Throw(), M_THROW);
		addMessageListener((arguments) => ThrowUp(), M_THROW_UP);
		addMessageListener((arguments) => ThrowDisable(), M_THROW_DISABLE);
		addMessageListener((arguments) => ThrowEnable(), M_THROW_ENABLE);

		illegalStates.Add(Animator.StringToHash( "Life related.StayDeathFall") );
		//illegalStates.Add(Animator.StringToHash( "Life related.Win") );
		qRight = Quaternion.Euler(0, 180, 0);
		qLeft = Quaternion.Euler(0, 0, 0);
		qUp = Quaternion.Euler(0, 0, 270);
		spawnPos = gameObject.transform.FindChild("ThrowPosition").transform.gameObject;
		spawnPosUp =gameObject.transform.FindChild("ThrowPositionUp").transform.gameObject;

	}
	
	
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();
	}

	void Throw()
	{
		/*if( InIllegalState() ) {

			print ("Illigal on");// illigal state is NotMonoBased working
			return;
		}*/

		print (enabledThrow);
		if(enabledThrow == true && !anim.GetBool(AnimatorConstants.DEAD ))
		{
			print("thow pressed");
			CreateThrowHitBoxObject () ;
			dispatchMessage( M_THROW_SUCCESS );
			anim.SetTrigger (M_THROW);
			playThrowSound();
		}
	


	}
	
	public void playThrowSound ()
	{
		if( throwSound != null )
		{
			audio.clip = throwSound;
			audio.Play();
		}
	}

	void ThrowUp()
	{
		//if( InIllegalState() ) return;
		if(enabledThrow == true && !anim.GetBool(AnimatorConstants.DEAD ))
		{
			CreateThrowHitBoxUpObject () ;
			anim.SetTrigger (M_THROW_UP);
			dispatchMessage( M_THROW_SUCCESS );
			playThrowSound();
		}
	}
	
	void ThrowDisable()
	{
		enabledThrow = false;
	}

	void ThrowEnable()
	{
		enabledThrow = true;
	}


	void CreateThrowHitBoxObject () 
	{
		GetComponent<GeekBehaviour>().dispatchMessage( AuraC.M_THROW_RECEIVED );

		if( GeekPhysicsC.getFacingDir( this.gameObject ).x >= 0) 
		{
			Vector3 spawnPosition  = spawnPos.transform.position;// new Vector3(transform.position.x + GetComponent<BoxCollider2D>().size.x + 1.0f, 
			                                   // transform.position.y + 1.0f, 0.0f);
			GameObject newThrowHitBoxObject = Instantiate(ThrowHitBoxObject, spawnPosition, qRight) as GameObject;

			newThrowHitBoxObject.GetComponent<ThrowMovementC>().throwDir = ThrowDirection.RIGHT;
			newThrowHitBoxObject.transform.parent = transform;
		}

		 else if( GeekPhysicsC.getFacingDir( this.gameObject ).x < 0) 
		{
			Vector3 spawnPosition  = new Vector3(transform.position.x - GetComponent<BoxCollider2D>().size.x - 1.0f, 
			                                     transform.position.y + 1.0f, 0.0f);
			GameObject newThrowHitBoxObject = Instantiate(ThrowHitBoxObject, spawnPosition, qLeft) as GameObject;
			
			newThrowHitBoxObject.GetComponent<ThrowMovementC>().throwDir = ThrowDirection.LEFT;
			newThrowHitBoxObject.transform.parent = transform;

		}
	

	}
	void CreateThrowHitBoxUpObject () 
	{
		GetComponent<GeekBehaviour>().dispatchMessage( AuraC.M_THROW_RECEIVED );
		Vector3 spawnPosition  = spawnPosUp.transform.position; //new Vector3(transform.position.x, 
		                                    //transform.position.y + GetComponent<BoxCollider2D>().size.y + offsetY , 0.0f);

		GameObject newThrowHitBoxObject = Instantiate(ThrowHitBoxObject, spawnPosition, qUp) as GameObject;
		
		newThrowHitBoxObject.GetComponent<ThrowMovementC>().throwDir = ThrowDirection.UP;
		newThrowHitBoxObject.transform.parent = transform;
	}



}

using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ClashC : GeekBehaviour {
	
	public static string M_CLASH_RECEIVED = "M_CLASH_RECEIVED";
	public static string M_CLASH_FRAME = "M_CLASH_FRAME";
	public Vector2 power;
	GameObject clashedObj;
	private bool clashFinished = false;	//temp soltion as ClashBackFlipMovement() is called many times 
	GameObject damageDealer = null;
	private GameObject clashHandler;
	public AudioClip clashSound = null;
	
	// Use this for initialization
	void Start () {
		base.Start();
		addMessageListener( (arguments) => OnClashReceived((GameObject)arguments[0]), M_CLASH_RECEIVED );
		clashHandler = GameObject.Find("CameraFocus");
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE );
	}

	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("Crash.Crash") && clashFinished == false)
		{
			ClashBackFlipMovement();
			InputDisabledStatus backFlip = gameObject.AddComponent<InputDisabledStatus>();
			backFlip.duration = 0.5f;
			clashFinished = true;
			clashHandler.GetComponent<ClashDealerC>().OnClashFinished();
		}
	}

	void ClashBackFlipMovement()
	{
		Vector2 distance = clashedObj.transform.position - transform.position;
		if(distance.x == 0)
		{
			distance.x = tf.localScale.x;
		}
		print (distance.x);
		//rigidbody2D.velocity -= distance.normalized * power.x;
		rb2D.velocity = new Vector2 (rb2D.velocity.x -distance.normalized.x * power.x,
		                                    rb2D.velocity.y + power.y );

	}


	public void playClashSound ()
	{
		if( clashSound != null )
		{
			audio.clip = clashSound;
			audio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();

	}
	


	void OnClashReceived ( GameObject clashedObj)
	{
		
		clashFinished = false;
		anim.SetBool(AnimatorConstants.DAMAGED, false);	
		anim.SetTrigger(AnimatorConstants.CLASH);
		this.clashedObj = clashedObj;
		ClashStatusEffect clash = gameObject.AddComponent<ClashStatusEffect>();
		clash.duration = 10f;
		
	}


	void OnKnockbackElapsed ()
	{

		//gameStatus.GetComponent<GeekBehaviour>().dispatchMessage(ClashDealerC.M_CLASH_FINISHED);

	}
}

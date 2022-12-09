using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SlashC : GeekBehaviour{

	public static string M_SLASH_EXIT = "SlashExit";
	public static string M_SLASH = "TriggerSlash";
	public static string M_SLASHED = "sLASHED";
	public static string M_SLASH_START = "M_SLASH_START";
	public static string M_CHARGED_SLASHED = "M_CHARGED_SLASHED";
	public static string M_SLASH_SUCCESS = "M_SLASH_SUCCESS";
	
	public AudioClip ChargeCompleteSFX = null;
	private float slashType = 0.0f;
	public bool slashing = false;
	
	public AudioClip[] slashSound = null;

	public AudioClip chargedSlashSound = null;

	bool charged = false;

	public LiteTimer chargeTimer;
	public float maxCharge = 1f;
	public float lastCharge = 0f;
	// Use this for initialization

	public GameObject chargeParticles;


	public bool chargeConfirmed  = false;
	bool wasGroundedIncharge = false;
	public Vector2 AerialChargeFriction = new Vector2(0.5f, 0.8f);
	
	void Start () {
		base.Start();
		
		chargeTimer = new LiteTimer( maxCharge );
		chargeTimer.onElapsed += ChargeElapsed;
		chargeTimer.resetOnFinish = false;
		chargeParticles.particleSystem.enableEmission = false;
		
		addMessageListener( (arguments) => SlashStart(), M_SLASH_START);
		addMessageListener( (arguments) => Slash(), M_SLASH);
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE);
		addMessageListener( args => OnGetGrounded(), ContactStateC.M_GET_GROUNDED );

		illegalStates.Add(Animator.StringToHash( "Life related.StayDeathFall") );
		//illegalStates.Add(Animator.StringToHash( "Life related.Win") );
	}

	void SlashStart ()
	{
		//print ("slashing: " + slashing );
		if (slashing == true ) return;
		chargeTimer.start();
		//chargeParticles.particleSystem.Play();

		//print ("slashStart");

		wasGroundedIncharge = false;
		anim.SetBool("SlashCharge", true);
		
	}

	void OnGetGrounded ()
	{
		wasGroundedIncharge = true;
	}

	void ChargeElapsed ( LiteTimer timer)
	{
		chargeParticles.particleSystem.enableEmission = false;
		wasGroundedIncharge = true; //when charge is full, stop slow falling
		audio.clip = ChargeCompleteSFX;
		audio.Play();
		print ("charge elapsed");
	}

	public void StopChargeParticle()
	{
		chargeTimer.stop();
		chargeParticles.particleSystem.enableEmission = false;
	}

	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(curHash == Animator.StringToHash("SlashState.SlashUp") )
		{
			rb2D.velocity += new Vector2(0, 0.8f );
			chargeTimer.stop();
			
		}
		else if ( curHash == Animator.StringToHash("SlashState.Slash" ) )
		{
			if( GeekInput.GetStickVector( GeekInput.LEFT, GetComponent<PlayerC>().ID).x > 0.5f )
			{
				rb2D.velocity += new Vector2(0.5f * GeekPhysicsC.getFacingDir(gameObject).x, 0 );
			}
			chargeTimer.stop();
		}
		else if( curHash == Animator.StringToHash("SlashState.SlashDown"))
		{
			chargeTimer.stop();
		}
		else{
			//slashing = false;
		}

		if(prevHash == Animator.StringToHash("SlashState.ChargedSlashStand") 
		   || 
		   prevHash == Animator.StringToHash("SlashState.ChargedSlashFloat")
		   ||
		   prevHash == Animator.StringToHash("SlashState.ChargedSlashRun")
		   )
		{
			slashing = false;
			chargeConfirmed = false;
			chargeTimer.stop();
		}
		else if( curHash == Animator.StringToHash("SlashState.ChargedSlashStand") 
		        || 
		        curHash == Animator.StringToHash("SlashState.ChargedSlashFloat")
		        ||
		        curHash == Animator.StringToHash("SlashState.ChargedSlashRun")
		)
		{
			slashing = true;
		}
		

		
	}
	
	
	// Update is called once per frame
	void Update () {

		base.Update();
		chargeTimer.Update();

		if(anim.GetBool(AnimatorConstants.DEAD) == true)
		{
			chargeParticles.particleSystem.enableEmission = false;
			chargeConfirmed = false;
		}
		
		if( chargeTimer.time > 0.2f )
		{
			chargeParticles.particleSystem.enableEmission = true;
		}
		
	}
	
	void FixedUpdate()
	{
		/*if( slashing == true && GetComponent<ContactStateC>().grounded == false)
		{
			rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f );
		}
		else*/ if ( chargeTimer.time > 0.2f && GetComponent<ContactStateC>().grounded == false && wasGroundedIncharge == false )
		{
			print ("chargeTime: " + chargeTimer.time + " | wasgroundedincharge " + wasGroundedIncharge ); 
			rb2D.velocity = new Vector2( rb2D.velocity.x * AerialChargeFriction.x, rb2D.velocity.y * AerialChargeFriction.y);
		}
	}


	void Slash()
	{

		if(GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("SlashState.Slash")
		   &&
		   GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("SlashState.SlashUp")
		   &&
		   GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("SlashState.SlashDown")
		   &&
		   anim.GetBool(AnimatorConstants.BLOCKED) == false
		   // While blocking slash soundshould not be used
		   &&
		   anim.GetBool(AnimatorConstants.DEAD) == false
		   && 
		   GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("ChargedSlashStand")
		   && 
		   GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("ChargedSlashRun")
		   && 
		   GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("ChargedSlashFloat")
		   )
		{
			lastCharge = chargeTimer.time;
			
			if(chargeTimer.time >= maxCharge)
			{
				anim.SetTrigger("TriggerChargedSlash");
				chargeConfirmed = true;
				playChargedSlashSound ();
				
				dispatchMessage( M_CHARGED_SLASHED );
			}
			else{
				anim.SetFloat (AnimatorConstants.SLASH_TYPE, slashType);
				playSlashSound ();
				anim.SetTrigger (M_SLASH);
				playSlashSound ();
				slashType += 0.5f;
				if(slashType > 1){
					slashType = 0.0f;
				}
			}
			anim.SetBool("SlashCharge", false);

		}

		
		chargeParticles.particleSystem.enableEmission = false;
		chargeTimer.stop();
		//chargeTimer.pause();


	}

	//you must call this in an anima
	public void playSlashSound ()
	{
		//print ("playSlashSound");
		if( slashSound[0] != null && slashType == 0f )
		{
			//audio.clip = slashSound[0];
			AudioPlay(slashSound[0]);
		}
		if( slashSound[1] != null && slashType == 0.5f )
		{
			//audio.clip = slashSound[1];
			//audio.Play();
			AudioPlay(slashSound[1]);
		}
		if( slashSound[2] != null && slashType == 1.0f)
		{
			//audio.clip = slashSound[2];
			//audio.Play();
			AudioPlay(slashSound[2]);
		}
	}

	public void playChargedSlashSound ()
	{
		if( chargedSlashSound != null)
		{
			audio.clip = chargedSlashSound;
			audio.Play();
		}
	}





}

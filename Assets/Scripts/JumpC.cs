using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class JumpC : GeekBehaviour {

	public static string M_JUMP_SUSTAIN = "JumpSustain";
	public static string M_JUMP_IMPULSE = "Jump";
	public static string M_JUMP_FINISH = "JumpFinish";
	public static string M_JUMP = "Jump";
	public static string M_DOUBLE_JUMP = "DoubleJump";

	public Vector2 acceleration = new Vector2( 1f, 1f );
	public float jumpImpluse = 2f;
	public float jumpSustain = 0.5f;

	public int jumpTimer = 0;
	public int maxJumpTimer = 10;
	public int maxDoubleJumpTimer = 5;
	
	public Vector2 wallJumpPower;
	public float maxJump = 20;
	
	public AudioClip flipSound = null;
	public AudioClip jumpSound = null;
	public int maxDoubleJump = 0;
	public int doubleJumpCount = 0;
	
	//it's okay to make a reference here, because this is so reliant on status C, not okay normally to reference components though!
	ContactStateC stateC;
	FacingDirectionC facingC;

	protected bool grounded
	{
		get{ return anim.GetBool(AnimatorConstants.GROUNDED) ; }
	}

	protected bool doubleJumped
	{
		get{ return anim.GetBool(AnimatorConstants.DOUBLE_JUMPED) ; }
	}

	
	// Use this for initialization
	void Start () {

		base.Start();

		addMessageListener( arguments => Jump () , M_JUMP );
		addMessageListener( arguments => JumpSustain () , M_JUMP_SUSTAIN );
		addMessageListener( arguments => JumpFinish () , M_JUMP_FINISH );
		addMessageListener( (arguments) => OnGrounded(), ContactStateC.M_GET_GROUNDED );
		
		stateC = GetComponent<ContactStateC>();
		facingC = GetComponent<FacingDirectionC>();


	}
	
	// Update is called once per frame
	void Update () {

		if(jumpTimer > 0 )	jumpTimer--;


		if(jumpTimer == 1) anim.SetBool (M_JUMP, false);

	}

	void OnGrounded ()
	{
		doubleJumpCount = 0;
	}

	void Jump()
	{
		if(anim.GetBool(AnimatorConstants.DEAD) == true) return; 
		//player is on air and double jump is already used, thrid times pressing jump button does not work
		if( grounded == false &&  doubleJumpCount >= maxDoubleJump && stateC.huggingWall == false ) return;

		//player is on air and not yet falling and double jump is not yet used then you can use double jump
		else if( stateC.huggingWall )
		{
			//Check this later -Nanna
			//rb2D.velocity += new Vector2( (facingC.facing.x *-1) * wallJumpPower.x, wallJumpPower.y );
			rb2D.velocity = new Vector2( rb2D.velocity.x + ( (facingC.facing.x *-1) * wallJumpPower.x), wallJumpPower.y );
			anim.SetBool (M_JUMP, true);
			
			print ("walljump");
			jumpTimer = maxJumpTimer;
			//print ("waljump with xPower: " + wallJumpPower + " facing: " + statusC.facing );
		}
		else if( grounded == false && doubleJumpCount < maxDoubleJump)// double jump shold not be allowed if the player is falling
		{
			anim.SetBool (M_DOUBLE_JUMP, true);
			jumpTimer = maxDoubleJumpTimer;
			rb2D.velocity = new Vector2( rb2D.velocity.x, jumpImpluse);
			doubleJumpCount++;
			//print("double jump " + doubleJumpCount);
		}
		// if plyaer is on ground you can start normal jump first 
		else if( grounded == true )
		{
			anim.SetBool (M_DOUBLE_JUMP, false);
			anim.SetBool (M_JUMP, true);
			jumpTimer = maxJumpTimer;
			rb2D.velocity += new Vector2(0, jumpImpluse);
		}

		if( rb2D.velocity.y > maxJump) {
			//rb2D.velocity = new Vector2( rb2D.velocity.x, maxJump );
		}
	}
	
	public void playFlipSound()
	{
		if( flipSound != null && audio.isPlaying == false )
		{
			audio.clip = flipSound;
			audio.Play();
		}
	}

	public void playJumpSound()
	{
		if( jumpSound != null && audio.isPlaying == false )
		{
			audio.clip = jumpSound;
			audio.Play();
		}
	}

	void JumpSustain()
	{
		if( grounded == true || jumpTimer == 0) return;
		//anim.SetBool (M_JUMP, true);
		rb2D.velocity += new Vector2(0, jumpSustain );
	}

	void JumpFinish(){

		anim.SetBool (M_JUMP, false);

	}
	


}

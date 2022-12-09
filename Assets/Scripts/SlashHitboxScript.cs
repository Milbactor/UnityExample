using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class SlashHitboxScript : Hitbox {
	
	private bool alreadyBlockCounted = false; //obsolete delete this
	private bool alreadySlashCounted = false; 
	private bool alreadyCut = false;

	public AudioClip CutSound;

	private GameObject clashDealer;

	LiteTimer blockHitCooldown;	// Use this for initialization
	override protected void Start () {
		
		base.Start();
		clashDealer = GameObject.Find("CameraFocus");
		onTimer += new OnTimeEvent( onHitboxTimeout);

		blockHitCooldown = new LiteTimer(1f );
		
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if(GetComponent<CircleCollider2D>().enabled == false)
		{
			if(alreadyBlockCounted)
				alreadyBlockCounted = false;
			
			if(alreadySlashCounted )
				alreadySlashCounted = false;

			if(alreadyCut)
				alreadyCut = false;
			
		}

		blockHitCooldown.Update();
	}

	void playCutSound ()
	{
		if( CutSound != null)
		{
			audio.PlayOneShot(CutSound);
		}
	}	
	
	void OnTriggerEnter2D( Collider2D collider )
	{
		
		GameObject target = collider.gameObject;

		if(target.tag == "Log" && alreadyCut == false)
		{
			playCutSound();
			target.GetComponent<GeekBehaviour>()
				.dispatchMessage(LogThrowCutC.M_LOG_THROW_RECEIVED,
				                 target.transform.position);
			alreadyCut = true;
			Destroy(target);
		}
		
		if (target.name.Equals( "BlockHitbox" ) && owner.GetComponent<StunC>() == null) 
		{
			//block is used and the block is against the opponent direction 
			owner.GetComponent<MessageDispatcher>().dispatchMessage( ParryC.M_GOT_PARRIED );
			target.transform.parent.GetComponent<MessageDispatcher>().dispatchMessage( ParryC.M_PARRY_SUCCESFUL );
		}
		else if(/* target.tag == "Player" */ target.GetComponent<DamageC>() != null && target != owner && alreadySlashCounted == false)
		{
			if(
				(target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.Slash" ) ||
			    target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.Slash2" ) ||
			   	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.Slash3" ) || 
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashUp" ) || 
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashDown" ) || 
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.ChargedSlashRun" ) || 
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashChargeUp" ) ||
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.ChargedSlashStand" ) ||
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.ChargedSlashFloat" ) ||
			 	target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashChargeDown" )
			 ) &&
				target.GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("Fatiegue") && target.GetComponent<Animator>().GetBool(AnimatorConstants.DEAD) == false 
			)
			{
				print(owner.name + " hit: " + target.name);


				alreadySlashCounted = true;
				InputDisabledStatus clash =  owner.AddComponent<InputDisabledStatus>();
				clash.duration = 0.5f;
				clashDealer.GetComponent<ClashDealerC>().OnClashReceived ();
				//clashDealer.GetComponent<GeekBehaviour>().dispatchMessage( ClashDealerC.M_CLASH_RECEIVED);
			}
			else 
			{
				float powerX = ( power * directionX) * GeekPhysicsC.getFacingDir( owner ).x; 
				float powerY = ( power * directionY) ;
				
				Vector2 damageVec = new Vector2 ( powerX, powerY );
				//notify target that it's hit  , make sure not to get hit while clashing taking place
				if(target.GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("Crash.Crash") && 
				   owner.GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("Crash.Crash"))
				   
					target.GetComponent<MessageDispatcher>().dispatchMessage( DamageC.M_DAMAGE_RECEIVED, 
						owner, 
					    new Vector2(directionX, directionY), 
					    power, 
					    "Slash"
					    );		
				
				if(alreadySlashCounted == false)
				{
					alreadySlashCounted = true;	
				}
				
				//print ("#slashed");

			}
		}
		else if (target.name.Equals(this.name)) {

			print ("####clash####");
			//both should go to fatiegue, instead of clash	
			if(owner.GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("Fatiegue")
			   && target.transform.parent.gameObject.GetComponent<Animator>().GetBool(AnimatorConstants.DEAD) == false 
			   && target.transform.parent.gameObject.GetComponent< GeekBehaviour >().animationHash != Animator.StringToHash("Fatiegue")
			   )
			{
				InputDisabledStatus clash =  owner.AddComponent<InputDisabledStatus>();
				clash.duration = 0.5f;
				alreadySlashCounted = true;
				clashDealer.GetComponent<ClashDealerC>().OnClashReceived ();

				//clashDealer.GetComponent<GeekBehaviour>().dispatchMessage( ClashDealerC.M_CLASH_RECEIVED);
			}
		}
		
	}


	void OnTriggerExit2D( Collider2D collider ){

		if(alreadyBlockCounted)
			alreadyBlockCounted = false;
		
		if(alreadySlashCounted )
			alreadySlashCounted = false;

		if(alreadyCut)
			alreadyCut = false;
	}
	
	
	
	
	
	void onHitboxTimeout (object sender, System.EventArgs e)
	{
		onTimer -= onHitboxTimeout;
	}
	
}

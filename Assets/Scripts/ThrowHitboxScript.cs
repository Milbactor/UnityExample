using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class ThrowHitboxScript : Hitbox {
	

	public Vector2 facingDirection;
	protected bool alreadyCut = false;
	public AudioClip CutSound;
	protected LiteTimer destroyTimer;
	public GameObject damageEffect;

	protected void OnElapseTimer( LiteTimer timer)
	{
		Destroy (this.gameObject);
	}

	// Use this for initialization
	override protected void Start () {

		destroyTimer = new LiteTimer(10f);
		destroyTimer.onElapsed += OnElapseTimer;
		destroyTimer.start();
	
		base.Start();
		onTimer += new OnTimeEvent( onHitboxTimeout);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.RIGHT)
			facingDirection = new Vector2(1f,0f);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.LEFT)
			facingDirection = new Vector2(-1f,0f);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.UP)
			facingDirection = new Vector2(0,1f);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.DOWN)
			facingDirection = new Vector2(0,-1f);

		//reset child-parent relationship 
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		destroyTimer.Update();
	}

	protected virtual void DamageProcedure (GameObject target)
	{
		print ("facing direction : " + facingDirection);
		target.GetComponent<GeekBehaviour>().dispatchMessage( 
		                                                     DamageC.M_DAMAGE_RECEIVED, 
		                                                     this.gameObject, 
		                                                     facingDirection,  
		                                                     power,
		                                                     "Shoot"
		                                                     );
		                                                     
		if( damageEffect != null )
		{
			GameObject eff = GameObject.Instantiate( damageEffect ) as GameObject;
			eff.transform.position = target.transform.position;
		}
		
		Destroy(this.gameObject);

	}
	
	protected virtual void OnTriggerEnter2D( Collider2D collider )
	{
		print ("override throw script in trigger thorwhitbox");
		GameObject target = collider.gameObject;
		

		/*if (target.name.Equals( "BlockHitbox" ) ) 
		{
			//block is used and the block is against the opponent direction 
			if( GeekPhysicsC.getFacingDir( target.transform.parent.gameObject ).x != facingDirection.x )
			{
				
				print ("####block####");
				//
				Destroy(this.gameObject);
			
			}
			else 
			{
				print ("####Throw from behind####");
				DamageProcedure(target.transform.parent.gameObject);
				
			}
			return;
			
		}
		
		else*/ if (target.tag == "Player" && target != owner) 
		{
			print ("####Throw####");
			//I didnt meant to be rebellious but slashhitbox did not work...
			if ( target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.Slash" )
			    && GeekPhysicsC.getFacingDir( target ).x != facingDirection.x )
			{
				if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.LEFT)
				{
					GetComponent<ThrowMovementC>().throwDir = ThrowDirection.RIGHT;
					transform.localScale 
						= new Vector3(-transform.localScale.x, transform.localScale.y,0);
					facingDirection.x = 1f;

					transform.position = 
						new Vector3(target.transform.position.x + target.GetComponent<BoxCollider2D>().size.x,
						            transform.position.y, 0);
					owner = target;
				}
				else if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.RIGHT)
				{
					GetComponent<ThrowMovementC>().throwDir = ThrowDirection.LEFT;
					transform.localScale 
						= new Vector3(-transform.localScale.x, transform.localScale.y,0);
					facingDirection.x = -1f;

					transform.position = 
						new Vector3(target.transform.position.x - target.GetComponent<BoxCollider2D>().size.x,
						            transform.position.y, 0);
					owner = target;
				}
				else 
				{
					DamageProcedure(target);
				}
			}
			else if ( target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashUp" ) )
			{
				if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.DOWN)
				{
					GetComponent<ThrowMovementC>().throwDir = ThrowDirection.UP;
					transform.localScale 
						= new Vector3(transform.localScale.x, -transform.localScale.y,0);
					facingDirection.x = 1f;

					transform.position = 
						new Vector3(transform.position.x,
						            + GetComponent<BoxCollider2D>().size.y + 1, 0);
					owner = target;
				}
				else 
				{
					DamageProcedure(target);
				}
			}
			else if ( target.GetComponent< GeekBehaviour >().animationHash == Animator.StringToHash("SlashState.SlashDown" ) )
			{
				if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.UP)
				{
					GetComponent<ThrowMovementC>().throwDir = ThrowDirection.DOWN;
					transform.localScale 
						= new Vector3(transform.localScale.x, -transform.localScale.y,0);
					facingDirection.x = -1f;
					transform.position = 
						new Vector3(transform.position.x,
						            - GetComponent<BoxCollider2D>().size.y - 1, 0);
					owner = target;
				}
				else 
				{
					DamageProcedure(target);
				}
			}



			else{
				DamageProcedure(target);
				GetComponent<Collider2D>().enabled = false;
			}
			return;

		} 

		else if(target.tag == "Log" && alreadyCut == false)
		{

			playCutSound();
			renderer.enabled = false;
			GetComponent<ThrowMovementC>().enabled = false; // use addlistner stuff
			GetComponent<CircleCollider2D>().enabled = false;
			alreadyCut = true;
			Vector3 LogVec = target.transform.localPosition;
			target.GetComponent<GeekBehaviour>().dispatchMessage( LogThrowCutC.M_LOG_THROW_RECEIVED, LogVec );
			//Destroy(this.gameObject, CutSound.length);
			Destroy(target);
		

		}

	}

	protected void playCutSound()
	{
		if( CutSound != null)
		{
			audio.PlayOneShot(CutSound);
		}
		
	}

	
	protected void onHitboxTimeout (object sender, System.EventArgs e)
	{
		onTimer -= onHitboxTimeout;
		GameObject.Destroy(this.gameObject);
	}
	

	
}

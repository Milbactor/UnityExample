using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DamageC : GeekBehaviour {

	public static string M_DAMAGE_RECEIVED = "M_DAMAGE_RECEIVED";
	public static string M_DAMAGE_POWER_UP_RECEIVED = "M_DAMAGE_POWER_UP_RECEIVED";

	public static string M_DAMAGE_TYPE = "M_DAMAGE_TYPE";
	
	protected Vector2 power;
	
	Vector2 damagePowerUpRate = new Vector2(1.4f, 1.2f);
	private Vector2 damagePowerUp = new Vector2(1.0f, 1.0f);
	
	public float knockBackDuration = 0.5f;
	
	public AudioClip[] hitSounds;

	public AudioClip damageSound; 
	public int health = 1;
	
	public bool canDie = false;
	public bool rotateInKnockback = false;

	LiteTimer hitNotifyTimer;
	float maxHitNotifyTime = 2.0f;
	
	public GameObject damageEffect;


	public virtual bool InKnockback
	{
		get{ return GetComponent<KnockbackStatusEffect>() != null; }
	}

	public virtual bool isKnockback
	{
		get { return hitNotifyTimer.playing; }
	}
	// Use this for initialization
	 void Start () {
		base.Start();
	
		if(particleSystem != null && knockBackDuration > 0)
			particleSystem.enableEmission = false;
				
		hitNotifyTimer = new LiteTimer(maxHitNotifyTime);
		hitNotifyTimer.onElapsed += HandleOnElapsed;

		addMessageListener( (arguments) => OnDamageReceived((GameObject) arguments[0] ,  (Vector2)arguments[1], (float)arguments[2], (string)arguments[3]),  M_DAMAGE_RECEIVED );
		addMessageListener( (arguments) => OnDamagePowerUpReceived(), M_DAMAGE_POWER_UP_RECEIVED );
		addMessageListener( (arguments) => OnGotParried(), ParryC.M_GOT_PARRIED );
	}

	void HandleOnElapsed(LiteTimer timer)
	{
		hitNotifyTimer.stop();
	}

	// Update is called once per frame
	void Update () 
	{
		base.Update();
	}

	protected virtual void OnGotParried ()
	{
		StunC stun = gameObject.AddComponent< StunC>() as StunC;
		stun.addMessageListener( args => OnParriedStunEnd(), StunC.M_ON_STUN_END );
		stun.duration = 2f;
		anim.SetTrigger( AnimatorConstants.GOT_PARIED );
		
	}

	void OnParriedStunEnd ()
	{
		anim.SetTrigger( AnimatorConstants.PARIED_COMPLETE );
	}
	
	public virtual void playHitSound()
	{
		if( hitSounds.Length == 0 ) return;
		
		int index = (int)Random.Range (0, hitSounds.Length );
		if( hitSounds[index] != null && anim.GetBool(AnimatorConstants.DEAD) == false)
		{
			//audio.PlayOneShot(hitSounds[index]);
			AudioPlay(hitSounds[index]);
			//audio.clip = hitSounds[index];
			//audio.Play();
		}

	}
	public void playDamageSound()
	{
		if(damageSound != null)
		{
			audio.PlayOneShot(damageSound);
		}

	}
	
	

	//comes after being freazed(so to say Impact animation)
	protected virtual void DamageApply( GameObject dmgdealer, Vector2 directionVec,float power, bool charged = false )
	{
		//GameManager.instance.freezeScreen(5f, 0.01f );
		if( particleSystem != null && knockBackDuration > 0) particleSystem.Emit(200);
		applyDamage( dmgdealer, directionVec, power);
		playHitSound();	
		hitNotifyTimer.start();


		if( knockBackDuration > 0f ){
			KnockbackStatusEffect effect = gameObject.AddComponent<KnockbackStatusEffect>();
			effect.duration = knockBackDuration;
		}
			
		if( damageEffect != null )
		{
			GameObject eff = GameObject.Instantiate( damageEffect ) as GameObject;
			eff.transform.position = transform.position;
		}
	}
	
//directionvec is base damage direction
	protected virtual void applyDamage (GameObject dmgdealer, Vector2 directionVec, float power, bool charged = false)
	{
		health--;
		if( health <= 0 && canDie )
		{
			GameObject.Destroy( this.gameObject );
		}
		Vector2 distanceDirection = directionVec;

		if(dmgdealer.tag != "Player")
			directionVec = new Vector2( directionVec.x * dmgdealer.transform.localScale.x, directionVec.y );
		else
			directionVec = new Vector2( directionVec.x * GeekPhysicsC.getFacingDir(dmgdealer).x, directionVec.y );// as GeekPhysicsC.getFacingDir checks rigidbody

		if( dmgdealer.GetComponent<PlayerC>() != null ) 
			distanceDirection = GeekInput.GetStickVectorWorld( GeekInput.LEFT, dmgdealer.GetComponent<PlayerC>().ID);

		float charge = 1f;
		if( dmgdealer.GetComponent<SlashC>() != null && dmgdealer.GetComponent<SlashC>().chargeConfirmed) {
			charge += dmgdealer.GetComponent<SlashC>().lastCharge * (2 + damagePowerUp.magnitude);

		}

		rb2D.velocity = Vector2.Lerp(directionVec * power, distanceDirection * power, distanceDirection.magnitude ) * 20 * charge;
		if( rotateInKnockback )
		{
		
			if( directionVec.x < 0)
			{
				tf.eulerAngles = new Vector3(0, 0, 90 );
			}
			else if( directionVec.x > 0 ) //can also be zero, meaning it would be a verticle effect
			{
				tf.eulerAngles = new Vector3(0, 0, -90 );
			}
		 }
	}
	
	void knockBackDurationEnd ( StatusEffect effect)
	{
		anim.SetBool(AnimatorConstants.DAMAGED, false );
		GetComponent<GeekBehaviour>().dispatchMessage(GameHUD.M_SET_DISHONOR_RECIEVED);
	}
	
	protected virtual void OnDamageReceived (GameObject dmgdealer, Vector2 directionVec, float power, string hitbox)
	{	
		print ("M_DAMAGED_RECEIVED triggered" );
		dmgdealer.GetComponent<MessageDispatcher>().dispatchMessage( SlashC.M_SLASH_SUCCESS );
		//mean's the character is still in knockback, and it shouldnt be allowed to be hit again - BETTER TO USE ILLEGAL STATE HERE
		if( anim.GetBool(AnimatorConstants.DEAD) == true) 
		{
			return;
		}
		
		Camera.main.GetComponent<SmoothLookC>().shake();

		GameManager.instance.dispatchMessage(M_DAMAGE_TYPE, this.gameObject, hitbox); //TODO need to add argument in the dispatching class hitbox,

		playDamageSound();
		anim.SetBool(AnimatorConstants.DAMAGED, true );
		anim.SetBool(AnimatorConstants.BLOCKED, false);
		anim.SetTrigger(AnimatorConstants.DAMAGE_SLASH);
		GetComponent<GeekBehaviour>().dispatchMessage(GameHUD.M_SET_DISHONOR_RECIEVED);

		dispatchMessage(ImpactC.M_IMPACT);
		DamageApply( dmgdealer, directionVec, power);
	}
	
	protected virtual void OnDamagePowerUpReceived()
	{ 
		print ("damage powerup " + damagePowerUp );
		damagePowerUp =  new Vector2 (damagePowerUp.x * damagePowerUpRate.x, damagePowerUp.y * damagePowerUpRate.y);
	}


}

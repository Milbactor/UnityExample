using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class RawrDamageC : GeekBehaviour{

	// Use this for initialization
	public static string M_DAMAGE_RECEIVED = "M_DAMAGE_RECEIVED";
	public static string M_DAMAGE_POWER_UP_RECEIVED = "M_DAMAGE_POWER_UP_RECEIVED";
	
	protected Vector2 power;
	
	Vector2 damagePowerUpRate = new Vector2(1.4f, 1.2f);
	private Vector2 damagePowerUp = new Vector2(1.0f, 1.0f);
	
	public float knockBackDuration = 0.5f;
	
	public AudioClip[] hitSounds;
	
	public AudioClip damageSound; 
	
	
	public bool InKnockback
	{
		get{ return GetComponent<KnockbackStatusEffect>() != null; }
	}
	
	// Use this for initialization
	void Start () {
		base.Start();
		
		if(particleSystem != null)
			particleSystem.enableEmission = false;
		
		
		addMessageListener( (arguments) => OnDamageReceived((GameObject) arguments[0] ,  (Vector2)arguments[1], (float)arguments[2], (string)arguments[3] ),  M_DAMAGE_RECEIVED );
		addMessageListener( (arguments) => OnDamagePowerUpReceived(), M_DAMAGE_POWER_UP_RECEIVED );
	}
	
	
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();
	}

	

	void playHitSound()
	{
		int index = (int)Random.Range (0, hitSounds.Length );
		if( hitSounds[index] != null && anim.GetBool(AnimatorConstants.DEAD) == false)
		{
			audio.PlayOneShot(hitSounds[index]);
			
		}
		
	}

	void playDamageSound()
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
		particleSystem.Emit(200);
		applyDamage( dmgdealer, directionVec, power);
		playHitSound();
		
		KnockbackStatusEffect effect = gameObject.AddComponent<KnockbackStatusEffect>();
		effect.duration = knockBackDuration;
	}
	
	//directionvec is base damage direction
	protected virtual void applyDamage (GameObject dmgdealer, Vector2 directionVec, float power, bool charged = false)
	{
		//Vertical attack give massive damage, adjust the powerRate in x, y seperately
		print ("applyDamage call " + Random.value);
		Vector2 distanceDirection = directionVec;
		
		//Martino, the Master of rigidbody2D, is going to change throw object with rigidbody, so this issue will not be there anymore 
		if(dmgdealer.tag != "Player")
			directionVec = new Vector2( directionVec.x * dmgdealer.transform.localScale.x, directionVec.y );
		else
			directionVec = new Vector2( directionVec.x * GeekPhysicsC.getFacingDir(dmgdealer).x, directionVec.y );// as GeekPhysicsC.getFacingDir checks rigidbody
		//Vector2 distanceDirection = new Vector2( transform.position.x - dmgdealer.transform.position.x ,transform.position.y - dmgdealer.transform.position.y );
		//distanceDirection = distanceDirection.normalized;;
		
		//UGLY AS PHUCK
		if( dmgdealer.GetComponent<PlayerC>() != null ) 
			distanceDirection = GeekInput.GetStickVectorWorld( GeekInput.LEFT, dmgdealer.GetComponent<PlayerC>().ID);
		
		
		print ("distanceDirection : " + distanceDirection );
		//		print ("chargetimer: " + dmgdealer.GetComponent<SlashC>().chargeTimer.time );
		
		//rb2D.velocity += directionVec * power * 10;
		float charge = 1f;
		if( dmgdealer.GetComponent<SlashC>() != null && dmgdealer.GetComponent<SlashC>().chargeConfirmed) {
			charge += dmgdealer.GetComponent<SlashC>().chargeTimer.time * (2 + damagePowerUp.magnitude);
			
		}
		
		rb2D.velocity = Vector2.Lerp(directionVec * power, distanceDirection * power, distanceDirection.magnitude ) * 20 * charge;
		/*
		if(power.y != 0) {
			rb2D.velocity += power *  10 * damagePowerUp.y;
		}
		else {
			rb2D.velocity += power * 10 * damagePowerUp.x;
		}*/
		
		if( directionVec.x < 0)
		{
			tf.eulerAngles = new Vector3(0, 0, 90 );
		}
		else if( directionVec.x > 0 ) //can also be zero, meaning it would be a verticle effect
		{
			tf.eulerAngles = new Vector3(0, 0, -90 );
		}
		
	}
	
	void knockBackDurationEnd ( StatusEffect effect)
	{
		anim.SetBool(AnimatorConstants.DAMAGED, false );
		GetComponent<GeekBehaviour>().dispatchMessage(GameHUD.M_SET_DISHONOR_RECIEVED);
	}
	
	protected virtual void OnDamageReceived (GameObject dmgdealer, Vector2 directionVec, float power, string hitbox = "Rawr")
	{	
		//mean's the character is still in knockback, and it shouldnt be allowed to be hit again - BETTER TO USE ILLEGAL STATE HERE
		if( anim.GetBool(AnimatorConstants.DEAD) == true) 
		{
			return;
		}
		
		Camera.main.GetComponent<SmoothLookC>().shake();
		
		playDamageSound();
		anim.SetBool(AnimatorConstants.DAMAGED, true );
		anim.SetBool(AnimatorConstants.BLOCKED, false);
		anim.SetTrigger(AnimatorConstants.DAMAGE_SLASH);
		GetComponent<GeekBehaviour>().dispatchMessage(GameHUD.M_SET_DISHONOR_RECIEVED);
		//freazeFinished = false;
		//this.power = power;
		dispatchMessage(ImpactC.M_IMPACT);
		
		//FreezeStatusEffect freeze = gameObject.AddComponent<FreezeStatusEffect>();
		//effect.addMessageListener( args => knockBackDurationEnd( (StatusEffect) args[0] ), StatusEffect.M_STATUS_ENDED );
		DamageApply( dmgdealer, directionVec, power);
	}
	
	protected virtual void OnDamagePowerUpReceived()
	{ 
		print ("damage powerup " + damagePowerUp );
		damagePowerUp =  new Vector2 (damagePowerUp.x * damagePowerUpRate.x, damagePowerUp.y * damagePowerUpRate.y);
	}
}

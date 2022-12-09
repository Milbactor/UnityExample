using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BossDamageC : DamageC{

	public override bool InKnockback
	{
		get{ return GetComponent<BossKnockbackStatusEffect>() != null; }
	}

	// Use this for initialization
	void Start () {

		base.Start();
		addMessageListener( (arguments) => OnDamageReceived((GameObject) arguments[0] ,  (Vector2)arguments[1], (float)arguments[2],(string)arguments[3]),  M_DAMAGE_RECEIVED );
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}

	public override void playHitSound()
	{
		int index = (int)Random.Range (0, hitSounds.Length );
		audio.PlayOneShot(hitSounds[index]);
	}

	protected override void DamageApply( GameObject dmgdealer, Vector2 directionVec,float power, bool charged = false )
	{
		applyDamage( dmgdealer, directionVec, power);
		playHitSound();
		
		BossKnockbackStatusEffect effect = gameObject.AddComponent<BossKnockbackStatusEffect>();
		effect.duration = knockBackDuration;
	}


	//directionvec is base damage direction
	protected override void applyDamage (GameObject dmgdealer, Vector2 directionVec, float power, bool charged = false)
	{
		//Vertical attack give massive damage, adjust the powerRate in x, y seperately
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
		float charge = 1f;
		if( dmgdealer.GetComponent<SlashC>() != null && dmgdealer.GetComponent<SlashC>().chargeConfirmed) {
			charge += dmgdealer.GetComponent<SlashC>().chargeTimer.time;
			
		}
		
		rb2D.velocity = Vector2.Lerp(directionVec * power, distanceDirection * power, distanceDirection.magnitude ) * 20 * charge;

		//Do you need this for Boss?
	/*	if( directionVec.x < 0)
		{
			tf.eulerAngles = new Vector3(0, 0, 90 );
		}
		else if( directionVec.x > 0 ) //can also be zero, meaning it would be a verticle effect
		{
			tf.eulerAngles = new Vector3(0, 0, -90 );
		}*/
		
	}



	protected override void OnDamageReceived (GameObject dmgdealer, Vector2 directionVec, float power, string hitbox = "")
	{	
		print ("hello");

		Camera.main.GetComponent<SmoothLookC>().shake();
		
		playDamageSound();
		anim.SetBool(AnimatorConstants.DAMAGED, true );
		DamageApply( dmgdealer, directionVec, power);
	}

	protected override void OnDamagePowerUpReceived(){}

	protected override void OnGotParried(){}
	
}

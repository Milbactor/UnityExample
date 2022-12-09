using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class UltimateMoveHitboxScript : ThrowHitboxScript {

	// Use this for initialization
	void Start () {
		base.Update();

		destroyTimer = new LiteTimer(10f);
		destroyTimer.onElapsed += OnElapseTimer;
		destroyTimer.start();

		onTimer += new OnTimeEvent( onHitboxTimeout);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.RIGHT)
			facingDirection = new Vector2(1f,0f);
		if(GetComponent<ThrowMovementC>().throwDir == ThrowDirection.LEFT)
			facingDirection = new Vector2(-1f,0f);
		
		//reset child-parent relationship 
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		destroyTimer.Update();
	}

	
	protected override void DamageProcedure (GameObject target)
	{
		print ("facing direction : " + facingDirection);
		target.GetComponent<GeekBehaviour>().dispatchMessage( DamageC.M_DAMAGE_RECEIVED, 
		                                                     this.gameObject, 
		                                                     facingDirection,  
		                                                     power,
		                                                     "Ultimate"
		                                                     );
	}


	protected override void OnTriggerEnter2D( Collider2D collider )
	{
		print ("override throw script in trigger ultimate");
		GameObject target = collider.gameObject;
		
		if (target.tag == "Player" && target != owner) 
		{
			DamageProcedure(target);
		} 
		else if(target.tag == "Log" )
		{
			
			playCutSound();
			alreadyCut = true;
			Vector3 LogVec = target.transform.localPosition;
			target.GetComponent<GeekBehaviour>().dispatchMessage( LogThrowCutC.M_LOG_THROW_RECEIVED, LogVec );
			Destroy(target);
			
			
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		print ("cutlog exit?");
		GameObject target = collider.gameObject;

		if(target.tag == "Log")
		{
			alreadyCut = false;
		}
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class FireBallHitBoxScript : Hitbox {
	


	// Use this for initialization
	override protected void Start () {

		base.Start();
		transform.parent = null;
		GetComponent<FireBallMovementC>().SetMovement();

	}
	
	// Update is called once per frame
	void Update () {
		base.Update();

	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		GameObject target = collider.gameObject;
		
		if (target.name == "hitboxGameObject" || 
		    (target.GetComponent<SlashC>() != null && target.GetComponent<SlashC>().slashing == true) ) 
		{
			GetComponent<FireBallMovementC>().targetPoint = owner;
			owner =  target;
			GetComponent<FireBallMovementC>().SetMovement();
			
		}
		else if (target.tag == "Player" && target != owner)
		{
			DamageProcedure(target);
		}
		
	}

	void DamageProcedure (GameObject target)
	{
		target.GetComponent<GeekBehaviour>().dispatchMessage(
			DamageC.M_DAMAGE_RECEIVED, 
			this.gameObject, 
			new Vector2(-directionX, directionY), 
			power, 
			"FireBall"
			);
		Destroy(this.gameObject);
		
	}

	void onHitboxTimeout (object sender, System.EventArgs e)
	{
		onTimer -= onHitboxTimeout;
		GameObject.Destroy(this.gameObject);
	}


}

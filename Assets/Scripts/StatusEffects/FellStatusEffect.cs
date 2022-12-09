
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FellStatusEffect : MovementDisabledStatus {

	GeekPhysicsC physics;
	
	public float prevFriction;
	public Vector2 prevMaxVelocity;

	
	// Use this for initialization
	void Start () {
	
		physics = GetComponent<GeekPhysicsC>();

		renderer.enabled = false;
		propertyValues.Add( (int)SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );

		base.Start();
		
		manager.registerBaseValue( this, SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );


		physics.maxVelocity = new Vector2(1000f,1000f); //?? how much?? - not sure, but I noticed it is not always reset =[
		addMessageListener( args => OnBoundsPassed(), 
		                   SaveMoveResetC.M_SAVEMOVE_BOUND_PASSED);
	}
	


	void OnBoundsPassed ()
	{
		//print ("## FELLSTATUSEFFECT : ON BOUNDS PASSED ");
	
		reset();
		if( manager.valueModifiers( SamudaiStatusProperties.MAX_VELOCITY ) <=1) 
			physics.maxVelocity = (Vector2)manager.getBaseValue( SamudaiStatusProperties.MAX_VELOCITY );
		

		Destroy( this );
	}
	
}

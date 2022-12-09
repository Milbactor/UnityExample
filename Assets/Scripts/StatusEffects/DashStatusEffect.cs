using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DashStatusEffect : StatusEffect3{
	
	GeekPhysicsC physics;
	PlayerMovementC movement;

	public float speed = 0; 
	override protected void Start()
	{
		physics = GetComponent<GeekPhysicsC>();
		movement = GetComponent<PlayerMovementC>();
		
		propertyValues.Add( (int)SamudaiStatusProperties.FRICTION, physics.frictionX );
		propertyValues.Add( (int)SamudaiStatusProperties.GRAVITY, physics.gravity );
		propertyValues.Add( (int)SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );
		propertyValues.Add( (int)SamudaiStatusProperties.MOVEMENT_ENABLED, movement.movementEnabled ); 
		
		base.Start();
		
		manager.registerBaseValue( this, SamudaiStatusProperties.FRICTION, physics.frictionX );
		manager.registerBaseValue( this, SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );
		manager.registerBaseValue( this, SamudaiStatusProperties.GRAVITY, physics.gravity );
		manager.registerBaseValue( this, SamudaiStatusProperties.MOVEMENT_ENABLED, movement.movementEnabled );
		
		physics.maxVelocity = Vector2.one * speed;
		physics.gravity = 0;
		physics.frictionX = 1;
		GetComponent<GeekBehaviour>().dispatchMessage( PlayerC.M_INPUT_DISABLE);
		
	}
	

	override protected void Update()
	{
		base.Update();
		
	}

	
	override public void reset()
	{
		base.reset();
		
		if( manager.valueModifiers( SamudaiStatusProperties.FRICTION ) <=1) 
			physics.frictionX = (float)manager.getBaseValue( SamudaiStatusProperties.FRICTION );
			
		if( manager.valueModifiers( SamudaiStatusProperties.MAX_VELOCITY ) <=1) 
			physics.maxVelocity = (Vector2)manager.getBaseValue( SamudaiStatusProperties.MAX_VELOCITY );
			
		if( manager.valueModifiers( SamudaiStatusProperties.GRAVITY ) <=1) 
			physics.gravity = (float)manager.getBaseValue( SamudaiStatusProperties.GRAVITY );
			
		
		GetComponent<MessageDispatcher>().dispatchMessage( PlayerC.M_INPUT_ENABLE);
		
	}
}

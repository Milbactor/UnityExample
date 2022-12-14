//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;


namespace AssemblyCSharp
{
	public class KnockbackStatusEffect : StatusEffect3
	{
		GeekPhysicsC physics;
		PlayerC player;
		
		override protected void Start()
		{
			//needs to be called before base()
			physics = GetComponent<GeekPhysicsC>();
			player = GetComponent<PlayerC>();
			propertyValues.Add( (int)SamudaiStatusProperties.FRICTION, physics.frictionX );
			propertyValues.Add( (int)SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );
			propertyValues.Add( (int)SamudaiStatusProperties.GRAVITY, physics.gravity );
			propertyValues.Add( (int)SamudaiStatusProperties.INPUT_ENABLED, player.inputEnabled );
			
		
			base.Start();
							
			manager.registerBaseValue( this, SamudaiStatusProperties.FRICTION, physics.frictionX );
			manager.registerBaseValue( this, SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );
			manager.registerBaseValue( this, SamudaiStatusProperties.GRAVITY, physics.gravity );
			manager.registerBaseValue( this, SamudaiStatusProperties.INPUT_ENABLED, player.inputEnabled );
			
			physics.gravity = 0.2f;
			physics.frictionX = 0.98f;
			physics.maxVelocity = new Vector2(1000f,1000f);
			GetComponent<MessageDispatcher>().dispatchMessage( PlayerC.M_INPUT_DISABLE);

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
				
			//if( manager.valueModifiers( SamudaiStatusProperties.INPUT_ENABLED ) <=1) 
				//physics.gravity = (bool)manager.getBaseValue( SamudaiStatusProperties.INPUT_ENABLED );
				
			player.inputEnabled = true;
			
			tf.eulerAngles = Vector3.zero;
			//GetComponent<MessageDispatcher>().dispatchMessage( PlayerC.M_INPUT_ENABLE);
			anim.SetBool(AnimatorConstants.DAMAGED, false );
		}
		
	}
}



using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BossKnockbackStatusEffect : StatusEffect3
	{
		GeekPhysicsC physics;
		
		override protected void Start()
		{
			//needs to be called before base()
			physics = GetComponent<GeekPhysicsC>();
			propertyValues.Add( (int)SamudaiStatusProperties.FRICTION, physics.frictionX );
			propertyValues.Add( (int)SamudaiStatusProperties.MAX_VELOCITY, physics.maxVelocity );
			propertyValues.Add( (int)SamudaiStatusProperties.GRAVITY, physics.gravity );
			
			base.Start();
			
			//values here should not be set to current values if there is already another statuseffect in effect (maxVelocity would be 1000 instead of 10
			//for example			
			if( manager.getEarliestStatusEffectingProperty( (int)SamudaiStatusProperties.FRICTION ) == this ){					
				manager.baseValues[ (int)SamudaiStatusProperties.FRICTION ] = physics.frictionX;
			}
			if( manager.getEarliestStatusEffectingProperty( (int)SamudaiStatusProperties.MAX_VELOCITY ) == this ) {
				manager.baseValues[ (int)SamudaiStatusProperties.MAX_VELOCITY ] = physics.maxVelocity;
			}
			
			if( manager.getEarliestStatusEffectingProperty( (int)SamudaiStatusProperties.GRAVITY ) == this ) {
				manager.baseValues[ (int)SamudaiStatusProperties.GRAVITY ] = physics.gravity;
			}
			
			physics.gravity = 0.2f;
			physics.frictionX = 0.98f;
			physics.frictionY = 0.98f;
			physics.maxVelocity = new Vector2(1000f,1000f);
			GetComponent<GeekBehaviour>().dispatchMessage( PlayerC.M_INPUT_DISABLE);
			
		}
		
		override public void reset()
		{
			base.reset();
			
			if( manager.valueModifiers( SamudaiStatusProperties.FRICTION ) <=1) 
				physics.frictionX = (float)manager.getBaseValue( SamudaiStatusProperties.FRICTION );
			
			if( manager.valueModifiers(SamudaiStatusProperties.MAX_VELOCITY) <= 1)
				physics.maxVelocity = (Vector2)manager.getBaseValue( SamudaiStatusProperties.MAX_VELOCITY );
			
			if( manager.statusEffects[(int)SamudaiStatusProperties.GRAVITY ].Count <= 1)
				physics.gravity = (float)manager.getBaseValue(SamudaiStatusProperties.GRAVITY); 
			
			tf.eulerAngles = Vector3.zero;
			GetComponent<MessageDispatcher>().dispatchMessage( PlayerC.M_INPUT_ENABLE);
			anim.SetBool(AnimatorConstants.DAMAGED, false );
		}
		
	}
}
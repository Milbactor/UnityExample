using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ClashStatusEffect : StatusEffect3{
	
	GeekPhysicsC physics;
	PlayerC player;
	
	override protected void Start()
	{
		player = GetComponent<PlayerC>();
		physics = GetComponent<GeekPhysicsC>();
		
		propertyValues.Add( (int)SamudaiStatusProperties.INPUT_ENABLED, player.inputEnabled );
		propertyValues.Add( (int)SamudaiStatusProperties.GRAVITY, physics.gravity );
		
		base.Start();
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE);
		
		manager.registerBaseValue( this, SamudaiStatusProperties.INPUT_ENABLED, player.inputEnabled );
		manager.registerBaseValue( this, SamudaiStatusProperties.GRAVITY, physics.gravity );
		
		player.inputEnabled = false;
		physics.gravity = 0;
		
	}
	override protected void Update()
	{
		base.Update();
		
	}
	
	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("Crash.Crash"))
		{
			Destroy(this);
		} 
	}
	
	override public void reset()
	{
		base.reset();
		
		if( manager.valueModifiers( SamudaiStatusProperties.GRAVITY ) <=1) 
			physics.gravity = (float)manager.getBaseValue( SamudaiStatusProperties.GRAVITY );
			
		if( manager.valueModifiers( SamudaiStatusProperties.INPUT_ENABLED ) <=1) 
			player.inputEnabled = (bool)manager.getBaseValue( SamudaiStatusProperties.INPUT_ENABLED );
		
	
	}
}

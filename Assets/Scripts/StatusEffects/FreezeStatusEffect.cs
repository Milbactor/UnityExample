using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FreezeStatusEffect : StatusEffect{

	GeekPhysicsC physics;
	float prevGravity;
	
	override protected void Start()
	{

		base.Start();
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE);
		
		print ("freezestatus duration: " + duration );
		
		physics = GetComponent<GeekPhysicsC>();
		prevGravity = physics.gravity;
		GetComponent<GeekBehaviour>().dispatchMessage( PlayerC.M_INPUT_DISABLE);
		GetComponent<GeekPhysicsC>().gravity = 0;
		
	}
	override protected void Update()
	{
		base.Update();

	}

	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("Damages.Freeze"))
		{
			DestroyImmediate(this);
		} 
	}
	
	override public void reset()
	{
		base.reset();
		physics.gravity = prevGravity;

	}
}

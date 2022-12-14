using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FatiegueStatusEffect : InputDisabledStatus
{
	
	override protected void Start()
	{
		base.Start();
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE);
	}



	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("Main.Fatigue"))
		{
			Destroy(this);
		} 
	}
	

}



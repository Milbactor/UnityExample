using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FatiegueC : GeekBehaviour {

	public static string M_BLOCK_RECEIVED = "M_BLOCK_RECEIVED";
	public static string M_SLASH_RECEIVED = "M_SLASH_RECEIVED";
	public int blockEffectiveCounter; //private 
	public int slashCounter; //private 
	public int maxSlashCounter = 10;
	public int maxBlockEffecitveCounter = 10;
	private float startTimerBlock = 0.0f;
	public float maxTimerBlock = 10.0f;
	private float startTimerSlash = 0.0f;
	public float maxTimerSlash = 10.0f;




/*	public bool fatigue
	{
		get{ return anim.GetBool(AnimatorConstants.FATIGUE ) ; } fatigue is trigger now. 
	}*/

	// Use this for initialization
	void Start () 
	{
		base.Start();
		addMessageListener( (arguments) => OnBlockRecieved(), M_BLOCK_RECEIVED );
		addMessageListener( (arguments) => OnSlashRecieved(), M_SLASH_RECEIVED );
		blockEffectiveCounter  = slashCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		//if the timer taken pass certain time, reset the counter
		if(Time.time - startTimerBlock >= maxTimerBlock)
		{
			blockEffectiveCounter = 0;
		}

		if(Time.time - startTimerSlash >= maxTimerSlash )
		{
			slashCounter = 0;
		}


	}
	
	void OnBlockRecieved()
	{
		//get timer when the hit is taken
		startTimerBlock = Time.time;

		//Due to this function called multiple times while in trigger, wrongly counted multiple times
		blockEffectiveCounter++;
		print ("asdfadfsafd");

		if(blockEffectiveCounter == maxBlockEffecitveCounter)
		{
			anim.SetTrigger (AnimatorConstants.FATIGUE);
			FatiegueStatusEffect effect = gameObject.AddComponent<FatiegueStatusEffect>();
			effect.duration = 10f;
			blockEffectiveCounter = 0;

		}

	}

	//called from slash animation
	public void OnSlashRecieved()
	{
		slashCounter++;
		startTimerSlash = Time.time;

		if(slashCounter == maxSlashCounter)
		{
			anim.SetTrigger (AnimatorConstants.FATIGUE);
			FatiegueStatusEffect effect = gameObject.AddComponent<FatiegueStatusEffect>();
			effect.duration = 10f;
			slashCounter = 0;	
		}

	}






}

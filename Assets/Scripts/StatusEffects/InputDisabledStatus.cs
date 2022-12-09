using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class InputDisabledStatus : StatusEffect
{

	override protected void Start()
	{
		base.Start();

		GetComponent<GeekBehaviour>().dispatchMessage( PlayerC.M_INPUT_DISABLE);
	}

	override protected void Update()
	{
		base.Update();
	}

	override public void reset()
	{
		base.reset();
		GetComponent<GeekBehaviour>().dispatchMessage( PlayerC.M_INPUT_ENABLE );
		//print ("movement enabled");
	}
}



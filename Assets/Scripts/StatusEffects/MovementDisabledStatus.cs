using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MovementDisabledStatus : StatusEffect3
{
	PlayerMovementC movement;
	
	override protected void Start()
	{
		movement = GetComponent<PlayerMovementC>();
		propertyValues.Add( (int)SamudaiStatusProperties.MOVEMENT_ENABLED, movement.movementEnabled );
	
		base.Start();
	
		dispatchMessage(PlayerMovementC.M_TOGGLE_MOVEMENT, false );
	}
	
	override protected void Update()
	{
		base.Update();
	}
	
	override public void reset()
	{
		base.reset();
		//dispatchMessage(PlayerMovementC.M_TOGGLE_MOVEMENT, true );

		//grabbing dispatcher to go around (enabled == true ) check, because calling destroy() on status end  will put enabled on flase
		//preventing the message from being dispatched
		if( manager.statusEffects[(int)SamudaiStatusProperties.MOVEMENT_ENABLED ].Count <= 1) dispatcher.dispatchMessage(PlayerMovementC.M_TOGGLE_MOVEMENT, true );
		//print ("## MOVEMENTDISABLEDSTATUS reset");
	}
}



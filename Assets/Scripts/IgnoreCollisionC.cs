using UnityEngine;
using System.Collections;

public class IgnoreCollisionC : GeekBehaviour{

	public static string M_YAXIS = "M_YAXIS_PRESSED";
	public static string M_YAXIS_RELEASED = "M_YAXIS_RELEASED";

	// Use this for initialization
	void Start () {
		addMessageListener( (args) => HazardCollisionIgnore(),  M_YAXIS ); 
		addMessageListener( (args) => HazardCollisionNotIgnore(), M_YAXIS_RELEASED  ); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HazardCollisionIgnore()
	{
		//allowed to pass
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Hazard"), LayerMask.NameToLayer ("players"), true);

	}

	void HazardCollisionNotIgnore()
	{
		//not allowed to pass
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Hazard"), LayerMask.NameToLayer ("players"), false);
		
	}
}
	
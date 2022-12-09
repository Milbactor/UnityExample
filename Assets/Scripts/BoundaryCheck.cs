using UnityEngine;
using System.Collections;

public class BoundaryCheck : GeekBehaviour {

	protected Rect bounds;
	static public string M_BOUNDARYMESSAGE = "Boundary Check";
	public float marginX = 0.0f;
	// Use this for initialization
	void Start () 
	{
		base.Start();
		bounds = LevelBounds.instance.bounds;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();
		checkBoundaries();
	}

	void checkBoundaries()
	{
		//left boundary
		if( transform.position.x <= bounds.x - marginX - ( bounds.width / 2 ) )
		{
			dispatchMessage(BoundaryCheck.M_BOUNDARYMESSAGE, new Vector2(-1,0) );
		}
		//right boundary
		else if ( transform.position.x >= bounds.x + ( bounds.width / 2 ) + marginX )
		{
			dispatchMessage(BoundaryCheck.M_BOUNDARYMESSAGE, new Vector2(1,0) );
		}
		
		//top boundary
		if( transform.position.y  <= bounds.y - ( bounds.height / 2 ) )
		{
			dispatchMessage(BoundaryCheck.M_BOUNDARYMESSAGE, new Vector2(0,1) );
		}
		//bottom boundary
		else if ( transform.position.y  >= bounds.y + ( bounds.height / 2 ) )
		{
			dispatchMessage(BoundaryCheck.M_BOUNDARYMESSAGE, new Vector2(0,-1) );
		}
		
	}
}

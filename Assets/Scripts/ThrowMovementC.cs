using UnityEngine;
using System.Collections;

public enum ThrowDirection {RIGHT, LEFT, UP, DOWN};

public class ThrowMovementC : GeekBehaviour {
	
	//public enum ThrowDirection {RIGHT, LEFT, UP};
	public  ThrowDirection throwDir;
	public float speed = 0.5f;
	public bool MoveStart = true; // this is turned of when it is ultimate object instantiated 



	// Use this for initialization
	void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		base.Update();

		if(MoveStart == true)
		{
			if(throwDir == ThrowDirection.RIGHT)
			{
				rb2D.velocity = new Vector2(speed, 0 );
			}
			if(throwDir == ThrowDirection.LEFT)
			{
			    rb2D.velocity = new Vector2(-speed, 0 );
			}

			if(throwDir == ThrowDirection.UP)
			{
			    rb2D.velocity = new Vector2(0, speed );
			}

			if(throwDir == ThrowDirection.DOWN)
			{
				rb2D.velocity = new Vector2(0, -speed );
			}
		}
	}

	//called from ultimate object animation
	public void StartMove()
	{
		MoveStart = true;
	}
	
}

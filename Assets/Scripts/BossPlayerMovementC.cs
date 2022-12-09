using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class BossPlayerMovementC : PlayerMovementC {

	public static string MOVE_UP = "MOVE_UP";
	public static string MOVE_DOWN = "MOVE_DOWN";

	public static string PAUSE_VERTICAL = "PAUSE_VERTICAL";
	public static string PAUSE_HORIZONTAL = "PAUSE_HORIZONTAL";
	

	
	// Use this for initialization
	override protected void Start () {
		
		base.Start();
		
		addMessageListener ((arguments) => moveUp(), MOVE_UP );
		addMessageListener ((arguments) => moveDown(), MOVE_DOWN );
		//addMessageListener ((arguments) => pauseVertical(), PAUSE_VERTICAL );
		//addMessageListener ((arguments) => pauseHorizontal(), PAUSE_HORIZONTAL );
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		dispatchMessage( APPLY_IMPULSE, applyImpulse);
		
		List<GameObject> otherPlayers = GameManager.instance.GetOtherPlayers( gameObject);
		foreach( GameObject otherPlayer in otherPlayers )
		{
			if(otherPlayer.name.Contains("Hero") || otherPlayer.name.Contains("Heroine"))
			{
				if( tf.position.x - otherPlayer.transform.position.x > 0 )
				{
					transform.localScale = new Vector3 ( Mathf.Abs( transform.localScale.x), 1, 1);
				}
				else
				{
					transform.localScale = new Vector3 ( -Mathf.Abs( -transform.localScale.x), 1, 1);
				}
			}

			if(otherPlayer.name.Contains("Villain") || otherPlayer.name.Contains("HenchMan"))
			{
				if( tf.position.x - otherPlayer.transform.position.x > 0 )
				{
					transform.localScale = new Vector3 ( Mathf.Abs( transform.localScale.x), 1, 1);
				}
				else
				{
					transform.localScale = new Vector3 ( -Mathf.Abs( -transform.localScale.x), 1, 1);
				}
			}
		}
	}
	
	void LateUpdate(){
		applyImpulse = false;
		
	}
	
	override protected void moveRight()
	{
		if( movementEnabled == false ) return;
		applyImpulse = true;

		rigidbody2D.velocity += new Vector2( acceleration.x * aerialDrag * Time.deltaTime, 0 );
		//transform.localScale = new Vector3 ( -Mathf.Abs( transform.localScale.x), 1, 1);
		//SendMessage("changeFlip", false, SendMessageOptions.DontRequireReceiver );
		//onFlipChanged( false );
	}
	
	override protected void moveLeft()
	{
		if( movementEnabled == false ) return;
		
		applyImpulse = true;
		
		rigidbody2D.velocity -= new Vector2( acceleration.x * aerialDrag * Time.deltaTime, 0 );

		//transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), 1, 1);
	}

	void moveUp()
	{
		if( movementEnabled == false ) return;
		
		applyImpulse = true;
		
		rigidbody2D.velocity -= new Vector2( 0, acceleration.y *  Time.deltaTime );
		
		//transform.localScale = new Vector3 (-Mathf.Abs(transform.localScale.x), 1, 1);
	}

	void moveDown()
	{
		if( movementEnabled == false ) return;
		
		applyImpulse = true;
		
		rigidbody2D.velocity += new Vector2( 0, acceleration.y * Time.deltaTime );
		
		//transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), 1, 1);
	}

	void pauseHorizontal()
	{
		rigidbody2D.velocity = new Vector2(0, 0);
	}

	void pauseVertical()
	{
		rigidbody2D.velocity = new Vector2(0, 0);
	}

}

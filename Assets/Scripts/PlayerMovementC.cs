using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerMovementC : GeekBehaviour {

	public static string APPLY_IMPULSE = "APPLY_IMPULSE";
	public static string M_TOGGLE_MOVEMENT = "M_TOGGLE_MOVEMENT";

	int ID = 1;
	public Vector2 acceleration = new Vector2( 5f, 5f );
	public float aerialDrag = 0.8f;
	public bool applyImpulse = false;

	public static string MOVE_LEFT = "MOVE_LEFT";
	public static string MOVE_RIGHT = "MOVE_RIGHT";

	public bool movementEnabled = true;
	protected bool grounded
	{
		get{ return anim.GetBool(AnimatorConstants.GROUNDED) ; }
	}

	
	// Use this for initialization
	override protected void Start () {
	
		base.Start();

		addMessageListener( (arguments) => { movementEnabled = (bool)arguments[0] ; }, M_TOGGLE_MOVEMENT );
		addMessageListener ((arguments) => moveRight(), MOVE_RIGHT );
		addMessageListener ((arguments) => moveLeft(), MOVE_LEFT );

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		//dispatchMessage( PlayerMovementC.APPLY_IMPULSE, applyImpulse);
		anim.SetFloat( AnimatorConstants.VELOCITY_Y, rigidbody2D.velocity.y);
	}

	void LateUpdate(){
		applyImpulse = false;

	}

	virtual protected void moveRight()
	{
		if( movementEnabled == false ) return;
		applyImpulse = true;
		
		if( grounded ) {
			rigidbody2D.velocity += new Vector2( acceleration.x * Time.deltaTime, 0);
		}
		else
		{
			rigidbody2D.velocity += new Vector2( acceleration.x * aerialDrag * Time.deltaTime, 0 );
		}
		
		
		transform.localScale = new Vector3 ( Mathf.Abs( transform.localScale.x), 1, 1);
		//SendMessage("changeFlip", false, SendMessageOptions.DontRequireReceiver );
		//onFlipChanged( false );
	}

	virtual protected void moveLeft()
	{
		if( movementEnabled == false ) return;

		applyImpulse = true;
		
		if( grounded ) {
			rigidbody2D.velocity -= new Vector2 (acceleration.x * Time.deltaTime, 0);
		}else
		{
			rigidbody2D.velocity -= new Vector2( acceleration.x * aerialDrag * Time.deltaTime, 0 );
		}
		
		transform.localScale = new Vector3 (-Mathf.Abs(transform.localScale.x), 1, 1);
		//SendMessage("changeFlip", true, SendMessageOptions.DontRequireReceiver );
		//onFlipChanged( true );
	}

	void onFlipChanged(bool flipValue)
	{
		if (flipValue == true) {
			transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * -1, 1, 1);
		} else {
			transform.localScale = new Vector3 ( Mathf.Abs(transform.localScale.x ) , 1, 1);
		}
	}
	

	
}

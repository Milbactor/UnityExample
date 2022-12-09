using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GeekPhysicsC : GeekBehaviour {

	private float _gravity = 0.9f;
	public float gravityScale = 1f;
	public Vector2 maxVelocity = new Vector2( 30f, 5f );
	public float frictionX = 0.85f;
	public float frictionY = 1f;
	
	public Vector2 prevVelocity = Vector2.zero;

	bool applyImpulse = false;

	public float gravity
	{
		get { return _gravity; }
		set { _gravity = value ; }
	}

	protected bool grounded
	{
		get{ return anim.GetBool(AnimatorConstants.GROUNDED) ; }
	}

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		addMessageListener((arguments) => applyImpulse = (bool)arguments [0], PlayerMovementC.APPLY_IMPULSE);
	
	}

	void Update(){

	}
	// Update is called once per frame
	void FixedUpdate () 
	{

		rb2D.velocity -= new Vector2 (0, gravity * gravityScale);

		if(applyImpulse == false)
			rb2D.velocity =  new Vector2 (rb2D.velocity.x * frictionX, rb2D.velocity.y);;
			
		rb2D.velocity =  new Vector2 (rb2D.velocity.x, rb2D.velocity.y * frictionY);;

		if ( Mathf.Abs ( rb2D.velocity.x ) > maxVelocity.x  ) {
			rb2D.velocity = new Vector2(maxVelocity.x * GetDirection(rb2D).x , rb2D.velocity.y);
		}

		
		if ( Mathf.Abs ( rb2D.velocity.y ) > maxVelocity.y  ) {
			rb2D.velocity = new Vector2( rb2D.velocity.x , maxVelocity.y * GetDirection(rb2D).y );
		}
		
		prevVelocity = rb2D.velocity;
	}

	void LateUpdate()
	{
		/*if( rb2D.velocity.y <= 0.1f)
		{
			//not allowed to pass
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Hazard"), LayerMask.NameToLayer ("players"), false);
		}
		else
		{
			//allowed to pass
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Hazard"), LayerMask.NameToLayer ("players"), true);
		}*/

	
		

	}



	public static Vector2 getFacingDir (GameObject gameObject)
	{
		StatusC status = gameObject.GetComponent<StatusC>();
		if( status != null )
		{
			/// ///
			return new Vector2(gameObject.transform.localScale.x, 1);
			//return status.facing;
		}
		
		return GetDirection( gameObject.rigidbody2D );
	}

	public static Vector2 GetDirection(Rigidbody2D rigid){

		Vector2 direction = Vector2.zero;
		if(rigid.velocity.x > 0)
		{
			direction.x = 1;
		}
		else
		{
			direction.x = -1;
		}
		if(rigid.velocity.y > 0)
		{
			direction.y = 1;
		
		}
		else
		{
			direction.y = -1;
		}
		return direction; 
	}
}

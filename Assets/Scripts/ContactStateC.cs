using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ContactStateC : GeekBehaviour {


	public bool huggingWall = false;
	private BoxCollider2D col;
	public float maxWallHangTimer = 1.0f;	//TODO use LiteTImer stuff instead. 
	public float wallHugFallSpeed = -20f; 
	private float startWallHangTimer = 0.0f;
	public bool grounded = false;

	public float wallHugFriction = 0.9f;

	public static string M_GET_GROUNDED = "M_GET_GROUNDED";
	public static string M_GET_UNGROUNDED = "M_GET_UNGROUNDED";


	// Use this for initialization
	void Start () {
		base.Start();
		col = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

		base.Update();

		if(huggingWall == true && Time.time - startWallHangTimer >= maxWallHangTimer)
		{
			rb2D.velocity = new Vector2((GeekPhysicsC.getFacingDir(this.gameObject).x *-1)*10, wallHugFallSpeed );//wallHugFallSpeed);
		}
		

	}

	
	void FixedUpdate()
	{
		checkWall();
		checkGround();
		
		if(huggingWall == true )
		{
			rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * wallHugFriction );//wallHugFallSpeed);
		}
	}

	void checkWall ()
	{
		if( anim.GetBool(AnimatorConstants.BLOCKED ) == true || grounded == true) return; //ugly solution
		
		Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
		
		Vector3 leftBottom = new Vector3 ( tf.localPosition.x - col.size.x/2f + col.center.x * tf.localScale.x - 0.1f, tf.localPosition.y - (colliderSize.y / 2) + 0.1f, 0);
		Vector3 leftTop = new Vector3 ( tf.localPosition.x - col.size.x/2f + col.center.x * tf.localScale.x - 0.1f, tf.localPosition.y + (colliderSize.y / 2) - 0.1f, 0);
		
		Vector3 rightBottom = new Vector3 (tf.localPosition.x + col.size.x/2f + col.center.x *  tf.localScale.x + 0.1f, tf.localPosition.y - (colliderSize.y / 2) + 0.1f, 0);
		Vector3 rightTop = new Vector3 (tf.localPosition.x + col.size.x/2f + col.center.x *  tf.localScale.x + 0.1f, tf.localPosition.y + (colliderSize.y / 2) - 0.1f, 0);
		
		Debug.DrawLine( rightBottom, rightTop, Color.blue );
		Debug.DrawLine( leftBottom, leftTop, Color.blue );
		
		int layerMask = (1 << LayerMask.NameToLayer("BoundsLayer") );
		layerMask = ~layerMask;
		
		RaycastHit2D[] hitsLeft = Physics2D.LinecastAll( leftBottom, leftTop, layerMask ); //12 is hazard
		RaycastHit2D[] hitsRight = Physics2D.LinecastAll( rightBottom, rightTop, layerMask );
		
		//later, change 2f into the half of collider y +/- a bit 
		if( hitsLeft.Length > 0 && !GeekTools.layerInRaycastHits( hitsLeft, "players") 
		   && GeekTools.childInRaycastHits( hitsLeft, gameObject ) == false 
		   )
		{
			if(huggingWall == false)
			{
				startWallHangTimer = Time.time;
			}
			//rb2D.velocity = new Vector2( 0, rb2D.velocity.y );
			huggingWall = true;
			anim.SetBool(AnimatorConstants.WALL_HUGGGING, true );
			anim.SetBool(AnimatorConstants.BLOCKED, false);
			
			//GetComponent<SpriteRenderer>().color = Color.blue;
			
		}else if( hitsRight.Length > 0 && !GeekTools.layerInRaycastHits( hitsRight, "players")
		         && GeekTools.childInRaycastHits( hitsRight, gameObject ) == false 
		         ) 
		{
			if(huggingWall == false)
			{
				startWallHangTimer = Time.time;
			}
			huggingWall = true;
			anim.SetBool( AnimatorConstants.WALL_HUGGGING, true );
			anim.SetBool(AnimatorConstants.BLOCKED, false);
		}
		else
		{
			huggingWall = false;
			anim.SetBool( AnimatorConstants.WALL_HUGGGING, false );
		} 
		
		
	}

	void checkGround() {
		
		Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
		
		Vector3 middlePos = new Vector3(tf.localPosition.x, tf.localPosition.y - 2f,0);
		Vector3 leftPos = new Vector3 ( tf.localPosition.x - col.size.x/2f + col.center.x * tf.localScale.x, tf.localPosition.y - (colliderSize.y / 2) - 0.1f, 0);
		Vector3 rightPos = new Vector3 (tf.localPosition.x + col.size.x/2f + col.center.x *  tf.localScale.x, tf.localPosition.y - (colliderSize.y / 2) - 0.1f, 0);
		
		int boundsLayer = (1 << LayerMask.NameToLayer("BoundsLayer") );
		RaycastHit2D[] hits = Physics2D.LinecastAll( leftPos, rightPos );
		Debug.DrawLine( leftPos, rightPos, Color.red );

		
		//later, change 2f into the half of collider y +/- a bit 
		if(hits.Length > 0)
		{
			getGrounded();
		}
		else if ( grounded == true ){
			getUngrounded();
		}
		
		

		
	}
	
	void getGrounded()
	{
		dispatchMessage( M_GET_GROUNDED);
		
		anim.SetBool(AnimatorConstants.GROUNDED, true);
		anim.SetTrigger(AnimatorConstants.GET_GROUNDED);
		grounded = true;
		huggingWall = false;
	}
	
	void getUngrounded()
	{
		dispatchMessage( M_GET_UNGROUNDED);
		
		anim.SetBool( AnimatorConstants.GROUNDED, false);
		grounded = false;
	}

}

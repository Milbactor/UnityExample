using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LogC : GeekBehaviour {
	public float xSpeed = 0;
	public float floatspeed = 2f;
	public float slideSpeed = 200f;
	bool layerChanged = false;

	private LiteTimer knocbackTimer = new LiteTimer(10.0f);

	// Use this for initialization
	void Start () {
		base.Start();
		knocbackTimer.onElapsed += knockBackDurationEnd;
		
		xSpeed = transform.position.x / 20;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		knocbackTimer.Update();

		if(transform.position.y < -17.0f && layerChanged == false)
		{
			renderer.sortingLayerName = "Default";
			layerChanged = true;
		}

	}

	void FixedUpdate()
	{
		//means it didnt hit the ground yet
		if ( rb2D.fixedAngle ) {
			rb2D.velocity = new Vector2( xSpeed, rb2D.velocity.y );
		}
	}


	void OnCollisionEnter2D( Collision2D collision)
	{
		if(collision.gameObject.tag == "Border")
		{
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			knocbackTimer.start();

		}
	}
	void OnCollisionStay2D( Collision2D collision)
	{
		if(collision.gameObject.tag == "Water")
		{
			if( transform.position.x <= 0 ) {
				xSpeed = -floatspeed;
			}else {
				xSpeed = floatspeed;
			}
			
			rigidbody2D.velocity = new Vector2( xSpeed, 0 );
			rigidbody2D.fixedAngle = true;

		}
		else if( collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Log" )
		{
			//only slide if the log's it's hitting are not falling down anymore
			if( collision.gameObject.tag == "Log" && collision.gameObject.rigidbody2D.velocity.y > -0.5f ) return;
			//Destroy( this.gameObject );
			
			rigidbody2D.fixedAngle = false;
			
			if( transform.position.x <= 0 ) {
				xSpeed = -slideSpeed;
			}else {
				xSpeed = slideSpeed;
			}
			
			rigidbody2D.velocity = new Vector2( xSpeed, 0 );

		}
	}





	void knockBackDurationEnd ( LiteTimer timer)
	{
		Destroy (this.gameObject);
	}
	
	
}

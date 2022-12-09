using UnityEngine;
using System.Collections;

public class OneWayPlatformC : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D( Collision2D collision)
	{
		if( collision.gameObject.rigidbody2D.velocity.y > 0 )
		{
			Physics2D.IgnoreCollision( collision.gameObject.collider2D, collider2D, true );
		}
		else
		{
			Physics2D.IgnoreCollision( collision.gameObject.collider2D, collider2D, false );
		}
	}
	
	
	void OnCollisionStay2D( Collision2D collision)
	{
		if( collision.gameObject.rigidbody2D.velocity.y > 0 )
		{
			Physics2D.IgnoreCollision( collision.gameObject.collider2D, collider2D, true );
		}
		else
		{
			Physics2D.IgnoreCollision( collision.gameObject.collider2D, collider2D, false );
		}
	}
	
	void OnCollisionExit2D( Collision2D collision)
	{
		Physics2D.IgnoreCollision( collision.gameObject.collider2D, collider2D, false ) ;
	}
}

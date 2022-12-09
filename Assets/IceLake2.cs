using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class IceLake2 : GeekBehaviour {

	public int states = 2;
	public GameObject wholeIce;
	public GameObject crackedIce;
	public GameObject brokenIce;
	// Use this for initialization
	void Start () {
	
		updateState( GameData.lakeState );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void updateState( int newState )
	{
		if( newState == 0 )
		{
			wholeIce.SetActive( true );
			crackedIce.SetActive( false );
			brokenIce.SetActive( false );
		}
		else if( newState == 1 )
		{
			wholeIce.SetActive( true );
			crackedIce.SetActive( true );
			brokenIce.SetActive( false );
		}
		else if( newState == 2 )
		{
			wholeIce.SetActive( false );
			crackedIce.SetActive( false );
			brokenIce.SetActive( true );
			collider2D.enabled = false;
		}
	}
	
	void OnCollisionEnter2D( Collision2D collision )
	{
		print ("prevVelocity.y: " + collision.gameObject.GetComponent<GeekPhysicsC>().prevVelocity.y );
		if( /*collision.gameObject.GetComponent<GeekPhysicsC>().prevVelocity.y <= -10 &&*/ GameData.lakeState < states/* && 
		   collision.gameObject.GetComponent<KnockbackStatusEffect>() != null*/)
		{
			GameData.lakeState++;
			updateState( GameData.lakeState );
		}
	}
}

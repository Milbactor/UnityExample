using UnityEngine;
using System.Collections;

public class Portalc : GeekBehaviour {

	public static string M_ON_LEVEL_BOUNDARIES_PASSED = "M_ON_LEVEL_BOUNDARIES_PASSED";
	public Vector2 direction = Vector2.zero;
	 
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	
	void OnCollisionEnter2D( Collision2D col )
	{
	  //print ("entered portal: " + col.gameObject.tag + " , damage: " + col.gameObject.GetComponent<Animator>().GetBool("Damage") );
	
		if( col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<MessageDispatcher>().dispatchMessage(M_ON_LEVEL_BOUNDARIES_PASSED, col.gameObject, direction );
		}
	}
	
	
}

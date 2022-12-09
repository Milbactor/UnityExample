using UnityEngine;
using System.Collections;

public class SaveMoveResetC : GeekBehaviour {
	public static string M_SAVEMOVE_BOUND_PASSED = "M_SAVEMOVE_BOUND_PASSED";

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Player")
		{
			col.gameObject.GetComponent<MessageDispatcher>().
				dispatchMessage( M_SAVEMOVE_BOUND_PASSED, col.gameObject );
		}
	}
	

}

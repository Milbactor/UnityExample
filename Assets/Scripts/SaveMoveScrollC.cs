using UnityEngine;
using System.Collections;

public class SaveMoveScrollC : ScrollC {

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	protected override void OnScrollCollect( Collider2D col )
	{
		//base.OnScrollCollect( col );
		if(col.gameObject.tag != "Player") return;
		col.gameObject.GetComponent< StatusC>().saveMoveAmount ++;
		gameObject.SetActive( false );
		dispatchMessage( InventoryC.M_ON_COLLECT, this );
		audio.clip = pickupSound;
		audio.Play( );
		
		print ( "setActive false" );
	}
	
}

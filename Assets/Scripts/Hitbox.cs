
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Hitbox : GeekBehaviour {

	public static string M_HITBOX_HIT = "M_HITBOX_HIT";
	
	protected List<GameObject> hitObjects;
	
	public delegate void OnTimeEvent(object sender, EventArgs e);
	public event OnTimeEvent onTimer;

	public float directionX = 0;
	public float directionY = 0;
	public float power = 50;
	int time = 20;
	int timeCounter = 0;
	
	public GameObject owner = null;
	
	// Use this for initialization
	override protected void Start () {
	
		base.Start();
		owner = transform.parent.gameObject;
		hitObjects = new List<GameObject>();	
	}

	protected virtual void OnTriggerEnter2D( Collider2D collider)
	{
		if( hitObjects.Contains( collider.gameObject ) ) return;
		
		hitObjects.Add( collider.gameObject );
		dispatchMessage( M_HITBOX_HIT, this, collider.gameObject );
	}
	
	protected virtual void OnTriggerExit2D( Collider2D collider)
	{
		//for throw collided object is desinged to be destroyed instantly need this check.
		if(collider == null ) return;
		if(collider.gameObject == null) return;
		if(hitObjects == null) return;
		if( hitObjects.Contains(collider.gameObject) ) hitObjects.Remove( collider.gameObject );
	}

	void Update()
	{
		base.Update();	
		timeCounter++;
		if (timeCounter == time) {

			if( onTimer != null)
				onTimer( this, EventArgs.Empty);
		}
	}

}

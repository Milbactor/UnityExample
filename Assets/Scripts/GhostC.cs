using UnityEngine;
using System.Collections;


public class GhostC : GeekBehaviour {

	Vector2 vel; // Holds the random velocity
	float switchDirection = 3.0f;
	float curTime = 0.0f;
	public float maxVel = 3.0f; // Hold the speed
	Rigidbody2D stuff; // Needs rigidbody attached with a collider

	// Use this for initialization
	void Start () {
		base.Start();
		SetVel();
		stuff = GetComponent<Rigidbody2D>();
		GetComponent<MessageDispatcher>().addMessageListener((arguments) => OnBondaryEnter((Vector2)arguments[0] ), BoundaryCheck.M_BOUNDARYMESSAGE );
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
		/*Quaternion newRotation = Quaternion.LookRotation(transform.position);
		newRotation.x = 0.0f;
		newRotation.y = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 0.5f);*/

		if (curTime < switchDirection) 
		{
			curTime += 1 * Time.deltaTime;
		}
		else 
		{
			SetVel();
			if (Random.value > .5)
			{
				switchDirection += Random.value;

			}
			else 
			{
				switchDirection -= Random.value;
			
			}

			if (switchDirection < 1) 
			{
				switchDirection = 1 + Random.value;
			}
			curTime = 0;
		}

	}

	void LateUpdate()
	{
	}

	void FixedUpdate()
	{
		stuff.velocity = vel;
	}
	void SetVel()
	{
		if (Random.value > .5) {
			vel.x = maxVel * maxVel * Random.value;
		}
		else {
			vel.x = -maxVel* maxVel * Random.value;
		}
		if (Random.value > .5) {
			vel.y = maxVel * maxVel * Random.value;
		}
		else {
			vel.y = -maxVel *maxVel * Random.value;
		}

	}

	void OnBondaryEnter(Vector2 location )
	{
		if(location.x < 0 || location.x > 0)
		{
			vel.x = -vel.x;

		}
		if(location.y < 0 || location.y > 0)
		{
			vel.y = -vel.y;
			
		}
	}





}

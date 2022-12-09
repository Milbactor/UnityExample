using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class HatMovementC : MonoBehaviour {

	LiteTimer flyingTimer;
	float flyingTime = 0.5f;
	Vector2 flyingSpeed = new Vector2(5f,1f);
	public float scaleX;

	void timeElapsed( LiteTimer timer)
	{	
		collider2D.isTrigger = true;
	}

	// Use this for initialization
	void Start () {
		flyingSpeed.x = flyingSpeed.x * scaleX;
		flyingTimer = new LiteTimer(flyingTime);
		flyingTimer.onElapsed += timeElapsed;
		flyingTimer.start();
	}
	
	// Update is called once per frame
	void Update () {

		flyingTimer.Update();

		if(flyingTimer.playing == true)
		{
			rigidbody2D.velocity = flyingSpeed;
		}

		if(transform.position.y < -30f)
		{
			Destroy(this.gameObject);
		}
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class BridgeC : MonoBehaviour {

	LiteTimer fallTimer; 
	LiteTimer destroyTimer;
	public float fallTime = 10f;
	float destroyTime = 20f;

	public GameObject ColliderObject; 

	public List<GameObject> childObjects;

	public float minBlowingPower;
	public float maxBlowingPower;
	
	bool blowAway = false;
	
	float value = 0f;
	
	float blowCounter = 0f;

	// Use this for initialization
	void Start () {
		//fallTimer = new LiteTimer(fallTime);
		destroyTimer = new LiteTimer(destroyTime);
		//fallTimer.onElapsed += HandleonElapsed;
		destroyTimer.onElapsed += DestroyTimeOnElaspsed;
//		fallTimer.start();
	}

	public void DestroyBridge ( LiteTimer timer, int direction = 0)
	{
		GameData.bridgeBroken = true;
		
		gameObject.AddComponent<Rigidbody2D>();
		destroyTimer.start();
		
		blowAway = true;
		value = Random.Range(minBlowingPower, maxBlowingPower);
		
		if( direction == 0 )
		{
			direction = 1;
			if(Random.value > 0.5f) direction = -1;
		}
		
		value *= direction;
		
		foreach(GameObject obj in childObjects)
		{
			obj.AddComponent<Rigidbody2D>();
			obj.GetComponent<Rigidbody2D>().gravityScale = 0f;
			obj.rigidbody2D.fixedAngle = false;
			
			/*float size = obj.GetComponent<SpriteRenderer>().sprite.bounds.size.magnitude;
			obj.rigidbody2D.AddForce( new Vector2 ( value * 10f / size,  Random.Range( minBlowingPower, maxBlowingPower) ) );
		
			obj.rigidbody2D.angularVelocity = Random.Range( 800 / size,  1000 / size );*/
			                         
		}
		
	
		
	}

	void DestroyTimeOnElaspsed(LiteTimer timer)
	{
		Destroy(this.gameObject);
	}
	// Update is called once per frame
	void Update () {
		//fallTimer.Update();
		destroyTimer.Update();

	}
	
	void FixedUpdate()
	{
		if( blowAway == false ) return;
		
		ColliderObject.collider2D.enabled = false;
		float i = 0;
		foreach(GameObject obj in childObjects)
		{

			float size = obj.GetComponent<SpriteRenderer>().sprite.bounds.size.magnitude;
			//print ("size: " + obj.GetComponent<SpriteRenderer>().sprite.bounds.size.magnitude);
			
			if( blowCounter <= (size / 50) + ( i * 0.1f) ) continue;
			
			obj.rigidbody2D.velocity = new Vector2 ( Mathf.Lerp( obj.rigidbody2D.velocity.x, value * 1.5f / size, 0.005f ),
			            Mathf.Lerp( obj.rigidbody2D.velocity.y, Random.Range( minBlowingPower, maxBlowingPower) * 0.5f / size , 0.005f) );
			obj.rigidbody2D.angularVelocity = Random.Range( 800 / size,  1000 / size );
			
			i++;
		}
		
		blowCounter += Time.fixedDeltaTime;
	}
}

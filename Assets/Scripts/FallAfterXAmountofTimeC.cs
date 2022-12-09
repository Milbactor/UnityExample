using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FallAfterXAmountofTimeC : MonoBehaviour {

	LiteTimer fallTimer; 
	public float fallTime = 10f;

	void Start () {
		fallTimer = new LiteTimer(fallTime);
		fallTimer.onElapsed += HandleonElapsed;
		fallTimer.start();
	}
	
	void HandleonElapsed ( LiteTimer timer)
	{
		collider2D.enabled = false;
		gameObject.AddComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

		fallTimer.Update();
		//yes its hard coded
		if(transform.position.y < - 30f)
		{
			gameObject.rigidbody2D.isKinematic = true;
		}
	}
}

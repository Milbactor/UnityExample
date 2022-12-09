using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class IceCorpseFallC : MonoBehaviour {
	
	//LiteTimer fallTimer;
	//float maxTimer = 5f;

	// Use this for initialization
	void Start () {
		//fallTimer = new LiteTimer(maxTimer);
		//fallTimer.onElapsed += OnTimeElapsed;
		collider2D.isTrigger = true;
		renderer.sortingLayerName = "Background";
		renderer.sortingOrder = -1;
	}
	
	// Update is called once per frame
	void Update () {
		//fallTimer.Update();

		if(transform.position.y < -30f)
		{
			rigidbody2D.isKinematic = true;
		}

	}
	
	/*void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player" && fallTimer.playing == false)
		{
			fallTimer.start();
		}
	}

	void OnTimeElapsed(LiteTimer time)
	{
		fallTimer.stop();
		collider2D.isTrigger = true;
	}*/
}

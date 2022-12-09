using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class IcePlateC : MonoBehaviour {

	public GameObject normal;
	public GameObject cracked;
	LiteTimer destroyTimer;
	float maxTimer = 5.0f;
	float y; 

	// Use this for initialization
	void Start () {
		destroyTimer = new LiteTimer(maxTimer);
		destroyTimer.onElapsed += destroyElapsed;
		y = transform.position.y;
	}

	void destroyElapsed( LiteTimer timer)
	{	
		rigidbody2D.gravityScale = 1;
		collider2D.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		destroyTimer.Update();
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		print (col.gameObject.name);
		if(col.gameObject.tag != "Player") return;
		normal.renderer.enabled = false;
		cracked.renderer.enabled = true;
		if(destroyTimer.playing == false)
			destroyTimer.start();
	}
}

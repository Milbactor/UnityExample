using UnityEngine;
using System.Collections;

public class BirdC : GeekBehaviour {

	public float SpeedX = 1.0f;

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		transform.position = new Vector2(transform.position.x + SpeedX * Time.deltaTime ,transform.position.y);

	}




}

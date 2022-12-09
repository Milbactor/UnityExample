using UnityEngine;
using System.Collections;

public class HitboxC : MonoBehaviour {

	public float time = 1f;
	public float damage = 0f;
	public float knockback = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//if( time < 0 ) destroy
	}

	void onTriggerEnter2D(Collision2D info){

	}
}

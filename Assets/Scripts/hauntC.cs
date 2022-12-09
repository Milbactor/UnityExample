using UnityEngine;
using System.Collections;

[RequireComponent( typeof( BoidC ) )]
public class hauntC : MonoBehaviour {

	public float hauntDistance = 5f;
	BoidC boidC;
	// Use this for initialization
	void Start () {
		
		boidC = GetComponent<BoidC>();
	}
	
	// Update is called once per frame
	void Update () {
	
		float distance = Vector3.Distance( transform.position, boidC.target.transform.position );
		if( distance < hauntDistance ) boidC.target.rigidbody2D.velocity *= 0.9f ;
	}
}

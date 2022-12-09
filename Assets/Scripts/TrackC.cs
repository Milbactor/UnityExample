using UnityEngine;
using System.Collections;

public class TrackC : MonoBehaviour {
	
	// Use this for initialization
	public GameObject focusObj;
	public Vector3 offset = Vector3.zero;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		this.transform.position = new Vector3(focusObj.transform.position.x, focusObj.transform.position.y)  + offset;
	}
}

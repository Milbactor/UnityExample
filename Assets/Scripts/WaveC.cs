using UnityEngine;
using System.Collections;

public class WaveC : MonoBehaviour {

	public float speed = 100;
	public float startMovement = 0;
	public float distance = 0.5f;
	private float counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		counter += speed * Time.deltaTime;
		transform.position += new Vector3( Mathf.Sin( counter ) * distance * Time.deltaTime * 100 , 0 );
	}
}

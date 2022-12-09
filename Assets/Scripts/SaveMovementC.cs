using UnityEngine;
using System.Collections;

public class SaveMovementC : MonoBehaviour {
	
	private float timer = 0.0f;
	private float maxTimer = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(timer < maxTimer)
		{
			transform.localPosition = 
				new Vector3(transform.localPosition.x, 
			            transform.localPosition.y 
				            + 0.14f , 
				            0);
			timer += Time.deltaTime;

		}
		else {
			timer = 0.0f;
			this.enabled = false;
		}

	}
}

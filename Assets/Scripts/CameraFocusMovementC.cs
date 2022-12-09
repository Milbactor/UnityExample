using UnityEngine;
using System.Collections;

public class CameraFocusMovementC : GeekBehaviour {

	public GameObject player1;
	public GameObject player2;
	public static CameraFocusMovementC instance;

	Quaternion zero;
	
	void Awake()
	{
		instance = this;
		print("setting camerafocus instance");
	}

	// Use this for initialization
	void Start () {

		zero = Quaternion.Euler(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {

		transform.localPosition 
			= new Vector3( ((player1.transform.localPosition.x + player2.transform.localPosition.x)/2),
			              ((player1.transform.localPosition.y + player2.transform.localPosition.y)/2),0);

		transform.localRotation 
			= zero ;


	}
}

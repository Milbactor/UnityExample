using UnityEngine;
using System.Collections;

public class denilsC : MonoBehaviour {

	denilsC nils;
	public int modifier;
	int counter = 0;
	float value = 0f;
	string owner = "nils";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)){
			counter += 1;
			print (owner + " owns " + counter );

		}


		if (Input.GetKeyDown (KeyCode.KeypadEnter)){
	
			martino (modifier);


		}
	}

	int martino ( int mod) {

		return counter *= mod;

		


	}
}

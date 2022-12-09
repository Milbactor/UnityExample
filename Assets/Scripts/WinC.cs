using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class WinC : MonoBehaviour {
	
	float inputAllowedTime = 7f;
	float timer;

	// Use this for initialization
	void Start ()
	{
		timer = Time.time;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time - timer < inputAllowedTime) return;
		if( Input.anyKeyDown || Input.GetButtonDown( GeekInput.STARTBUTTON + 1 ) )
		{
			Application.LoadLevel( "MainMenu" );
			Destroy(GameObject.Find("PlayerData"));
			Destroy(GameObject.Find("GameStatistics"));
		}
		  
	}

}

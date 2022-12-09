using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SakuraC : GeekLevel {

	public GameObject bridge;
	// Use this for initialization
	void Start () {
	
		base.Start();
		
		if( GameData.bridgeBroken ) bridge.SetActive( false );
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
}

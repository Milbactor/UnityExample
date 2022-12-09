using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SuddenDeath : GeekBehaviour {

	public static string M_ACTIVATE_SUDDEN_DEATH = "ACTIVATE_SUDDEN_DEATH";
	// Use this for initialization
	override protected void Start () {
	
		base.Start();
	
	}
	
	// Update is called once per frame
	override protected void Update () {
	
		base.Update();
	}
	
	public virtual void Activate()
	{
	
	}
}

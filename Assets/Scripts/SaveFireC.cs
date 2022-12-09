using UnityEngine;
using System.Collections;

public class SaveFireC : SaveObject {

	// Use this for initialization
	override protected void Start () {
		
		base.Start() ;
	}

	// Update is called once per frame
	override protected void Update () {
		base.Update();
	}

	//called from animator
	public void DestroySelf()
	{
		Destroy(this.gameObject);
	}



}

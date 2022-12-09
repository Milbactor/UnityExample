using UnityEngine;
using System.Collections;

public class DustC : GeekBehaviour {

	public GameObject dustObject;
	DustScript dust;
	private bool enabled = true;

	// Use this for initialization
	void Start () {
		base.Start();
		dustObject = (GameObject)GameObject.Instantiate(Resources.Load("Dust"));
		if(dustObject == null)
			enabled = false;
		else
			dust = dustObject.GetComponent<DustScript>();

	}
	
	// Update is called once per frame
	void Update () {
	
		base.Update();
	
	}

	//called event in land
	public void EnterDustLand ()
	{
		if(enabled == true)
			dust.EnterDustLand(transform.position);

	}

	//called event in jumpstart
	public void EnterDustStart ()
	{
		if(enabled == true)
			dust.EnterDustStart(transform.position);
	}

	
}

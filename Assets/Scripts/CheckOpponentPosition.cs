using UnityEngine;
using System.Collections;

public class CheckOpponentPosition : MonoBehaviour {

	GameObject owner;
	// Use this for initialization
	void Start () {
		owner = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject != owner && col.gameObject.tag == "Player")
		{
			owner.GetComponent<RawrC>().opponnentIn = true;
		}
		else 
		{
			owner.GetComponent<RawrC>().opponnentIn = false;
		}

	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject != owner && col.gameObject.tag == "Player")
		{
			owner.GetComponent<RawrC>().opponnentIn = false;
		}
		
	}
}

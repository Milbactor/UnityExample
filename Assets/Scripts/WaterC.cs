using UnityEngine;
using System.Collections;

public class WaterC : MonoBehaviour {

	public GameObject splashPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<FellStatusEffect>() == null ) {
			collider.gameObject.GetComponent<MessageDispatcher>().dispatchMessage(StatusC.M_FELL_WATER);
		
			GameObject splash = Instantiate( splashPrefab ) as GameObject ;
			splash.transform.position = collider.transform.position;	
		}
	}
	
}

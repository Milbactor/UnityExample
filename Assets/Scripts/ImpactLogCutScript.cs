using UnityEngine;
using System.Collections;

public class ImpactLogCutScript : MonoBehaviour {
	
	Animator ImpactAnim;
	
	// Use this for initialization
	void Start () {
		
		ImpactAnim = GetComponent<Animator>();
		transform.eulerAngles = new Vector3(0,0,0);
		print ("impactlog");
		SetImpact();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void SetImpact()
	{


	}
	
	//called from animator 
	public void DestroySelf()
	{
		Destroy (this.gameObject);
	}
}

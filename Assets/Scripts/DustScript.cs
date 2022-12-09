using UnityEngine;
using System.Collections;

public class DustScript : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator>();
	}
	


	public void DisableSelf(){
	}



	public void EnterDustLand (Vector3 fixedPosition)
	{
		//anim.enabled = true;
		anim.SetTrigger("Land");
		transform.position = new Vector3 (fixedPosition.x, fixedPosition.y -0.8f, 0);
		
	}
	
	public void EnterDustStart (Vector3 fixedPosition)
	{
		anim.enabled = true;
		anim.SetTrigger("Start");
		transform.position = new Vector3 (fixedPosition.x, fixedPosition.y -1f, 0);
	}
}

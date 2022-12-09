using UnityEngine;
using System.Collections;

public class ImpactScript : MonoBehaviour {

	public Vector3 customScale;
	// Use this for initialization
	void Start () {
		transform.localScale = customScale;
		transform.eulerAngles = new Vector3(0,0,0);
	}

	// Update is called once per frame
	void Update () {

	}
	void LateUpdate()
	{

	}

	//called from animation end frame
	public void DestroyOnEndEnimation()
	{
		Destroy (this.gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class ImpactC : GeekBehaviour {
	
	public static string M_IMPACT = "TriggerImpact";
	public string impactObjName = "ImpactSlash";

	// Use this for initialization
	void Start () {
		addMessageListener((arguments) => SetImpact(), M_IMPACT);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetImpact( )
	{
		GameObject impact = Instantiate(Resources.Load(impactObjName)) as GameObject;
		impact.transform.parent = gameObject.transform;
		impact.transform.localPosition = new Vector3(0,0,0);


	}
}

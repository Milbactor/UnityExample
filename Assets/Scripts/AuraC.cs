using UnityEngine;
using System.Collections;

public class AuraC : GeekBehaviour {
	
	private GameObject aura;
	public static string M_THROW_RECEIVED = "M_THROW_RECEIVED";
	public static string M_AURA_DISABLE = "M_AURA_DISABLE";
	public float maxTimer = 10.0f;
	private float startTimer = 0.0f;
	private Quaternion q;
	private bool AuraEnable = true;
	public Vector2 auraPosition = new Vector2(-0.2f, 0.8f);
	
	// Use this for initialization
	void Start ()
	{
		base.Start ();
		addMessageListener((arguments) => AuraDisable(), M_AURA_DISABLE);
		aura = (GameObject)GameObject.Instantiate(Resources.Load(this.gameObject.name + "Aura"));

		aura.transform.parent = gameObject.transform;
		aura.transform.localPosition = new Vector3(auraPosition.x, auraPosition.y, 0);
		addMessageListener( (arguments) => OnThrowRecieved(), M_THROW_RECEIVED );
		
		q = Quaternion.Euler(0, 0,90);
		aura.SetActive(false);
		startTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if(AuraEnable == false)
		{
			aura.SetActive(false);
		}
		
		if(Time.time - startTimer >= maxTimer)
		{
			if(AuraEnable== true)
				aura.SetActive(true);
			GetComponent<GeekBehaviour>().dispatchMessage( ThrowC.M_THROW_ENABLE );
			
		}
		
		if(aura.activeSelf == true)
		{
			if(anim.GetFloat("L_XAXIS") > 0.2f || anim.GetFloat("L_XAXIS") < -0.2f)
			{
				aura.transform.rotation = q;
				aura.transform.localPosition = new Vector3(-0.8f, 0, 0);
				
			}
			else 
			{
				aura.transform.rotation = Quaternion.identity;
				aura.transform.localPosition = new Vector3(auraPosition.x, auraPosition.y, 0);
				
			}
		}
	}
	
	void OnThrowRecieved()
	{
		//start counting timeer 
		startTimer = Time.time;
		//set the aura off;
		aura.SetActive(false);
		//disable the throw 
		GetComponent<GeekBehaviour>().dispatchMessage( ThrowC.M_THROW_DISABLE );
		
	}
	void AuraDisable()
	{
		AuraEnable = false;
	}
}

using UnityEngine;
using System.Collections;

public class ScrollC : GeekBehaviour {

	bool collected = false;
	public AudioClip pickupSound = null;
	// Use this for initialization
	void Start () {
		
		base.Start();
	}
	
	void Update()
	{
		base.Update();
	}
	
	void Awake()
	{
		base.Awake();
	}
	// Update is called once per frame
	void FixedUpdate () {
	
		//transform.position += new Vector3(0, Mathf.Sin( Time.fixedDeltaTime ) );
	}
	
	void OnTriggerEnter2D( Collider2D col)
	{
		 OnScrollCollect( col );
	}
	
	protected virtual void OnScrollCollect( Collider2D col )
	{
		if( col.gameObject.tag == "Player" && collected == false )
		{
			col.gameObject.GetComponent<MessageDispatcher>().dispatchMessage( InventoryC.M_ON_COLLECT, this );
			dispatchMessage( InventoryC.M_ON_COLLECT, this );
			collected = true;
			
			audio.clip = pickupSound;
			audio.Play( );
			
			print ("picked up");
			//gameObject.SetActive( false );
			//GameObject.Destroy( this.gameObject );
		}
	}
}

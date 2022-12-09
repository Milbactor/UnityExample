using UnityEngine;
using System.Collections;

[RequireComponent( typeof(InventoryC) ) ]
public class UltimateMoveC : GeekBehaviour {

	public static string M_USE_ULTIMATEMOVE = "M_USE_SUPERMOVE";
	public static string M_PLAY_SOUND = "M_PLAY_SOUND";

	InventoryC inventory;
	public bool debug = false;

	public GameObject ThrowHitBoxObject;
	public AudioClip throwSound = null;
	protected Quaternion qRight;
	protected Quaternion qLeft;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		qRight = Quaternion.Euler(0, 180, 0);
		qLeft = Quaternion.Euler(0, 0, 0);
		inventory = GetComponent<InventoryC>();
		addMessageListener((arguments) => UltimateMove(), M_USE_ULTIMATEMOVE);
	}
	

	// Update is called once per frame
	void Update () {
		base.Update();
	}

	void UltimateMove()
	{
		print(inventory.getItemCount( typeof(ScrollC) ));
		int scrollNo = 1;
		if( debug == false ) scrollNo = inventory.getItemCount( typeof(ScrollC) ) ;
		//if( scrollNo < 1 && debug == false) return;
		
		if( inventory.getItemCount( typeof(ScrollC) ) < 1 && debug == false) return;
		anim.SetTrigger( "TriggerUltimateMove" );
		InputDisabledStatus effect = gameObject.AddComponent<InputDisabledStatus>();
		effect.duration = 1.6f;
		CreateUltimateHitBoxObject ( scrollNo ) ;
		//CreateThrowHitBoxObject(inventory.getItemCount( typeof(ScrollC) ));
	}

 	void CreateUltimateHitBoxObject (int scrollNo) 
	{
		Vector3 spawnPosition  = new Vector3(transform.position.x , 
		                                     transform.position.y, 0.0f);

		inventory.removeItem( typeof(ScrollC) ) ;
		
		if( GeekPhysicsC.getFacingDir( this.gameObject ).x >= 0) 
		{
			GameObject newThrowHitBoxObject = Instantiate(ThrowHitBoxObject, spawnPosition, qRight) as GameObject;
			
			newThrowHitBoxObject.GetComponent<ThrowMovementC>().throwDir = ThrowDirection.RIGHT;
			newThrowHitBoxObject.GetComponent<ThrowMovementC>().MoveStart = false;
			newThrowHitBoxObject.transform.parent = transform;
			newThrowHitBoxObject.GetComponent<UltimateMoveHitboxScript>().owner = this.gameObject;

			newThrowHitBoxObject.transform.localScale = 
				new Vector3(newThrowHitBoxObject.transform.localScale.x * scrollNo,
				            newThrowHitBoxObject.transform.localScale.y * scrollNo,
				            0);
			newThrowHitBoxObject.transform.position = 
				new Vector3(transform.position.x - 0.5f, 
				            transform.position.y + newThrowHitBoxObject.collider2D.bounds.size.y / 2, 0.0f);

		}
		
		else if( GeekPhysicsC.getFacingDir( this.gameObject ).x < 0) 
		{

			GameObject newThrowHitBoxObject = Instantiate(ThrowHitBoxObject, spawnPosition, qLeft) as GameObject;
			
			newThrowHitBoxObject.GetComponent<ThrowMovementC>().throwDir = ThrowDirection.LEFT;
			newThrowHitBoxObject.GetComponent<ThrowMovementC>().MoveStart = false;
			newThrowHitBoxObject.transform.parent = transform;
			newThrowHitBoxObject.GetComponent<UltimateMoveHitboxScript>().owner = this.gameObject;

			newThrowHitBoxObject.transform.localScale = 
				new Vector3(newThrowHitBoxObject.transform.localScale.x * scrollNo,
				            newThrowHitBoxObject.transform.localScale.y * scrollNo,
				            0);

			newThrowHitBoxObject.transform.position = 
				new Vector3(transform.position.x + 0.5f, 
				            transform.position.y + newThrowHitBoxObject.collider2D.bounds.size.y / 2, 0.0f);
			
		}

	

		
		
	}


}

using UnityEngine;
using System.Collections;

public class FacingDirectionC : GeekBehaviour {

	public Vector2 _facing = Vector2.one;
	private bool _flipped = false; 
	public Vector2 facing
	{
		get{ 
			if( flipped ) {
				
				return _facing = new Vector2(-1, 0); 
			}
			return _facing = new Vector2(1, 0 );
		}
	}
	
	public bool flipped
	{
		get{ 
			//return _flipped; //playermovementC is not using this anymore
			if( transform.localScale.x >= 0 ) {
				return false ;
			}
			
			return true;
		}
		set{ 
			
			if(value == true) {
				_facing.x = -1;
			}else
			{
				_facing.x = 1;
			}
			_flipped = value;
			
			//SendMessage("onFlipChanged", value, SendMessageOptions.DontRequireReceiver );
			
		}
	}

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	void changeFlip(bool value)
	{
		if( _flipped != value ) {
			//anim.SetBool("Turn", true);
			print ("flip");
			flipped = value;
		}
	}
}

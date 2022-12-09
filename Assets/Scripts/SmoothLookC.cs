using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SmoothLookC : MonoBehaviour {

	public Transform target;
	public float damping = 6f;
	public bool smooth = true;
	
	
	public float maxSize = 15f;
	public float minSize = 10f;	
	///////
	public float sizeInClash = 5.0f;
	public float zoomCoefficient = 0.3f;
	public float distance;
	
	public GameObject player1;
	public GameObject player2;
	Quaternion zero;
	Rect bounds;
	bool shaking = false;
	LiteTimer shaketimer = new LiteTimer (0.1f);
	bool allowResize = true;
	public float zoomSpeed = 0.5f;
	public float moveSpeed = 0.5f;
	
	public static SmoothLookC instance;
	
	void Awake()
	{
		instance = this;
		print ("setting smoothlook instance");
	}
	
	void Start () {

		zero = Quaternion.Euler(0,0,0);
		
		distance = Vector3.Distance(player1.transform.localPosition, player2.transform.localPosition);
		
		bounds = LevelBounds.instance.bounds;
		
		
	}
	public void shake () {
		iTween.ShakePosition (Camera.main.gameObject, new Hashtable{
			{ "time", 0.1f },
			{"amount", new Vector3( 0.3f, 0.3f ) }
		});
		shaketimer.start ();
	}


	void Update () {

		shaketimer.Update ();
		//if(player1.GetComponent<StatusC>().playerWin == true || player2.GetComponent<StatusC>().playerWin == true )
		//{
		////	distance = 0;
		//}
	//	else
	//	{
			distance = Vector3.Distance(player1.transform.localPosition, player2.transform.localPosition);
	//	}
		//transform.localRotation = zero;
	}
	
	void LateUpdate () {
	
		//if (target) { //very bad, spend ages looking what was wrong because there was no error
		if ( shaketimer.playing ) return;
		transform.localPosition = new Vector3(
			Mathf.Lerp(transform.localPosition.x,target.localPosition.x, moveSpeed),
			Mathf.Lerp(transform.localPosition.y,target.localPosition.y, moveSpeed),
			-10
			);
			transform.eulerAngles = new Vector3(0,0,0);
			transform.LookAt(target);
			
			//camera size
			checkBoundaries();
			
			//never set camera size hardcoded, causes for jittery camera movement
			if( allowResize ) camera.orthographicSize = Mathf.Lerp( camera.orthographicSize, distance*zoomCoefficient + minSize, zoomSpeed );

		if(distance*zoomCoefficient + minSize >= maxSize)
			{
				camera.orthographicSize = maxSize;
				//camera.orthographicSize = Mathf.Lerp( camera.orthographicSize, maxSize, zoomSpeed ); //sometimes too slow and you see unityspace
			}
		
		transform.eulerAngles = new Vector3(0,0,0);
	}
	
	void checkBoundaries()
	{
		float height = 2*Camera.main.orthographicSize;
		float width = height*Camera.main.aspect;
		
		bool allowResizeX = false;
		bool allowResizeY = false;
		
		//left boundary
		if( transform.position.x - ( width / 2  ) <= bounds.x - ( bounds.width / 2 ) )
		{
			transform.localPosition = new Vector3( bounds.x - ( bounds.width / 2 ) + (width / 2 ), transform.localPosition.y, -10 );
		}
		//right boundary
		else if ( transform.position.x + ( width / 2  ) >= bounds.x + ( bounds.width / 2 ) )
		{
			transform.localPosition = new Vector3( bounds.x + ( bounds.width / 2 ) - (width / 2 ), transform.localPosition.y, -10 );
		}
		else { allowResizeX = true;}
		
		//top boundary
		if( transform.position.y - ( height / 2  ) <= bounds.y - ( bounds.height / 2 ) )
		{
			transform.localPosition = new Vector3(transform.localPosition.x, bounds.y - ( bounds.height / 2 ) + (height / 2 ), -10 );
		}
		//bottom boundary
		else if ( transform.position.y + ( height / 2  ) >= bounds.y + ( bounds.height / 2 ) )
		{
			transform.localPosition = new Vector3( transform.localPosition.x, bounds.y + ( bounds.height / 2 ) - (height / 2 ), -10 );
		}
		else { allowResizeY = true;}
		
		if( allowResizeX && allowResizeY ) allowResize = true;
	}

}
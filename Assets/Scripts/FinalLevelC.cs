using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FinalLevelC : MonoBehaviour {

	private LiteTimer transformTimer;
	SmoothLookC smoothLook;
	GameObject transformPlayer;
	public GameObject crater;
	
	bool transforming = false;

	// Use this for initialization
	void Start () {
		
		GameManager.instance.players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);
		GameManager.instance.players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);
		
		transformTimer = new LiteTimer(2f);
		transformTimer.start();
		
		print ("level : " + GameData.currentLevel );

		transformTimer.onElapsed += HandleonElapsed;
		
		smoothLook = Camera.main.GetComponent<SmoothLookC>();
		
		transformPlayer = GameManager.instance.players[0].character;
		if( GameData.currentLevel == 1 )
		{
			//transform left player
			transformPlayer = GameManager.instance.players[0].character;
		}
		else
		{
			//transform right player
			transformPlayer = GameManager.instance.players[1].character;
		}

		GameManager.instance.addMessageListener( args => LevelOver(), GameManager.M_LEVEL_OVER );
	}

	void LevelOver()
	{
		print ("level over" );

		GameObject yarn = GameObject.Instantiate( Resources.Load("Yarn") ) as GameObject;
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		transformPlayer.GetComponent<MessageDispatcher>().dispatchMessage( BossTransformC.M_TRANSFORM );
		transformPlayer.GetComponent<MessageDispatcher>().addMessageListener( args => OnTransform((GameObject)args[0]), 
		                                                                     BossTransformC.M_TRANSFORM_COMPLETE );
		                                                                     
		smoothLook.target = transformPlayer.transform;
		smoothLook.zoomCoefficient = 0.3f;
		smoothLook.moveSpeed = 1 * Time.deltaTime;;
		smoothLook.zoomSpeed = 1 * Time.deltaTime;
		
		transforming = true;
		
		crater.SetActive(  true );
		
		/*iTween.ShakePosition (Camera.main.gameObject, new Hashtable{
			{ "time", 5 },
			{"amount", new Vector3( 0.2f, 0.2f ) }
		}); */
	}

	void OnTransform (GameObject transformedObj)
	{
		smoothLook.target = CameraFocusMovementC.instance.gameObject.transform;
		
		GameManager.instance.players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
		GameManager.instance.players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
		
		print (GameManager.instance.players[0]);
		print (GameManager.instance.players[1] );
		
		transforming = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		transformTimer.Update();
		
		if(transformTimer.playing )
		{
			smoothLook.moveSpeed = Mathf.Lerp( smoothLook.moveSpeed,  0.5f, 1 * Time.deltaTime );
			smoothLook.zoomSpeed = Mathf.Lerp( smoothLook.moveSpeed,  0.5f, 1 * Time.deltaTime );
		}
		else if( transforming )
		{
			smoothLook.maxSize = Mathf.Lerp( smoothLook.maxSize, 5, 1 * Time.deltaTime );
			smoothLook.minSize = Mathf.Lerp( smoothLook.maxSize, 5, 1 * Time.deltaTime );
			
			//Camera.main.GetComponent<SmoothLookC>().shake();
		}
		else
		{
			smoothLook.zoomCoefficient = Mathf.Lerp( smoothLook.zoomCoefficient, 0.3f, 1 * Time.deltaTime );
			smoothLook.maxSize = Mathf.Lerp( smoothLook.maxSize, 10, 1 * Time.deltaTime );
			smoothLook.minSize = Mathf.Lerp( smoothLook.maxSize, 7, 1 * Time.deltaTime );
		}
	}
}

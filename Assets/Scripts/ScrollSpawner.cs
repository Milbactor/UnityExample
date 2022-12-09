using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ScrollSpawner : MonoBehaviour {

	private LiteTimer timer;
	public int spawnTimer = 10;
	public int maxSpawn = 3;
	private int spawnCount = 0;
	float ratio = 0.2f;
	
	public Vector3 spawnLocation;
	GameObject bird;
	bool droppedScroll = false;
	
	GameObject[] scrolls;

	int highestScrolls = 0;
	public string nextScroll = "";

	// Use this for initialization
	void Start () {
	
		scrolls = new GameObject[ maxSpawn * 2 ];
		
		timer = new LiteTimer( spawnTimer );
		timer.onElapsed += HandleonElapsed;
		
		if( spawnTimer > 0 )timer.start();
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		bool canSpawn = false;
		highestScrolls = 0;
		foreach( Player player in GameManager.instance.players)
		{
			if( player.character.GetComponent<InventoryC>() == null ) continue;
			
			int playerScrolls = player.character.GetComponent<InventoryC>().getItemCount(typeof(ScrollC) ) ;
			if( highestScrolls < playerScrolls ) highestScrolls = playerScrolls; 
		}
		
		
		//calculate scroll to use
		if(Random.Range(0.0f,1.0f) > ratio )
		{
			nextScroll = "Scroll";
		}
		else
		{
			nextScroll = "SaveMoveScroll";
		}
		
		//calculate position to spawn
		spawnLocation = new Vector3( Random.Range( - LevelBounds.instance.bounds.width / 2f, LevelBounds.instance.bounds.width / 2f),
		                            Random.Range( - LevelBounds.instance.bounds.height / 4f, LevelBounds.instance.bounds.height / 3f ) );
		
		//print ("highest scrolls: " + highestScrolls + " | max: " + maxSpawn );

		//if( highestScrolls < maxSpawn )
		//{
			spawnBird();
			//SpawnScroll();
		//}
		
	}
	
	public void spawnBird()
	{                            
		bird = Instantiate( Resources.Load("Bird") ) as GameObject;
		bird.transform.position = new Vector3( LevelBounds.instance.bounds.center.x + LevelBounds.instance.bounds.width / 4, spawnLocation.y );
		droppedScroll = false;
		
		bird.GetComponent<MessageDispatcher>().addMessageListener((arguments) => OnBondaryEnter((Vector2)arguments[0] ), BoundaryCheck.M_BOUNDARYMESSAGE );
	}
	
	void OnBondaryEnter(Vector2 vec)
	{
		Destroy(bird);
	}
	
	void SpawnScroll()
	{
		GameObject scroll = null;
	
		scroll = (GameObject)GameObject.Instantiate( Resources.Load( nextScroll) );

		if(scroll == null && timer.playing == false){ timer.start(); return;  }
		scroll.GetComponent<MessageDispatcher>().addMessageListener(
			args => OnScrollCollect( (MonoBehaviour)args[0] ), InventoryC.M_ON_COLLECT ) ;
		
		scroll.transform.position = spawnLocation;

	}

	void OnScrollCollect ( MonoBehaviour behaviour)
	{
		//give the scroll some time to play it's sound
		iTween.ScaleTo( behaviour.gameObject, new Hashtable {
			{"scale", Vector3.zero},
			{"time", 1f },
			{"easetype", iTween.EaseType.linear },
			{"onComplete", "scaleComplete" }
		}); 
		
		timer.start();
	}
	
	void scaleComplete()
	{
		scrolls[ spawnCount ].SetActive( false );
	}
	// Update is called once per frame
	void Update () {
		
		//print (timer.time );
		timer.Update();
		
		if( bird != null && bird.transform.position.x < spawnLocation.x && droppedScroll == false)
		{
			droppedScroll = true;
			SpawnScroll();
		}
	}
}

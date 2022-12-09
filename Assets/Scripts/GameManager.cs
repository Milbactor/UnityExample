using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

/// <summary>
/// Game manager. This class contains all the minimum functionality that should happen in each level.
/// </summary>
public class GameManager : GeekBehaviour {

	public static string M_ENABLE_PAUSE = "M_ENABLE_PAUSE";
	public static string M_DISABLE_PAUSE = "M_DISABLE_PAUSE";
	public static string M_OPPENENT_CHANGE = "M_OPPENENT_CHANGE";
	public static string M_LEVEL_OVER = "M_LEVEL_OVER";

	public GameObject defaultHero; 
	public GameObject defaultVillain; 
	public GameObject defaultTest;
	public GameObject fpsText;
	public bool allowLoadNextLevel = false;	// required to wait for winning animation to finish
	public List<Player> players = new List<Player>();

	private PauseGUIC pouseGui;
	LiteTimer drawTimer;

	public static int maxPlayers = 2;
	public List<GameObject> spawnLocations = new List<GameObject>();
	public static GameManager instance = null;

	public Dictionary<int, int> waterCount = new Dictionary<int, int >();
	private LiteTimer screenFreezeTimer;
	private bool boss = false;

	GameObject dataObj; 
	GameStatisticsC stat;

	GameObject playerDataObj;
	PlayerDataC playerDataC;
	public bool loadPlayersFromCharacterSelection = true;

	int[] sides = new int[4]
	{
		-1, 1, -1, 1
	};
	private bool _paused = false;
	
	public bool paused {
		get{ return _paused; }
		set{
			_paused = value;
			if(_paused == true ) {
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}
	
	List<Player> defeatedPlayers
	{
		get {
			List<Player> p = new List<Player>();
			foreach( Player player in players)
			{
				if( player.defeated) p.Add( player );
			}
			
			return p;
		}
	}

	void Awake (){

		LoadGameStatistics();
		LoadPlayerData();

		instance = this;
		drawTimer = new LiteTimer( 2f );
		drawTimer.onElapsed += HandleDrawElapsed;
		
		screenFreezeTimer = new LiteTimer(1f);
		screenFreezeTimer.onElapsed += ScreenFreezeElapsed;
		Time.timeScale = 1f;
		
		if( loadPlayersFromCharacterSelection )
		{
			loadPlayers();
			setupCamera();
			setupPermanents();
		}
	}

	// Use this for initialization
	void Start () {
		
		
		base.Start();
		pouseGui = GetComponent<PauseGUIC>();
		pouseGui.enabled = false;
		addMessageListener( (arguments) => EnablePause(), M_ENABLE_PAUSE);
		addMessageListener( (arguments) => DisablePause(), M_DISABLE_PAUSE);
		
		//caused some issuaes when playing character from scene, instead of an instantiated character
		if( loadPlayersFromCharacterSelection ) addMessageListener( (arguments) => SetDamageData( (GameObject) arguments[0],(string)arguments[1]), DamageC.M_DAMAGE_TYPE);
		if(Application.loadedLevelName != "tutorial")
		{
			LoadSoundPlayer();
		}
		//dataObj = GameObject.Find("GameStatistics");
		stat = dataObj.GetComponent<GameStatisticsC>();
	}

	void ScreenFreezeElapsed ( LiteTimer timer)
	{
		Time.timeScale = 1f;
	}
	
	public void freezeScreen( float time, float speed )
	{
		screenFreezeTimer.duration = time;
		screenFreezeTimer.start();
		Time.timeScale = speed;
	}

	void HandleDrawElapsed ( LiteTimer timer)
	{
		Application.LoadLevel( GameData.levelData[GameData.currentLevel] );
	}

	void setupPermanents ()
	{
		/*foreach( string ghost in GameData.ghosts[ GameData.levelData[GameData.currentLevel ]]  )
		{
		
		} */
	}

	public void Register( GameObject character, int playerID )
	{
		Player player;
		
		if( playerID > players.Count)
		{
			player = new Player( playerID, 1 );
			players.Add( player );
		}
		else
		{
			player = players[playerID];
		}
		
		player.character = character;
	}

	void LoadGameStatistics()
	{
		int cnt = 0;
		foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
		{
			if(gameObj.name.Contains("GameStatistics"))
			{
				dataObj = gameObj;
				cnt++;
				break;
			}
		}
		if(cnt == 0)
		{
			print ("first time for instanting statistics object");
			dataObj = GameObject.Instantiate(Resources.Load("GameStatistics"))as GameObject;
			dataObj.name = "GameStatistics";
		}
	}

	void LoadPlayerData()
	{		
		int cnt = 0;
		foreach (var dataObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
		{
			if(dataObj.name.Contains("PlayerData"))
			{
				playerDataObj = dataObj;
				playerDataC = playerDataObj.GetComponent<PlayerDataC>();
				cnt++;
				
				print ("Current level :" + Application.loadedLevelName + " " + cnt);
				break;
			}
		}
		if(cnt == 0)
		{
			print ("first time for instanting playerdata object");
			playerDataObj = GameObject.Instantiate(Resources.Load("PlayerData"))as GameObject;
			playerDataObj.name = "PlayerData";
			playerDataC = playerDataObj.GetComponent<PlayerDataC>();
		}


	}

	void LoadSoundPlayer()
	{
		int cnt = 0;
		foreach (var dataObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
		{
			if(dataObj.name.Contains("SoundManager"))
			{
				cnt++;
				print ("Current level :" + Application.loadedLevelName + " " + cnt);
				break;
			}
		}
		if(cnt == 0)
		{
			print ("first time for instanting soundplayer object");
			GameObject soundObj = GameObject.Instantiate(Resources.Load("SoundManager")) as GameObject;
			soundObj.name = "SoundManager";
		}
	}

	void loadPlayers()
	{
		print ("setting up players");
		
		for(int i = 0; i < maxPlayers; i++ )
		{
			players.Add( new Player( i, sides[i] ) );
		}
		
		//normally this data would come from character select, but since that's not there yet, we'll just instantiate it here

		if(playerDataC.player1 == ResourceConstants.HERO)
		{
			defaultHero = GameObject.Instantiate( Resources.Load( ResourceConstants.HERO ) ) as GameObject;
			defaultHero.name = "Hero";
		}
		if(playerDataC.player1 == ResourceConstants.HEROINE)
		{
		   defaultHero = GameObject.Instantiate( Resources.Load( ResourceConstants.HEROINE ) ) as GameObject;
			defaultHero.name = "Heroine";
		}

		if(playerDataC.player2 == ResourceConstants.VILLAIN)
		{
			defaultVillain = GameObject.Instantiate( Resources.Load( ResourceConstants.VILLAIN ) ) as GameObject;
			defaultVillain.name = "Villain";
		}
		if(playerDataC.player2 == ResourceConstants.HENCHMAN)
		{
			defaultVillain = GameObject.Instantiate( Resources.Load( ResourceConstants.HENCHMAN ) ) as GameObject;
			defaultVillain.name = "HenchMan";
		}
		
		defaultHero.transform.position = spawnLocations[0].transform.position;
		defaultVillain.transform.position = spawnLocations[1].transform.position;


		defaultHero.GetComponent<PlayerC>().ID = 1; //not 0 because controllers uses ID's starting at 1
		players[0].character = defaultHero;
		players[0].side = sides[0];
		
		players[1].character = defaultVillain;
		players[1].side = sides[1];
		defaultVillain.GetComponent<PlayerC>().ID = 2;

		waterCount.Add( players[0].ID, 0 );
		waterCount.Add( players[1].ID, 0 );
		addListeners();
	}
	
	public GameObject setCharacterForPlayer( int playerID, GameObject obj, bool destroyExisting = true)
	{
		if( players[playerID].character != null && destroyExisting )
		{
			//remove existing listeners
			
			/*MessageDispatcher characterDispatcher = players[playerID].character.GetComponent<MessageDispatcher>();
			characterDispatcher.removeMessageHandler( args => OnLevelBoundsExit( (GameObject)args[0], (Vector2)args[1] ), Portalc.M_ON_LEVEL_BOUNDARIES_PASSED );
			characterDispatcher.removeMessageHandler( args => EnteredWater( (GameObject)args[0]), StatusC.M_ENTERED_WATER  );
			characterDispatcher.removeMessageHandler( args => ShowArrow((GameObject)args[0]), StatusC.M_CREATE_ARROW);
			characterDispatcher.removeMessageHandler( args => AllowLoadNextLevel(), StatusC.M_ALLOW_LOAD_NEXT_LEVEL);
			characterDispatcher.removeMessageHandler( args => OnSaveMoveBounds( args[0] as GameObject), SaveMoveResetC.M_SAVEMOVE_BOUND_PASSED );
			*/
			GameObject.Destroy( players[playerID].character );
		}
		
		players[playerID].character = obj;
		obj.GetComponent<PlayerC>().ID = playerID + 1;
		setListeners( players[playerID] );
		setupCamera();
		foreach(GameObject opponent in GetOtherPlayers(obj))
		{
			opponent.GetComponent<GeekBehaviour>()
				.dispatchMessage(BossStatusC.M_SET_OPPONENT, obj);
		}
		
		boss = true; //ugly, asuimg we only switch to bosses
		return obj;
	}
	
	public Player getPlayerByCharacter( GameObject character )
	{
		foreach( Player player in players )
		{
			if( character == player.character ) return player;
		}
		
		return null;
	}
	

	void addListeners()
	{
		//
		foreach( Player player in players )
		{
			setListeners( player );
		}
	}
	
	void setListeners( Player player )
	{
		MessageDispatcher characterDispatcher = player.character.GetComponent<MessageDispatcher>();
		characterDispatcher.addMessageListener( args => OnLevelBoundsExit( (GameObject)args[0], (Vector2)args[1] ), Portalc.M_ON_LEVEL_BOUNDARIES_PASSED );
		characterDispatcher.addMessageListener( args => EnteredWater( (GameObject)args[0]), StatusC.M_ENTERED_WATER  );
		characterDispatcher.addMessageListener( args => ShowArrow((GameObject)args[0]), StatusC.M_CREATE_ARROW);
		characterDispatcher.addMessageListener( args => AllowLoadNextLevel(), StatusC.M_ALLOW_LOAD_NEXT_LEVEL);
		characterDispatcher.addMessageListener( args => OnSaveMoveBounds( args[0] as GameObject), SaveMoveResetC.M_SAVEMOVE_BOUND_PASSED );
	}

	void AllowLoadNextLevel ()
	{
		allowLoadNextLevel = true;
	}

	void OnSaveMoveBounds (GameObject gameObject)
	{

	}
	

	void EnteredWater (GameObject obj)
	{
		print ("entered the water: " + obj.name );

		StatusC status = obj.GetComponent<StatusC>();
		Player player = getPlayerByCharacter( obj );
		waterCount[ player.ID ] ++;

		DamageC damage = obj.GetComponent<DamageC>();
		if(damage.isKnockback)
		{
			print ("fell because of hit");
			stat.SetFallCounter(player.ID, false);
		}
		else
		{
			print ("fell because of yourself");
			stat.SetFallCounter(player.ID, true);
		}
		//0 ->1 > 1
		if( waterCount[ player.ID ]  > status.saveMoveAmount && player.defeated == false)
		{
			GameObject corpse = null;
			if(Application.loadedLevelName == "level4")
			{
				//waiting for sprites for iceCorpse
				corpse = Instantiate(Resources.Load(player.character.name +  "IceCorpse"  ) ) as GameObject;
				corpse.transform.position = new Vector3(player.character.transform.position.x,
				                                        player.character.transform.position.y , 0);
				corpse.AddComponent<IceCorpseFallC>();
				GameData.addIceCorpse(player.character.name + "IceCorpse");
				//corpse = Instantiate( Resources.Load( "Corpse" + player.character.name ) ) as GameObject;
			}
			else
			{
				corpse = Instantiate( Resources.Load( "Corpse" + player.character.name ) ) as GameObject;
				corpse.transform.position = new Vector3(player.character.transform.position.x,
				                                        player.character.transform.position.y , 0);
				bool choice = false;
				if(Random.Range(0.0f,1.0f) > 0.5f) choice = true;
				corpse.GetComponent<Animator>().SetBool("choice",choice);
				
				Corpse corpseData = new Corpse(player.character.transform.position.x,
				                               player.character.transform.position.y, player.character.name, choice);
				GameData.addCorpse(corpseData);
			}

			//die
			Death( player );
		}
	}

	void Death( Player player){

		//print ("death");

		GameObject obj = player.character;


		DamageC damage = obj.GetComponent<DamageC>();
		if(damage.isKnockback)
		{
			//print ("death because of hit");
			stat.SetDeathCounter(player.ID, false);
		}
		else
		{
			//print ("death because of yourself");
			stat.SetDeathCounter(player.ID, true);
		}

		//inform the other plyaer that this player is dead 
		GameObject otherPlayer = GetOtherPlayers( player.character )[0];
		otherPlayer.GetComponent<StatusC>().OnOtherPlayerDeathInformed();
		
		//inform the game controller that this player is dead
		if(player.character.name == "Hero" || player.character.name == "Villain" || player.character.name == "Heroine" || player.character.name == "HenchMan" )
		{
			Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
			GameObject ghost = Instantiate( Resources.Load( player.character.name + "Ghost") ) as GameObject;
			ghost.transform.position = player.character.transform.position + new Vector3(0, 3);
			GameData.addGhost( player.character.name + "Ghost" );
			
			GameObject ghostOwner = GameObject.Find( ghost.name.Split('G')[0] );
			GameObject character = GameManager.instance.GetOtherPlayers( ghostOwner)[0];
			ghost.GetComponent<BoidC>().target = character;
			ghost.GetComponent<BoidC>().targetName = character.name;

			/*GameObject corpse = Instantiate( Resources.Load( "Corpse" + player.character.name ) ) as GameObject;
			corpse.transform.position = player.character.transform.position;
			Corpse corpseData = new Corpse(corpse.transform.position.x, corpse.transform.position.y, player.character.name);
			GameData.addCorpse(corpseData);*/
		}
		player.defeated = true;
		player.character.GetComponent<MessageDispatcher>().dispatchMessage(StatusC.M_DIED );
		
		Camera.main.GetComponent<SmoothLookC>().target = otherPlayer.transform;

		//not the right solution but status effect is not working correctly
		if(!obj.name.Contains("Boss"))
		{
			obj.GetComponent<SlashC>().StopChargeParticle();
		}
		obj.GetComponent<PlayerC>().enabled = false;
	
		foreach( Player p in players)
		{
			if( p.defeated == false ) return;
		}

		drawTimer.start();

		//NO CODE ALLOWED BELOW HERE
	}



	void OnLevelBoundsExit(GameObject obj, Vector2 direction, bool ignoreKnockback = false)
	{
		foreach( Player p in players )
		{
			//GameObject.DontDestroyOnLoad(player.character);
			//GameObject.DontDestroyOnLoad( this.gameObject );
		}
		
		Player player = getPlayerByCharacter( obj);
//		print ("entered by direction: " + direction.x + " | player side: " + player.side + " knocback: " + obj.GetComponent<DamageC>().InKnockback );

		//knocked into boundary 
		if(  obj.GetComponent<DamageC>().InKnockback || ignoreKnockback == true)// later change like in DamageC
		{
			if( /*direction.x == player.side &&*/ player.defeated == false)
			{
				print ("player.id : " + player.ID + " defeated" );
				Death( player );
				//obj.transform.position += new Vector3( obj.transform.localScale.x * 10, 0 );
				obj.renderer.enabled = false;
				obj.AddComponent<InputDisabledStatus>();

				if(!obj.name.Contains("Boss"))
				{
					obj.GetComponent<SlashC>().StopChargeParticle();
					obj.GetComponent<MessageDispatcher>().dispatchMessage(AuraC.M_AURA_DISABLE);
				}
				//obj.SetActive( false );
				obj.GetComponent<StatusC>().DefeatStandardProcedure();
				dispatchMessage( M_LEVEL_OVER );
			}	
		}
		
		//if a player is already dead
		else if( defeatedPlayers.Count > 0 && 
		        player.defeated == false && 
		        player.side != direction.x && 
		        allowLoadNextLevel == true)
		{
			LoadNextLevel( (int)direction.x );
			allowLoadNextLevel = false;	//this makes sure increment of currentlevel index once in a level even if the winner is siding his wall persistantly
		}

		print ("boss: " + boss );
		//if( boss) LoadNextLevel( (int)direction.x );
		//Application.LoadLevel( GameData.levelData[ GameData.currentLevel] );

	}
	
	public void LoadNextLevel( int direction)
	{
		GameData.currentLevel += direction;

		print ("loading level with index: " + GameData.currentLevel + " , " +  GameData.levelData[ GameData.currentLevel] );
		Application.LoadLevel( GameData.levelData[ GameData.currentLevel] );
	}
	

	void setupCamera ()
	{
		print ("setting up camera");
		CameraFocusMovementC.instance.player1 = players[0].character;
		CameraFocusMovementC.instance.player2 = players[1].character;
		
		Camera.main.GetComponent<SmoothLookC>().target = CameraFocusMovementC.instance.gameObject.transform;
		Camera.main.GetComponent<SmoothLookC>().player1 = players[0].character;
		Camera.main.GetComponent<SmoothLookC>().player2 = players[1].character;
		//SmoothLookC.instance.player1 =  players[0].character;
		//SmoothLookC.instance.player2 =  players[1].character;
	}




	

	void EnablePause()
	{
		paused = true;
		pouseGui.enabled = true;
		AudioListener.pause = true;
		players[0].character.GetComponent<PlayerC>().enabled = false;
		players[1].character.GetComponent<PlayerC>().enabled = false;
		//players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);
		//players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);
	}

	void DisablePause()
	{
		paused = false;
		pouseGui.enabled = false;
		AudioListener.pause = false;
		players[0].character.GetComponent<PlayerC>().enabled = true;
		players[1].character.GetComponent<PlayerC>().enabled = true;
		//players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
		//players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
	}

	public bool isPause()
	{
		return _paused;
	}
	
	// Update is called once per frame
	void Update () {

		drawTimer.Update();
		screenFreezeTimer.Update();
		
		for(int i = 0; i < players.Count; i++)
		{
			if ( Input.GetButtonUp(GeekInput.STARTBUTTON + (i + 1))  && _paused == false) {
				EnablePause();


			}
			else if( _paused == true && Input.GetButtonUp( GeekInput.STARTBUTTON + (i + 1))) 
			{
				DisablePause();


			}
		}
		
		if(Input.GetKeyUp (KeyCode.Escape)) {
			if (_paused == true) {
				DisablePause();

			}else {

				EnablePause(); 
			}
		}

		if (Input.GetKeyUp (KeyCode.Return) && _paused ){
			DisablePause();
		}

		if( Input.GetKeyUp(KeyCode.F1) )
		{
			fpsText.SetActive( !fpsText.activeSelf );
		}

		
	}
	

	public List<GameObject> GetOtherPlayers( GameObject self )
	{
		List< GameObject> otherPlayers = new List<GameObject>();
		foreach( Player obj in players )
		{
			if( obj.character != self) otherPlayers.Add(obj.character );
		}
		
		return otherPlayers;
	}

	public List<Player> GetOtherPlayersObjs( GameObject self )
	{
		List< Player > otherPlayers = new List<Player>();
		foreach( Player obj in players )
		{
			if( obj.character != self) otherPlayers.Add(obj );
		}
		
		return otherPlayers;
	}

	void ShowArrow(GameObject obj)
	{
		int side = getPlayerByCharacter( obj ).side;
		if( side < 0 )
		{
			GameObject.Instantiate( Resources.Load( "HeroArrow") );
		}
		else if( side > 0 )
		{
			GameObject.Instantiate( Resources.Load( "VillainArrow") );
		}
		//temp solution, new function required for this.
		Player player = getPlayerByCharacter(obj);
		stat.SetWinCounter(player.ID );
	}

	void SetDamageData(GameObject playerObj, string hitBox)
	{
		if( playerObj.tag != "Player") return;
		print ("set damage data " + hitBox);
		Player player = getPlayerByCharacter(playerObj);

		if( hitBox == "Slash")
		{
			stat.SetSlashCounter(player.ID );
		}
		if( hitBox.Contains("Shoot"))
		{
			stat.SetShootCounter(player.ID);
		}
		if( hitBox.Contains("Ultimate"))
		{
			stat.SetUltimateCounter(player.ID);
		}

	}
	

}
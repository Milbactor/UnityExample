using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class NetworkManager : MonoBehaviour {

	private HostData[] hostData = null;
	bool joining = false;
	bool creating = false;
	bool refreshing = false;
	
	string serverName = "";
	string serverPass = "";
	string serverInfo = "";
	string gameName = "Samudai";
	
	public static bool connected = false;
	
	int playerCounter = 1;
	
	string clientPass = "";
	Vector2 scrollPosition = Vector2.zero;
	// Use this for initialization
	void Start () {
		
		//refreshHostList();
	}
	
	// Update is called once per frame
	void Update () {
		
		if( refreshing )
		{
			if( MasterServer.PollHostList().Length > 0 ) hostData = MasterServer.PollHostList();
			refreshing = false;
			
			print ("refresihing host list");
		}
	}
	
	void OnGUI()
	{
		if( GUILayout.Button("Browse Games") )
		{
			joining = true;			
		}

		if( GUILayout.Button("Host Game") )
		{
			joining = false;
			creating = true;
		}
		
		if( GUILayout.Button("Add Player" ) || Input.GetKey(KeyCode.P) || Input.GetButton("Start_" + playerCounter) )
		{
			GameObject player = (GameObject)GameObject.Instantiate( Resources.Load("Samudai" ) );
			player.GetComponent<PlayerC>().ID = playerCounter;
			playerCounter++;
		}
		
		if( GUILayout.Button("Add AI" )  || Input.GetKey(KeyCode.O) || Input.GetButton("Back_" + 1) )
		{
			GameObject.Instantiate( Resources.Load("AI" ) );
		}
		
		
		if( joining == true )
		{
			showGameList();
			/*GUI.Window(0, new Rect( Screen.width / 4 ,
			           Screen.height / 6,
			           Screen.width / 1.5f ,
			           Screen.height / 2) ,
			           showGameList, "Game list");
			*/
			           
		}
		else if( creating)
		{
			showCreation();
		}
		
		
		
	
	}
	
	void OnPlayerConnected( NetworkPlayer player)
	{
		GeekGUI.timedWindow("new player connected", 1 );
		//GameObject obj = Network.Instantiate( Resources.Load("Samudai"), Vector3.zero, Quaternion.identity, 1 ) as GameObject;
		//obj.GetComponent<SpriteRenderer>().color = Color.red;
		
	}
	
	void spawnPlayer()
	{
	
	}
	
	void OnPlayerDisconnected( NetworkPlayer player)
	{
		GeekGUI.timedWindow("player disconnected", 1 );
		Network.RemoveRPCs( player );
		Network.DestroyPlayerObjects( player );
	}
	
	void OnServerInitialized()
	{
		GeekGUI.timedWindow( "Server initialized", 2 );
		connected = true;
		
		GameObject player = Network.Instantiate( Resources.Load("Samudai"), Vector3.zero, Quaternion.identity, 0 ) as GameObject;
		
		//GameObject.Find("CameraFocus").GetComponent<CameraFocusMovementC>().player1 = player;
		//GameObject.Find("CameraFocus").GetComponent<GeekBehaviour>().dispatchMessage(PlayersListC.ADD_PLAYER);
		//GameObject.Find("Main Camera").GetComponent<SmoothLookC>().player1 = player;
	}
	
	void OnApplicationQuit()
	{
		if( Network.isServer )
		{
			Network.Disconnect(200 );
			MasterServer.UnregisterHost();
		}
		
		else if( Network.isClient )
		{
			Network.Disconnect( );
		}
	}
	
	/*public static NetworkPlayer getNetworkPlayer( NetworkPlayer player )
	{
		NetworkView[] views = Resources.FindObjectsOfTypeAll<NetworkView>();
		foreach( NetworkView view in views )
		{	
			if( view.owner == player )
			{
				return view.owner;
			}
		}
		
		return NetworkPlayer
	}*/
	
	
	public static NetworkView getView( NetworkPlayer player )
	{
		NetworkView[] views = Resources.FindObjectsOfTypeAll<NetworkView>();
		foreach( NetworkView view in views )
		{	
			if( view.owner == player )
			{
				return view;
			}
		}
		
		return null;
	}
	
	[RPC]
	void allocateP2ViewID(NetworkPlayer player, NetworkViewID viewID)
		
	{
		getView( player ).viewID = viewID;
		
	}
	
	void OnConnectedToServer() 
	{
		GeekGUI.timedWindow( "connection succesfull", 2 );
		print ("succesfully connected to server");
		
		connected = true;
		
		// generate a new ID, scene client objects don't get a correct one, which causes isMine to always return false
		/*NetworkView viw = getView( Network.player );
		NetworkViewID viewID = Network.AllocateViewID();
		
		// set it locally
		networkView.viewID = viewID;
		
		// synchronize the server
		networkView.RPC("allocateP2ViewID", RPCMode.Server, viewID); //still doesn't work
		*/
		
		//exisiting scene objects are owned by the server, so we need respawn our player
		if( Network.isClient ) Network.Destroy( GameObject.Find("Hero") );
		//GameObject.Destroy( GameObject.Find("Hero") );
		GameObject player = Network.Instantiate( Resources.Load("Samudai"), Vector3.zero, Quaternion.identity, 0 ) as GameObject;
		
		//GameObject.Find("CameraFocus").GetComponent<CameraFocusMovementC>().player1 = player;

		//GameObject.Find("CameraFocus").GetComponent<GeekBehaviour>().dispatchMessage(PlayersListC.ADD_PLAYER);
		//GameObject.Find("Main Camera").GetComponent<SmoothLookC>().player1 = player;
	}

	void showGameList (/*int windowID = 0*/)
	{
		if( hostData != null )
		{
			GUI.Box( new Rect (Screen.width / 4 ,
			                  Screen.height / 8,
			                  Screen.width / 1.5f ,
			                  Screen.height / 1.5f), "game list");
			                  
			scrollPosition = GUI.BeginScrollView (new Rect (Screen.width / 4 ,
			                                                Screen.height / 6,
			                                                Screen.width / 1.5f ,
			                                                Screen.height / 2)
			                                                ,scrollPosition, new Rect (0, 0, 300, 1000 / hostData.Length * 30));
		
			GUI.Label(new Rect(30,0,100,20),"Game Name");
			GUI.Label(new Rect(120,0,100,20),"Server Info");
			GUI.Label(new Rect(200,0,100,20),"Player Count");
			GUI.Label(new Rect(300,0,100,20),"Password");
			
			for(int i = 0; i < hostData.Length; i++) {
				
				GUI.Label(new Rect(30,30 + i * 30,200,22),hostData[i].gameName);
				GUI.Label(new Rect(120,30 + i * 30,500,22),hostData[i].comment);
				GUI.Label(new Rect(200,30 + i * 30,100,20),hostData[i].connectedPlayers + " / " + hostData[i].playerLimit);
				
				if (hostData[i].passwordProtected){
						
					clientPass = GUI.PasswordField (new Rect (300,30 + i * 30,100,25), clientPass, "*"[0], 12);
				}
				else
				{
					GUI.Label(new Rect(300,30 + i * 30,100,20), "no password");
				}
					
				if (GUI.Button(new Rect(400,30 + i * 30,100,25),"Join")) {
						
					NetworkConnectionError error = Network.Connect(hostData[i], clientPass);
					joining = false;
					if( error != null ) 
					{
						print ("no network error");
					}
				}
					
			}
			
			GUI.EndScrollView ();
		}
		else
		{
			
			GUI.Label(new Rect(100, 50, 150, 50) ,"No Games Found.");
			
			if (GUI.Button(new Rect( 100, 100, 100, 20),"Refresh 2x")) {
				
				refreshHostList();
			}
			else if( GUI.Button(new Rect( 100, 120, 100, 20),"Back")) 
			{
				joining = false;
			}
		}
	}

	void showCreation ()
	{
		if (GUI.Button(new Rect(Screen.width/2 - 50,Screen.height/3 + 110,100,50),"Create")) {
			
			startServer();
			creating = false;
		}
		
		GUI.Label(new Rect (Screen.width/2 - 110,Screen.height/3,100,20),"Server Name:");
		GUI.Label(new Rect (Screen.width/2 + 40,Screen.height/3,100,20),"Password:");
		GUI.Label(new Rect (Screen.width/2 - 30,Screen.height/2 + 90,100,20),"Server Info:");
		
		serverName = GUI.TextField (new Rect (Screen.width/2 - 120,Screen.height/3 + 30,100,20), serverName, 12);
		serverPass = GUI.PasswordField (new Rect (Screen.width/2 + 20,Screen.height/3 + 30,100,20), serverPass, "*"[0], 12);
		serverInfo = GUI.TextArea (new Rect (Screen.width/2 - 70,Screen.height/2 + 120,150,40), serverInfo, 35);
		
		if (GUI.Button(new Rect(Screen.width/1.2f, Screen.height/  20 , 100, 20),"Back")) {
			
			creating = false;
		}
	}
	
	void startServer()
	{
		if (serverPass != ""){
			
			Network.incomingPassword = serverPass;
		}
		
		Network.InitializeServer(4, 8632, true );
		MasterServer.RegisterHost( gameName, serverName, serverInfo);
		
		print ("Starting server");
	}
	
	void refreshHostList () {
		
		//MasterServer.ClearHostList();
		MasterServer.RequestHostList(gameName);
		refreshing = true;
	}
	
	
}

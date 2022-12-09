using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MenuScript : MonoBehaviour {

	int selectedIndex = 0;

	public GUIStyle background;
	public Texture startButton;
	public Texture quitButton;
	//public Texture optionButton;
	public GUIStyle start;
	//public GUIStyle option;
	public GUIStyle quit;
	public float textRatio = 0.5f; 
	public Vector2 startButtonPos;
	//public Vector2 optionButtonPos;
	public Vector2 quitButtonPos;
	public Vector2 startButtonSize;

	bool canSelectJoystick = true;
	private Dictionary<GUIStyle, bool> selectedDictionary = new Dictionary<GUIStyle, bool>(); 
	public GUISkin gSkin;

	// Use this for initialization
	void Start () {

		selectedDictionary.Add(start, true);
		//selectedDictionary.Add(option, false);
		selectedDictionary.Add(quit, false);
	}

		
	GUIStyle GetDictionaryByIndex(int index)
	{
		int cnt = 0;
		foreach(KeyValuePair <GUIStyle, bool> pair in selectedDictionary)
		{
			if(cnt == index)
				return pair.Key;

			cnt++;
		}
		return null;
	}


	// Update is called once per frame
	void Update () 
	{

		for(int i = 1; i <= 1; i++)
		{
		
			GUIStyle selectedGuiSyle = GetDictionaryByIndex(selectedIndex);


			if ( ( Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + i) > 0.7f && canSelectJoystick ) ||  Input.GetKeyDown(KeyCode.S)) {

				print ("UP selectedIndex before change: " + selectedIndex );
				selectedDictionary[selectedGuiSyle] = false;
				selectedIndex--;
				
				if(selectedIndex < 0)
				{
					//selectedIndex = 0;
					selectedIndex = selectedDictionary.Keys.Count - 1;
				}
				
				selectedGuiSyle = GetDictionaryByIndex(selectedIndex);
				selectedDictionary[selectedGuiSyle]  = true;

				print ("UP selectedIndex after change: " + selectedIndex );
				
				canSelectJoystick = false;
			}
			else
			
			
			if ( (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + i) < -0.7f && canSelectJoystick ) || Input.GetKeyDown(KeyCode.W)) {

				print ("DOWN selectedIndex before change: " + selectedIndex );
				selectedDictionary[selectedGuiSyle] = false;
				selectedIndex++;
				if(selectedIndex >= selectedDictionary.Count)
				{
					selectedIndex = 0;
					//selectedIndex = selectedDictionary.Count -1;
				}
				selectedGuiSyle = GetDictionaryByIndex(selectedIndex);
				selectedDictionary[selectedGuiSyle] = true;
				
				print ("DOWN selectedIndex after change: " + selectedIndex );
				
				canSelectJoystick = false;
			}
			
	
			
			if( Input.GetButtonDown( GeekInput.ABUTTON + i ) || Input.GetButtonDown (GeekInput.STARTBUTTON+i) || Input.GetKeyDown (KeyCode.Return))
			{
				if(selectedGuiSyle == start )
				{
					Application.LoadLevel( "CharacterSelect" );
				}
//				else if ( selectedGuiSyle == option )
//				{
//				
//				}
				else if( selectedGuiSyle == quit )
				{
					Application.Quit();
				}
			}

			//print ("GUISTYLE IS: " + selectedGuiSyle);
		}

		if( Mathf.Abs( Input.GetAxis( GeekInput.LEFT + GeekInput.YAXIS + 1) ) == 0.0f && Mathf.Abs( Input.GetAxis( GeekInput.LEFT + GeekInput.YAXIS + 2) ) == 0.0f)
		{
			canSelectJoystick = true;
		}

	}

	void OnGUI ()
	{

		if (gSkin)
			GUI.skin = gSkin;
		else
			Debug.Log("Error NO GUI SKIN");


		GUI.Label (
			new Rect ( 
		          0, 
		          0, 
		          Screen.width, 
		          Screen.height), 
			"", 
			background
			);

		//void Draw(Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus);


		if (Event.current.type == EventType.Repaint)
		{

			bool selected = selectedDictionary[start];

	
			start.Draw
			(new Rect(Screen.width/2 + startButtonPos.x, Screen.height/2 + startButtonPos.y, startButton.width*textRatio, startButton.height*textRatio),
				 selected, false, false, false 
			 );
			 
			/*selected = selectedDictionary[option];
			
			option.Draw
				(new Rect(Screen.width/2 + optionButtonPos.x, Screen.height/2 + optionButtonPos.y, 	optionButton.width*textRatio, optionButton.height*textRatio),
				 selected, false, false, false
				 );*/
			bool isWebPlayer =
				(Application.platform == RuntimePlatform.OSXWebPlayer
				 || Application.platform == RuntimePlatform.WindowsWebPlayer);

			if(isWebPlayer == false){
			
				selected = selectedDictionary[quit];
				quit.Draw
					(new Rect(Screen.width/2 + quitButtonPos.x, Screen.height/2 + 	quitButtonPos.y, quitButton.width*textRatio, quitButton.height*textRatio),
					 selected, false, false, false
					 );
			}

		}

	}

}

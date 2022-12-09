using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System.Reflection;

public class DialogueSystem : MonoBehaviour {

	public GameObject ui_root;
	public static DialogueSystem instance;
	[System.Serializable]
	public class DialogueMessage
	{
		public string character;
		
		[Multiline]
		public string dialogue;
		public GameObject portrait;
		public PORTRAIT_ALIGNMENT portraitAlignment;
		public AudioClip sound;
		public string[] transitionEvents;
		public GameObject eventObj;
		
		public float transitionTime = 0f;
		
		public TutorialMain OnStartTarget;
		public string OnStart = "";
		
		
	}
	

	public enum PORTRAIT_ALIGNMENT
	{
		LEFT, 
		RIGHT
	}

	[System.Serializable]
	public class Dialogue
	{
		public string characterCombination;
		
		GameObject dialogueBox;
		public List<DialogueMessage> dialogueLines;
		int currentLine = 0;
		GameObject label;
		private LiteTimer transitionTimer;
		bool active = false;
		bool writeComplete = false;
		
		public Dictionary<string, int> eventCounter = new Dictionary<string, int>();
		
		
		public void Start()
		{
			//base.Start();
			
			dialogueBox = Instantiate( Resources.Load("DialogueBox") ) as GameObject;
			//dialogueBox = NGUITools.AddChild( DialogueSystem.instance.ui_root,  Resources.Load("DialogueBox") as GameObject );
			
			label = dialogueBox.transform.Find("Label").gameObject;
			label.GetComponent<UILabel>().text = dialogueLines[ currentLine].dialogue;
			
			GameObject portraitHolder = dialogueBox.transform.FindChild("portrait").gameObject;
			//portrait.GetComponent<SpriteRenderer>().sprite = dialogueLines[currentLine].portrait;
			
			//GameObject portrait = Instantiate( dialogueLines[ currentLine].portrait ) as GameObject;
			GameObject portrait = NGUITools.AddChild( portraitHolder, dialogueLines[ currentLine].portrait );
			//portrait.transform.parent = portraitHolder.transform;
			//portrait.transform.localPosition = Vector3.zero;
			portrait.transform.localScale = new Vector3(50, 50 );
			
			transitionTimer = new LiteTimer( dialogueLines[currentLine].transitionTime );
			transitionTimer.onElapsed += NextDialogue;
			
			label.GetComponent<GeekTypeWriter>().onElapsed += HandleonElapsed;
			
			active = true;
			
			if( dialogueLines[ currentLine].portraitAlignment == PORTRAIT_ALIGNMENT.RIGHT ){
	
				print ( "show portrait right" );
				portrait.transform.position = new Vector2( 618, portrait.transform.position.y );
				
			}
			
			if( dialogueLines[ currentLine ].sound != null ){
				DialogueSystem.instance.audio.clip = dialogueLines[ currentLine].sound;
				DialogueSystem.instance.audio.Play();
			}
			
		
			label.GetComponent<GeekTypeWriter>().onElapsed += WritingComplete;
		}

		void WritingComplete (GeekTypeWriter writer)
		{
			//currentLine++;
			writeComplete = true;
			print ( "transition events length: " + dialogueLines[ currentLine ].transitionEvents.Length + 
			       " event obj : " +  dialogueLines[ currentLine].eventObj );
			
			if( dialogueLines[ currentLine ].transitionEvents.Length > 0 && dialogueLines[ currentLine].eventObj != null)
			{
				foreach( string transitionEvent in dialogueLines[ currentLine ].transitionEvents)
				{
					print ("[dialoguesystem] add event [" + transitionEvent + " ] to gameobject " + dialogueLines[ currentLine].eventObj );
					dialogueLines[ currentLine].eventObj.GetComponent<MessageDispatcher>().addMessageListener( 
					                                                                                          args => OnEvent( ), transitionEvent );
					
					eventCounter.Add( transitionEvent, 0 );
				}
			}
			
			//call function
			if( dialogueLines[currentLine].OnStart != "")
			{
				print ("onStart function: " + dialogueLines[currentLine].OnStart + " onStartTarget : " + dialogueLines[currentLine].OnStartTarget );
				MethodInfo info = dialogueLines[currentLine].OnStartTarget.GetType().GetMethod( dialogueLines[currentLine].OnStart) as MethodInfo;
				info.Invoke( dialogueLines[currentLine].OnStartTarget , new object[0] );
				
				//needs to be public
			}
		}
		
		void OnEvent()
		{
			if(writeComplete == false) return;
			
			string eventType = MessageDispatcher.lastEvent;
			if( eventCounter.ContainsKey( eventType ) == false ) return; //only listen to keys that have been registered in the eventcounter
			//since we don't remove the message listereners
			
			print ("dialogueSystem received event: " + eventType );
			eventCounter[eventType]++;
			
			foreach( string transitionEvent in dialogueLines[ currentLine ].transitionEvents)
			{
				if( eventCounter[transitionEvent] == 0 ) return;
			}
			NextDialogue( null );
		}
		
		public void Update()
		{
			//base.Update();
			if( active ) transitionTimer.Update();
		}

		void HandleonElapsed (GeekTypeWriter writer)
		{
			if( transitionTimer.duration > 0 )transitionTimer.start();
		}
		
		void NextDialogue( LiteTimer timer )
		{
			eventCounter.Clear();
			currentLine++;
			writeComplete = false;
			
			//print ("currentLine: " + currentLine + " maxLines: " + dialogueLines.Count );
			
			if( currentLine < dialogueLines.Count ){
				
				label.GetComponent<UILabel>().text = dialogueLines[ currentLine].dialogue;
				label.GetComponent<GeekTypeWriter>().reset();
				GameObject portraitHolder = dialogueBox.transform.FindChild("portrait").gameObject;
				
				GameObject.Destroy( portraitHolder.transform.GetChild(0).gameObject );
				GameObject portrait = NGUITools.AddChild( portraitHolder, dialogueLines[ currentLine].portrait );
				portrait.transform.localScale = new Vector3(50, 50 );
				//portraitHolder.GetComponent<SpriteRenderer>().sprite = dialogueLines[currentLine].portrait;
				
				transitionTimer.duration = dialogueLines[ currentLine].transitionTime;
				if( transitionTimer.duration > 0 ) transitionTimer.start();
				
				if( dialogueLines[ currentLine].portraitAlignment == PORTRAIT_ALIGNMENT.RIGHT ){
					
					print ( "show portrait right" );
					portrait.transform.position = new Vector3( 618f, portrait.transform.position.y );
					//GameObject portrait = Instantiate( dialogueLines[ currentLine].portrait ) as GameObject;
					//GameObject portrait = NGUITools.AddChild( portraitHolder, dialogueLines[ currentLine].portrait );
					//portrait.transform.parent = portraitHolder.transform;
					//portrait.transform.localPosition = Vector3.zero;
					
				}
				
				if( dialogueLines[ currentLine].sound != null ){
					DialogueSystem.instance.audio.clip = dialogueLines[ currentLine].sound;
					DialogueSystem.instance.audio.Play();
				} 
				
			}
			
			
			
			else
			{
				GameObject.Destroy( dialogueBox );
			}
		}
	}
	
	public Dialogue[] dialogues;    
	// Use this for initialization
	void Start () {
		
		instance = this;
		
		string characterCombination = "";
		foreach( Player player in GameManager.instance.players )
		{
			characterCombination += player.character.name;
		}
		
		List<Dialogue> dialoguesForCombi = getDialoguesforCombination( characterCombination);
		
		print ("found dialogues for: " + characterCombination + " : " + dialoguesForCombi.Count );
		int randomDialogue = Mathf.FloorToInt( Random.value * dialoguesForCombi.Count);
		Dialogue dialogue = dialoguesForCombi[ randomDialogue ];
		dialogue.Start();
	}
	
	private List<Dialogue> getDialoguesforCombination( string combination)
	{
		List<Dialogue> queriedDialogues = new List<Dialogue>();
		foreach( Dialogue dialogue in dialogues )
		{
			if(dialogue.characterCombination == combination || dialogue.characterCombination == "*")
				queriedDialogues.Add( dialogue );
		}
		
		return queriedDialogues;
	} 
	
	// Update is called once per frame
	void Update () {
		
		foreach( Dialogue dialogue in dialogues )
		{
			dialogue.Update();
		}
	}
}

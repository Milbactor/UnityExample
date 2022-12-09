using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MainWaterFall : GeekLevel  {

	public GameObject bird;
	public DialogueSystem dialogueSystem;

	void Awake()
	{
		if( GameData.dialoguePlayed ) dialogueSystem.gameObject.SetActive( false );
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();

		bird.GetComponent<MessageDispatcher>().addMessageListener((arguments) => OnBondaryEnter((Vector2)arguments[0] ), BoundaryCheck.M_BOUNDARYMESSAGE );

		Application.targetFrameRate = 60;
		GameManager.instance.players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);
		GameManager.instance.players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_DISABLE);

		GameData.dialoguePlayed = true;
	}
	
	void OnBondaryEnter(Vector2 vec)
	{
		Destroy(bird);
	
		GameManager.instance.players[0].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
		GameManager.instance.players[1].character.GetComponent<MessageDispatcher>().dispatchMessage(PlayerC.M_INPUT_ENABLE);
	}
}

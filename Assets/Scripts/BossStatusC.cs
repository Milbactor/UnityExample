using UnityEngine;
using System.Collections;

public class BossStatusC : StatusC {

	public static string M_SET_OPPONENT = "M_SET_OPPONENT";

	protected override bool UsingSuperMove
	{
		get{ return false ; }
	}

	
	// Use this for initialization
	void Start () {
		base.Start ();
		addMessageListener( (args) => OnDeath(), M_DIED );
	}


	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(
		   opponentLost == true && 
		   playerWin == false && 
		   usingSaveMove == false 
		   )
		{
			WinProcedure(); 
		}

	}

	public override void DefeatStandardProcedure()
	{
		//WRITE TO DO IN BOSS WHEN BOSS LOST 
	}

	protected override void WinProcedure()
	{
		playerWin = true;
		GetComponent<MessageDispatcher>().dispatchMessage(M_CREATE_ARROW, this.gameObject);

		GetComponent<MessageDispatcher>().dispatchMessage(M_ALLOW_LOAD_NEXT_LEVEL);
		//WRITE TO DO IN BOSS WHEN BOSS WIN
	}

	protected override void OnDeath ()
	{
		dispatchMessage( PlayerC.M_INPUT_DISABLE );
	}
}

using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class StatusC : GeekBehaviour {
	
	
	public static string M_CREATE_ARROW = "M_CREATE_ARROW";
	public static string M_FELL_WATER = "M_FELL_WATER";
	public static string M_ENTERED_WATER = "M_ENTERED_WATER";
	public static string M_DIED = "M_DIED";
	public static string M_ALLOW_LOAD_NEXT_LEVEL = "M_ALLOW_LOAD_NEXT_LEVEL";
	public bool saveUseAllowed = true; 
	public int saveCount = 0;
	public int saveMoveAmount = 1;
	protected bool opponentLost = false;
	protected bool alreadyEntered = false; 
	public bool playerWin = false;
	public bool usingSaveMove = false; //while being saved winning animation need to wait
	private float saveTimer = 0.0f; //TODO use lighttimer, ask martino how to call death related messages in gamemanager 
	public float maxSaveTimer = 10.0f;
	private BoxCollider2D col;
	public AudioClip waterSound = null;
	public AudioClip[] CatSounds = null;                   
	public AudioClip WinSound = null;
	private bool fellWater = false;
	private bool WinAnimFinished = false;

	float saveMoveResetHeight = 10f;
	public List<GeekBehaviour> deathDisabledComponents = new List <GeekBehaviour>();
	

	protected bool grounded
	{
		get{ return anim.GetBool(AnimatorConstants.GROUNDED); }
	}

	protected virtual bool UsingSuperMove
	{
		//get{ return GetComponent<SuperMoveC>().UsingSuperMove; }
		get{ return false;}
	}

	

	
	// Use this for initialization
	void Start () 
	{
		base.Start();
		
		addMessageListener( (args) => OnSaveObjectInstantiate(), SaveObjectC.M_INSTANTIATE_SAVE_OBJECT);
		addMessageListener((arguments) => { if(PlayerFell()) { dispatchMessage(SaveObjectC.M_INSTANTIATE_SAVE_OBJECT); } }, JumpC.M_JUMP_IMPULSE);
		addMessageListener( (args) => OnDeath(), M_DIED );
		addMessageListener( (args) => OnWaterFell(), M_FELL_WATER);
		addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE );
		//addMessageListener( args => OnSaveMoveBounds( args[0] as GameObject), SaveMoveResetC.M_SAVEMOVE_BOUND_PASSED);
		
		col = GetComponent<BoxCollider2D>();
	}
	
	public void OnSaveMoveBounds (GameObject gameObject)
	{ 
		dispatchMessage( SaveMoveResetC.M_SAVEMOVE_BOUND_PASSED, this.gameObject );
		alreadyEntered = false;
		fellWater = false;
		usingSaveMove = false;
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		base.Update();

		if(saveCount >= saveMoveAmount) // 0->1 > 1
		{
			saveUseAllowed = false; //next fall saveUse not allowed anymroe in this case
		}
		else
		{
			saveUseAllowed = true;
		}
		
	/*	if(usingSaveMove == true && tf.position.y >   saveMoveResetHeight )
		{
			OnSaveMoveBounds(this.gameObject);
		}*/
		
		//if other player lost, set the winning anim true,
		if(grounded == true &&
		   opponentLost == true && 
		   playerWin == false && 
		   anim.GetBool(AnimatorConstants.DEAD) == false && 
		   UsingSuperMove == false && 
		   usingSaveMove == false 
		   )
		{
			WinProcedure(); 
		}
		
		
	}


	
	
	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("Life related.Win") && WinAnimFinished == false)
		{
			WinAnimFinished = true;
			GetComponent<MessageDispatcher>().dispatchMessage(M_CREATE_ARROW, this.gameObject);
			GetComponent<MessageDispatcher>().dispatchMessage(M_ALLOW_LOAD_NEXT_LEVEL);
		}
	}
	
	
	
	void OnWaterFell ()
	{
		fellWater = true;
		if(alreadyEntered == false){
			dispatchMessage( M_ENTERED_WATER, this.gameObject ); 
			playWaterSound();
			DefeatStandardProcedure();
			FellStatusEffect fell = gameObject.AddComponent<FellStatusEffect>();
			//print ("saveCount: " + saveCount + " , saveMouveAmount: " + saveMoveAmount );
		}

		
	}
	
	protected virtual void OnDeath ()
	{
		dispatchMessage( PlayerC.M_INPUT_DISABLE );
		dispatchMessage( AuraC.M_AURA_DISABLE);
		DefeatStandardProcedure();
		saveUseAllowed = false;

	}
	
	//called any ways of defeat(smashed on wall, fell water once, fell water twice etc)
	public virtual void DefeatStandardProcedure()
	{
		if(alreadyEntered == false)
		{
			playCatSound();
			float corpseType = Random.Range (0.0f, 1.0f);
			if(corpseType <= 0.5f)
			{
				corpseType = 0.0f;
			}
			else
			{
				corpseType = 1.0f;
			}
			anim.SetFloat("CorpseType", corpseType);
			anim.SetBool(AnimatorConstants.DAMAGED, false);
			anim.SetTrigger(AnimatorConstants.DIE);
			anim.SetBool(AnimatorConstants.DEAD, true);
			
		}
		//its required in order to avoid duplicate calls 
		alreadyEntered = true;
		
	}
	
	
	
	
	
	void OnSaveObjectInstantiate()
	{
		saveCount++;

		GetComponent<GeekBehaviour>().dispatchMessage(GameHUD.M_RESET_DISHONOR_RECIEVED);
	}
	
	
	void playWaterSound ()
	{
		if( waterSound != null )
		{
			audio.PlayOneShot(waterSound);
			//audio.clip = waterSound;
			//audio.Play();
		}
	}
	
	public void playCatSound ()
	{
		int index = (int)Random.Range (0, CatSounds.Length );
		if( CatSounds[index] != null  )
		{
			audio.PlayOneShot( CatSounds[index] );
			//audio.clip = CatSound;
			//audio.Play();
		}
	}
	
	public void playWinSound ()
	{
		if( WinSound != null  )
		{
			audio.PlayOneShot(WinSound);
			//audio.clip = CatSound;
			//audio.Play();
		}
	}
	
	protected virtual void WinProcedure()
	{
		anim.SetTrigger(AnimatorConstants.WIN);
		playerWin = true;
		playWinSound ();
		WinStatusEffect win = gameObject.AddComponent<WinStatusEffect>();
		win.duration = 10f;
	}
	
	
	
	
	
	//make function which is informed of other player's loss
	public void OnOtherPlayerDeathInformed()
	{
		opponentLost = true;
		
	}
	
	public bool PlayerFell()
	{
		//  print ( "fellwater: " + fellWater + "saveAllowed " +  saveUseAllowed);
		if(fellWater && saveUseAllowed == true)
			return true;
		else
			return false;
	}



	
	
	
}
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerC : GeekBehaviour {

	public int ID = 1;
	public bool inputEnabled = true;
	public static string M_INPUT_DISABLE = "InputDisable";
	public static string M_INPUT_ENABLE = "InputEnable";
	

	protected bool xInput = false;
	protected bool yInput = false;

	float launchPower = 120f;

	// Use this for initialization
	override protected void Start () {
		base.Start();
		addMessageListener((arguments) => InputEnable(), M_INPUT_ENABLE);
		addMessageListener((arguments) => InputDisable(), M_INPUT_DISABLE);
		
		/*// generate a new ID, scene client objects don't get a correct one, which causes isMine to always return false
		if( networkView != null )
		{
			NetworkViewID viewID = Network.AllocateViewID();
	
			// set it locally
			networkView.viewID = viewID;
			
			// synchronize the server
			
			networkView.RPC("allocateP2ViewID", RPCMode.Server, viewID);
		
		} */
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(anim.GetBool(AnimatorConstants.DEAD) == true)
		{
			anim.SetBool(AnimatorConstants.BLOCKED, false);
		}
		//setAnim ();

		//print ( "client: " + Network.isClient + " | server : " + Network.isServer);
		//if( networkView != null )// print ( "isMine: " + networkView.isMine );
		if (inputEnabled == true ) 
		{
			if( networkView == null || NetworkManager.connected == false) {
				HandleInput ();
				setAnim ();
			}
			else if( networkView.isMine == true )
			{
				HandleInput();
				//setAnim (); //very expensive
			}
		}


	
	
	}

	//dont know which player is best to put this function
	public virtual void OnLaunch()
	{

		SendMessage("LaunchStart");
		//rb2D.velocity = new Vector2(0, launchPower);
		anim.SetBool (AnimatorConstants.DEAD, false);
		GetComponent<StatusC>().usingSaveMove = true; // need better solutino to access this variable
		GetComponent<DamageC>().enabled = true;
		//rigidbody2D.velocity = new Vector2(0,50);
		gameObject.AddComponent<SaveMoveStatusEffect>();
	}

	
	protected virtual void setAnim ()
	{
		
		anim.SetFloat (AnimatorConstants.L_XAXIS, Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID));
		anim.SetFloat (AnimatorConstants.L_YAXIS, Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID));
		/*anim.SetFloat ("L_ANGLE", GeekInput.GetStickAngle( GeekInput.LEFT,  ID));
		anim.SetFloat ("L_MAGNITUDE", GeekInput.GetStickVector( GeekInput.LEFT,  ID).magnitude);*/
	}

	protected void HandleInput ()
	{
		checkStick();
		checkButtons();

	}

	protected virtual void checkStick ()
	{
		if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID) > 0.2f)
		{
			dispatchMessage( PlayerMovementC.MOVE_RIGHT);
			if(xInput == false )anim.SetBool(AnimatorConstants.X_INPUT, true);
			
			xInput = true;
		}

		else if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID) < -0.2f) {
			
			dispatchMessage(PlayerMovementC.MOVE_LEFT);
			if(xInput == false ) anim.SetBool(AnimatorConstants.X_INPUT, true);
			xInput = true;
		} 
		else if( xInput == true )
		{
			xInput = false;
			anim.SetBool(AnimatorConstants.X_INPUT, false);
		}
		
		if (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) > 0.2f) {
			if( yInput == false 
			   )anim.SetBool (AnimatorConstants.Y_INPUT, true);

			dispatchMessage(IgnoreCollisionC.M_YAXIS);
			yInput = true;
				
		} else if (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) < -0.2f) {


			if( yInput == false )anim.SetBool (AnimatorConstants.Y_INPUT, true);
			if(GetComponent<ContactStateC>().grounded == false)
				dispatchMessage(IgnoreCollisionC.M_YAXIS);
			yInput = true;
			
		} else if( yInput == true ) {
			anim.SetBool (AnimatorConstants.Y_INPUT, false);
			dispatchMessage(IgnoreCollisionC.M_YAXIS_RELEASED);
			yInput = false;
		}
		
	}

	protected virtual void checkButtons ()
	{
		if (Input.GetButtonDown(GeekInput.ABUTTON + ID)) {
		
			//SendMessage( JumpC.M_JUMP_IMPULSE, SendMessageOptions.DontRequireReceiver );
			dispatchMessage( JumpC.M_JUMP_IMPULSE );

		}
		
		if(Input.GetButton (GeekInput.ABUTTON + ID )){

			dispatchMessage( JumpC.M_JUMP_SUSTAIN );
		}
		//* 
		if(Input.GetButtonUp (GeekInput.ABUTTON + ID )){
			
			dispatchMessage( JumpC.M_JUMP_FINISH );
		}
		
		if (Input.GetButtonDown (GeekInput.YBUTTON + ID)) {

			//print (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID));
			if(Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) > 0.2f)
			{
				dispatchMessage(ThrowC.M_THROW_UP);	
			}
			else
			{
				dispatchMessage(ThrowC.M_THROW);
			}

			// it does not react well, the function is called from animation
		} /*else {
			
			dispatchMessage(ThrowC.M_THROW_EXIT);
		}*/
		
		
		if( Input.GetButtonDown( GeekInput.XBUTTON + ID )  )
		{
			dispatchMessage(SlashC.M_SLASH_START );

		}
		else if(Input.GetButton( GeekInput.XBUTTON + ID ))
		{

		}
		else if (Input.GetButtonUp(GeekInput.XBUTTON + ID) ) {
			
			dispatchMessage (SlashC.M_SLASH);
			print ("slash button released");

		}
		else {
			// if button is not pressed at all make sure all flags are set off to avoid stuck.
			anim.SetBool("SlashCharge", false);
			//dispatchMessage(BlockC.M_BLOCK_EXIT);

		}
		

		if( Input.GetButtonDown( "LB_" + ID ) ){
			dispatchMessage(BlockC.M_BLOCK);
			
		}
		
		if( Input.GetButtonDown( "Back_" + ID ) )
		{
			print ("back pressed");
			//dispatchMessage(BossTransformC.M_TRANSFORM );
			//HeroBoss
		}

		
		if( Input.GetButtonDown( "RB_" + ID ) )
		{
			dispatchMessage(UltimateMoveC.M_USE_ULTIMATEMOVE );
		}
		
		if (Input.GetButtonDown (GeekInput.BBUTTON + ID) )
		{
			dispatchMessage( DashC.M_ON_DASH_ACTIVATE, new Vector2( Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID) ,
			                                                       Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) ) );
		}
		

	}

	//


	void InputEnable()
	{
		inputEnabled = true;

	}

	void InputDisable ()
	{
		inputEnabled = false;
	}
}

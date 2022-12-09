using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BossPlayerC : PlayerC  {
	
	// Use this for initialization
	override protected void Start () {
		base.Start();

	}
	
	// Update is called once per frame
	void Update () {

		if (inputEnabled == true ) 
		{
			if( networkView == null || NetworkManager.connected == false) {
				HandleInput ();
				setAnim ();
			}
			else if( networkView.isMine == true )
			{
				HandleInput();
			}
		}
		
	}


	protected override void setAnim ()
	{	
	}
	
	
	protected override void checkStick ()
	{
		if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID) > 0.2f)
		{
			dispatchMessage( BossPlayerMovementC.MOVE_RIGHT);
			xInput = true;
		}
		
		else if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + ID) < -0.2f)
		{
			dispatchMessage(PlayerMovementC.MOVE_LEFT);
			xInput = true;
		} 
		else if( xInput == true )
		{
			xInput = false;
			dispatchMessage(BossPlayerMovementC.PAUSE_HORIZONTAL);
		}
		
		if (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) < -0.2f) 
		{
			dispatchMessage(BossPlayerMovementC.MOVE_UP);
			yInput = true;
			
		} else if (Input.GetAxis (GeekInput.LEFT + GeekInput.YAXIS + ID) > 0.2f) 
		{
			yInput = true;
			dispatchMessage(BossPlayerMovementC.MOVE_DOWN);
			
		} else if( yInput == true ) {
			yInput = false;
			dispatchMessage(BossPlayerMovementC.PAUSE_VERTICAL);
		}
		
	}
	
	protected override void checkButtons ()
	{
		if (Input.GetButtonDown(GeekInput.ABUTTON + ID)) {
			
		}
		
		if(Input.GetButton (GeekInput.ABUTTON + ID ))
		{

		}
		if(Input.GetButtonUp (GeekInput.ABUTTON + ID )){
		
		}
		
		if (Input.GetButtonDown (GeekInput.YBUTTON + ID)) 
		{
			dispatchMessage(RawrC.M_RAWR);
		}
			

		
		if( Input.GetButtonDown( GeekInput.XBUTTON + ID )  )
		{
			dispatchMessage(FireBallShootC.M_SHOOT);
		}
		else if(Input.GetButton( GeekInput.XBUTTON + ID ))
		{
			
		}
		else if (Input.GetButtonUp(GeekInput.XBUTTON + ID) ) {

		}
		else {

		}
		
		if( Input.GetButtonDown( "Back_" + ID ) )
		{
			print ("back pressed");
			//dispatchMessage(BossTransformC.M_TRANSFORM );
			//HeroBoss
		}
		
		
		if( Input.GetButtonDown( "LB_" + ID ) ){
			
		}

		
		if( Input.GetButtonDown( "RB_" + ID ) )
		{
		}

	
	}
	


}

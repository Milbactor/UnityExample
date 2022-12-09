using UnityEngine;
using System.Collections;

public class HenchmansHatC : GeekBehaviour {

	public GameObject hat;
	bool throwDone = false;

	// Use this for initialization
	void Start () {


		if(Application.loadedLevelName == "demo") 
		{
			GetComponent<Animator>().SetBool("HenchmansHatThrowAllowed", true);
			addMessageListener( args => OnEnterAnimationState( (int) args[0], (int) args[1]), M_ENTER_ANIMATION_STATE );
		}
		else 
		{
			GetComponent<Animator>().SetBool("HenchmansHatThrowAllowed", false);
		}
	}


	void OnEnterAnimationState (int prevHash, int curHash)
	{
		if(prevHash == Animator.StringToHash("JumpState.hatThrow@henchman") && throwDone == false )
		{
			GetComponent<Animator>().SetBool("HenchmansHatThrowAllowed", false);
			GameObject newHat = Instantiate( hat )as GameObject;
			newHat.transform.position = new Vector3 (
				 transform.position.x + 2* GeekPhysicsC.getFacingDir(this.gameObject).x,
				transform.position.y + 1, 0);
			newHat.GetComponent<HatMovementC>().scaleX = GeekPhysicsC.getFacingDir(this.gameObject).x;
			throwDone = true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}

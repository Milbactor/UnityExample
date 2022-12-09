using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BlockC : GeekBehaviour{

	public static string M_BLOCK = "Block";
	public static string M_BLOCK_EXIT = "BlockExit";
	
	public AudioClip blockSound = null;

	/*public bool blocked
	{
		get{ return anim.GetBool(AnimatorConstants.BLOCKED) ; }
	}*/

	public void playBlockSound ()
	{
		if( blockSound != null )
		{
			audio.clip = blockSound;
			audio.Play();
		}
	}

	// Use this for initialization
	void Start () {
		base.Start();
		addMessageListener((arguments) => Block(), M_BLOCK );
		addMessageListener((arguments) => BlockExit(), M_BLOCK_EXIT );

	}
	
	// Update is called once per frame
	void Update () {

	}

	void Block()
	{
		anim.SetBool (M_BLOCK, true);
		//GetComponent<SlashC>().enabled = false;
	}
	
	void BlockExit()
	{
		anim.SetBool (M_BLOCK, false);
		//GetComponent<SlashC>().enabled = true;
	}
}

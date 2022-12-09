using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PressAnyButton : GeekBehaviour {

	bool startPressed = false;
	public AudioClip pressSound;

	UILabel text;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(text == null)
		{
			text = GameObject.Find("DialogueBox(Clone)").transform.FindChild("Label").GetComponent<UILabel>();
		}
		if(text.text != "Press any button to return Main menu.")return;
		if(Input.anyKeyDown == true && startPressed == false)
		{
			playPressSound ();
			startPressed = true;
			Application.LoadLevel("MainMenuNew");
		}
	
	}

	public void playPressSound ()
	{
		if( pressSound != null )
		{
			audio.clip = pressSound;
			audio.Play();
		}
	}
}

using UnityEngine;
using System.Collections;

public class ButtonEventHanderC : MonoBehaviour {

	public string btnName; 
	public string nextSceneName;
	public AudioClip selectSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonSelected(GameObject btn)
	{

		playSelectSound ();
		if(btn.name.Equals("") || btn.name.Equals("Exit"))
		{
			bool isWebPlayer =
				(Application.platform == RuntimePlatform.OSXWebPlayer
				 || Application.platform == RuntimePlatform.WindowsWebPlayer);
			
			if(isWebPlayer == false)
			{
				Application.Quit();
			}
		}
		else if(btn.name.Equals(btnName))
		{
			Application.LoadLevel(nextSceneName);
		}
	}

	public void playSelectSound ()
	{
		if( selectSound != null )
		{
			audio.clip = selectSound;
			audio.Play();
		}
	}
}

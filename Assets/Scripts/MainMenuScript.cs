using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {


	public GameObject menuObj;
	public GameObject startTxt;
	bool startPressed = false;
	public AudioClip pressSound;
	public AudioClip bgm;

	// Use this for initialization
	void Start () {
	
		if(	SoundPlayer.instance.isPlayBGM() == false)
		{
			SoundPlayer.instance.playBGM(bgm.name,0,true);
		}
		menuObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(menuObj.activeSelf == false)
		{
			if(Input.anyKeyDown == true && startPressed == false)
			{
				menuObj.SetActive(true);
				startTxt.SetActive(false);
				playPressSound ();
				startPressed = true;
			}
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

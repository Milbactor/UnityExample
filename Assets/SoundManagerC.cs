using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManagerC : MonoBehaviour {

	public List<AudioClip> BGMList;
	float fadeTimer = 10f;
	int index = 0;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	// Use this for initialization
	void Start () 
	{

		if(BGMList.Count == 0) 
		{
			//play default sound ;
		}
		SoundPlayer.instance.playBGM(BGMList[index].name, 0, true );
	}
	
	// Update is called once per frame
	void Update () {
		SoundPlayer.instance.updateBGM();
	}

	//event 

	public void ChangeBGM()
	{
		index++;
		if(index > BGMList.Count) index = 0;
		SoundPlayer.instance.stopBGM(fadeTimer);
		SoundPlayer.instance.playBGM(BGMList[index].name, fadeTimer, true );
	}
}

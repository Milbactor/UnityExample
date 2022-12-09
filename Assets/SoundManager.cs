using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class SoundManager : MonoBehaviour {

	public List<AudioClip> BGMList;
	Dictionary<string, int> levelBGMData; // combination of level name and index of BGMlist
	float fadeTimer = 10f;
	int index = 0;

	public static SoundManager instance = null;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this.gameObject);
		levelBGMData = new Dictionary<string, int>();
		SetLevelBGMData();
	}
	void SetLevelBGMData()
	{
		//dont know which sount to be used so no structure can be implemented
		levelBGMData.Add(GameData.levelData[1], 0);// beach
		levelBGMData.Add(GameData.levelData[2], 1);// bamboo
		levelBGMData.Add(GameData.levelData[3], 1);//level2
		levelBGMData.Add(GameData.levelData[4], 1);//waterfall
		levelBGMData.Add(GameData.levelData[5], 1);//lava
		levelBGMData.Add(GameData.levelData[6], 1);//icelake
		levelBGMData.Add(GameData.levelData[7], 0);//summit
		//levelBGMData.Add(GameData.levelData[5], 0);// level3 

	}
	// Use this for initialization
	void Start () 
	{
		if(BGMList.Count == 0) 
		{
			//play default sound ;
		}
		SoundPlayer.instance.playBGM(BGMList[0].name, 0, true );
	}
	
	// Update is called once per frame
	void Update () {
		SoundPlayer.instance.updateBGM();
	}

	//event 
	public void ChangeBGMByIndex(int newIndex)
	{
		if(newIndex > BGMList.Count || newIndex < 0)  return;
		SoundPlayer.instance.stopBGM(fadeTimer);
		SoundPlayer.instance.playBGM(BGMList[index].name, fadeTimer, true );
		index = newIndex;
	}

	public void ChangeBGM(AudioClip audio)
	{
		SoundPlayer.instance.stopBGM(fadeTimer);
		SoundPlayer.instance.playBGM(audio.name, fadeTimer, true );
	}
	
	public void ChangeBGMByLevel(string levelName)
	{
		if(index == levelBGMData[levelName]) return;
		SoundPlayer.instance.stopBGM(fadeTimer);
		SoundPlayer.instance.playBGM(BGMList[levelBGMData[levelName]].name, fadeTimer, true );
	}


}

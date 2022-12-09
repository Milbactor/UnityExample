using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPlayer {
	
	public const float BGM_VOLUME = 0.5f;
	
	private static SoundPlayer _instance= null;
	
	public static SoundPlayer instance {
		get{
			if (_instance == null) {
				_instance = new SoundPlayer();
			}
			return _instance;
		}
	}
	
	SoundPlayer() {
	}
	
	AudioSource audioSource;
	Dictionary<string, AudioClipInfo> audioClips = new Dictionary<string, AudioClipInfo>();
	
	BGMPlayer curBGMPlayer;
	BGMPlayer fadeOutBGMPlayer;
	
	private string beforeSe = null;
	
	// AudioClip information
	class AudioClipInfo {
		public string resourceName;
		public string name;
		public AudioClip clip;
		public float startTime;
		
		public AudioClipInfo( string resourceName, string name ) {
			this.resourceName = resourceName;
			this.name = name;
		}
	}
	
	public void CacheClear() {
		audioClips.Clear ();
	}
	

	public void playBGM( string bgmName, float fadeTime ) {
		SetBGMPlayer(bgmName,fadeTime, true);
	}
	
	public void playBGM( string bgmName, float fadeTime , bool loopflag) {
		SetBGMPlayer(bgmName,fadeTime, loopflag);
	}
	
	void SetBGMPlayer( string bgmName, float fadeTime ,bool loopflag) {
		bgmName = "BGM/" + bgmName;
		// destory old BGM
		if ( fadeOutBGMPlayer != null )fadeOutBGMPlayer.destory();
		
		// change to fade out for current BGM
		if ( curBGMPlayer != null ) {
			curBGMPlayer.stopBGM( fadeTime );
			fadeOutBGMPlayer = curBGMPlayer;
		}
		
		// play new BGM
		if (!audioClips.ContainsKey( bgmName )) {
			// null BGM
			Debug.Log("new bgm play-----------------");
			curBGMPlayer = new BGMPlayer(bgmName,loopflag);
			curBGMPlayer.playBGM( fadeTime );
			audioClips.Add( bgmName, new AudioClipInfo( bgmName, bgmName) );
		} else {
			Debug.Log("bgm play-----------------");
			curBGMPlayer = new BGMPlayer( audioClips[ bgmName ].resourceName , loopflag);
			curBGMPlayer.playBGM( fadeTime );
		}
	}
	
	
	
	public void playBGM() {
		
		if ( curBGMPlayer != null && !curBGMPlayer.hasFadeOut())curBGMPlayer.playBGM();
		if ( fadeOutBGMPlayer != null && !curBGMPlayer.hasFadeOut())fadeOutBGMPlayer.playBGM();
	}
	
	public void pauseBGM() {
		if ( curBGMPlayer != null )curBGMPlayer.pauseBGM();
		if ( fadeOutBGMPlayer != null )fadeOutBGMPlayer.pauseBGM();
	}
	
	public void stopBGM( float fadeTime ) {
		if ( curBGMPlayer != null )curBGMPlayer.stopBGM( fadeTime );
		if ( fadeOutBGMPlayer != null )fadeOutBGMPlayer.stopBGM( fadeTime );
	}
	
	public void updateBGM(){
		if ( curBGMPlayer != null )curBGMPlayer.update();
		if ( fadeOutBGMPlayer != null )fadeOutBGMPlayer.update();
	}
	
	public bool isLoopBGM(){
		if ( curBGMPlayer != null )return curBGMPlayer.IsBgmLoop();
		if ( fadeOutBGMPlayer != null )return fadeOutBGMPlayer.IsBgmLoop();
		
		return false;
	}

	public float GetBgmCurrentTime() {
		if (curBGMPlayer != null)
			return curBGMPlayer.GetTime ();
		return 0;
	}
	
	public float GetBgmLength() {
		if (curBGMPlayer != null)
			return curBGMPlayer.GetTimeLength ();
		return 0;
	}
	
	public bool isPlayBGM()
	{
		return (curBGMPlayer != null);
	}

	
	
}











using UnityEngine;
using System.Collections;


class BGMPlayer {
	
	GameObject obj;
	AudioSource source;
	State state;
	float fadeInTime = 0.0f;
	float fadeOutTime = 0.0f;
	
	bool fadeoutFlag = false;
	bool loopbgmflag = false;
	
	public BGMPlayer() {
		
	}
	
	
	
	// State
	class State {
		protected BGMPlayer bgmPlayer;
		public State( BGMPlayer bgmPlayer ) {
			this.bgmPlayer = bgmPlayer;
		}
		public virtual void playBGM() {}
		public virtual void pauseBGM() {}
		public virtual void stopBGM() {}
		public virtual void update() {}
	}
	
	class Wait : State {
		public Wait( BGMPlayer bgmPlayer ) : base( bgmPlayer ) {}
		
		public override void playBGM() {
			
			if ( bgmPlayer.fadeInTime > 0.0f )bgmPlayer.state = new FadeIn( bgmPlayer );
			else bgmPlayer.state = new Playing( bgmPlayer );
		}
	}
	
	
	class Playing : State {
		public Playing( BGMPlayer bgmPlayer ) : base( bgmPlayer ) {
			if ( bgmPlayer.source.isPlaying == false ) {
				bgmPlayer.source.volume = SoundPlayer.BGM_VOLUME;
				bgmPlayer.source.Play();
			}
		}
		
		public override void pauseBGM() {
			bgmPlayer.state = new Pause( bgmPlayer, this );
			bgmPlayer.loopbgmflag = false;
		}
		
		public override void stopBGM() {
			bgmPlayer.state = new FadeOut( bgmPlayer );
			bgmPlayer.loopbgmflag = false;
		}
		
		public override void update(){
			bgmPlayer.loopbgmflag = bgmPlayer.source.isPlaying;
		}
	}
	
	
	class Pause : State {
		State preState;
		
		public Pause( BGMPlayer bgmPlayer, State preState ) : base( bgmPlayer ) {
			this.preState = preState;
			bgmPlayer.source.Pause();
		}
		
		public override void stopBGM() {
			bgmPlayer.source.Stop();
			bgmPlayer.state = new Wait( bgmPlayer );
		}
		
		public override void playBGM() {
			bgmPlayer.state = preState;
			bgmPlayer.source.Play();
		}
	}
	
	
	class FadeIn : State {
		float t = 0.0f;
		
		public FadeIn( BGMPlayer bgmPlayer ) : base( bgmPlayer ) {
			bgmPlayer.source.Play();
			bgmPlayer.source.volume = 0.0f;
		}
		
		public override void pauseBGM() {
			bgmPlayer.state = new Pause( bgmPlayer, this );
		}
		
		public override void stopBGM() {
			bgmPlayer.state = new FadeOut( bgmPlayer );
		}
		
		public override void update() {
			
			t += Time.deltaTime;
			bgmPlayer.source.volume = SoundPlayer.BGM_VOLUME * (1.0f -  t / bgmPlayer.fadeInTime);
			if ( t >= bgmPlayer.fadeInTime ) {
				bgmPlayer.source.volume = SoundPlayer.BGM_VOLUME;
				bgmPlayer.state = new Playing( bgmPlayer );
			}
		}
	}
	
	
	class FadeOut : State {
		float initVolume;
		float t = 0.0f;
		
		public FadeOut( BGMPlayer bgmPlayer ) : base( bgmPlayer ) {
			initVolume = bgmPlayer.source.volume;
		}
		
		public override void pauseBGM() {
			bgmPlayer.state = new Pause( bgmPlayer, this );
		}
		
		public override void update() {
			
			t += Time.deltaTime;
			bgmPlayer.source.volume = initVolume * ( 1.0f - t / bgmPlayer.fadeOutTime );
			if ( t >= bgmPlayer.fadeOutTime ) {
				bgmPlayer.source.volume = 0.0f;
				bgmPlayer.source.Stop();
				bgmPlayer.state = new Wait( bgmPlayer );
				
				bgmPlayer.fadeoutFlag = false;
			}else{
				//fadeing out 
				bgmPlayer.fadeoutFlag = true;
			}
		}
	}
	
	
	public BGMPlayer(string bgmFileName){
		SetBGMPlayer(bgmFileName, true);
	}
	
	public BGMPlayer(string bgmFileName , bool loopflag){
		SetBGMPlayer(bgmFileName, loopflag);
	}
	
	
	void SetBGMPlayer( string bgmFileName ,bool loopflag) {
		AudioClip clip = Resources.Load (bgmFileName,typeof(AudioClip))as AudioClip;
		
		if ( clip != null ) {
			obj = new GameObject( "BGMPlayer" );
			source = obj.AddComponent<AudioSource>();
			source.volume = SoundPlayer.BGM_VOLUME;
			source.clip = clip;
			source.loop = loopflag;
			source.panLevel = 0;
			MonoBehaviour.DontDestroyOnLoad(source);
			state = new Wait( this );
			
		} else{
			Debug.LogWarning( "BGM " + bgmFileName + " is not found." );
		}
	}
	
	public void destory() {
		if ( source != null )
			GameObject.Destroy( obj );
	}
	
	public void playBGM() {
		if ( source != null ) {
			state.playBGM();
		}
	}
	
	public void playBGM( float fadeTime ) {
		if ( source != null ) {
			this.fadeInTime = fadeTime;
			state.playBGM();
		}else{
			Debug.Log("source = null");
		}
	}
	
	public void pauseBGM() {
		if ( source != null )state.pauseBGM();
	}
	
	public void stopBGM( float fadeTime ) {
		if ( source != null ) {
			fadeOutTime = fadeTime;
			state.stopBGM();
		}
	}
	
	public void update() {
		if ( source != null )state.update();
	}
	
	public bool IsBgmLoop(){
		return loopbgmflag;
	}

	public bool hasFadeOut(){
		return fadeoutFlag;
	}
	
	public float GetTime() {
		return source.time;
	}
	
	public float GetTimeLength() {
		return source.clip.length;
	}
	
}

using UnityEngine;
using System.Collections;


// need this class in the scene where in the end the currentl bgm needs to be stoped;
public class FinishBGMScript : MonoBehaviour {

	public float fadeTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FinishBGM()
	{
		SoundPlayer.instance.stopBGM(fadeTime);
	}
}

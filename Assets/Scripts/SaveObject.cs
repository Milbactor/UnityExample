using UnityEngine;
using System.Collections;

public class SaveObject : MonoBehaviour {


	public GameObject owner; 
	public AudioClip summonSound = null;
	private bool saveUsed = false;

	// Use this for initialization
	protected virtual void Start () {
		
		playSummonSound();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	public void playSummonSound ()
	{
		if( summonSound != null )
		{
			audio.clip = summonSound;
			audio.Play();
		}
	}

	
	public void LaunchPlayer()
	{
		if(saveUsed == false)
		{
			owner.SendMessage("OnLaunch");
			saveUsed = true;
		}
		
	}
}

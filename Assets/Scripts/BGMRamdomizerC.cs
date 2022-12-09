using UnityEngine;
using System.Collections;

public class BGMRamdomizerC : MonoBehaviour {


	public AudioClip[] BGMSound = null;

	// Use this for initialization
	void Start () {

		int random = Mathf.FloorToInt (Random.value * BGMSound.Length);
		playBGMSound(random);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playBGMSound (int n)
	{

		for(int i = 0; i < BGMSound.Length; i++)
		{
			if(i == n)
			{
				if( BGMSound[i] != null)
				{
					audio.clip = BGMSound[i];
					audio.Play();
				}
				break;
			}
		}

	}

}

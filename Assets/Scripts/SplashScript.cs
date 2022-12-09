using UnityEngine;
using System.Collections;

public class SplashScript : MonoBehaviour {

	float duration = 2f;
	bool temp = false;
	float counter = 0;

	public GameObject[] pictures;
	int currentPicture = 0;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.anyKeyDown)
		{
			Application.LoadLevel("MainMenu");
			return;
		}

		if( currentPicture == pictures.Length)
		{
			Application.LoadLevel("MainMenu");
			print ("going to mainmenu");
			return;
		}

		Color textureColor = pictures[currentPicture].GetComponent<SpriteRenderer>().color;
		
		textureColor.a = Mathf.PingPong(counter, duration) / duration;

		if(textureColor.a >  0.5f)
			temp = true;

	
		pictures[currentPicture].GetComponent<SpriteRenderer>().color = textureColor;
		counter += Time.deltaTime;

		if(textureColor.a < 0.005f && temp)
		{
			
			currentPicture++;
			temp = false;
			counter = 0;
		}

	}



}

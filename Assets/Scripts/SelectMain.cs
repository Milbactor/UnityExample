using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SelectMain : MonoBehaviour {

	public GameObject playerDataObj;
	PlayerDataC data;

	public GameObject hero;
	public GameObject heroine;
	public GameObject villain;
	public GameObject henchman;

	public GameObject goodMask;
	public GameObject badMask;

	public AudioSource good;
	public AudioSource bad;
	public AudioClip goodSound = null;
	public AudioClip badSound = null;
	bool soundPlayedGood = false;
	bool soundPlayedBad = false;

	bool goodSelectConfirmed = false;
	bool badSelectConfirmed = false;
	public AudioClip ConfirmSoundHero = null;
	public AudioClip ConfirmSoundHeroine = null;
	public AudioClip ConfirmSoundVillain = null;
	public AudioClip ConfirmSoundHenchMan = null;

	LiteTimer startTimer;
	float startTime = 2f;



	// Use this for initialization
	void Start () {
		data = playerDataObj.GetComponent<PlayerDataC>();
		heroine.SetActive(false);
		henchman.SetActive(false);
		data.player1 = "Hero";
		data.player2 = "Villain";
		data.player3 = "Heroine";
		data.player4 = "HenchMan";

		startTimer = new LiteTimer(startTime);
		startTimer.onElapsed += timeElapsed;

		//at the moment, normal mode only
		data.mode = PlayMode.NORMAL;
	}

	void timeElapsed( LiteTimer timer)
	{	
		startTimer.stop();
		SendMessage("FinishBGM");
		Application.LoadLevel("demo");
	}
	
	// Update is called once per frame
	void Update () {

		startTimer.Update();

		if(goodSelectConfirmed == true && badSelectConfirmed == true )
		{
			if(Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2"))
			{
				timeElapsed( new LiteTimer(0));
			}
		}



		if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 1) > 0.2f || Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 1) < -0.2f) 
		{

			if(soundPlayedGood == false && goodMask.activeSelf == true)
			{
				PlaySwapSound("good");
				soundPlayedGood = true;
				string temp = data.player3 ;
				data.player3 = data.player1;
				data.player1 = temp;
			}

		}
		if  (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 2) > 0.2f || Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 2) < -0.2f) 
		{
			if(soundPlayedBad == false &&  badMask.activeSelf == true)
			{
				PlaySwapSound("bad");
				soundPlayedBad = true;
				string temp = data.player2 ;
				data.player2 = data.player4;
				data.player4 = temp;
			}

		}


		if(data.player1 == "Hero")
		{
			hero.SetActive(true);
			heroine.SetActive(false);
		}
		if(data.player1 == "Heroine")
		{
			heroine.SetActive(true);
			hero.SetActive(false);
		}
		if(data.player2 == "Villain")
		{
			villain.SetActive(true);
			henchman.SetActive(false);
		}
		if(data.player2 == "HenchMan")
		{
			henchman.SetActive(true);
			villain.SetActive(false);
		}

		if(Input.GetButtonDown( GeekInput.ABUTTON + 1 ) )
		{
			if(goodSelectConfirmed == false) 
			{
				PlayConfirmSound("good");
			}
			goodSelectConfirmed = true;
			goodMask.SetActive(false);
		}
		if(Input.GetButtonDown( GeekInput.BBUTTON + 1 ) )
		{
			if(goodSelectConfirmed == false && badSelectConfirmed == false) 
			{
				Application.LoadLevel("MainMenuNew");
			}
			goodSelectConfirmed = false;
			if(startTimer.playing) startTimer.stop();
			goodMask.SetActive(true);
		}

		if(Input.GetButtonDown( GeekInput.ABUTTON + 2 ) )
		{
			if(badSelectConfirmed == false) 
			{
				PlayConfirmSound("bad");
			}
			badSelectConfirmed = true;
			badMask.SetActive(false);

		}
		if(Input.GetButtonDown( GeekInput.BBUTTON + 2 ) )
		{
			if(goodSelectConfirmed == false && badSelectConfirmed == false) 
			{
				Application.LoadLevel("MainMenuNew");
			}
			badSelectConfirmed = false;
			if(startTimer.playing) startTimer.stop();
			badMask.SetActive(true);
		}

		if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 1) <= 0.2f && Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 1) >= -0.2f) 
		{
			soundPlayedGood = false;
		}
		if (Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 2) <= 0.2f && Input.GetAxis (GeekInput.LEFT + GeekInput.XAXIS + 2) >= -0.2f) 
		{
			soundPlayedBad = false;
		}

		if(goodSelectConfirmed == true && badSelectConfirmed == true && startTimer.playing == false)
		{
			startTimer.start();
		}
	}

	void PlaySwapSound(string side)
	{
		if(side == "good")
		{
			good.PlayOneShot(goodSound);
		}
		if(side == "bad")
		{
			bad.PlayOneShot(badSound);
		}

	}

	void PlayConfirmSound(string side)
	{
		if(side == "good")
		{
			good.PlayDelayed(0.5f);
			if(hero.activeSelf == true) good.PlayOneShot(ConfirmSoundHero);
			if(heroine.activeSelf == true) good.PlayOneShot(ConfirmSoundHeroine);
		}
		if(side == "bad")
		{
			bad.PlayDelayed(0.5f);
			if(villain.activeSelf == true) bad.PlayOneShot(ConfirmSoundVillain);
			if(henchman.activeSelf == true) bad.PlayOneShot(ConfirmSoundHenchMan);
		}

	}
}

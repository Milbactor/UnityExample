using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class FireBallShootC : GeekBehaviour {




	Queue<GameObject> fireBalls = new Queue<GameObject>();
	GameObject fireBall;
	public GameObject spawnLocation;
	int fireballIndex = 0;
	int fireballIndexMax = 15;
	public static string M_SHOOT = "M_SHOOT";
	public GameObject opponent; 
	public float shootCoolDownTime = 1.0f;
	LiteTimer shootCoolDownTimer;
	public AudioClip shootSound = null;


	void OnSetOpponent(GameObject obj)
	{
		opponent = obj;
	}


	// Use this for initialization
	void Start () {

		base.Start();

		addMessageListener((arguments) => OnSetOpponent((GameObject)arguments[0]), BossStatusC.M_SET_OPPONENT);
		List<GameObject> opponents = GameManager.instance.GetOtherPlayers( this.gameObject);
		opponent = opponents[0];

		shootCoolDownTimer = new LiteTimer(shootCoolDownTime);
		
		print ("loading fireball: " + this.gameObject.name + "Fireball" );
		fireBall = Resources.Load(this.gameObject.name + "Fireball") as GameObject;
		
		addMessageListener((arguments) => Shoot(), M_SHOOT);
		
		
		//good idea to think about performance :D, this is called a pool btw
		//preserve memory for objects in advance 
		/*for(int i = 0; i < fireballIndexMax; i++)
		{
			fireBalls.Enqueue(Resources.Load(this.gameObject.name + "Fireball") as GameObject);
		}	*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();
		shootCoolDownTimer.Update();
	}

	void Shoot()
	{
		if(shootCoolDownTimer.playing == true) return;

		anim.SetTrigger("TriggerShoot");
		playShootSound();
		shootCoolDownTimer.start();
		//GameObject ball = Instantiate(fireBalls.Dequeue());
		GameObject ball = Instantiate(fireBall) as GameObject;
		ball.transform.parent = transform;
		ball.transform.position = spawnLocation.transform.position;
		ball.GetComponent<FireBallHitBoxScript>().owner = this.gameObject;//temp
		ball.GetComponent<FireBallMovementC>().targetPoint = opponent;
		
	}

	public void playShootSound()
	{
		if(shootSound != null)
		{
			audio.PlayOneShot(shootSound);
		}
	}
}

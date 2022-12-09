using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class IceLakeC : MonoBehaviour {

	public List<GameObject> surfaces;
	LiteTimer clackTimer;
	LiteTimer breakTimer; 
	LiteTimer collapseTimer;
	float clackTime = 10f;
	float breakTime = 4f;
	float collapseTime = 4f;
	int maxPlayers = 2;
	int cntPlayer =0;


	// Use this for initialization
	void Start () {
		clackTimer = new LiteTimer(clackTime);
		breakTimer = new LiteTimer(breakTime);
		collapseTimer = new LiteTimer(collapseTime);
		clackTimer.onElapsed += clackElapsed;
		breakTimer.onElapsed += breakElapsed;
		collapseTimer.onElapsed += CollapseElapsed;
	}

	
	void clackElapsed( LiteTimer timer)
	{	
		breakTimer.start();
		surfaces[0].renderer.enabled = false;
		surfaces[1].renderer.enabled = true;
	}

	void breakElapsed( LiteTimer timer)
	{	
		breakTimer.stop();
		collapseTimer.start();
		surfaces[1].renderer.enabled = false;
		surfaces[2].renderer.enabled = true;
	}

	void CollapseElapsed( LiteTimer timer)
	{	
		collapseTimer.stop();
		surfaces[2].renderer.enabled = false;
		collider2D.enabled = false;
	}


	
	// Update is called once per frame
	void Update () 
	{
		clackTimer.Update();
		breakTimer.Update();
		collapseTimer.Update();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == ("Player"))
		{
			print ("adasdfafd");
			cntPlayer++; 
			if(clackTimer.playing == false)
				clackTimer.start();

		}

	}


	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.tag == ("Player") )
		{
			cntPlayer--; 
		}


		if(cntPlayer  == 0)
		{
			clackTimer.stop();
		}
		
	}
}

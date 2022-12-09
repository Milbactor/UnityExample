using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statistics {

	public int id; //player id
	public int winCounter = 0;
	public Dictionary<string, int> selfFallCount; // level name, and countof a each type of death;
	public Dictionary<string, int> fallCount;
	public Dictionary<string, int> selfDeathCount;
	public Dictionary<string, int> deathCount;
	public Dictionary<string, int> slashHitCount; //  level name, count of slash knockback
	public Dictionary<string, int> shootHitCount; // level name, count of shoot knockback
	public Dictionary<string, int> ultimateHitCount; // level name, count of slash knockback
	
	public Statistics (int id)
	{
		Debug.Log("constructor of statistics for player " + id);		
		this.id = id;
		selfFallCount = new Dictionary<string, int>();
		fallCount = new Dictionary<string, int>();
		selfDeathCount = new Dictionary<string, int>();
		deathCount = new Dictionary<string, int>();
		slashHitCount = new Dictionary<string, int>();
		shootHitCount = new Dictionary<string, int>();
		ultimateHitCount = new Dictionary<string, int>();
	}


	public void SetUltimateHitCount(string level)
	{
		Debug.Log("set ultimate hit counter for : " + id);
		Debug.Log("level with this ultimate hit is : " + level);
		if(ultimateHitCount.ContainsKey(level) == false)
		{
			ultimateHitCount.Add(level, 1);
		}
		else
		{
			ultimateHitCount[level]++;
		}
		Debug.Log("current counter of shoot in this level is : " + ultimateHitCount[level]);
	}

	public void SetShootHitCount(string level)
	{
		Debug.Log("set shot counter for : " + id);
		Debug.Log("level with this shot is : " + level);
		if(shootHitCount.ContainsKey(level) == false)
		{
			shootHitCount.Add(level, 1);
		}
		else
		{
			shootHitCount[level]++;
		}
		Debug.Log("current counter of shoot in this level is : " + shootHitCount[level]);
	}

	public void SetSlashHitCount(string level)
	{
		Debug.Log("set slashed counter for : " + id);
		Debug.Log("level with this slashed is : " + level);
		if(slashHitCount.ContainsKey(level) == false)
		{
			slashHitCount.Add(level, 1);
		}
		else
		{
			slashHitCount[level]++;
		}
		Debug.Log("current counter of slash in this level is : " + slashHitCount[level]);
	}

	public void SetSelfFallData(string level) 
	{

		if(selfFallCount.ContainsKey(level) == false)
		{
			Debug.Log("for the first time self fall data");
			selfFallCount.Add(level, 1);
		}
		else
		{
			selfFallCount[level] = selfFallCount[level] + 1 ;
		}
		Debug.Log("[player] " + id  + " total self fall :"  + selfFallCount.Count);
		Debug.Log("[player] " + id  + " [count of self fall] of " + level +  " is " + selfFallCount[level]);

	}

	public void SetFallData(string level)
	{
		Debug.Log("set Fall Data for : " + id);
		Debug.Log("level of fall is : " + level);
		if(fallCount.ContainsKey(level) == false)
		{
			fallCount.Add(level, 1);
		}
		else
		{
			fallCount[level]++;
		}
		Debug.Log("current counter of fall in this level is : " + fallCount[level]);
	}

	public void SetSelfDeathData(string level)
	{
		Debug.Log("set Self death Data for : " + id);
		Debug.Log("level of self death is : " + level);
		if(selfDeathCount.ContainsKey(level) == false)
		{
			selfDeathCount.Add(level, 1);
		}
		else
		{
			selfDeathCount[level]++;
		}
		Debug.Log("current counter of self DeathCount in this level is : " + selfDeathCount[level]);
	}

	public void SetDeathData(string level)
	{
		Debug.Log("set death Data for : " + id);
		Debug.Log("level of death is : " + level);
		if(deathCount.ContainsKey(level) == false)
		{
			deathCount.Add(level, 1);
		}
		else
		{
			deathCount[level]++;
		}
		Debug.Log("current counter of DeathCount in this level is : " + deathCount[level]);
	}

	public void SetWinCount()
	{
		Debug.Log("set win Data for : " + id);
	
		winCounter++;
		Debug.Log("current counter of DeathCount in this level is : " + winCounter);
	}
}

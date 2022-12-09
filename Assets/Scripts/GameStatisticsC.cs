using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameStatisticsC : MonoBehaviour {


	public List<Statistics> playerStatistics;
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		playerStatistics = new List<Statistics>();
	}

	// Use this for initialization
	void Start () {

		//temporary solution, later change to dynamic 
		print("Instantiate each player's data");
		playerStatistics.Add(new Statistics(1));
		playerStatistics.Add(new Statistics(2));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetFallCounter(int id, bool self)
	{
		print ("set fall counter in component");
		if(self == true)
		{
			for(int i = 1; i < GameData.levelData.Length - 1; i++) 
			{
				if(Application.loadedLevelName == GameData.levelData[i])
				{
					print (Application.loadedLevelName);
					playerStatistics[id].SetSelfFallData(GameData.levelData[i]);
					break;
				}
			}

		}
		else
		{
			for(int i = 1; i < GameData.levelData.Length - 1; i++) 
			{
				if(Application.loadedLevelName == GameData.levelData[i])
				{
					playerStatistics[id].SetFallData(GameData.levelData[i]);
					break;
				}
			}
		}

	}

	public void SetDeathCounter(int id, bool self)
	{
		print ("set death counter in component");
		if(self == true)
		{
			for(int i = 1; i < GameData.levelData.Length - 1; i++) 
			{
				if(Application.loadedLevelName == GameData.levelData[i])
				{
					playerStatistics[id].SetSelfDeathData(GameData.levelData[i]);
					break;
				}
			}
		}
		else
		{
			for(int i = 1; i < GameData.levelData.Length - 1; i++) 
			{
				if(Application.loadedLevelName == GameData.levelData[i])
				{
					playerStatistics[id].SetDeathData(GameData.levelData[i]);
					break;
				}
			}
		}
	}

	public void SetSlashCounter(int id)
	{

		print ("set slash counter in component");
		for(int i = 1; i < GameData.levelData.Length - 1; i++) 
		{
			if(Application.loadedLevelName == GameData.levelData[i])
			{
				playerStatistics[id].SetSlashHitCount(GameData.levelData[i]);
				break;
			}
		}
	}

	public void SetShootCounter(int id)
	{
		print ("set shoot counter in component");
		for(int i = 1; i < GameData.levelData.Length - 1; i++) 
		{
			if(Application.loadedLevelName == GameData.levelData[i])
			{
				playerStatistics[id].SetShootHitCount(GameData.levelData[i]);
				break;
			}
		}
	}

	public void SetUltimateCounter(int id)
	{
		print ("set ulitmate counter in component");
		for(int i = 1; i < GameData.levelData.Length - 1; i++) 
		{
			if(Application.loadedLevelName == GameData.levelData[i])
			{
				playerStatistics[id].SetUltimateHitCount(GameData.levelData[i]);
				break;
			}
		}
	}

	public void SetWinCounter(int id)
	{
		print ("set win counter in component id : " +id);
		playerStatistics[id].SetWinCount();
	}

}

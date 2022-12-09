using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class ScoreC : MonoBehaviour {

	public UILabel playerName;
	public UILabel winScore;
	public UILabel selfFallScore;
	public UILabel fallScore;
	public UILabel selfDeathScore;
	public UILabel deathScore;
	public UILabel slashScore;
	public UILabel shootScore;
	public UILabel ultimateScore;
	public GameObject icon1;
	public GameObject icon2;
	public string name1;
	public string name2;
	
	PlayerDataC playerDataC;

	GameStatisticsC stat;
	// Use this for initialization

	void Awake()
	{
		stat = GameObject.Find("GameStatistics").GetComponent<GameStatisticsC>();

		playerDataC = GameObject.Find("PlayerData").GetComponent<PlayerDataC>();

		if(gameObject.name == "Good")
		{
			playerName.text = playerDataC.player1;
		}
		if(gameObject.name == "Bad" )
		{
			playerName.text = playerDataC.player2;
		}
		
		if(playerName.text ==  name1) icon2.SetActive(false);
		if(playerName.text == name2) icon1.SetActive(false);
	}
	void Start () 
	{


		selfFallScore.text = "Fall Self:\n";
		fallScore.text = "Fall:\n";
		selfDeathScore.text = "Suicide:\n";
		deathScore.text = "Death:\n";
		slashScore.text = "Slashed:\n";
		shootScore.text = "Shot:\n";
		ultimateScore.text = "Hit by Ultimate Move:\n";


		int index = 0;
		if(gameObject.name == "Good") index = 0;
		if(gameObject.name == "Bad") index = 1;

		winScore.text = "Win times: " + stat.playerStatistics[index].winCounter;
		//print ("id for index 0 of playerStatistics is " +  stat.playerStatistics[index].id);
		for(int i = 1; i < GameData.levelData.Length - 1; i++)
		{
			if(stat.playerStatistics[index].selfFallCount.ContainsKey(GameData.levelData[i]))
			{
				selfFallScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].selfFallCount[GameData.levelData[i]].ToString() + "\n");
			}	
			if(stat.playerStatistics[index].fallCount.ContainsKey(GameData.levelData[i]))
			{
				fallScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].fallCount[GameData.levelData[i]].ToString() + "\n");
			}
			if(stat.playerStatistics[index].selfDeathCount.ContainsKey(GameData.levelData[i]))
			{
				selfDeathScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].selfDeathCount[GameData.levelData[i]].ToString() + "\n");
			}
			if(stat.playerStatistics[index].deathCount.ContainsKey(GameData.levelData[i]))
			{
				deathScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].deathCount[GameData.levelData[i]].ToString() + "\n");
			}
			if(stat.playerStatistics[index].slashHitCount.ContainsKey(GameData.levelData[i]))
			{
				slashScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].slashHitCount[GameData.levelData[i]].ToString() + "\n");
			}
			if(stat.playerStatistics[index].shootHitCount.ContainsKey(GameData.levelData[i]))
			{
				shootScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].shootHitCount[GameData.levelData[i]].ToString() + "\n");
			}
			if(stat.playerStatistics[index].ultimateHitCount.ContainsKey(GameData.levelData[i]))
			{
				ultimateScore.text += (GameData.levelData[i] + " : " + stat.playerStatistics[index].ultimateHitCount[GameData.levelData[i]].ToString() + "\n");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

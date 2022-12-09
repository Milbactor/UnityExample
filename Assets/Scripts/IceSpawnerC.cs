using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class IceSpawnerC : MonoBehaviour {

	private float time = 0;
	public float size = 10;
	public float spawnTick = 60f;
	private int logCount = 0;
	public int maxCount = 0;
	List<string> iceCorpses;

	// Use this for initialization
	void Start () {
		iceCorpses = new List<string>();
		foreach(string ice in GameData.getIceCorpses())
		{
			iceCorpses.Add(ice);
			maxCount++;
		}
		time += Time.deltaTime;
		
		if( time >= spawnTick )
		{
			time = 0;
			spawn();
		}
	

	}
	
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;
		
		if( time >= spawnTick )
		{
			time = 0;
			spawn();
		}
	}

	void spawn()
	{
		if( logCount >= maxCount || maxCount <= 0 ) return;

		print ("log count :" + logCount);
		print (iceCorpses[logCount]);
		GameObject ice = (GameObject)Instantiate( Resources.Load(iceCorpses[logCount]));
		ice.transform.position = new Vector2( transform.position.x + Random.Range( - size, size ), transform.position.y );
		ice.rigidbody2D.drag = Random.Range(5, 10 );
		ice.AddComponent<LogC>();
		
		if( Random.value > 0.5f )
			ice.transform.localScale = new Vector3( ice.transform.localScale.x, -1, 1 );

		GameData.DestroyIce(logCount);
		logCount++;
		logCount--;
		maxCount--;
			
	}
		

}

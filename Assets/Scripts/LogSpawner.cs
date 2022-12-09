using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpawnMode
{
	RANDOM, 
	ORGANIZED
}

public class LogSpawner : MonoBehaviour {

	private float time = 0;
	public float spawnTick = 2f;
	public float size = 10;
	private int logCount = 0;
	public int maxCount = 10;
	private List< GameObject> logs = new List<GameObject > ();
	
	private int spawnCount = 0;
	
	public SpawnMode spawnMode = SpawnMode.ORGANIZED;
	// Use this for initialization
	void Start () {
		
		//preSpawn();
	}
	
	void preSpawn()
	{
		for(int i = 0; i < maxCount / 2; i++)
		{
			GameObject log = (GameObject)Instantiate( Resources.Load("log1") );
			logs.Add( log );
			
			log.transform.position = new Vector2( transform.position.x + Random.Range( - size, size ), transform.position.y + -Random.value * 30 );
			log.GetComponent<LogC>().xSpeed = log.transform.position.x / 20;
			log.rigidbody2D.drag = Random.Range(5, 10 );
			
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
		if( logCount >= maxCount && maxCount != -1 ) return;


		string logName = "log1";
		float num = (int)Random.Range (0, 3);
		if( num == 0)
		{
			logName = "log1";
		}
		else if( num == 1)
		{
			logName = "log2";
		}
		else
		{
			logName = "log3";
		}

		if(spawnMode == SpawnMode.RANDOM )
		{
			GameObject log = (GameObject)Instantiate( Resources.Load(logName) );
			logs.Add( log );
		
			log.transform.position = new Vector2( transform.position.x + Random.Range( - size, size ), transform.position.y );
			log.GetComponent<LogC>().xSpeed = log.transform.position.x / 20;
			log.rigidbody2D.drag = Random.Range(5, 10 );
	
			if( Random.value > 0.5f )
				log.transform.localScale = new Vector3( log.transform.localScale.x, -1, 1 );
				
			logCount++;
			spawnCount++;
	
		}
		else if( spawnMode == SpawnMode.ORGANIZED )
		{

			//spawn one
			if( spawnCount % 2 == 0 )
			{
				GameObject log = (GameObject)Instantiate( Resources.Load(logName) );
				log.transform.position = new Vector3( -1.5f, 8, 0 );
				logs.Add( log );

				if( Random.value > 0.5f )
					log.transform.localScale = new Vector3(log.transform.localScale.x, -1, 1 );

				logCount++;
			
			}
			else //spawn side
			{
				GameObject log1 = (GameObject)Instantiate( Resources.Load(logName) );
				log1.transform.position = new Vector3( -7.1f, 8, 0 );
				log1.GetComponent<LogC>().xSpeed = log1.transform.position.x / 20;
				logs.Add( log1 );

				if( Random.value > 0.5f )
					log1.transform.localScale = new Vector3(log1.transform.localScale.x, -1, 1 );

				GameObject log2 = (GameObject)Instantiate( Resources.Load(logName) );
				log2.transform.position = new Vector3( 4.7f, 8, 0 );
				log2.GetComponent<LogC>().xSpeed = log2.transform.position.x / 20;
				logs.Add( log2 );

				if( Random.value > 0.5f )
					log2.transform.localScale = new Vector3(log2.transform.localScale.x, -1, 1 );

				logCount += 2;
			}
				

			
			spawnCount++;
		}
		
	
	}
}

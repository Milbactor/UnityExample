using UnityEngine;
using System.Collections;

public class LogThrowCutC : GeekBehaviour {

	public static string M_LOG_THROW_RECEIVED = "M_LOG_THROW_RECEIVED";
	private Quaternion q;
	public GameObject cutWood1_1;
	public GameObject cutWood1_2;
	public GameObject cutWood2_1;
	public GameObject cutWood2_2;

	
	// Use this for initialization
	void Start () {
		base.Start();
		addMessageListener( (arguments) => OnLogThrowRecieved( (Vector3) arguments[0] ), M_LOG_THROW_RECEIVED);
		q = Quaternion.Euler(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLogThrowRecieved(Vector3 LogVec)
	{
		GameObject impact = Instantiate(Resources.Load("ImpactLogCut")) as GameObject;
		impact.transform.position 
			= new Vector3 (transform.position.x, 
			               transform.position.y, 
			               transform.position.z);

		if(Random.value < 0.5){

			Vector3 spawnPosition 
				= new Vector3(LogVec.x + 1.0f, LogVec.y, 0.0f);
			GameObject newWood1 = 
				Instantiate(cutWood1_1, spawnPosition, q) as GameObject;
			
			spawnPosition 
				= new Vector3(LogVec.x -  1.0f, LogVec.y, 0.0f);
			GameObject newWood2 = 
				Instantiate(cutWood1_2, spawnPosition, q) as GameObject;
			
		}else {
			Vector3 spawnPosition 
				= new Vector3(LogVec.x , LogVec.y +  1.0f, 0.0f);
			GameObject newWood1 = 
				Instantiate(cutWood2_1, spawnPosition, q) as GameObject;
			
			spawnPosition 
				= new Vector3(LogVec.x , LogVec.y -  1.0f, 0.0f);
			GameObject newWood2 = 
				Instantiate(cutWood2_2, spawnPosition, q) as GameObject;
			
		}

	}




}

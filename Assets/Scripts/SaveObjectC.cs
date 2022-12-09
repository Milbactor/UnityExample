using UnityEngine;
using System.Collections;

public class SaveObjectC : GeekBehaviour {

	public static string M_INSTANTIATE_SAVE_OBJECT = "M_INSTANTIATE_SAVE_OBJECT";
	public GameObject SaveObject;
	private Quaternion q;
	float saveObjectSpawnPosY;
	public float SaveObjectSpawnPosXMargin = 0f;
	// Use this for initialization
	void Start () {
	
		base.Start();
		addMessageListener( (args) => InstantiateSaveObject(), M_INSTANTIATE_SAVE_OBJECT);

	}
	
	// Update is called once per frame
	void Update () {
		base.Update();

	}

	void InstantiateSaveObject()
	{
		string objName = "SaveMoveSpawn" + this.gameObject.name;
		print (objName);
		saveObjectSpawnPosY = GameObject.Find(objName).transform.position.y;
		print (saveObjectSpawnPosY);
		Vector3 spawnPosition = new Vector3(transform.position.x + SaveObjectSpawnPosXMargin, saveObjectSpawnPosY, 0.0f);
		GameObject newSaveObject = Instantiate( SaveObject, spawnPosition, q) as GameObject;
		newSaveObject.GetComponent<SaveObject>().owner = this.gameObject;

	}
}

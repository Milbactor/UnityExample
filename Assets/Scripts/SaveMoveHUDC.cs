using UnityEngine;
using System.Collections;

public class SaveMoveHUDC : MonoBehaviour {


	StatusC status;
	public Texture texture;
	public float ySpacing = 100;
	// Use this for initialization
	void Start () {

		status = GetComponent<StatusC>();

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		int j = 0;
		int counter = status.saveMoveAmount - status.saveCount;

		if(GetComponent<PlayerC>().ID == 1)
		{
			for(int i = 0; i < counter; i++)
			{	
				GUI.DrawTexture( new Rect( i * 75 + 5, 75 + ySpacing, 75, 50), texture ); 
			}
		}

		if(GetComponent<PlayerC>().ID == 2)
		{
			float left = 3 * 75f;
			for(int i = 0; i < counter; i++)
			{
				GUI.DrawTexture( new Rect( left - (i * 75 + 5) + Screen.width* 5/6, 75 + ySpacing, 75, 50), texture );
			}
		}
	}
}

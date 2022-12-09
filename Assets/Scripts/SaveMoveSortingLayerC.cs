using UnityEngine;
using System.Collections;

public class SaveMoveSortingLayerC : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if(Application.loadedLevelName == "level4")
		{
			renderer.sortingLayerName = "Background";
			renderer.sortingOrder = -1;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

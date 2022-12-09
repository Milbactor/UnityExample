using UnityEngine;
using System.Collections;

public class WaterSplashC : MonoBehaviour {

	public string sortingLayerName = "Babber";
	public int sortingOrder = 2;

	// Use this for initialization
	void Start () {
		// Set the sorting layer of the particle system.
		
		particleSystem.renderer.sortingLayerName = sortingLayerName;
		
		particleSystem.renderer.sortingOrder = sortingOrder;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class SpriteSortingLayer : MonoBehaviour {

	public string layer;
	public int sortingOrder;
	public Component target;
	// Use this for initialization
	void Start () {
		
		if( target != null )
		{
			target.renderer.sortingLayerName = layer;
			target.renderer.sortingOrder = sortingOrder;
			return;
		}
		renderer.sortingLayerName = layer;
		renderer.sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

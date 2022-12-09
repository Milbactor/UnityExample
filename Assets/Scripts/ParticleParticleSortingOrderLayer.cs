using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleParticleSortingOrderLayer : MonoBehaviour
{
	public string SortLayerName;
	public int SortOrder;

	// Use this for initialization
	void Start () {
		// Set the sorting layer of the particle system.
		
		particleSystem.renderer.sortingLayerName = SortLayerName;
		
		particleSystem.renderer.sortingOrder = SortOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

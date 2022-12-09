using UnityEngine;
using System.Collections;
//using UnityEditor;
using AssemblyCSharp;

public class LevelBounds : MonoBehaviour {

	public static LevelBounds instance;
	public Rect bounds;
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		bounds.x = transform.position.x;
		bounds.y = transform.position.y;
		GeekTools.GizmosDrawRect( transform.position, bounds );
	}
	
	
	// Use this for initialization
	void Start () {
	
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

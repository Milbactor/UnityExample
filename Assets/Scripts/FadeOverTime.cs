using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class FadeOverTime : MonoBehaviour {
	
	public float duration = 1f;
	LiteTimer timer;
	Color white;
	// Use this for initialization
	void Start () {
		
		timer = new LiteTimer( duration );
		timer.start();
		
		white = new Color(1, 1, 1, 0 );
	}
	
	// Update is called once per frame
	void Update () {
		
		timer.Update();
		
		Color currentColor = ( (SpriteRenderer)renderer ).color;
		((SpriteRenderer)renderer ).color = Color.Lerp( currentColor, white,( timer.time / timer.duration) );
		
		//print ("timer.time / timer.duration: " + timer.time / timer.duration );
	}
}

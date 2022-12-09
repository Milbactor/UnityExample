using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DestroyAfterTimeC : MonoBehaviour {

	private LiteTimer timer;
	public float duration = 1f;
	// Use this for initialization
	void Start () {

		timer = new LiteTimer( duration );
		timer.onElapsed += HandleonElapsed;
		timer.start();
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

		timer.Update();
	}

	void LateUpdate()
	{
		
	}
}

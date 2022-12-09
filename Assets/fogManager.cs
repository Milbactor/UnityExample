using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class fogManager : MonoBehaviour {
	
	LiteTimer appearTimer;
	LiteTimer durationTimer;
	public float appearTime;
	public float durationTime;

	public ParticleSystem[] fogs;
	// Use this for initialization
	void Start () {
		appearTimer = new LiteTimer (appearTime);
		durationTimer = new LiteTimer (durationTime);
		
		durationTimer.onElapsed += durationElapsed;
		appearTimer.onElapsed += HandleonElapsed;

		foreach(ParticleSystem fog in fogs )
		{
			fog.enableEmission = false;
		}
		appearTimer.start();
	}
	
	void durationElapsed (LiteTimer timer)
	{
		appearTimer.start ();

		foreach(ParticleSystem fog in fogs )
		{
			fog.enableEmission = false;
		}
	}
	
	void HandleonElapsed(LiteTimer timer)
	{
		durationTimer.start ();

		foreach(ParticleSystem fog in fogs )
		{
			fog.enableEmission = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		appearTimer.Update();
		durationTimer.Update();

		if( appearTimer.playing )
		{
			foreach(ParticleSystem fog in fogs )
			{
				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ fog.particleSystem.particleCount ];
				fog.particleSystem.GetParticles( particles );
				for(int i = 0; i < particles.Length; i++)
				{
					particles[i].color = Color32.Lerp( particles[i].color, new Color(1, 1, 1, 0 ), Time.deltaTime );;
				}
				
				fog.particleSystem.SetParticles( particles, fog.particleSystem.particleCount );
			}
		}
	}
}

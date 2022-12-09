using UnityEngine;
using System.Collections;

public class WaterStrokesC : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ particleSystem.particleCount ];
		particleSystem.GetParticles( particles );
	}
	
	// Update is called once per frame
	void Update () {
	
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ particleSystem.particleCount ];
		particleSystem.GetParticles( particles );
		for(int i = 0; i < particles.Length; i++)
		{
			particles[i].position = new Vector3( particles[i].position.x, 0, particles[i].position.z); 
			particles[i].velocity = new Vector3( particles[i].position.x / 5, 0f, particles[i].velocity.z );
		}

		particleSystem.SetParticles( particles, particleSystem.particleCount );
	}
}

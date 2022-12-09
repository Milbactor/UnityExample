using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class WindManager : MonoBehaviour {

	public Rect[] windAreas; 
	public ParticleSystem[] particleSystems;
	public Vector2[] windSpeed;
	public Vector2 minWindspeed;
	public Vector2 maxWindSpeed;
	public int minWindChangeTime = 10;
	public int maxWindChangeTime = 30;
	public float particleWindScale = 5f;
	public AudioClip windsound = null;
	
	public LiteTimer windDirectionTimer;
	public LiteTimer windinterval;
	public float intervaltime = 30f;
	public int activeWindArea = 0;
	
	public int windCount = 0;
	public int windDestroyCount = 1;
	public BridgeC bridge;
	// Use this for initialization
	void Start () {

		audio.clip = windsound;
		windDirectionTimer = new LiteTimer( Random.Range( minWindChangeTime, maxWindChangeTime ) );
		windDirectionTimer.onElapsed += ChangeWindSpeed;
		windinterval = new LiteTimer ( intervaltime );
		windinterval.onElapsed += onintervalelapsed;
		
		windSpeed = new Vector2[ windAreas.Length];
		ChangeWindSpeed( null );
		
		/*foreach( ParticleSystem system in particleSystems )
		{
			//system.enableEmission = false;
			if(system.GetComponent<SakuraC>() != null){
				//system.enableEmission = false;
			}
		}*/
	}
	
	void OnDrawGizmos()
	{
		foreach( Rect windArea in windAreas )
		{
			Gizmos.color = new Color(1, 1, 1, 0.2f);
			Gizmos.DrawCube( windArea.center, windArea.size );
			//GeekTools.GizmosDrawRect( new Vector2(windArea.x, windArea.y ), windArea );
		}
	}
	

	void ChangeWindSpeed ( LiteTimer timer)
	{
		foreach( ParticleSystem system in particleSystems )
		{
			if(system.GetComponent<SakuraC>() != null){
				//system.enableEmission = false;
			}
			//system.enableEmission = false;
		}
		
		for(int i = 0; i < windSpeed.Length; i++ )
		{
			windSpeed[i] = Vector2.zero;
		}
		windinterval.start ();

		activeWindArea++;
		if(activeWindArea >= windAreas.Length) activeWindArea = 0;
		
		//cheap way
		/*
		foreach( ParticleSystem system in particleSystems )
		{
			system.startSpeed = windSpeed.x * 15;
		}
		*/
		
	}

	void onintervalelapsed ( LiteTimer timer)
	{
		audio.Play ();
		windDirectionTimer.duration = Random.Range( minWindChangeTime, maxWindChangeTime );;

	
		float random = Random.value;
		int direction = 1;
		if (random > 0.5f ) direction = -1;
		
		windSpeed[activeWindArea] = new Vector2 ( Random.Range( minWindspeed.x, maxWindSpeed.x ), Random.Range( minWindspeed.y, minWindspeed.y ) );
		windSpeed[activeWindArea] *= direction;
		windDirectionTimer.start();
	

		/*foreach( ParticleSystem system in particleSystems )
		{
			system.enableEmission = true;
		}*/
		
		//destroy bridge
		if( windCount == windDestroyCount )
		{
			bridge.DestroyBridge( null, direction );
		}
		windCount++;
	}
	
	// Update is called once per frame
	void Update () {
		
		windDirectionTimer.Update();
		windinterval.Update();
		
		//if(windinterval.playing ) return;
		
		Rect windArea = windAreas[ activeWindArea ];
	
		foreach( Player player in GameManager.instance.players )
		{
			if( windArea.Contains( player.character.transform.position ) )
				player.character.rigidbody2D.velocity += windSpeed[activeWindArea];
		}
	
		
		//expensive way
		
		foreach( ParticleSystem system in particleSystems )
		{
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ system.particleCount ];
			system.GetParticles( particles );
			for(int j = 0; j < particles.Length; j++)
			{
				if( windArea.Contains( particles[j].position ) ) {
					//particles[j].color = Color.green;
					particles[j].velocity = new Vector3( windSpeed[activeWindArea].x * particleWindScale, particles[j].velocity.y, particles[j].velocity.z );
				}
				else
				{
					//particles[j].color = Color.white;
					particles[j].velocity = new Vector3( particles[j].velocity.x * 0.98f, particles[j].velocity.y, particles[j].velocity.z );
				}
			}
			
			system.SetParticles( particles, system.particleCount );
		}
		
	}
}

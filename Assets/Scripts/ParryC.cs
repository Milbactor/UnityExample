using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ParryC : GeekBehaviour {
	
	public static string M_GOT_PARRIED = "M_GOT_PARRIED";
	public static string M_PARRY_SUCCESFUL = "M_PARRY_SUCCESFUL";
	public static string M_PARRIED = "M_PARRIED";
	
	public float blockTime;
	public float cooldownTime;
	public float postUseDelay;

	LiteTimer blockTimer;
	LiteTimer blockCooldown;
	
	public AudioClip activateSound;
	public AudioClip parrySound;
	private LiteTimer blinkTimer;
	public LiteTimer useDelayTimer;
	// Use this for initialization
	public GameObject particles;
	
	LiteTimer particleTimer;
	
	void Start () {
		
		base.Start();
		
		blockCooldown = new LiteTimer( cooldownTime );
		useDelayTimer = new LiteTimer( postUseDelay );
		blockTimer = new LiteTimer( blockTime );
		blockCooldown.onElapsed += cooldownElapsed;
		blockTimer.onElapsed += HandleonElapsed;
		useDelayTimer.onElapsed += OnPostUseDelayEnded;
		
		particleTimer = new LiteTimer(0.2f);
		particleTimer.onElapsed += OnParticleEnd;
		
		blinkTimer = new LiteTimer(0.2f );
		blinkTimer.onElapsed += BlinkElapsed;
		
		addMessageListener( (args) => OnBlockActivate(), BlockC.M_BLOCK  ); 
		illegalStates.Add(Animator.StringToHash( "Life related.StayDeathFall") );
		//illegalStates.Add(Animator.StringToHash( "Life related.Win") );

		particles.particleSystem.enableEmission = false;
		addMessageListener((arguments) => parrySucessfull(), M_PARRY_SUCCESFUL );
	}
	
	void HandleonElapsed ( LiteTimer timer)
	{
		print ("elapsed parryc");
		rb2D.velocity = Vector2.zero;
		
		if( postUseDelay <= 0) {
			OnPostUseDelayEnded( timer );
		}else {
			useDelayTimer.start();
		}
		
		anim.SetBool (BlockC.M_BLOCK, false);
		GameObject.Destroy(gameObject.GetComponent<MovementDisabledStatus>() );
	}

	void parrySucessfull ()
	{
		print ("parry sucessfull");
		particles.particleSystem.enableEmission = true;
		particleTimer.start();
		
		audio.clip = parrySound;
		audio.Play();
		
		anim.SetBool (BlockC.M_BLOCK, false );
	}
	
	void OnPostUseDelayEnded( LiteTimer timer)
	{
		blockCooldown.start();
	}

	void cooldownElapsed ( LiteTimer timer)
	{
		
	}

	void OnParticleEnd ( LiteTimer timer)
	{
		particles.particleSystem.enableEmission = false;
	}

	void BlinkElapsed ( LiteTimer timer)
	{
		gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
	}
	
	void OnBlockActivate(  )
	{
		if( InIllegalState() ) return;
		if(anim.GetBool("Dead") == true) return;// temp solution because InIllegalState is not working
		if(GetComponent<StatusC>().usingSaveMove == true) return; // temp solution needing to have better way of access to this variable
		if( blockCooldown.playing || blockTimer.playing || useDelayTimer.playing ) return;
		
		anim.SetBool (BlockC.M_BLOCK, true);
		
		audio.clip = activateSound;
		audio.Play();
		
		//blink
		blinkTimer.start();;
		gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("GUI/Text Shader");
		
		blockTimer.start();

		gameObject.AddComponent<MovementDisabledStatus>();
		//particles.particleSystem.enableEmission = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		base.Update();
		
		blockTimer.Update();
		blockCooldown.Update();
		useDelayTimer.Update();
		particleTimer.Update();
		blinkTimer.Update();
		
		//print ("cooldownTimer: " + dashCooldown.time );
		if( blockTimer.playing )
		{
			//print ("dashTimer: " + dashTimer.time );
		}
	}
}

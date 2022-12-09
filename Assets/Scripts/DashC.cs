using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DashC : GeekBehaviour {

	public static string M_ON_DASH_ACTIVATE = "M_ON_DASH_ACTIVATE";
	public static string M_DASH_COMPLETE = "M_DASH_COMPLETE";
	public float speed;
	public float dashTime;
	public float cooldownTime;
	public float postUseDelay;
	public Color dashcolor = Color.white;
	public Shader dashshader;
	
	float prevGravity = 0;
	
	LiteTimer dashTimer;
	LiteTimer dashCooldown;
	
	public AudioClip[] dashSound;
	public LiteTimer useDelayTimer;
	// Use this for initialization

	
	public float ghostTime = 0.5f;
	
	float trailTimer = 0f;
	public float retainVelocity = 0f;

	void Start () {

		base.Start();

		dashCooldown = new LiteTimer( cooldownTime );
		useDelayTimer = new LiteTimer( postUseDelay );
		dashTimer = new LiteTimer( dashTime );
		dashTimer.onElapsed += HandleonElapsed;
		useDelayTimer.onElapsed += OnPostUseDelayEnded;

		addMessageListener( (args) => OnDashActivate( (Vector2) args[0] ), M_ON_DASH_ACTIVATE  ); 
		illegalStates.Add(Animator.StringToHash( "Life related.StayDeathFall") );
		//illegalStates.Add(Animator.StringToHash( "Life related.Win") );


	}

	void HandleonElapsed ( LiteTimer timer)
	{
		//print ("elapsed");
		//rb2D.velocity = Vector2.zero;
		rb2D.velocity *= retainVelocity;

		if( postUseDelay <= 0) {
			OnPostUseDelayEnded( timer );
		}else {
			useDelayTimer.start();
		}
	}
	
	void OnPostUseDelayEnded(LiteTimer timer)
	{
	
		dashCooldown.start();
		print ("starting cooldown");
		
		dispatchMessage( M_DASH_COMPLETE );
	}

	void OnDashActivate( Vector2 direction )
	{
		if( InIllegalState() ) return;
		if(anim.GetBool("Dead") == true) return;// temp solution because InIllegalState is not working
		if(GetComponent<StatusC>().usingSaveMove == true) return; // temp solution needing to have better way of access to this variable
		if( dashCooldown.playing || dashTimer.playing || useDelayTimer.playing ) return;

		anim.SetTrigger(AnimatorConstants.DASH);
		audio.clip = dashSound[ Mathf.FloorToInt( Random.value * 1.99f) ];
		audio.Play();

		dashTimer.start();
		DashStatusEffect dashEffect = gameObject.AddComponent<DashStatusEffect>();
		dashEffect.duration = dashTime;
		dashEffect.speed = this.speed;

		rb2D.velocity = direction * speed;

		/*GetComponent<GeekPhysicsC>().gravityScale = 0;
		GetComponent<GeekPhysicsC>().friction = 1;
		GetComponent<GeekPhysicsC>().maxVelocity = Vector2.one * speed;;
		
		prevGravity = prevGravity;

		dispatchMessage(PlayerC.M_INPUT_DISABLE );*/
		
		
		//particles.particleSystem.enableEmission = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		base.Update();

		dashTimer.Update();
		dashCooldown.Update();
		useDelayTimer.Update();
		
		if( dashTimer.playing )
		{
			trailTimer += Time.deltaTime;
			
			if(trailTimer > 0.03f)
			{
				GameObject ghost = new GameObject();
				DestroyAfterTime destroy = ghost.AddComponent<DestroyAfterTime>();
				destroy.time = ghostTime;
				
				FadeOverTime fade = ghost.AddComponent<FadeOverTime>();
				fade.duration = destroy.time;
				
				SpriteRenderer spriteRenderer = ghost.AddComponent<SpriteRenderer>();
				spriteRenderer.material.shader = dashshader;
				spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
				spriteRenderer.color = dashcolor;
				spriteRenderer.sortingLayerName = "Players";
				
				ghost.transform.localScale = tf.localScale;
				ghost.transform.position = tf.position;
				
				trailTimer = 0f;
			}
		}
	}
}

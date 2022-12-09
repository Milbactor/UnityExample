using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BossTransformC : GeekBehaviour {

	public static string M_TRANSFORM = "M_TRANSFORM";
	public static string M_TRANSFORM_COMPLETE = "M_TRANSFORM_COMPLETE";
	
	public GameObject boss;
	public GameObject transformParticles;
	
	private LiteTimer transformTimer;
	public float transformationTime = 1f;
	// Use this for initialization
	void Start () {
	
		base.Start();
		
		transformTimer = new LiteTimer( 2.5f );
		transformTimer.onElapsed += HandleonElapsed;
		
		addMessageListener(agrs => OnTransform(), M_TRANSFORM );
	}

	void HandleonElapsed ( LiteTimer timer)
	{
		//transformParticles.particleSystem.Stop();
		transformParticles.particleSystem.enableEmission = false;
	
		GameObject bossObj = GameObject.Instantiate( boss ) as GameObject;
		bossObj.transform.position = tf.position;

		bossObj.name =  this.gameObject.name +"Boss";
		GameManager.instance.GetComponent<GeekBehaviour>().dispatchMessage(GameManager.M_OPPENENT_CHANGE, bossObj);
		//ugly for now
		PlayerC player = GetComponent<PlayerC>();
		
		dispatchMessage(M_TRANSFORM_COMPLETE, bossObj);
		GameManager.instance.setCharacterForPlayer( player.ID -1, bossObj );
	}

	void OnTransform ()
	{
		transformParticles = GameObject.Instantiate( transformParticles) as GameObject;
		//transformParticles.transform.parent = tf;
		transformParticles.transform.position = tf.position;
		transformTimer.start();
		
		anim.SetTrigger( AnimatorConstants.TRANSFORM );
	}
	
	// Update is called once per frame
	void Update () {
	
		transformTimer.Update();
		base.Update();
	}
}

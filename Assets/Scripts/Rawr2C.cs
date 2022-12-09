using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class Rawr2C : GeekBehaviour {
	
	public static string M_RAWR = "M_RAWR";
	public GameObject opponent; 
	public float rawrCoolDownTime = 5.0f;
	LiteTimer rawrCoolDownTimer;
//	public float power = 1.5f;
	//public float distance = 5f;
	//public bool opponnentIn = false;
	public GameObject rawrArea;
//	public ParticleSystem roarParticles;
	private LiteTimer roarPArticleTimer;
	
	//public AudioClip[] rawrSounds;
	
	
	void OnSetOpponent(GameObject obj)
	{
		opponent = obj;
	}
	
	
	// Use this for initialization
	void Start () {
		base.Start();
		
		addMessageListener((arguments) => OnSetOpponent((GameObject)arguments[0]), BossStatusC.M_SET_OPPONENT);
		List<GameObject> opponents = GameManager.instance.GetOtherPlayers( this.gameObject);
		opponent = opponents[0];
		
		
		rawrCoolDownTimer = new LiteTimer(rawrCoolDownTime);
		addMessageListener((arguments) => Rawr(), M_RAWR);
		
		//roarParticles.enableEmission = false;
		roarPArticleTimer = new LiteTimer(0.7f );
		roarPArticleTimer.onElapsed += HandleonElapsed;
	}
	
	void HandleonElapsed ( LiteTimer timer)
	{
	//	roarParticles.enableEmission = false;
	//	roarParticles.Stop();
		rawrArea.collider2D.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		rawrCoolDownTimer.Update();
		roarPArticleTimer.Update();
	}
	
	
	
	
	void Rawr()
	{
		if(rawrCoolDownTimer.playing) return;
	//	anim.SetTrigger("TriggerRawr");
	//	playRawrSound ();
		
		//roarParticles.enableEmission = true;
		roarPArticleTimer.start();
	//	roarParticles.Play();
		rawrArea.collider2D.enabled = true;
		//if( Vector3.Distance( transform.position, opponent.transform.position) > distance) return;
		
		
		
		
		InputDisabledStatus rawStatus = this.gameObject.AddComponent<InputDisabledStatus>();
		rawrCoolDownTimer.start();
		rawStatus.duration = 0.7f;
		//Vector3 lookPos = opponent.transform.position - tf.position;
		//Vector3 normalizedLookPos = lookPos.normalized;
	//	Vector2 direction = new Vector2(-normalizedLookPos.x,normalizedLookPos.y);
		//need different class from damageC which is similar to damageC though 
	//	opponent.GetComponent<GeekBehaviour>().dispatchMessage(
	//		DamageC.M_DAMAGE_RECEIVED, this.gameObject, direction,  power, "Rawr" );
	}
	
	/*public void playRawrSound ()
	{
		int index = Mathf.FloorToInt (Random.value * rawrSounds.Length);
		if(rawrSounds[index] != null)
		{
			audio.clip = rawrSounds[index];
			audio.Play();
		}
	}*/
	
}

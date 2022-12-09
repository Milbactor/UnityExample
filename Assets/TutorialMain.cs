using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class TutorialMain : MonoBehaviour {

	public ScrollSpawner scrollSpawner;
	public GameObject player2;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if( Input.GetButton( GeekInput.STARTBUTTON + 1 ) && player2.activeSelf == false)
		{
			//player2.SetActive( true );
			player2.GetComponent<PlayerC>().enabled = true;
			player2.GetComponent<AIController>().enabled = false;
			GameManager.instance.Register( player2, 1 );
		}
	}
	
	public void spawnScroll()
	{
		scrollSpawner.nextScroll = "Scroll";
		scrollSpawner.spawnLocation = Vector3.zero;
		scrollSpawner.spawnBird();
		
		print ("########## SPAWNING SCROLL #########");
	}
	
	public void BlockAI()
	{
		player2.GetComponent<AIController>().aiMode = AIMODE.ATTACK;
		print ("setting AIMODE to Attack ");
	}
	
	public void StopAI()
	{
		player2.GetComponent<AIController>().aiMode = AIMODE.NONE;
	}
}

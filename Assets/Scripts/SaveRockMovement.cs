using UnityEngine;
using System.Collections;




public class SaveRockMovement : SaveObject{


//	private bool goingUp = true;
	private bool goingDown = false;
	public float movingSpeed = 0.4f;
	private float currentTime;



	// Use this for initialization
	override protected void Start () {
		
		base.Start() ;
	}
	

	// Update is called once per frame
	override protected void Update () 
	{
		base.Update();
	//	if(goingUp)
	//	{
//		if(transform.position.y < - 15)
//			{
//				transform.localPosition = 
//					new Vector3(transform.localPosition.x, transform.localPosition.y +  movingSpeed, 0);
//				/*if(!soundStarted){
//					AudioSource.PlayClipAtPoint (
//						rockSummon, transform.position, 1);
//					soundStarted = true;
//				}*/
//			} 
//			else 
//			{
//				goingUp = false;
//				//soundStarted = false;
//				currentTime = Time.time;
//				GetComponent<BoxCollider2D>().enabled = false;
//			}
	//	}
		
		//if((Time.time - currentTime) > 1 && !goingUp)
		//{
			//goingDown = true;
		//}
		
		
		
		if(goingDown)
		{
			if(transform.position.y > - 35)
			{	
				transform.position = 
					new Vector3(transform.localPosition.x, transform.localPosition.y - movingSpeed, 0);
				/*if(!soundStarted){
					AudioSource.PlayClipAtPoint (
						rockCrumble, transform.position, 1);
					soundStarted = true;
				}*/
			}
			else 
			{
				goingDown = false;
				Destroy(this.gameObject);
			}
		}
	}



	public void DestroySelf()
	{
		goingDown = true;
	}


}

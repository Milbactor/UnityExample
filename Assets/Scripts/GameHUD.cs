using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameHUD : GeekBehaviour
{

	public GUISkin guiSkin;
	public float nativeVerticalResolution = 420.0f;
	public Texture2D[] dishonor;
	public Vector2 DishonorImageOffset = new Vector2(10,20);
	private int lengthDishonor;
	private bool startDraw = false;
	public AudioClip WarningSound;
	private bool dishonorWarned = false;
	public static string M_SET_DISHONOR_RECIEVED = "M_SET_DISHONOR";
	public static string M_RESET_DISHONOR_RECIEVED = "M_RESET_DISHONOR";


	void Awake ()
	{
		lengthDishonor = 0;
	}

	void Start()
	{
		base.Start();
		//addMessageListener((arguments) => SetDishonor(), M_SET_DISHONOR_RECIEVED );
		//addMessageListener((arguments) => ResetDishonor(), M_RESET_DISHONOR_RECIEVED );
		
	}
	
 
	public void SetDishonor (){

		if(anim.GetBool(AnimatorConstants.DEAD) == false)
		{
			startDraw = true;
			lengthDishonor++;
	
			if( lengthDishonor % 2 != 0 && lengthDishonor < 6)
			{
				GetComponent<GeekBehaviour>().dispatchMessage(DamageC.M_DAMAGE_POWER_UP_RECEIVED);
			}
			if(lengthDishonor >= 6){
				lengthDishonor = 6;
				if(dishonorWarned == false)
				{
					playWarningSound();
					dishonorWarned = true;
				}
			}
		}
	}
	
		
	void ResetDishonor(){
		lengthDishonor = 0;
	}

	void playWarningSound()
	{
		if( WarningSound != null)
		{
			audio.PlayOneShot(WarningSound);
		}
		
	}

	void OnGUI ()
	{
	
		if(startDraw){
			GUI.skin = guiSkin;
			GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.height / nativeVerticalResolution, Screen.height / nativeVerticalResolution, 1));
			for(int index = 0; index < lengthDishonor; index++){

				Player player = GameManager.instance.getPlayerByCharacter(this.gameObject);
				if(player.side > 0 ){
					
					float scaledResolutionWidth = nativeVerticalResolution / Screen.height * Screen.width;
					if(index%2 == 0){
						GUI.Label (new Rect 
						(scaledResolutionWidth - DishonorImageOffset.x - dishonor[index].width + dishonor[index].width*(index/2),
						  DishonorImageOffset.y, 
						dishonor[index].width, dishonor[index].height), dishonor[index]);
					}
					if(index%2 != 0){
						GUI.Label (new Rect 
						(scaledResolutionWidth - DishonorImageOffset.x - dishonor[index].width + dishonor[index].width*((index-1)/2),
						  DishonorImageOffset.y, 
						dishonor[index].width, dishonor[index].height), dishonor[index]);
					}
				}
				
				if(player.side < 0) {
					
					float scaledResolutionWidth = nativeVerticalResolution / Screen.height * Screen.width;
					if(index%2 == 0){
						GUI.Label (new Rect 
						( DishonorImageOffset.x + dishonor[index].width*(index/2),
						 DishonorImageOffset.y, 
						dishonor[index].width, dishonor[index].height), dishonor[index]);
					}
					if(index%2 != 0){
						GUI.Label (new Rect 
						(DishonorImageOffset.x +dishonor[index].width*((index-1)/2),
					  DishonorImageOffset.y, 
						dishonor[index].width, dishonor[index].height), dishonor[index]);
					}
				}
					
			}
		}
	}		
	
}

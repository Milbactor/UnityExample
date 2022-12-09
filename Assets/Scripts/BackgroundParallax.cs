using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	public float parallaxScale;              // The proportion of the camera's movement to move the backgrounds by.
	public float parallaxReductionFactor;   // How much less each successive layer should parallax.
	public float smoothing;                 // How smooth the parallax effect should be.

	float maxMovement = 8.0f;	//if you move more than 8, NG
	private Transform cam;               // Shorter reference to the main camera's transform.
	private Vector3 previousCamPos;          // The postion of the camera in the previous frame.
	private Vector3 previousTargetPosition;
	public Vector2 modifier = Vector2.one;
	public float depth;
	
	void Awake ()
	{
		// Setting up the reference shortcut.
		cam = Camera.main.transform;
//		print (cam);
	}
	
	
	void Start ()
	{
		// The 'previous frame' had the current frame's camera position.
		previousCamPos = cam.position;
		previousTargetPosition = transform.position;
	}
	
	
	void Update ()
	{

		// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
		float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScale;
		float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScale;


		// ... set a target x position which is their current position plus the parallax multiplied by the reduction.
		float backgroundTargetPosX = transform.position.x + parallaxX * (depth * parallaxReductionFactor + 1) * modifier.x;
		float backgroundTargetPosY = transform.position.y + parallaxY * (depth * parallaxReductionFactor + 1) * modifier.y;

		if(Mathf.Abs(backgroundTargetPosX - previousTargetPosition.x) > maxMovement)
		{
			backgroundTargetPosX = previousTargetPosition.x;
		}
		if(Mathf.Abs(backgroundTargetPosY - previousTargetPosition.y) > maxMovement)
		{
			backgroundTargetPosY = previousTargetPosition.y;
		}

		// Create a target position which is the background's current position but with it's target x position.
		Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, 0);




		// Lerp the background's position between itself and it's target position.
		transform.position = Vector3.Lerp (transform.position, backgroundTargetPos, smoothing * Time.deltaTime);

		// Set the previousCamPos to the camera's position at the end of this frame.
		previousCamPos = cam.position;

		previousTargetPosition = transform.position;
	}
}
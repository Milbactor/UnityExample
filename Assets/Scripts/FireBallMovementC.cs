using UnityEngine;
using System.Collections;



public class FireBallMovementC : GeekBehaviour {


	public  ThrowDirection throwDir;
	public float speed = 5f;
	public GameObject targetPoint;

	// Use this for initialization
	void Start () {
		base.Start();

	}
	//called after boss is not parent obejct of this anymore in fireballhitbosScript; //TODO re-write with dispachmesage stuff later
	public void SetMovement()
	{
		Vector3 lookPos = targetPoint.transform.position - transform.position;
		
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		Vector3 normalizedLookPos = lookPos.normalized;
		GetComponent<FireBallHitBoxScript>().directionX = -normalizedLookPos.x;
		GetComponent<FireBallHitBoxScript>().directionY = normalizedLookPos.y;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
		rigidbody2D.velocity = (targetPoint.transform.position - tf.position).normalized * speed; 

	}


	// Update is called once per frame
	void Update () {
		base.Update();

	}
}

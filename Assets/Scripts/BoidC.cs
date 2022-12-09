using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public enum BOID
{
	SEEK, 
	FLEE,
	ADAPT,
	NONE
}

public class BoidC : GeekBehaviour {

	public float _maxVelocity;
	
	
	public float maxVelocity
	{
		get{ return _maxVelocity * velocityMultiplier; }
		set { _maxVelocity = value; }
	}
	
	public float maxForce = 0.3f;
	private Vector2 steeringForce = Vector2.zero;
	public int sightRange = 50;
	public int minDistance = 15;
	public int mass = 1;
	Vector2 desired = Vector2.zero;
	
	public float fleeMultiplier = 2f;
	
	public BOID currentMode = BOID.FLEE;
	public float fleeDistance = 2f;
	public float seekDistance = 10f;
	
	float velocityMultiplier = 1f;
	public Vector2 steeringModifier = Vector2.one;
	
	public GameObject target;
	protected List<GameObject> boids = new List<GameObject>();
	
	static GameObject averagePosition;
	public bool flocking = true;
	public string targetName = "Hero";
	// Use this for initialization
	void Start () {
	
		base.Start();
		
		if( averagePosition == null ) averagePosition = new GameObject("averagePosition");
		target = GameObject.Find( targetName);
		
		//addMessageListener(FishC.M_KILLED, args => onKilled() );
	}

	void onKilled ()
	{
		enabled = false;
		tf.localRotation = Quaternion.Euler( Vector3.zero );
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		
		
		
	}
	
	protected void setRotation()
	{
		if (rb2D.velocity.x != 0 || rb2D.velocity.y != 0) {
			//this.rotation = Vector2.angle(velocity.x, velocity.y);
		}
		
		//tf.localScale = new Vector3(-1, tf.localScale.y, tf.localScale.z );
	}
	
	void FixedUpdate()
	{
		if( flocking == true ) flock( boids );
		if( target != null ) 
		{
			float distance = Vector3.Distance( tf.position, target.transform.position );
			if( currentMode == BOID.FLEE /*|| currentMode == BOID.ADAPT */ &&  distance < fleeDistance)
			{
				velocityMultiplier = fleeMultiplier;
				flee( target.gameObject.transform.position );
			}
			else if( currentMode == BOID.SEEK) // && distance > fleeDistance)
			{
				seek( target.gameObject.transform.position );
			}
			else
			{
				velocityMultiplier = 1f;
			}
		}
		
		
		if( tf.position.y >= -22 ) 
		{
			fleeInDirection( new Vector2( 0, -10 ) );
		}
		
		//truncate(steeringForce, maxForce); //makes it dissapear sometimes?
		//steeringForce.scaleBy(1 / mass);
		if (steeringForce.magnitude > maxForce ) {
			steeringForce = steeringForce.normalized;
			steeringForce *= maxForce;
		}
		
		//trace(steeringForce);
		
		rb2D.velocity += steeringForce;
		
		if(rb2D.velocity.magnitude > maxVelocity )
		{
			rb2D.velocity = rb2D.velocity.normalized;
			rb2D.velocity *= maxVelocity;
		}
		rb2D.velocity = truncate(rb2D.velocity, maxVelocity);
		
		
		//setRotation();
		
		steeringForce = Vector2.zero;
		//tf.rotation = Quaternion.Euler( new Vector3( 0 , 0, (Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x ) * Mathf.Rad2Deg) -180 ));
	
	}
	
	public void flock(List<GameObject> boids)
	{
		Vector2 averageVelocity = rb2D.velocity;
		Vector3 averagePosition = Vector3.zero;//tf.transform.position;//Vector3.zero;
		
		float inSightCount = 0;
		
		//align 
		boids.Clear();
		Collider2D[] colliders = Physics2D.OverlapCircleAll( tf.position, minDistance );
		foreach( Collider2D col in colliders  )
		{
			if( col.name == this.name )boids.Add(col.gameObject );
		}
		
		for( int i = 0; i < boids.Count; ++i)
		{
			GameObject b = boids[i];
			
			if(b != this.gameObject && inSight(b))
			{
				averageVelocity += b.rigidbody2D.velocity;
				averagePosition += b.transform.position;
				
				if (tooClose( b ) ) this.flee(b.transform.position);
				
				
				inSightCount++;
			}
		}
		
		//cohere
		if (inSightCount > 0)
		{
			//print ("insightCount: " + inSightCount );
			averageVelocity *= (1 / inSightCount);
			averagePosition *= (1 / inSightCount);
			
			this.seek(averagePosition);
			//averageVelocity -= rb2D.velocity;
			
			BoidC.averagePosition.transform.position = averagePosition;
			//print (averagePosition );
			
			//steeringForce += averageVelocity / 3;
			//steeringForce += new Vector2(1, 0 ) * MAX_VELOCITY;//averageVelocity;
		}
		
	}
	
	public Vector2 truncate(Vector2 vector, float max) {
		
		float i;
		
		if(vector.magnitude == 0 ) return vector;
		
		i = max / vector.magnitude;
		//i = i < 1.0f ? 1.0f : i;
		if( i < 1.0f ) i = 1.0f;
		
		vector *= i;
		return vector;
		
	}
	
	private bool tooClose( GameObject boid) 
	{
		if (Vector3.Distance(tf.position, boid.transform.position) > sightRange) return false;
		
		Vector3 heading = new Vector2(rb2D.velocity.x, rb2D.velocity.y );
		heading.Normalize();
		Vector3 difference = boid.transform.position - tf.position;
		float dotProd = Vector3.Dot(difference, heading);
		
		if (dotProd < 0) return false;
		
		return true;
	}
	
	private bool inSight(GameObject b)
	{
		return Vector3.Distance(tf.position, b.transform.position) < minDistance;
	}
	
	void flee( Vector3 target)
	{
		Vector2 force = Vector2.zero;
		desired = tf.position - target;
		desired.Normalize();
		desired *= maxVelocity;
		
		force = desired - rb2D.velocity;
		force = truncate( force, maxForce );
		
		steeringForce += new Vector2( force.x * steeringModifier.x, force.y * steeringModifier.y );
		//steeringForce += force;
		
		//print ("fleeing");
	}
	
	void fleeInDirection( Vector2 direction)
	{
		steeringForce += direction * maxForce;
	}
	
	public void seek(Vector3 target)
	{
		Vector2 force = Vector2.zero;
		desired = target - tf.position;
		desired.Normalize();
		desired *= maxVelocity;
		
		force = desired - rb2D.velocity;
		
		force = truncate( force, maxForce);
		steeringForce += force;
		
	
		//print ("seeking");
	}
}

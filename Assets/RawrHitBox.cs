using UnityEngine;
using System.Collections;

public class RawrHitBox : MonoBehaviour {

	public string explosionPrefabName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		GameObject target = col.gameObject;
		if(target.name.Contains("FireBall") && !target.GetComponent<Hitbox>().owner.name.Contains("Boss") )
		{
			ExplosionEffect(target);
			Destroy(target);
		}
		if(target.name.Contains("ThrowHitBox") )
		{
			ExplosionEffect(target);
			Destroy(target);
		}
		if(target.name.Contains("UltimateMoveObject") )
		{
			print ("akdjflakdsj");
			ExplosionEffect(target);
			Destroy(target);
		}
	}

	void ExplosionEffect(GameObject target)
	{
		GameObject explosionObj = Instantiate(Resources.Load(explosionPrefabName)) as GameObject;
		explosionObj.transform.position = target.transform.position;
	}
}

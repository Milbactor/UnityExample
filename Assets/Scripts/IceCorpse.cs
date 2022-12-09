using UnityEngine;
using System.Collections;

public class IceCorpse {


	public float x;
	public float y;
	public string name;

	public IceCorpse(float newX, float newY, string name)
	{
		x = newX;
		y = newY;
		name = name + "IceCorpse";
		Debug.Log(name);
	}
}

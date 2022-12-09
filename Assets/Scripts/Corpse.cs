using System;
using System.Collections;

public class Corpse {

	public float x;
	public float y;
	public string playerName;
	public bool choice;

	public Corpse(float newX, float newY, string name, bool flag)
	{
		x = newX;
		y = newY;
		playerName = name;
		choice = flag;
	}
}
